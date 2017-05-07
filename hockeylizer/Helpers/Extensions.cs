using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using hockeylizer.Services;
using hockeylizer.Models;
using Hangfire.Dashboard;
using hockeylizer.Data;
using System.Linq;
using System.IO;
using System;

namespace hockeylizer.Helpers
{
    public static class Extensions
    {
        public static Guid GetAvailableTeamId(this ApplicationDbContext db)
        {
            var teamId = new Guid();

            while (db.AppTeams.Find(teamId) != null)
            {
                teamId = new Guid();
            }

            return teamId;
        }
    }

    public class HangfireAuth : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			var httpContext = context.GetHttpContext();

			// Allow all authenticated users to see the Dashboard (potentially dangerous).
			return httpContext.User.Identity.IsAuthenticated;
		}
	}

	public class ChopAlyze : IChopService
	{
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

		public ChopAlyze(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
		{
			this._db = db;
            this._hostingEnvironment = hostingEnvironment;
		}

		public async void ChopAlyzeSession(int sessionId)
		{
			var session = _db.Sessions.Find(sessionId);
			if (session == null) throw new Exception("Session hittas inte");

			var blobname = session.FileName;
			var path = Path.Combine(_hostingEnvironment.WebRootPath, "videos");
			path = Path.Combine(path, blobname);

			var player = _db.Players.Find(session.PlayerId);
			if (player == null) throw new Exception("Spelare hittas inte");

			var download = await FileHandler.DownloadBlob(path, blobname, player.RetrieveContainerName());
			if (!download) throw new Exception("Videoklippet kunde inte laddas ned");

			// Analyze the video

			var targets = _db.Targets.Where(shot => shot.SessionId == sessionId).ToArray();

		    var pointDict = new Dictionary<int, Point2d>
		    {
		        {1, new Point2d(10, 91)},
		        {2, new Point2d(10, 18)},
		        {3, new Point2d(173, 18)},
		        {4, new Point2d(173, 91)},
		        {5, new Point2d(91.5, 101)}
		    };

            var points = new Point2i[] { };
			var offsets = new Point2d[] { };

		    for (var hp = 0; hp < targets.Length; hp++)
			{
				points[hp] = new Point2i(targets[hp].XCoordinate ?? 0, targets[hp].YCoordinate ?? 0);

				Point2d coordinates;
				var shot = targets[hp].Order;

				if (pointDict.ContainsKey(shot))
				{
					coordinates = pointDict[shot];
				}
				else if (pointDict.ContainsKey(hp % 5))
				{
					coordinates = pointDict[hp % 5];
				}
				else
				{
					coordinates = new Point2d(0, 0);
				}

				offsets[hp] = coordinates;
			}

			const int width = 183;
			const int height = 122;

			foreach (var t in targets)
			{
				var analysis = AnalysisBridge.AnalyzeShot(t.TimestampStart, t.TimestampEnd, points, width, height, offsets, path);

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

			// End of analysis

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

				await _db.SaveChangesAsync();
			}

			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
	}

	public interface IChopService
	{
        void ChopAlyzeSession(int sessionId);
	}
}
