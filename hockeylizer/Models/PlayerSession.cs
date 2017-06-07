﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using hockeylizer.Services;
using System;

namespace hockeylizer.Models
{
	public class PlayerSession
	{
		public PlayerSession()
		{
            this.Created = DateTime.Now;
			this.Targets = new HashSet<Target>();

			this.Deleted = false;
			this.VideoRemoved = false;

		    this.Analyzed = false;
		    this.Chopped = false;
        }

		// to = targetorder, ts = timestamps
		public void AddTargets(List<int> to, List<TargetCoordsVm> coords, List<ShotTimestampVm> ts)
		{
			var index = 1;
            var co = coords.Count;

			for (var t = 0; t < ts.Count; t++)
			{
                var target = new Target(to[t % to.Count], index, ts[t].start, ts[t].end, coords[t % (co - 1)].xCoord, coords[t % (co - 1)].yCoord, null, null)
				{
					RelatedSession = this
				};

				index++;

				this.Targets.Add(target);
			}
		}

		public PlayerSession(string path, string name, int playerId, int interval, int rounds, int shots, int noTargets)
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

            this.Analyzed = false;
            this.Chopped = false;

			this.SomethingFailed = false;
            this.DeleteFailed = false;

            this.Created = DateTime.Now;
			this.Targets = new HashSet<Target>();
		}

		public void Delete()
		{
			this.Deleted = true;
			this.VideoPath = string.Empty;
		}

		public void RemoveVideo()
		{
			FileHandler.DeleteVideo(this.VideoPath, this.Player.RetrieveContainerName());

			this.VideoRemoved = true;
			this.VideoPath = string.Empty;
		}

		[Key]
		public int SessionId { get; set; }
        public DateTime Created { get; set; }

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

        public bool Analyzed { get; set; }
	    public bool Chopped { get; set; }

        public bool SomethingFailed { get; set; }
        public string WhySomethingFailed { get; set; }

        public bool DeleteFailed { get; set; }
        public string DeleteFailedWhere { get; set; }

        public virtual ICollection<Target> Targets { get; set; }
	}
}
