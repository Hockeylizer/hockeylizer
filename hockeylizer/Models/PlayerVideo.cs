﻿using System.ComponentModel.DataAnnotations.Schema;
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
            this.TargetCoords = new HashSet<TargetCoord>();

            this.Deleted = false;
        }

        public void AddTimeStamps(List<ShotTimestampVm> timestamps)
        {
            foreach (var ts in timestamps)
            {
                var timestamp = new ShotTimestamp(ts.Start, ts.End)
                {
                    Video = this
                };

                this.Timestamps.Add(timestamp);
            }
        }

        public void AddTargets(List<int> targetOrder) 
        {
			var index = 1;
			foreach (var t in targetOrder)
			{
				var target = new Target(t, index)
				{
					RelatedVideo = this
				};
				index++;

				this.Targets.Add(target);
			}
        }

        public void AddTargetCoordinates(List<TargetCoordsVm> targetCoordinates)
        {
			foreach (var tc in targetCoordinates)
			{
				var targetCoordinate = new TargetCoord(tc.xCoord, tc.yCoord)
				{
					Video = this
				};

				this.TargetCoords.Add(targetCoordinate);
			}
        }

        public PlayerVideo(string path, int playerId, int interval, int rounds, int shots, int noTargets)
        {
            this.VideoPath = path;
            this.PlayerId = playerId;

            this.Interval = interval;
            this.Rounds = rounds;

            this.Shots = shots;
            this.NumberOfTargets = noTargets;

            this.Deleted = false;

            this.Targets = new HashSet<Target>();
            this.Timestamps = new HashSet<ShotTimestamp>();
            this.TargetCoords = new HashSet<TargetCoord>();
        }

        public void Delete() 
        {
            this.Deleted = true;
            this.VideoPath = string.Empty;
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

        public bool Deleted { get; set; }

        public virtual ICollection<Target> Targets { get; set; }
        public virtual ICollection<ShotTimestamp> Timestamps { get; set; }
        public virtual ICollection<TargetCoord> TargetCoords { get; set; }
    }
}
