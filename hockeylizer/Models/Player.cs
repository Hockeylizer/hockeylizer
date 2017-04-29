﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace hockeylizer.Models
{
    public class Player
    {
        public Player()
        {
            this.containerId = new Guid();
            this.Videos = new HashSet<PlayerVideo>();
        }

        public Player(string name, Guid? teamId = null)
        {
            this.Name = name;
            this.TeamId = teamId;
            this.Videos = new HashSet<PlayerVideo>();
            this.containerId = new Guid();
        }

        public string RetrieveContainerName()
        {
            if (this.containerId == null)
            {
                this.containerId = new Guid();
            }

            return  this.containerId + "-" + this.PlayerId;
        }

        [Key]
        public int PlayerId { get; set; }
        public string Name { get; set; }

        [ForeignKey("Team")]
        public Guid? TeamId { get; set; }
        public virtual AppTeam Team { get; set; }

        private Guid containerId { get; set; }
        public virtual ICollection<PlayerVideo> Videos { get; set; }
    }
}
