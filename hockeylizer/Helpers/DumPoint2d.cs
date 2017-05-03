using System;
using System.Runtime.InteropServices;

namespace hockeylizer.Helpers
{
    // This class is only used internally by the bridge.
    // Shold probably be a private inner class of Bridge 
    [StructLayout(LayoutKind.Sequential)]
    class DumPoint2d
    {
        public double x;
        public double y;
    }
}
