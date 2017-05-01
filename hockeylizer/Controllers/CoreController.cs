﻿﻿﻿﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using hockeylizer.Services;
using hockeylizer.Helpers;
using hockeylizer.Models;
using hockeylizer.Data;
using System.Linq;
using System.IO;
using System;

namespace hockeylizer.Controllers
{
    public class CoreController : Controller
    {
        private readonly string appkey;
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment hostingEnvironment;

        public CoreController(ApplicationDbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            appkey = "langY6fgXWossV9o";
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

		[HttpPost]
		[AllowAnonymous]
        public JsonResult CreateTeam(string token) 
        {
            CreateTeamResult response;

            if (token != appkey)
            {
                response = new CreateTeamResult(false, "Laget kunde inte skapas då fel token angavs. Appen är inte registrerad.");
            }
            else 
            {
                var teamId = db.GetAvailableTeamId();

                var team = new AppTeam(teamId);
                db.AppTeams.Add(team);

                db.SaveChanges();

                response = new CreateTeamResult(true, "Laget skapades. Appen är nu registrerad.")
                {
                    TeamId = team.TeamId
                };
            }

            return Json(response);
        }

        // Add player
        [HttpPost]
        [AllowAnonymous]
        public JsonResult AddPlayer(string name, Guid? teamId, string token)
        {
            AddPlayerResult response;

            if (token == appkey)
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                {
                    response = new AddPlayerResult("Spelaren " + name + " kunde inte läggas till då namnet saknas.", false);
                    return Json(response);
                }

                var team = db.AppTeams.Find(teamId);
                if (teamId == null || team == null)
                {
                    response = new AddPlayerResult("Spelaren " + name + " kunde inte läggas till då appteamet som hen tillhör saknas.", false);
                    return Json(response);
                }

                var player = new Player(name, teamId);
                try
                {
                    db.Players.Add(player);
                    db.SaveChanges();

                    db.Entry(player).GetDatabaseValues();

                    response = new AddPlayerResult("Spelaren " + name + " lades till utan problem", true, player.PlayerId);
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

        [HttpPost]
        [AllowAnonymous]
        public JsonResult UpdatePlayerName(UpdateNameVm vm)
        {
            GeneralResult r;

            if (vm.token != appkey)
            {
                r = new GeneralResult(false, "Token var inkorrekt");
            }
            else
            {
                if (!vm.Validate())
                {
                    return Json(vm.Result);
                }

                var player = db.Players.Find(vm.playerId);
                if (player == null) 
                {
                    r = new GeneralResult(false, "Spelaren finns inte.");
                }
                else
                {
                    player.Name = vm.name;
                    db.SaveChanges();

                    r = new GeneralResult(true, "Namnet uppdaterades!");
                }
            }

            return Json(r);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DeletePlayer(DeletePlayerVm vm)
        {
            GeneralResult r;

            if (vm.token != appkey)
            {
                r = new GeneralResult(false, "Token var inkorrekt");
            }
            else
            {
                if (!vm.Validate())
                {
                    return Json(vm.Result);
                }

                var player = db.Players.Find(vm.playerId);
                if (player == null)
                {
                    r = new GeneralResult(false, "Spelaren finns inte.");
                }
                else
                {
                    player.Deleted = true;
                    db.SaveChanges();

                    r = new GeneralResult(true, "Spelaren raderades!");
                }
            }

            return Json(r);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetAllPlayers(Guid teamId, string token)
        {
            GetPlayersResult response;
            if (token == appkey)
            {
                response = new GetPlayersResult(true, "Alla spelare hämtades",
		                        db.Players.Where(p => p.TeamId == teamId && !p.Deleted).Select(p => 
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
        public async Task<JsonResult> UploadVideo(UploadVideoVm vm)
        {
            VideoResult vr;

            if (vm.token == appkey)
            {
                var pl = db.Players.Find(vm.playerId);

                if (pl == null)
                {
                    vr = new VideoResult("Spelaren kunde inte hittas.", false);
                }
                else
                {
                    if (!vm.Validate())
                    {
                        return Json(vm.vr);
                    }

                    var v = await FileHandler.UploadVideo(vm.video, pl.RetrieveContainerName(), "video");

                    if (string.IsNullOrEmpty(v.FilePath))
                    {
                        vr = new VideoResult("Videoklippet kunde inte laddas upp!", false);
                    }
                    else
                    {
                   
                        var savedVideo = new PlayerVideo(v.FilePath, v.FileName, (int)vm.playerId, (int)vm.interval, (int)vm.rounds, (int)vm.shots, (int)vm.numberOfTargets);
                        db.Videos.Add(savedVideo);

                        savedVideo.AddTargets(vm.targetOrder, vm.targetCoords, vm.timestamps);

                        db.SaveChanges();
                        db.Entry(savedVideo).GetDatabaseValues();

                        vr = new VideoResult("Videoklippet laddades upp!", true, savedVideo.VideoId);
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

                var deleted = FileHandler.DeleteVideo(video.VideoPath, video.Player.RetrieveContainerName());

                if (deleted)
                {
                    video.Delete();
                    db.SaveChanges();

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

                foreach (PlayerVideo v in db.Videos.Where(v => !v.Deleted).ToList())
                {
                    string videoPath;

                    try
                    {
                        videoPath = await FileHandler.GetShareableVideoUrl(v.VideoPath);
                    }
                    catch
                    {
                        videoPath = "";
                    }

                    var videoInfo = new VideoVmSmall
                    {
                        VideoId = v.VideoId,
                        VideoPath = videoPath,
                        Interval = v.Interval,
                        Rounds = v.Rounds,
                        Shots = v.Shots,
                        NumberOfTargets = v.NumberOfTargets,
                        Targets = v.Targets.ToList()
                    };

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
        public JsonResult GetFramesFromVideo(int? videoId, string token)
        {
            GetFramesResult response;
            if (token == appkey)
            {
                var video = db.Videos.Find(videoId);

                if (video != null && !video.Deleted)
                {
                    response = new GetFramesResult(true, "Alla frames hämtades", new List<string>());

                    for (var img = 0; img <= 1500; img++)
                    {
                        response.Images.Add("https://cdn.pixabay.com/photo/2016/06/24/16/49/hockey-puck-1477440_960_720.png");
                    }
                }
                else
                {
                    response = new GetFramesResult(false, "Videon finns inte", new List<string>());
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
        public async Task<JsonResult> ChopVideo(int videoId, string token)
        {
            GeneralResult response;
            if (token == appkey)
            {
                var video = db.Videos.Find(videoId);

                if (video == null)
                {
                    response = new GeneralResult(false, "Videon finns inte");
                    return Json(response);
                }

                var blobname = video.FileName;
                var path = Path.Combine(hostingEnvironment.WebRootPath, "videos");
                path = Path.Combine(path, blobname);

                var player = db.Players.Find(video.PlayerId);

                if (player == null)
                {
                    response = new GeneralResult(false, "Spelaren finns inte");
                    return Json(response);
                }

                var download = await FileHandler.DownloadBlob(path, blobname, player.RetrieveContainerName());

                if (!download)
                {
                    response = new GeneralResult(false, "Videon kunde inte laddas ned.");
                    return Json(response);
                }

                // Logik för att choppa video

                var pictureUrls = new List<string>();
                if (pictureUrls.Any())
                {
                    //foreach (var url in pictureUrls)
                    //{
                    //    var picture = new PictureToAnalyze(url, videoId);
                    //    await db.Pictures.AddAsync(picture);
                    //}
                    
                    //await db.SaveChangesAsync();
                }

                if (!System.IO.File.Exists(path))
                {
                    response = new GeneralResult(false, "Videon kunde inte raderas då den inte existerar.");
                    return Json(response);
                }

                System.IO.File.Delete(path);

                response = new GeneralResult(true, "Allt fixat!");
                return Json(response);
            }

            response = new GeneralResult(false, "Inkorrekt token");
            return Json(response);
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult del()
        {
            foreach (var vi in db.Videos)
            {
                db.Remove(vi);
            }
            db.SaveChanges();
            return Json("Done");
        }

        //Daniels funktion 
        [HttpPost]
        [AllowAnonymous]
        public JsonResult getHitsOverviewSVG(int videoId, string token)
        {

            // Just return a goddamn picture of the goal with no hits.
            // It's something.
            var path = hostingEnvironment.WebRootPath + "/images/hitsOverview.svg";

            var svgURL = @"http://hockeylizer.azurewebsites.net/images/hitsOverview.svg";

            string svgStr = System.IO.File.ReadAllText(path);

            return Json(svgURL);

        //    GeneralResult response;
        //    if (token == appkey)
        //    {
        //        var analysisList = db.AnalysisResults.Where(ar => ar.VideoId == videoId);

        //        // Lite osäker på om ett query utan resultat ger
        //        // void eller en enumarable av längd noll.
        //        if (analysisList == null || analysisList.Count() == 0)
        //        {
        //            response = new GeneralResult(false, "Analysen finns inte");
        //            return Json(response);
        //        }

        //        foreach (var analysis in analysisList)
        //        {
                    
        //        }

        //    }

        //    response = new GeneralResult(false, "Inkorrekt token");
        //    return Json(response);
        }
    }
}