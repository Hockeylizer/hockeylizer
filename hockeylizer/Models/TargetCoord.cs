using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace hockeylizer.Models
{
    public class TargetCoord
    {
        public TargetCoord()
        {
            this.xCoord = 0;
            this.yCoord = 0;
        }

        public TargetCoord(int? xc, int? yc)
        {
            this.xCoord = xc;
            this.yCoord = yc;
        }

        [Key]
        public int CoordinateId { get; set; }

        [ForeignKey("Video")]
        public int VideoId { get; set; }
        public virtual PlayerVideo Video { get; set; }

        public int? xCoord { get; set; }
        public int? yCoord { get; set; }
    }
}
