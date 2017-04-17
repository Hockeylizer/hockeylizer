using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using hockeylizer.Services;
using hockeylizer.Models;
using hockeylizer.Data;
using System.Linq;
using System;

namespace hockeylizer.Controllers
{
    public class CoreController : Controller
    {
        private readonly string appkey;
        private readonly ApplicationDbContext db;

        public CoreController(ApplicationDbContext _db)
        {
            appkey = "langY6fgXWossV9o";
            db = _db;
        }

        // Add player
        [HttpPost]
        [AllowAnonymous]
        public JsonResult AddPlayer(string name, string token)
        {
            AddPlayerResult response;

            if (token == appkey)
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                {
                    response = new AddPlayerResult("Spelaren " + name + " kunde inte läggas till då namnet saknas.", false);
                    return Json(response);
                }

                var player = new Player(name);
                try
                {
                    db.Players.Add(player);
                    db.SaveChanges();

                    response = new AddPlayerResult("Spelaren " + name + " lades till utan problem", true);
                }
                catch (Exception e)
                {
                    response = new AddPlayerResult("Spelaren " + name + " kunde inte läggas till. Felmeddelande: " + e.Message, false);
                }
            }
            else
            {
                response = new AddPlayerResult("Token var inkorrekt", false);
            }

            return Json(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetAllPlayers(string token)
        {
            GetPlayersResult response;
            if (token == appkey)
            {
                response = new GetPlayersResult(true, "Alla spelare hämtades",
                    db.Players.Select(p =>
                        new PlayerVmSmall
                        {
                            PlayerId = p.PlayerId,
                            Name = p.Name
                        }).ToList());
            }
            else
            {
                response = new GetPlayersResult(false, "Token var inkorrekt", new List<PlayerVmSmall>());
            }

            return Json(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> UploadVideo(int? playerId, IFormFile video, string token)
        {
            VideoResult vr;
            if (token == appkey)
            {
                if (playerId == null)
                {
                    vr = new VideoResult("Spelaren kunde inte hittas då spelarens id inte var med i requesten.", false);
                    return Json(vr);
                }

                var pl = db.Players.Find(playerId);

                if (pl == null)
                {
                    vr = new VideoResult("Spelaren kunde inte hittas.", false);
                }
                else
                {
                    if (video == null || video.Length == 0)
                    {
                        vr = new VideoResult("Videoklippet kunde inte laddas upp då videon saknas!", false);
                        return Json(vr);
                    }

                    // Logik för att ladda upp video
                    var v = await ImageHandler.UploadVideo(video, db, pl, "video");

                    if (!v)
                    {
                        vr = new VideoResult("Videoklippet kunde inte laddas upp!", false);
                    }
                    else
                    {
                        vr = new VideoResult("Videoklippet laddades upp!", true);
                    }
                }
            }
            else
            {
                vr = new VideoResult("Token var inkorrekt", false);
            }

            return Json(vr);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DeleteVideo(int videoId, string token)
        {
            GeneralResult response;
            if (token == appkey)
            {
                var video = db.Videos.Find(videoId);

                if (video == null)
                {
                    response = new GeneralResult(false, "Inkorrekt token");
                    return Json(response);
                }

                var deleted = ImageHandler.DeleteImage(videoId, db);

                if (deleted)
                {
                    response = new GeneralResult(true, "Videoklippet raderades");
                    return Json(response);
                }

                response = new GeneralResult(false, "Videoklippet kunde inte raderas");
                return Json(response);
            }

            response = new GeneralResult(false, "Inkorrekt token");
            return Json(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetAllVideos(string token)
        {
            GetVideosResult response;
            if (token == appkey)
            {
                response = new GetVideosResult(true, "Alla videor hämtades", new List<VideoVmSmall>());

                foreach (PlayerVideo v in db.Videos.ToList())
                {
                    var videoPath = await ImageHandler.GetShareableVideoUrl(v);
                    var videoInfo = new VideoVmSmall { VideoId = v.VideoId, VideoPath = videoPath };

                    response.Videos.Add(videoInfo);
                }
            }
            else
            {
                response = new GetVideosResult(false, "Token var inkorrekt", new List<VideoVmSmall>());
            }

            return Json(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetFramesFromVideo(int videoId, string token)
        {
            GetFramesResult response;
            if (token == appkey)
            {
                response = new GetFramesResult(true, "Alla frames hämtades", new List<string>());

                for (var img = 0; img <= 15; img++)
                {
                    response.Images.Add(HttpContext.Request.Host + "images/hp.png");
                }
            }
            else
            {
                response = new GetFramesResult(false, "Token var inkorrekt", new List<string>());
            }

            return Json(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AnalyzeVideo(int videoId, string token)
        {
            GeneralResult response;
            if (token == appkey)
            {
                var video = db.Videos.Find(videoId);

                if (video == null)
                {
                    response = new GeneralResult(false, "Inkorrekt token");
                    return Json(response);
                }

                // Logik för analysen
            }

            response = new GeneralResult(false, "Inkorrekt token");
            return Json(response);
        }
    }
}
