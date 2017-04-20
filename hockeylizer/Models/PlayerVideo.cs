using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace hockeylizer.Models
{
    public class PlayerVideo
    {
        private PlayerVideo()
        {
            this.Targets = new HashSet<Target>();
            this.Timestamps = new HashSet<ShotTimestamp>();
        }

        public PlayerVideo(string path, int playerId, int interval, int rounds, int shots, int noTargets)
        {
            this.VideoPath = path;
            this.PlayerId = playerId;
            this.Timestamps = new HashSet<ShotTimestamp>();

            this.Interval = interval;
            this.Rounds = rounds;

            this.Shots = shots;
            this.NumberOfTargets = noTargets;

            this.Targets = new HashSet<Target>();
            this.Timestamps = new HashSet<ShotTimestamp>();
        }

        [Key]
        public int VideoId { get; set; }
        public string VideoPath { get; set; }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public int Interval { get; set; }
        public int Rounds { get; set; }

        public int Shots { get; set; }
        public int NumberOfTargets { get; set; }

        public virtual ICollection<Target> Targets { get; set; }
        public virtual ICollection<ShotTimestamp> Timestamps { get; set; }
    }
}
