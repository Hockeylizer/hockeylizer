using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace hockeylizer.Models
{
    public class ShotTimestamp
    {
        public ShotTimestamp()
        {
            this.start = 0;
            this.end = 2000;
        }

        public ShotTimestamp(long start, long end)
        {
            this.start = start;
            this.end = end;
        }

        [Key]
        public int TimestampId { get; set; }
        public long start { get; set; }
        public long end { get; set; }

        [ForeignKey("Video")]
        public int VideoId { get; set; }
        public virtual PlayerVideo Video { get; set; }
    }
}
