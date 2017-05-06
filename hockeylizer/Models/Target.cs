using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace hockeylizer.Models
{
    public class Target
    {
        public Target()
        {
            this.HitGoal = false;
            this.FramesToAnalyze = new HashSet<FrameToAnalyze>();
        }

        public Target(int targetNumber, int order, int tsStart, int tsEnd, int? xc, int? yc, int? xcA, int? ycA)
        {
            this.TargetNumber = targetNumber;
            this.Order = order;

            this.TimestampStart = tsStart;
            this.TimestampEnd = tsEnd;

            this.XCoordinate = xc;
            this.YCoordinate = yc;

            this.XCoordinateAnalyzed = xcA;
            this.YCoordinateAnalyzed = ycA;
            this.HitGoal = false;

            this.FramesToAnalyze = new HashSet<FrameToAnalyze>();
        }

        [Key]
        public int TargetId { get; set; }
        public int TargetNumber { get; set; }
        public int Order { get; set; }

        public int TimestampStart { get; set; }
        public int TimestampEnd { get; set; }

        public int? XCoordinate { get; set; }
        public int? YCoordinate { get; set; }

        public int? XCoordinateAnalyzed { get; set; }
        public int? YCoordinateAnalyzed { get; set; }
        public bool HitGoal { get; set; }

        [ForeignKey("RelatedSession")]
        public int SessionId { get; set; }
        public virtual PlayerSession RelatedSession { get; set; }

        public virtual ICollection<FrameToAnalyze> FramesToAnalyze { get; set; }
    }
}
