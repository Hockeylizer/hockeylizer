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
            this.containerId = Guid.NewGuid();
            this.Videos = new HashSet<PlayerVideo>();
            this.Deleted = false;
        }

        public Player(string name, Guid? teamId = null)
        {
            this.Name = name;
            this.TeamId = teamId;
            this.Videos = new HashSet<PlayerVideo>();
            this.containerId = Guid.NewGuid();
            this.Deleted = false;
        }

        public void UpdateContainerId()
        {
            this.containerId = Guid.NewGuid();
        }

        public string RetrieveContainerName()
        {
            return  this.containerId + "-" + this.PlayerId;
        }

        [Key]
        public int PlayerId { get; set; }
        public string Name { get; set; }

        [ForeignKey("Team")]
        public Guid? TeamId { get; set; }
        public virtual AppTeam Team { get; set; }

        public bool Deleted { get; set; }
        public Guid containerId { get; set; }
        public virtual ICollection<PlayerVideo> Videos { get; set; }
    }
}
