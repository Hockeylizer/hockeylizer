namespace hockeylizer.Models
{
    /// <summary>
    /// A container for 2D points, together with an intented target int. 2d1T stands for 2 doubles 1 Target
    /// </summary>
    public class Point2d1T
    {
        public Point2d1T(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.target = -1;
        }
        public Point2d1T(double x, double y, int target)
        {
            this.x = x;
            this.y = y;
            this.target = target;
        }

        public Point2d toPoint2d()
        {
            return new Point2d(x, y);
        }

        /// <summary>
        /// Returns only the coordinate doubles.
        /// </summary>
        /// <returns></returns>
        public double[] toCoordsArray()
        {
            return new double[] { x, y };
        }

        public double x { get; set; }
        public double y { get; set; }
        public int target { get; set; }
    }
}
