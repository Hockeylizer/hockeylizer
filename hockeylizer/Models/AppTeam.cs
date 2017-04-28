using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using hockeylizer.Models;
using System;

namespace hockeylizer.Models
{
    public class AppTeam
    {
        public AppTeam()
        {
            this.TeamId = new Guid();
            this.Players = new HashSet<Player>();
        }

        public AppTeam(Guid teamId)
        {
            this.TeamId = teamId;
            this.Players = new HashSet<Player>();
        }

        [Key]
        public Guid TeamId { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}
