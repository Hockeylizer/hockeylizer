using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace hockeylizer.Models
{
    public class Target
    {
        public Target() { }

        public Target(int targetNumber, int order, long? tsStart, long? tsEnd, int? xc, int? yc)
        {
            this.TargetNumber = targetNumber;
            this.Order = order;
        }

        [Key]
        public int TargetId { get; set; }
        public int TargetNumber { get; set; }
        public int Order { get; set; }

        public long? TimestampStart { get; set; }
        public long? TimestampEnd { get; set; }

        public int? XCoordinate { get; set; }
        public int? YCoordinate { get; set; }

        [ForeignKey("RelatedVideo")]
        public int VideoId { get; set; }
        public virtual PlayerVideo RelatedVideo { get; set; }
    }
}
