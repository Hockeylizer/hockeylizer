using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace hockeylizer.Models
{
    public class ShotTimestamp
    {
        public ShotTimestamp()
        {
            this.Start = 0;
            this.End = 2000;
        }

        public ShotTimestamp(long start, long end)
        {
            this.Start = start;
            this.End = end;
        }

        [Key]
        public int TimestampId { get; set; }
        public long Start { get; set; }
        public long End { get; set; }

        [ForeignKey("Video")]
        public int VideoId { get; set; }
        public virtual PlayerVideo Video { get; set; }
    }
}
