namespace hockeylizer.Models
{
    public class Point2d
    {
        public Point2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double norm()
        {
            return System.Math.Sqrt(x * x + y * x);
        }

        public double x;
        public double y;
    }
}
