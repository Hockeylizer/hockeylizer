using System;
using System.IO;
using System.Linq;
using hockeylizer.Data;
using hockeylizer.Models;
using Microsoft.AspNetCore.Hosting;

namespace hockeylizer.Services
{
    public class ChopService : IChopService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ChopService(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            this._db = db;
            this._hostingEnvironment = hostingEnvironment;
        }

        public async void ChopSession(int sessionId)
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
                        _db.Frames.Add(picture);
                    }
                }
            }

            session.Chopped = true;

            await _db.SaveChangesAsync();

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

    public interface IChopService
    {
        void ChopSession(int sessionId);
    }
}
