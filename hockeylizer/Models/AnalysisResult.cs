using System;
using System.Runtime.InteropServices;

namespace Bridge
{
    // The return value of AnalysisBridge.AnalyzeShot(...)
    [StructLayout(LayoutKind.Sequential)]
    public class AnalysisResult
    {
        // The frame that we think the puck hit the goal
        public int FrameNr
        {
            get { return frameNr; }
        }

        // The pixel we think it hit the goal at.
        public Point2i HitPoint
        {
            get { return new Point2i(pX, pY); }
        }


        private int frameNr;
        private int pX;
        private int pY;
    }
}
