namespace hockeylizer.Models
{
    class DecodeInterval
    {
        public DecodeInterval() { }

        public DecodeInterval(int start, int end)
        {
            this.startMs = start;
            this.endMs = end;
        }

        public int startMs;
        public int endMs;
    }
}
