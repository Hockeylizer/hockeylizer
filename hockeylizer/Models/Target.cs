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

            this.AnalysisFailed = false;
            this.ChopFailed = false;

            this.HitGoal = false;
            this.ManuallyAnalyzed = false;

            this.FramesToAnalyze = new HashSet<FrameToAnalyze>();
        }

        [Key]
        public int TargetId { get; set; }
        public int TargetNumber { get; set; }
        public int Order { get; set; }

        public int TimestampStart { get; set; }
        public int TimestampEnd { get; set; }

        public double? XCoordinate { get; set; }
        public double? YCoordinate { get; set; }

        public double? XOffset { get; set; }
        public double? YOffset { get; set; }

        public double? XCoordinateAnalyzed { get; set; }
        public double? YCoordinateAnalyzed { get; set; }

        public int? FrameHit { get; set; }
        public int? RealFrameHit { get; set; }

        public bool HitGoal { get; set; }
        public bool ManuallyAnalyzed { get; set; }

        public bool AnalysisFailed { get; set; }
		public string AnalysisFailedReason { get; set; }

		public bool ChopFailed { get; set; }
		public string ChopFailedReason { get; set; }

        [ForeignKey("RelatedSession")]
        public int SessionId { get; set; }
        public virtual PlayerSession RelatedSession { get; set; }

        public virtual ICollection<FrameToAnalyze> FramesToAnalyze { get; set; }
    }
}
