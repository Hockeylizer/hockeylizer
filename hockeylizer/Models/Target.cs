using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace hockeylizer.Models
{
    public class Target
    {
        public Target() { }

        public Target(int targetNumber, int order)
        {
            this.TargetNumber = targetNumber;
            this.Order = order;
        }

        [Key]
        public int TargetId { get; set; }
        public int TargetNumber { get; set; }
        public int Order { get; set; }

        [ForeignKey("RelatedVideo")]
        public int VideoId { get; set; }
        public virtual PlayerVideo RelatedVideo { get; set; }
    }
}
