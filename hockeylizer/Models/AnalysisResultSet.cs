/* Hittades på av Daniel. Tanken är att det är en lista med
 * alla individuella analysresultat för en given video.
 */

using System;
using System.Runtime.InteropServices;

namespace idk //TEMP Vad ska stå här?
{
    // The return value of AnalysisBridge.AnalyzeShot(...)
    [StructLayout(LayoutKind.Sequential)]
    public class AnalysisResultSet
    {
        //an AnalysisResult has: (int FrameNr, Point2i HitPoint, Point2d OffsetFromTarget, bool DidHitGoal)
        public List<AnalysisResult> analysisResultList { get; set; }
        public int numberOfHits { get; set; }
        public int VideoId { get; set; }
    }

}
