using System.Collections.Generic;

namespace hockeylizer.Models
{
    public class GeneralResult
    {
        public GeneralResult()
        {
            this.Completed = false;
            this.Desctiption = "Okänt fel";
        }

        public GeneralResult(bool comp, string desc)
        {
            this.Completed = comp;
            this.Desctiption = desc;
        }

        public bool Completed { get; set; }
        public string Desctiption { get; set; }
    }

    public class ShotTimestampVm
    {
        public long Start { get; set; }
        public long End { get; set; }
    }

    public class AddPlayerResult
    {
        public AddPlayerResult()
        {
            this.Description = "Kunde inte lägga till, okänt fel uppstod";
            this.Completed = false;
        }

        public AddPlayerResult(string desc, bool comp)
        {
            this.Description = desc;
            this.Completed = comp;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
    }

    public class PlayerVmSmall
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
    }

    public class GetPlayersResult
    {
        public GetPlayersResult()
        {
            this.Completed = false;
            this.Description = "Ett fel uppstod";
            this.Players = new List<PlayerVmSmall>();
        }

        public GetPlayersResult(bool comp, string desc, List<PlayerVmSmall> ps)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Players = ps;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
        public List<PlayerVmSmall> Players { get; set; }
    }

    public class VideoVmSmall
    {
        public int VideoId { get; set; }
        public string VideoPath { get; set; }

        public int? Interval { get; set; }
        public int? Rounds { get; set; }
        public int? Shots { get; set; }
        public int? NumberOfTargets { get; set; }

        public List<ShotTimestamp> Timestamps { get; set; }
        public List<Target> Targets { get; set; }
    }

    public class GetVideosResult
    {
        public GetVideosResult()
        {
            this.Completed = false;
            this.Description = "Ett fel uppstod";
            this.Videos = new List<VideoVmSmall>();
        }

        public GetVideosResult(bool comp, string desc, List<VideoVmSmall> vs)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Videos = vs;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
        public List<VideoVmSmall> Videos { get; set; }
    }

    public class GetFramesResult
    {
        public GetFramesResult()
        {
            this.Completed = false;
            this.Description = "Ett fel uppstod";
            this.Images = new List<string>();
        }

        public GetFramesResult(bool comp, string desc, List<string> imgs)
        {
            this.Completed = comp;
            this.Description = desc;
            this.Images = imgs;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
    }

    public class VideoResult
    {
        public VideoResult()
        {
            this.Description = "Kunde inte lägga till, okänt fel uppstod";
            this.Completed = false;
        }

        public VideoResult(string desc, bool comp)
        {
            this.Description = desc;
            this.Completed = comp;
        }

        public bool Completed { get; set; }
        public string Description { get; set; }
    }

    public class TargetCoordsVm
    {
        public int? xCoord { get; set; }
        public int? yCoord { get; set; }
    }
}
