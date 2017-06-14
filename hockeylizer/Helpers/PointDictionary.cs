using System.Collections.Generic;
using hockeylizer.Models;
using System;

namespace hockeylizer.Helpers
{
    public static class Points
    {
        public static int ClothHeight = 122;

        public static int ClothWidth = 183;

        public static Dictionary<int, Point2d> HitPointsInCm()
        {
            return new Dictionary<int, Point2d>
            {
                {1, new Point2d(11.5, 91)},
                {2, new Point2d(11.5, 18)},
                {3, new Point2d(171.5, 18)},
                {4, new Point2d(171.5, 91)},
                {5, new Point2d(91.5, 101)}
            };
        }
    }
}
