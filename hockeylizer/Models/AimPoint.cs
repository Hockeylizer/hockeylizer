using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace hockeylizer.Models
{
    public class AimPoint
    {
        public AimPoint()
        {
            this.XCoordinate = 0;
            this.YCoordinate = 0;
        }

        public AimPoint(int? x, int? y)
        {
            this.XCoordinate = x;
            this.YCoordinate = y;
        }

        [Key]
        public int AimPointId { get; set; }

        public int? XCoordinate { get; set; }
		public int? YCoordinate { get; set; }

        [ForeignKey("Session")]
        public int SessionId { get; set; }

        public virtual PlayerSession Session { get; set; }
    }
}
