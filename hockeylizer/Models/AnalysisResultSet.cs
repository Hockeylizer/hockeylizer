/* Hittades på av Daniel. Tanken är att det är en lista med
 * alla individuella analysresultat för en given video.
 */
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace hockeylizer.Models
{
	public class AnalysisResult
	{
        public AnalysisResult()
        {
            
        }

		//public AnalysisResult(int videoId)
		//{
  //          this.VideoId = videoId;
		//}

		[Key]
		public int AnalysisId { get; set; }

        // Detta kommer vara en individuell point

        // Lägg till resten av allt
        public bool HitGoal { get; set; }

        //[ForeignKey("Video")]
        //public int? VideoId { get; set; }
        //public virtual PlayerVideo Video { get; set; }
	}
}
