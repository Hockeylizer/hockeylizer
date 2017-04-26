﻿using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using hockeylizer.Services;
using hockeylizer.Models;
using hockeylizer.Data;
using Newtonsoft.Json;
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
                    response = new AddPlayerResult("Spelaren " + name + " kunde inte l�ggas till d� namnet saknas.", false);
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
                    response = new AddPlayerResult("Spelaren " + name + " kunde inte l�ggas till. Felmeddelande: " + e.Message, false);
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
                response = new GetPlayersResult(true, "Alla spelare h�mtades",
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

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<JsonResult> UploadVideo([FromBody, FromForm]int? playerId, IFormFile video, int? interval, int? rounds, int? shots, int? numberOfTargets, List<ShotTimestampVm> timestamps, List<int> targetOrder, List<TargetCoordsVm> targetCoords, string token)
        //{
        //    VideoResult vr;
        //    if (token == appkey)
        //    {
        //        if (playerId == null)
        //        {
        //            vr = new VideoResult("Spelaren kunde inte hittas d� spelarens id inte var med i requesten.", false);
        //            return Json(vr);
        //        }

        //        var pl = db.Players.Find(playerId);

        //        if (pl == null)
        //        {
        //            vr = new VideoResult("Spelaren kunde inte hittas.", false);
        //        }
        //        else
        //        {
        //            if (video == null || video.Length == 0)
        //            {
        //                vr = new VideoResult("Videoklippet kunde inte laddas upp d� videon saknas!", false);
        //                return Json(vr);
        //            }

        //            if (interval == null)
        //            {
        //                vr = new VideoResult("Videoklippet kunde inte laddas upp d� intervall saknas!", false);
        //                return Json(vr);
        //            }

        //            if (rounds == null)
        //            {
        //                vr = new VideoResult("Videoklippet kunde inte laddas upp d� rundor saknas!", false);
        //                return Json(vr);
        //            }

        //            if (numberOfTargets == null)
        //            {
        //                vr = new VideoResult("Videoklippet kunde inte laddas upp d� antal skott saknas!", false);
        //                return Json(vr);
        //            }

        //            if (!timestamps.Any())
        //            {
        //                vr = new VideoResult("Videoklippet kunde inte laddas upp d� timestamps saknas!", false);
        //                return Json(vr);
        //            }

        //            if (!targetOrder.Any())
        //            {
        //                vr = new VideoResult("Videoklippet kunde inte laddas upp d� skottordning saknas!", false);
        //                return Json(vr);
        //            }

        //            //if (!targetCoords.Any())
        //            //{
        //            //    vr = new VideoResult("Videoklippet kunde inte laddas upp d� koordinater f�r skotten saknas!", false);
        //            //    return Json(vr);
        //            //}

        //            // Logik f�r att ladda upp video
        //            var v = await ImageHandler.UploadVideo(video, db, pl, "video");

        //            if (string.IsNullOrEmpty(v))
        //            {
        //                vr = new VideoResult("Videoklippet kunde inte laddas upp!", false);
        //            }
        //            else
        //            {
        //                var savedVideo = new PlayerVideo(v, (int)playerId, (int)interval, (int)rounds, (int)shots, (int)numberOfTargets);
        //                db.Videos.Add(savedVideo);

        //                foreach (var ts in timestamps)
        //                {
        //                    var timestamp = new ShotTimestamp(ts.Start, ts.End)
        //                    {
        //                        Video = savedVideo
        //                    };

        //                    savedVideo.Timestamps.Add(timestamp);
        //                }

        //                var index = 1;
        //                foreach (var t in targetOrder)
        //                {
        //                    var target = new Target(t, index)
        //                    {
        //                        RelatedVideo = savedVideo
        //                    };
        //                    index++;

        //                    savedVideo.Targets.Add(target);
        //                }

        //                foreach (var tc in targetCoords)
        //                {
        //                    var targetCoordinate = new TargetCoord(tc.xCoord, tc.yCoord)
        //                    {
        //                        Video = savedVideo
        //                    };

        //                    savedVideo.TargetCoords.Add(targetCoordinate);
        //                }

        //                db.SaveChanges();

        //                vr = new VideoResult("Videoklippet laddades upp!", true);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        vr = new VideoResult("Token var inkorrekt", false);
        //    }

        //    return Json(vr);
        //}

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> UploadVideo(UploadVideoVm vm)
        {
            VideoResult vr;

            if (vm.token == appkey)
            {
                if (vm.playerId == null)
                {
                    vr = new VideoResult("Spelaren kunde inte hittas då spelarens id inte var med i requesten.", false);
                    return Json(vr);
                }

                var pl = db.Players.Find(vm.playerId);

                if (pl == null)
                {
                    vr = new VideoResult("Spelaren kunde inte hittas.", false);
                }
                else
                {
                    if (vm.video == null || vm.video.Length == 0)
                    {
                        vr = new VideoResult("Videoklippet kunde inte laddas upp då videon saknas!", false);
                        return Json(vr);
                    }

                    if (vm.interval == null)
                    {
                        vr = new VideoResult("Videoklippet kunde inte laddas upp då intervall saknas!", false);
                        return Json(vr);
                    }

                    if (vm.rounds == null)
                    {
                        vr = new VideoResult("Videoklippet kunde inte laddas upp då rundor saknas!", false);
                        return Json(vr);
                    }

                    if (vm.numberOfTargets == null)
                    {
                        vr = new VideoResult("Videoklippet kunde inte laddas upp då antal skott saknas!", false);
                        return Json(vr);
                    }

                    if (!vm.timestamps.Any())
                    {
                        vr = new VideoResult("Videoklippet kunde inte laddas upp då timestamps saknas!", false);
                        return Json(vr);
                    }

                    if (!vm.targetOrder.Any())
                    {
                        vr = new VideoResult("Videoklippet kunde inte laddas upp då skottordning saknas!", false);
                        return Json(vr);
                    }

                    //if (!targetCoords.Any())
                    //{
                    //    vr = new VideoResult("Videoklippet kunde inte laddas upp då koordinater för skotten saknas!", false);
                    //    return Json(vr);
                    //}

                    // Logik f�r att ladda upp video
                    var v = await ImageHandler.UploadVideo(vm.video, db, pl, "video");

                    if (string.IsNullOrEmpty(v))
                    {
                        vr = new VideoResult("Videoklippet kunde inte laddas upp!", false);
                    }
                    else
                    {
                        var savedVideo = new PlayerVideo(v, (int)vm.playerId, (int)vm.interval, (int)vm.rounds, (int)vm.shots, (int)vm.numberOfTargets);
                        db.Videos.Add(savedVideo);

                        foreach (var ts in vm.timestamps)
                        {
                            var timestamp = new ShotTimestamp(ts.Start, ts.End)
                            {
                                Video = savedVideo
                            };

                            savedVideo.Timestamps.Add(timestamp);
                        }

                        var index = 1;
                        foreach (var t in vm.targetOrder)
                        {
                            var target = new Target(t, index)
                            {
                                RelatedVideo = savedVideo
                            };
                            index++;

                            savedVideo.Targets.Add(target);
                        }

                        foreach (var tc in vm.targetCoords)
                        {
                            var targetCoordinate = new TargetCoord(tc.xCoord, tc.yCoord)
                            {
                                Video = savedVideo
                            };

                            savedVideo.TargetCoords.Add(targetCoordinate);
                        }

                        db.SaveChanges();

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
        public JsonResult TestUpload([FromBody]UploadTimeStampVm vm)
        {
            VideoResult vr;

            if (string.IsNullOrEmpty(vm.token))
            {
                vr = new VideoResult("Token tom!", false);
                return Json(vr);
            }

            if (vm.token == appkey)
            {
                if (!vm.timestamps.Any())
                {
                    vr = new VideoResult("Videoklippet kunde inte laddas upp då timestamps för skotten saknas!", false);
                    return Json(vr);
                }

                vr = new VideoResult("Woop allt ser korrekt ut!", true);
            }
            else
            {
                vr = new VideoResult("Felaktig token", false);
            }       

            return Json(vr);
        }

        [HttpPost]
        [AllowAnonymous]
        public ContentResult Test(string json)
        {
            //VideoResult vr;
            //if (string.IsNullOrEmpty(vm.token))
            //{
            //    vr = new VideoResult("Token tom!", false);
            //    return Json(vr);
            //}

            //if (vm.token == appkey)
            //{
            //    if (!vm.timestamps.Any())
            //    {
            //        vr = new VideoResult("Videoklippet kunde inte laddas upp d� timestamps f�r skotten saknas!", false);
            //        return Json(vr);
            //    }

            //    vr = new VideoResult("Woop allt ser korrekt ut!", true);
            //}
            //else
            //{
            //    vr = new VideoResult("Felaktig token", false);
            //}       

            return Content(json);
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
                response = new GetVideosResult(true, "Alla videor h�mtades", new List<VideoVmSmall>());

                foreach (PlayerVideo v in db.Videos.ToList())
                {
                    string videoPath;

                    try
                    {
                        videoPath = await ImageHandler.GetShareableVideoUrl(v.VideoPath);
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
                        Timestamps = v.Timestamps.ToList(),
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

                if (video != null)
                {
                    response = new GetFramesResult(true, "Alla frames h�mtades", new List<string>());

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

                // Logik f�r analysen
            }

            response = new GeneralResult(false, "Inkorrekt token");
            return Json(response);
        }
    }
}
