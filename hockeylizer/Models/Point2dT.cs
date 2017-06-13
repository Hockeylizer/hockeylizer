namespace hockeylizer.Models
{
    /// <summary>
    /// A container for 2D points, together with an intented target int. 
    /// </summary>
    public class Point2dT
    {
        public Point2dT(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.target = -1;
        }
        public Point2dT(double x, double y, int target)
        {
            this.x = x;
            this.y = y;
            this.target = target;
        }

        public double x { get; set; }
        public double y { get; set; }
        public int target { get; set; }
    }
}
