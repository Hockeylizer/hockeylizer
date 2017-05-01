﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using hockeylizer.Services;

namespace hockeylizer.Models
{
    public class PlayerVideo
    {
        private PlayerVideo()
        {
            this.Targets = new HashSet<Target>();
            this.Deleted = false;
            this.VideoRemoved = false;
        }

        // to = targetorder, ts = timestamps
        public void AddTargets(List<int> to, List<TargetCoordsVm> coords, List<ShotTimestampVm> ts) 
        {
			var index = 1;
			for (var t = 0; t < to.Count; t++)
			{
				var target = new Target(to[t], index, ts[t].Start, ts[t].End, coords[t].xCoord, coords[t].yCoord)
				{
					RelatedVideo = this
				};
				index++;

				this.Targets.Add(target);
			}
        }

        public PlayerVideo(string path, string name, int playerId, int interval, int rounds, int shots, int noTargets)
        {
            this.VideoPath = path;
            this.FileName = name;
            this.PlayerId = playerId;

            this.Interval = interval;
            this.Rounds = rounds;

            this.Shots = shots;
            this.NumberOfTargets = noTargets;

            this.Deleted = false;
            this.VideoRemoved = false;

            this.Targets = new HashSet<Target>();
        }

        public void Delete() 
        {
            this.Deleted = true;
            this.VideoPath = string.Empty;
        }

		public void RemoveBlob()
		{
            FileHandler.DeleteVideo(this.VideoPath, this.Player.RetrieveContainerName());

			this.VideoRemoved = true;
			this.VideoPath = string.Empty;
		}

        [Key]
        public int VideoId { get; set; }

        public string VideoPath { get; set; }
        public string FileName { get; set; }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public int Interval { get; set; }
        public int Rounds { get; set; }

        public int Shots { get; set; }
        public int NumberOfTargets { get; set; }

        public bool Deleted { get; set; }
        public bool VideoRemoved { get; set; }

        public virtual ICollection<Target> Targets { get; set; }
    }
}
