using System;
using System.IO;
using System.Linq;
using hockeylizer.Data;
using hockeylizer.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

namespace hockeylizer.Services
{
    public class AnalyzeService : IAnalyzeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AnalyzeService(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            this._db = db;
            this._hostingEnvironment = hostingEnvironment;
        }

        public async void AnalyzeSession(int sessionId)
        {
            var session = _db.Sessions.Find(sessionId);
            if (session == null) throw new Exception("Session hittas inte");

            var blobname = session.FileName;
            var startpath = Path.Combine(_hostingEnvironment.WebRootPath, "videos");

            var path = Path.Combine(startpath, blobname);

            var count = 1;
            while (File.Exists(path))
            {
                var filetype = blobname.Split('.').LastOrDefault() ?? "mp4";
                var filestart = blobname.Split('-').FirstOrDefault() ?? "video";

                var filename = filestart + count + "." + filetype;

                path = Path.Combine(startpath, filename);
                count++;
            }

            var player = _db.Players.Find(session.PlayerId);
            if (player == null) throw new Exception("Spelare hittas inte");

            var download = await FileHandler.DownloadBlob(path, blobname, player.RetrieveContainerName());
            if (!download) throw new Exception("Videoklippet kunde inte laddas ned");

            // Analyze the video

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
                var analysis = AnalysisBridge.AnalyzeShot(t.TimestampStart, t.TimestampEnd, points.ToArray(), width, height, offsets.ToArray(), path);

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

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    public interface IAnalyzeService
    {
        void AnalyzeSession(int sessionId);
    }
}
