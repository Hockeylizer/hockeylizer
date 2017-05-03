using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using hockeylizer.Models;
using System;

namespace hockeylizer.Models
{
    public class FrameToAnalyze
    {
        public FrameToAnalyze()
        {
            this.Analyzed = false;
            this.FrameUrl = "";
        }

		public FrameToAnalyze(int targetId, string frameurl)
		{
			this.Analyzed = false;
            this.TargetId = targetId;
            this.FrameUrl = frameurl;
		}

        [Key]
        public int FrameId { get; set; }

        public string FrameUrl { get; set; }
        public bool Analyzed { get; set; }

        [ForeignKey("Target")]
        public int TargetId { get; set; }
        public virtual Target Target { get; set; }
    }
}