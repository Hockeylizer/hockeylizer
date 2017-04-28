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
            this.Videos = new HashSet<PlayerVideo>();
        }

        public Player(string name)
        {
            this.Name = name;
            this.Videos = new HashSet<PlayerVideo>();
        }

        public string RetrieveContainerName()
        {
            return this.Name.ToLower() + "-" + this.PlayerId;
        }

        [Key]
        public int PlayerId { get; set; }
        public string Name { get; set; }

        [ForeignKey("Team")]
        public Guid? TeamId { get; set; }
        public virtual AppTeam Team { get; set; }

        public virtual ICollection<PlayerVideo> Videos { get; set; }
    }
}
