using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace hockeylizer.Models
{
	public class AnalysisResult
	{
        public AnalysisResult() { }

        public AnalysisResult(int targetId)
        {
            this.TargetId = targetId;
        }

        [Key]
		public int AnalysisId { get; set; }

        // Detta kommer vara en individuell point

        // Lägg till resten av allt
        public bool HitGoal { get; set; }

        [ForeignKey("Target")]
        public int TargetId { get; set; }
        public virtual Target Target { get; set; }
    }
}
