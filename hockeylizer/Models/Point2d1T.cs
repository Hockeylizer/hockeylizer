using System;

namespace hockeylizer.Models
{
    /// <summary>
    /// A container for 2D points, together with an intented target int. 2d1T stands for 2 doubles 1 Target
    /// </summary>
    public class Point2d1T : Point2d {
        public Point2d1T(double x, double y, int target) : base(x, y) {
            if (target < 1 || 5 < target) throw new ArgumentOutOfRangeException("Target must be in the range 1-5.");
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

        public int target { get; set; }
    }
}
