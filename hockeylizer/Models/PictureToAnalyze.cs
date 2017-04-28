using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace hockeylizer.Models
{
    public class PictureToAnalyze
    {
        public PictureToAnalyze()
        {
            this.FilePath = string.Empty;
        }

		public PictureToAnalyze(string path, int videoId)
		{
            this.FilePath = path;
            this.VideoId = videoId;
		}

        [Key]
        public int PictureId { get; set; }
        public string FilePath { get; set; }

        [ForeignKey("Video")]
        public int? VideoId { get; set; }
        public virtual PlayerVideo Video { get; set; }
    }
}
