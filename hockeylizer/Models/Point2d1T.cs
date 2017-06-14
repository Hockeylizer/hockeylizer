namespace hockeylizer.Models
{
    /// <summary>
    /// A container for 2D points, together with an intented target int. 2d1T stands for 2 doubles 1 Target
    /// </summary>
    public class Point2d1T : Point2d {
        public Point2d1T(double x, double y) : base(x, y)  {
            this.target = -1;
        }
        public Point2d1T(double x, double y, int target) : base(x, y) {
            this.target = target;
        }
        public override string ToString() {
            return string.Format("[ x = {0:0.##}, y = {1:0.##}, target = {2:d} ]", x, y, target);
        }

        /// <summary>
        /// Returns only the coordinate doubles.
        /// </summary>
        /// <returns></returns>
        public double[] toCoordsArray() {
            return new double[] { x, y };
        }

        //public double x;
        //public double y;
        public int target { get; set; }
    }
}
