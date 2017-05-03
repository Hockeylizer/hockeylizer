using System;
using System.Collections.Generic;
using System.Text;

namespace Bridge
{
    class DecodeInterval
    {
        public DecodeInterval(int start, int end)
        {
            this.startMs = start;
            this.endMs = end;
        }
        public int startMs;
        public int endMs;
    }
}
