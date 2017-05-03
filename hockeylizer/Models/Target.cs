using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace hockeylizer.Models
{
    public class Target
    {
        public Target()
        {
            this.FramesToAnalyze = new HashSet<FrameToAnalyze>();
        }

        public Target(int targetNumber, int order, long? tsStart, long? tsEnd, int? xc, int? yc)
        {
            this.TargetNumber = targetNumber;
            this.Order = order;
            this.FramesToAnalyze = new HashSet<FrameToAnalyze>();
        }

        public void AddFrames(List<string> urls)
        {
            foreach (var url in urls)
            {
                var frame = new FrameToAnalyze();

            }
        }

        [Key]
        public int TargetId { get; set; }
        public int TargetNumber { get; set; }
        public int Order { get; set; }

        public long? TimestampStart { get; set; }
        public long? TimestampEnd { get; set; }

        public int? XCoordinate { get; set; }
        public int? YCoordinate { get; set; }

        [ForeignKey("RelatedSession")]
        public int SessionId { get; set; }
        public virtual PlayerSession RelatedSession { get; set; }

        //[ForeignKey("Analysis")]
        //public int? AnalysisResultId { get; set; }
        //public virtual AnalysisResult Analysis { get; set; }

        public virtual ICollection<FrameToAnalyze> FramesToAnalyze { get; set; }
    }
}
