﻿﻿﻿using Microsoft.AspNetCore.Authorization;
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
        public async Task<JsonResult> UpdatePlayerName(UpdateNameVm vm)
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
                    await db.SaveChangesAsync();

                    r = new GeneralResult(true, "Namnet uppdaterades!");
                }
            }

            return Json(r);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> DeletePlayer(DeletePlayerVm vm)
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
                    await db.SaveChangesAsync();

                    r = new GeneralResult(true, "Spelaren raderades!");
                }
            }

            return Json(r);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetAllPlayers([FromBody]Guid? teamId, string token)
        {
            GetPlayersResult response;
            if (token == appkey)
            {
                var team = db.AppTeams.Find(teamId);

                if (team == null)
                {
					// Team missing
                    response = new GetPlayersResult(false, "Appen saknas i databasen", new List<PlayerVmSmall>());
                    return Json(response);
				}

                response = new GetPlayersResult(true, "Alla spelare hämtades",
                                db.Players.Where(p => p.TeamId == teamId && !p.Deleted).Select(p =>
                                  new PlayerVmSmall
                                  {
                                      PlayerId = p.PlayerId,
                                      Name = p.Name
                                  }).ToList());
                return Json(response);
            }

            response = new GetPlayersResult(false, "Token var inkorrekt", new List<PlayerVmSmall>());
         
            return Json(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CreateSession(CreateSessionVm vm)
        {
            SessionResult sr;

            if (vm.token == appkey)
            {
                var pl = db.Players.Find(vm.playerId);

                if (pl == null)
                {
                    sr = new SessionResult("Spelaren kunde inte hittas.", false);
                }
                else
                {
                    if (!vm.Validate())
                    {
                        return Json(vm.sr);
                    }

                    var v = await FileHandler.UploadVideo(vm.video, pl.RetrieveContainerName(), "video");

                    if (string.IsNullOrEmpty(v.FilePath))
                    {
                        sr = new SessionResult("Videoklippet kunde inte laddas upp då något gick knas vid uppladdning!", false);
                    }
                    else
                    {
                   
                        var savedSession = new PlayerSession(v.FilePath, v.FileName, (int)vm.playerId, (int)vm.interval, (int)vm.rounds, (int)vm.shots, (int)vm.numberOfTargets);
                        db.Sessions.Add(savedSession);

                        savedSession.AddTargets(vm.targetOrder, vm.targetCoords, vm.timestamps);

                        db.SaveChanges();
                        db.Entry(savedSession).GetDatabaseValues();

                        sr = new SessionResult("Videoklippet laddades upp!", true, savedSession.SessionId);
                    }
                }
            }
            else
            {
                sr = new SessionResult("Token var inkorrekt", false);
            }

            return Json(sr);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> DeleteVideoFromSession(int sessionId, string token)
        {
            GeneralResult response;
            if (token == appkey)
            {
                var session = db.Sessions.Find(sessionId);

                if (session == null)
                {
                    response = new GeneralResult(false, "Sessionen kunde inte hittas");
                    return Json(response);
                }

                var deleted = FileHandler.DeleteVideo(session.VideoPath, session.Player.RetrieveContainerName());

                if (deleted)
                {
                    session.Delete();
                    await db.SaveChangesAsync();

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
		public JsonResult GetFramesFromShot(GetTargetFramesVm vm)
        {
			GetFramesFromShotResult response;
			if (vm.token == appkey)
			{
				var session = db.Sessions.Find(vm.sessionId);

				if (session == null)
				{
					response = new GetFramesFromShotResult(false, "Sessionen kunde inte hittas");
					return Json(response);
				}

				if (!vm.Validate())
				{
					return Json(vm.gr);
				}

				var shot = session.Targets.FirstOrDefault(t => t.Order == vm.shot);

				if (shot == null)
				{
					response = new GetFramesFromShotResult(false, "Skottet som skulle uppdateras kunde inte hittas.");
					return Json(response);
				}

                response = new GetFramesFromShotResult(true, "Skottets träffpunkt har uppdaterats!", shot.FramesToAnalyze.Select(frame => frame.FrameUrl).ToList());
				return Json(response);
			}

			response = new GetFramesFromShotResult(false, "Inkorrekt token");
			return Json(response);
        }

		[HttpGet]
		[AllowAnonymous]
		public JsonResult GetDataFromShot(GetTargetFramesVm vm)
		{
			GetDataFromShotResult response;
			if (vm.token == appkey)
			{
				var session = db.Sessions.Find(vm.sessionId);

				if (session == null)
				{
					response = new GetDataFromShotResult(false, "Sessionen kunde inte hittas");
					return Json(response);
				}

				if (!vm.Validate())
				{
					return Json(vm.gr);
				}

				var shot = session.Targets.FirstOrDefault(t => t.Order == vm.shot);

				if (shot == null)
				{
					response = new GetDataFromShotResult(false, "Skottet som skulle uppdateras kunde inte hittas.");
					return Json(response);
				}

				response = new GetDataFromShotResult(true, "Skottets träffpunkt har uppdaterats!", shot.FramesToAnalyze.Select(frame => frame.FrameUrl).ToList())
                {
                    TargetNumber = shot.TargetNumber,
                    Order = shot.Order,
                    XCoordinate = shot.XCoordinate,
                    YCoordinate = shot.YCoordinate
                };

				return Json(response);
			}

			response = new GetDataFromShotResult(false, "Inkorrekt token");
			return Json(response);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<JsonResult> UpdateTargetHit(UpdateTargetHitVm vm)
        {
			GeneralResult response;
			if (vm.token == appkey)
			{
				var session = db.Sessions.Find(vm.sessionId);

				if (session == null)
				{
					response = new GeneralResult(false, "Sessionen kunde inte hittas");
					return Json(response);
				}

                if (!vm.Validate())
				{
					return Json(vm.ur);
				}

                var shotToUpdate = session.Targets.FirstOrDefault(t => t.Order == vm.shot);

                if (shotToUpdate == null)
                {
					response = new GeneralResult(false, "Skottet som skulle uppdateras kunde inte hittas.");
					return Json(response);
                }

                shotToUpdate.XCoordinate = vm.x;
                shotToUpdate.YCoordinate = vm.y;

                await db.SaveChangesAsync();

                response = new GeneralResult(true, "Skottets träffpunkt har uppdaterats!");
				return Json(response);
			}

			response = new GeneralResult(false, "Inkorrekt token");
			return Json(response);
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<JsonResult> GetAllSessions(string token)
        //{
        //    GetSessionsResult response;
        //    if (token == appkey)
        //    {
        //        response = new GetSessionsResult(true, "Alla videor hämtades", new List<SessionVmSmall>());

        //        foreach (PlayerSession s in db.Sessions.Where(v => !v.Deleted).ToList())
        //        {
        //            string videoPath;

        //            try
        //            {
        //                videoPath = await FileHandler.GetShareableVideoUrl(s.VideoPath);
        //            }
        //            catch
        //            {
        //                videoPath = "";
        //            }

        //            var sessionInfo = new SessionVmSmall
        //            {
        //                SessionId = s.SessionId,
        //                VideoPath = videoPath,
        //                Interval = s.Interval,
        //                Rounds = s.Rounds,
        //                Shots = s.Shots,
        //                NumberOfTargets = s.NumberOfTargets,
        //                Targets = s.Targets.ToList()
        //            };

        //            response.Sessions.Add(sessionInfo);
        //        }
        //    }
        //    else
        //    {
        //        response = new GetSessionsResult(false, "Token var inkorrekt", new List<SessionVmSmall>());
        //    }

        //    return Json(response);
        //}

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetFramesFromSession(int? sessionId, string token)
        {
            GetFramesResult response;
            if (token == appkey)
            {
                var session = db.Sessions.Find(sessionId);

                if (session != null && !session.Deleted)
                {
                    response = new GetFramesResult(true, "Alla frames hämtades", new List<string>());

                    foreach (var t in session.Targets)
                    {
                        response.Images.AddRange(t.FramesToAnalyze.Select(frame => frame.FrameUrl));
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


        private async Task<GeneralResult> ChopVideo(int sessionId, string token)
        {
            GeneralResult response;
            if (token == appkey)
            {
                var session = db.Sessions.Find(sessionId);

                if (session == null)
                {
                    response = new GeneralResult(false, "Videon finns inte");
                    return response;
                }

                var blobname = session.FileName;
                var path = Path.Combine(hostingEnvironment.WebRootPath, "videos");
                path = Path.Combine(path, blobname);

                var player = db.Players.Find(session.PlayerId);

                if (player == null)
                {
                    response = new GeneralResult(false, "Spelaren finns inte");
                    return response;
                }

                var download = await FileHandler.DownloadBlob(path, blobname, player.RetrieveContainerName());

                if (!download)
                {
                    response = new GeneralResult(false, "Videon kunde inte laddas ned.");
                    return response;
                }

                // Logik för att choppa video

                var pictures = new List<DecodeFramesResult>();
                if (pictures.Any())
                {
                    foreach (var p in pictures)
                    {
                        var target = session.Targets.FirstOrDefault(shot => shot.Order == p.Shot);

                        if (target != null)
                        {
                            var picture = new FrameToAnalyze(target.TargetId, p.FrameUrl);
                            await db.Frames.AddAsync(picture);
                        }
                    }
                    
                    await db.SaveChangesAsync();
                }

                if (!System.IO.File.Exists(path))
                {
                    response = new GeneralResult(false, "Videon kunde inte raderas då den inte existerar.");
                    return response;
                }

                System.IO.File.Delete(path);

                response = new GeneralResult(true, "Allt fixat!");
                return response;
            }

            response = new GeneralResult(false, "Inkorrekt token");
            return response;
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