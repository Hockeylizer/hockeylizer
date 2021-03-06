using System;
using System.Runtime.InteropServices;

namespace hockeylizer.Models
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

        public int VideoFrameNr
        {
            get { return frameNr + first_frame; }
        }

        // The pixel we think it hit the goal at.
        public Point2i HitPoint
        {
            get { return new Point2i(pX, pY); }
        }

        // Positive y is up.
        public Point2d OffsetFromTarget
        {
            get { return new Point2d(offsetX, offsetY); }
        }

        public bool DidHitGoal
        {
            get { return hit; }
        }

        // If there was an error only ErrorMessage has sane values
        public bool WasErrors
        {
            get { return error; }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
        }

        public int LastFrame
        {
            get { return last_frame; }
        }
        public int FirstFrame
        {
            get { return first_frame; }
        }

        private bool error;
        private string errorMessage;
        private bool hit;
        private int frameNr;
        private int pX;
        private int pY;
        private double offsetX;
        private double offsetY;
        private int first_frame;
        private int last_frame;
    }
}
