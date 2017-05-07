﻿﻿﻿﻿﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using hockeylizer.Services;
using hockeylizer.Helpers;
using hockeylizer.Models;
using hockeylizer.Data;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using Hangfire;
using System;

namespace hockeylizer.Controllers
{
    public class CoreController : Controller
    {
        private readonly string _appkey;
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CoreController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _appkey = "langY6fgXWossV9o";
            this._db = db;
            this._hostingEnvironment = hostingEnvironment;
        }

		[HttpPost]
		[AllowAnonymous]
        public JsonResult CreateTeam([FromBody]CreateTeamVm vm) 
        {
            CreateTeamResult response;

            if (vm.token != _appkey)
            {
                response = new CreateTeamResult(false, "Laget kunde inte skapas då fel token angavs. Appen är inte registrerad.");
            }
            else 
            {
                var teamId = _db.GetAvailableTeamId();

                var team = new AppTeam(teamId);
                _db.AppTeams.Add(team);

                _db.SaveChanges();

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
        public JsonResult AddPlayer([FromBody]AddPlayerVm vm)
        {
            AddPlayerResult response;

            if (vm.token == _appkey)
            {
                if (string.IsNullOrEmpty(vm.name) || string.IsNullOrWhiteSpace(vm.name))
                {
                    response = new AddPlayerResult("Spelaren " + vm.name + " kunde inte läggas till då namnet saknas.", false);
                    return Json(response);
                }

                if (vm.teamId == null)
                {
                    response = new AddPlayerResult("Spelaren " + vm.name + " kunde inte läggas till då requesten saknar appid", false);
                    return Json(response);
                }

                var team = _db.AppTeams.Find(vm.teamId);
                if (vm.teamId == null || team == null)
                {
                    response = new AddPlayerResult("Spelaren " + vm.name + " kunde inte läggas till då appteamet som hen tillhör saknas.", false);
                    return Json(response);
                }

                var player = new Player(vm.name, vm.teamId);
                try
                {
                    _db.Players.Add(player);
                    _db.SaveChanges();

                    _db.Entry(player).GetDatabaseValues();

                    response = new AddPlayerResult("Spelaren " + vm.name + " lades till utan problem", true, player.PlayerId);
                }
                catch (Exception e)
                {
                    response = new AddPlayerResult("Spelaren " + vm.name + " kunde inte läggas till. Felmeddelande: " + e.Message, false);
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
        public async Task<JsonResult> UpdatePlayerName([FromBody]UpdateNameVm vm)
        {
            GeneralResult r;

            if (vm.token != _appkey)
            {
                r = new GeneralResult(false, "Token var inkorrekt");
            }
            else
            {
                if (!vm.Validate())
                {
                    return Json(vm.Result);
                }

                var player = _db.Players.Find(vm.playerId);
                if (player == null) 
                {
                    r = new GeneralResult(false, "Spelaren finns inte.");
                }
                else
                {
                    player.Name = vm.name;
                    await _db.SaveChangesAsync();

                    r = new GeneralResult(true, "Namnet uppdaterades!");
                }
            }

            return Json(r);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> DeletePlayer([FromBody]DeletePlayerVm vm)
        {
            GeneralResult r;

            if (vm.token != _appkey)
            {
                r = new GeneralResult(false, "Token var inkorrekt");
            }
            else
            {
                if (!vm.Validate())
                {
                    return Json(vm.Result);
                }

                var player = _db.Players.Find(vm.playerId);
                if (player == null)
                {
                    r = new GeneralResult(false, "Spelaren finns inte.");
                }
                else
                {
                    player.Deleted = true;
                    await _db.SaveChangesAsync();

                    r = new GeneralResult(true, "Spelaren raderades!");
                }
            }

            return Json(r);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetAllPlayers([FromBody]GetAllPlayersVm vm)
        {
            GetPlayersResult response;
            if (vm.token == _appkey)
            {
                if (vm.teamId == null)
                {
					response = new GetPlayersResult(false, "teamId saknas i requesten", new List<PlayerVmSmall>());
					return Json(response);
                }
                var team = _db.AppTeams.Find(vm.teamId);

                if (team == null)
                {
					// Team missing
                    response = new GetPlayersResult(false, "Appen saknas i databasen", new List<PlayerVmSmall>());
                    return Json(response);
				}

                response = new GetPlayersResult(true, "Alla spelare hämtades",
                                _db.Players.Where(p => p.TeamId == vm.teamId && !p.Deleted).Select(p =>
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

            if (vm.token == _appkey)
            {
                if (vm.playerId == null)
                {
                    sr = new SessionResult("SpelarId saknas i requesten.", false);
                    return Json(sr);
                }

                var pl = _db.Players.Find(vm.playerId);

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
                        _db.Sessions.Add(savedSession);

                        savedSession.AddTargets(vm.targetOrder, vm.targetCoords, vm.timestamps);

                        _db.SaveChanges();
                        _db.Entry(savedSession).GetDatabaseValues();

                        var sessionId = savedSession.SessionId;

                        // Chop video
                        BackgroundJob.Enqueue<CoreController>
						(
                            service => service.ChopSession(sessionId)
						);

                        // Analyze video
                        BackgroundJob.Enqueue<CoreController>
                        (
                            service => service.AnalyzeSession(sessionId)
                        );

                        sr = new SessionResult("Videoklippet laddades upp!", true, sessionId);
                    }
                }
            }
            else
            {
                sr = new SessionResult("Token var inkorrekt.", false);
            }

            return Json(sr);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public JsonResult AnalyzeThis([FromBody]SessionVm vm)
        //{
        //    GeneralResult response;
        //    if (vm.token == _appkey)
        //    {
        //        var session = _db.Sessions.Find(vm.sessionId);

        //        if (session == null)
        //        {
        //            response = new GeneralResult(false, "Sessionen kunde inte hittas");
        //            return Json(response);
        //        }

        //        BackgroundJob.Enqueue<CoreController>
        //        (
        //            service => service.AnalyzeSession(vm.sessionId)
        //        );

        //        response = new GeneralResult(true, "Sessionen är inlagd för analys");
        //        return Json(response);
        //    }

        //    response = new GeneralResult(false, "Fel token");
        //    return Json(response);
        //}

        public async Task<bool> AnalyzeSession(int sessionId)
        {
            var session = _db.Sessions.Find(sessionId);
            if (session == null) throw new Exception("Kunde inte hitta session.");

            var blobname = session.FileName;
            var startpath = Path.Combine(_hostingEnvironment.WebRootPath, "videos");

            var path = Path.Combine(startpath, blobname);

            var count = 1;
            while (System.IO.File.Exists(path))
            {
                var filetype = blobname.Split('.').LastOrDefault() ?? "mp4";
                var filestart = blobname.Split('-').FirstOrDefault() ?? "video";

                var filename = filestart + count + "." + filetype;

                path = Path.Combine(startpath, filename);
                count++;
            }

            var player = _db.Players.Find(session.PlayerId);
            if (player == null) throw new Exception("Kunde inte hitta spelare.");

            var download = await FileHandler.DownloadBlob(path, blobname, player.RetrieveContainerName());
            if (!download) throw new Exception("Kunde inte ladda ned film.");

            var targets = _db.Targets.Where(shot => shot.SessionId == sessionId).ToList();

            var pointDict = new Dictionary<int, Point2d>
            {
                {1, new Point2d(10, 91)},
                {2, new Point2d(10, 18)},
                {3, new Point2d(173, 18)},
                {4, new Point2d(173, 91)},
                {5, new Point2d(91.5, 101)}
            };

            var points = new List<Point2i>();
            var offsets = new List<Point2d>();

            var iter = 1;
            foreach (var hp in targets)
            {
                points.Add(new Point2i(hp.XCoordinate ?? 0, hp.YCoordinate ?? 0));

                Point2d coordinates;
                var shot = hp.Order;

                if (pointDict.ContainsKey(shot))
                {
                    coordinates = pointDict[shot];
                }
                else if (pointDict.ContainsKey(iter % 5))
                {
                    coordinates = pointDict[iter % 5];
                }
                else
                {
                    coordinates = new Point2d(0, 0);
                }

                offsets.Add(coordinates);
                iter++;
            }

            const int width = 183;
            const int height = 122;

            foreach (var t in targets)
            {
                var analysis = AnalysisBridge.AnalyzeShot(t.TimestampStart, t.TimestampEnd, points.ToArray(), width,
                    height, offsets.ToArray(), path);

                if (analysis.WasErrors) continue;

                var xHit = analysis.HitPoint.x;
                var yHit = analysis.HitPoint.y;
                var hit = analysis.DidHitGoal;

                t.XCoordinateAnalyzed = xHit;
                t.YCoordinateAnalyzed = yHit;
                t.HitGoal = hit;
            }

            session.Analyzed = true;

            await _db.SaveChangesAsync();

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            return true;
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public JsonResult ChopThis([FromBody] SessionVm vm)
        //{
        //    GeneralResult response;
        //    if (vm.token == _appkey)
        //    {
        //        var session = _db.Sessions.Find(vm.sessionId);

        //        if (session == null)
        //        {
        //            response = new GeneralResult(false, "Sessionen kunde inte hittas");
        //            return Json(response);
        //        }

        //        BackgroundJob.Enqueue<CoreController>
        //        (
        //            service => service.ChopSession(vm.sessionId)
        //        );

        //        response = new GeneralResult(true, "Sessionen är inlagd för uppkapning");
        //        return Json(response);
        //    }

        //    response = new GeneralResult(false, "Fel token");
        //    return Json(response);
        //}

        public async Task<bool> ChopSession(int sessionId)
        {
            var session = _db.Sessions.Find(sessionId);
            if (session == null) throw new Exception("Kunde inte hitta session.");

            var blobname = session.FileName;
            var startpath = Path.Combine(_hostingEnvironment.WebRootPath, "videos");

            var path = Path.Combine(startpath, blobname);

            var count = 1;
            while (System.IO.File.Exists(path))
            {
                var filetype = blobname.Split('.').LastOrDefault() ?? "mp4";
                var filestart = blobname.Split('-').FirstOrDefault() ?? "video";

                var filename = filestart + count + "." + filetype;

                path = Path.Combine(startpath, filename);
                count++;
            }

            var player = _db.Players.Find(session.PlayerId);
            if (player == null) throw new Exception("Kunde inte hitta spelare.");

            var download = await FileHandler.DownloadBlob(path, blobname, player.RetrieveContainerName());
            if (!download) throw new Exception("Kunde inte ladda ned film.");

            var intervals = session.Targets.Select(t => new DecodeInterval
            {
                startMs = t.TimestampStart,
                endMs = t.TimestampEnd
            }).ToArray();

            var shots = AnalysisBridge.DecodeFrames(path, BlobCredentials.AccountName, BlobCredentials.Key, player.RetrieveContainerName(), intervals);
            if (shots.Any())
            {
                foreach (var s in shots)
                {
                    var target = _db.Targets.FirstOrDefault(shot => shot.SessionId == session.SessionId && shot.Order == s.Shot);

                    if (target == null) continue;
                    foreach (var frame in s.Uris)
                    {
                        var picture = new FrameToAnalyze(target.TargetId, frame);
                        await _db.Frames.AddAsync(picture);
                    }
                }
            }

            session.Chopped = true;

            await _db.SaveChangesAsync();

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return true;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> DeleteVideoFromSession([FromBody]SessionVm vm)
        {
            GeneralResult response;
            if (vm.token == _appkey)
            {
                var session = _db.Sessions.Find(vm.sessionId);

                if (session == null)
                {
                    response = new GeneralResult(false, "Sessionen kunde inte hittas");
                    return Json(response);
                }

                var deleted = FileHandler.DeleteVideo(session.VideoPath, session.Player.RetrieveContainerName());

                if (deleted)
                {
                    session.Delete();
                    await _db.SaveChangesAsync();

                    response = new GeneralResult(true, "Videoklippet raderades");
                    return Json(response);
                }

                response = new GeneralResult(false, "Videoklippet kunde inte raderas");
                return Json(response);
            }

            response = new GeneralResult(false, "Inkorrekt token");
            return Json(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> IsAnalyzed([FromBody]SessionVm vm)
        {
            IsAnalyzedResult response;
            if (vm.token == _appkey)
            {
                var session = _db.Sessions.Find(vm.sessionId);

                if (session == null)
                {
                    response = new IsAnalyzedResult(false, "Sessionen kunde inte hittas", false);
                    return Json(response);
                }

                response = new IsAnalyzedResult(false, "Videoklippet kollat", session.Analyzed);
                return Json(response);
            }

            response = new IsAnalyzedResult(false, "Inkorrekt token", false);
            return Json(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> IsChopped([FromBody]SessionVm vm)
        {
            IsChoppedResult response;
            if (vm.token == _appkey)
            {
                var session = _db.Sessions.Find(vm.sessionId);

                if (session == null)
                {
                    response = new IsChoppedResult(false, "Sessionen kunde inte hittas", false);
                    return Json(response);
                }

                response = new IsChoppedResult(false, "Videoklippet kollat", session.Chopped);
                return Json(response);
            }

            response = new IsChoppedResult(false, "Inkorrekt token", false);
            return Json(response);
        }

        [HttpPost]
		[AllowAnonymous]
		public JsonResult GetFramesFromShot([FromBody]GetTargetFramesVm vm)
        {
			GetFramesFromShotResult response;
			if (vm.token == _appkey)
			{
				var session = _db.Sessions.Find(vm.sessionId);

				if (session == null)
				{
					response = new GetFramesFromShotResult(false, "Sessionen kunde inte hittas");
					return Json(response);
				}

				if (!vm.Validate())
				{
					return Json(vm.gr);
				}

				var shot = _db.Targets.FirstOrDefault(t => t.SessionId == session.SessionId && t.Order == vm.shot);

				if (shot == null)
				{
					response = new GetFramesFromShotResult(false, "Skottet kunde inte hittas.");
					return Json(response);
				}

                response = new GetFramesFromShotResult(true, "Skottets träffpunkter har hämtats!", _db.Frames.Where(f => f.TargetId == shot.TargetId).Select(f => f.FrameUrl).ToList())
                {
                    XCoordinate = shot.XCoordinateAnalyzed,
                    YCoordinate = shot.XCoordinateAnalyzed
                };

				return Json(response);
			}

			response = new GetFramesFromShotResult(false, "Inkorrekt token");
			return Json(response);
        }

		[HttpPost]
		[AllowAnonymous]
		public JsonResult GetDataFromShot([FromBody]GetTargetFramesVm vm)
		{
			GetDataFromShotResult response;
			if (vm.token == _appkey)
			{
				var session = _db.Sessions.Find(vm.sessionId);

				if (session == null)
				{
					response = new GetDataFromShotResult(false, "Sessionen kunde inte hittas");
					return Json(response);
				}

				if (!vm.Validate())
				{
					return Json(vm.gr);
				}

				var shot = _db.Targets.FirstOrDefault(t => t.SessionId == session.SessionId && t.Order == vm.shot);

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
                    YCoordinate = shot.YCoordinate,
                    XCoordinateAnalyzed = shot.XCoordinateAnalyzed,
                    YCoordinateAnalyzed = shot.YCoordinateAnalyzed
                };

				return Json(response);
			}

			response = new GetDataFromShotResult(false, "Inkorrekt token");
			return Json(response);
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<JsonResult> UpdateTargetHit([FromBody]UpdateTargetHitVm vm)
        {
			GeneralResult response;
			if (vm.token == _appkey)
			{
				var session = _db.Sessions.Find(vm.sessionId);

				if (session == null)
				{
					response = new GeneralResult(false, "Sessionen kunde inte hittas");
					return Json(response);
				}

                if (!vm.Validate())
				{
					return Json(vm.ur);
				}

                var shotToUpdate = _db.Targets.FirstOrDefault(t => t.SessionId == session.SessionId && t.Order == vm.shot);

                if (shotToUpdate == null)
                {
					response = new GeneralResult(false, "Skottet som skulle uppdateras kunde inte hittas.");
					return Json(response);
                }

                shotToUpdate.XCoordinate = vm.x;
                shotToUpdate.YCoordinate = vm.y;

                await _db.SaveChangesAsync();

                response = new GeneralResult(true, "Skottets träffpunkt har uppdaterats!");
				return Json(response);
			}

			response = new GeneralResult(false, "Inkorrekt token");
			return Json(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetFramesFromSession([FromBody]SessionVm vm)
        {
            GetFramesResult response;
            if (vm.token == _appkey)
            {
                var session = _db.Sessions.Find(vm.sessionId);

                if (session != null && !session.Deleted)
                {
                    response = new GetFramesResult(true, "Alla frames hämtades", new List<string>());

                    foreach (var t in _db.Targets.Where(tar => tar.SessionId == session.SessionId))
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

        // To be replaced by GetHitsOverviewSVG2. This returns an URL, the other an XML.
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetHitsOverviewSvg(int sessionId, string token)
        {
            var defaultSvgUrl = @"http://hockeylizer.azurewebsites.net/images/hitsOverview.svg";
            if (token != _appkey) return Json(defaultSvgUrl);
            var hitList = _db.Targets.Where(target => target.SessionId == sessionId);
            if (hitList == null || !hitList.Any())
            {
                return Json(defaultSvgUrl);
            }

            var svgBaseDir = _hostingEnvironment.WebRootPath + "/images/";
            var svgDoc = XDocument.Load(svgBaseDir + "hitsOverview.svg");
            var xmlNs = svgDoc.Root.Name.Namespace;

            var fill = new XAttribute("fill", "black");
            var radius = new XAttribute("r", 4);

            foreach (var hit in hitList)
            {
                if (hit.XCoordinate == null || hit.YCoordinate == null || hit.XCoordinateAnalyzed == null ||
                    hit.YCoordinate == null) continue;
                var xCoord = hit.XCoordinate + hit.XCoordinateAnalyzed;
                var yCoord = hit.YCoordinate + hit.YCoordinateAnalyzed;
                svgDoc.Root.Add(new XElement(xmlNs + "circle", fill, radius, xCoord, yCoord));
            }

            // Setup unique filename and write to it
            var timeStr = DateTime.Now.ToString("ddhhmmss");
            var guidStr = Guid.NewGuid().ToString().Substring(0, 7);
            var fileName = "hits" + timeStr + "rnd" + guidStr + ".svg";

            var fs = new FileStream(svgBaseDir + fileName, FileMode.Create);
            svgDoc.Save(fs);

            return Json(@"http://hockeylizer.azurewebsites.net/images/" + fileName);
        }

        // This is the real deal. When GetHitsOverviewSVG has been phased out
        // in the app, this will replace it.
        [HttpPost]
        [AllowAnonymous]
        public ContentResult GetHitsOverviewSvg2(int sessionId, string token)
        {
            var defaultSvgUrl = @"http://hockeylizer.azurewebsites.net/images/hitsOverview.svg";
            if (token != _appkey) return Content("Token var fel");

            var svgBaseDir = _hostingEnvironment.WebRootPath + "/images/";
            var svgDoc = XDocument.Load(svgBaseDir + "hitsOverview.svg");

            var hitList = _db.Targets.Where(target => target.SessionId == sessionId);
            if (hitList == null || !hitList.Any())
            {
                return Content(svgDoc.ToString());
            }
                                
            var xmlNs = svgDoc.Root.Name.Namespace;
            var fill = new XAttribute("fill", "black");
            var radius = new XAttribute("r", 4);

            foreach (var hit in hitList)
            {
                if (hit.XCoordinate == null || hit.YCoordinate == null || hit.XCoordinateAnalyzed == null ||
                    hit.YCoordinate == null) continue;

                var xCoord = hit.XCoordinate + hit.XCoordinateAnalyzed;
                var yCoord = hit.YCoordinate + hit.YCoordinateAnalyzed;
                svgDoc.Root.Add(new XElement(xmlNs + "circle", fill, radius, xCoord, yCoord));
            }

            return Content(svgDoc.ToString());
        }

    }
}