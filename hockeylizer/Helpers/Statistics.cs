using System.Collections.Generic;
using hockeylizer.Models;
using System.Linq;
using System;

namespace hockeylizer.Helpers
{
    public static class Statistics
    {
        public static Dictionary<int, Point2d> TargetCoords { get; }
            = new Dictionary<int, Point2d> {{ 1, new Point2d(10, 91)  },
                                            { 2, new Point2d(10, 18)  },
                                            { 3, new Point2d(173, 18) },
                                            { 4, new Point2d(173, 91) },
                                            { 5, new Point2d(91.5, 101)} };

        /// <summary></summary>
        /// <param name="sorted_nums">Must be sorted and non-empty.</param>
        /// <returns>The median of the values in sorted_nums</returns>
        public static double medianOnSorted(List<double> sorted_nums)
        {
            if (sorted_nums == null) throw new ArgumentNullException("sorted_nums");
            if (!sorted_nums.Any()) throw new InvalidOperationException("sorted_nums");
            int len = sorted_nums.Count;
            if (len % 2 == 1) return sorted_nums[(len - 1) / 2];
            else return (sorted_nums[len / 2 - 1] + sorted_nums[len / 2]) / 2;
        }

        /// <summary></summary>
        /// <param name="nums">Cannot be empty.</param>
        /// <returns></returns>
        public static double median(List<double> nums)
        {
            if (nums == null) throw new ArgumentNullException("nums");
            if (!nums.Any()) throw new InvalidOperationException("nums");
            nums.Sort();
            return medianOnSorted(nums);
        }

        public static double mean(IEnumerable<double> nums)
        {
            return nums.Average();
        }

        /// <summary>
        /// Returns the sum of (x_i - e) where x_i are the doubles in nums
        /// and e is the arithmetic mean of nums.
        /// </summary>
        /// <param name="nums">Cannot be empty.</param>
        /// <param name="expectedValue"></param>
        /// <returns></returns>
        public static double squaredErrorSum(IEnumerable<double> nums)
        {
            if (nums == null) throw new ArgumentNullException("nums");
            if (!nums.Any()) throw new InvalidOperationException("nums");
            var e = nums.Average();
            return squaredErrorSum(nums, e);
        }

        /// <summary>
        /// Returns the sum of (x_i - expectedValue) where x_i are the doubles in nums.
        /// </summary>
        /// <param name="nums">Cannot be empty.</param>
        /// <param name="expectedValue"></param>
        /// <returns></returns>
        public static double squaredErrorSum(IEnumerable<double> nums, double expectedValue)
        {
            if (nums == null) throw new ArgumentNullException("nums");
            if (!nums.Any()) throw new InvalidOperationException("nums");
            // The lambda argument applies that function to each element before summing.
            return nums.Sum((num => (num - expectedValue) * (num - expectedValue)));
        }

        /// <summary>
        /// Returns 0 for singleton, since this will probably be expected by pöbeln.
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static double variance(IEnumerable<double> nums)
        {
            if (nums == null) throw new ArgumentNullException("nums");
            if (!nums.Any()) throw new InvalidOperationException("nums");

            if (nums.Count() == 1) return 0;

            return squaredErrorSum(nums) / (nums.Count() - 1);
        }
        public static double standardDeviation(IEnumerable<double> nums)
        {
            if (nums == null) throw new ArgumentNullException("nums");
            if (!nums.Any()) throw new InvalidOperationException("nums");
            return Math.Sqrt(variance(nums));
        }

        /// <summary>
        /// For a given array length and percentage, calculates the smallest index such that at least that percentage is at,
        /// or before, the index. A percentage of 0 will return index 0 for convenience, even though it is undefined.
        /// </summary>
        /// <param name="numberOfItems">Must be a positive integer.</param>
        /// <param name="quantile">Muste be in the closed interval [0,1].</param>
        /// <returns>The smallest index such that at least a 'quantile'-part of the items is at or before that index.</returns>
        public static int quantileIndexLeft(int numberOfItems, double quantile)
        {
            if (numberOfItems <= 0) throw new ArgumentOutOfRangeException("numberOfItems", "Must be positive.");
            if (quantile < 0 || 1 < quantile) throw new ArgumentOutOfRangeException("quantile", "Must be in the interval [0, 1].");
            else return (int)Math.Floor(quantile * (numberOfItems - 1));
        }
        /// <summary>
        /// For a given array length and percentage, calculates the largest index such that at least that percentage is at,
        /// or after, the index. A percentage of 0 will return index the last index for convenience, even though it is formally undefined.
        /// </summary>
        /// <param name="numberOfItems">Must be a positive integer.</param>
        /// <param name="quantile">Muste be in the closed interval [0,1].</param>
        /// <returns>The largest index such that at least a 'quantile'-part of the items is at or after that index.</returns>
        public static int quantileIndexRight(int numberOfItems, double quantile)
        {
            if (numberOfItems <= 0) throw new ArgumentOutOfRangeException("numberOfItems", "Must be positive.");
            if (quantile < 0 || 1 < quantile) throw new ArgumentOutOfRangeException("quantile", "Must be in the interval [0, 1].");
            else return (int)Math.Ceiling((1 - quantile) * (numberOfItems - 1));
        }

        /// <summary>
        /// Creates a CSV-string (comma separated values, but semicolons ';' are used, not commas.)
        /// from the matrix. Each row is  a line and each column on a row is one value.
        /// </summary>
        /// <param name="matrix">You probably don't want the entries to contain semicolons.</param>
        /// <returns></returns>
        public static string matrixToCSV(string[][] matrix)
        {
            return matrixToCSV(matrix, ";");
        }

        /// <summary>
        /// Creates a CSV-string (comma separated values, but the separator paremeter is used, not commas.)
        /// from the matrix. Each row is  a line and each column on a row is one value.
        /// </summary>
        /// <param name="matrix">You probably don't want the entries to contain the separator.</param>
        /// <returns></returns>
        public static string matrixToCSV(string[][] matrix, string separator)
        {
            if (matrix == null) return "";
            if (separator == null) throw new ArgumentNullException("separator");
            if (separator == "") throw new ArgumentException("separator", "Cannot be an empty string.");

            var csv = new System.Text.StringBuilder();
            foreach (string[] row in matrix) csv.AppendLine(String.Join(separator, row));
            return csv.ToString().TrimEnd('\r', '\n');
        }

        /* Uses divide-and-conquer method to minimise precision loss.
         * For even less precision loss, in some cases, sort nums first.
         */
        private static double lowPrecisionLossSum(IEnumerable<double> nums, Func<double, double> lambda)
        {
            if (nums == null || !nums.Any()) { Console.WriteLine(0); return 0; }
            if (nums.Count() == 1) { Console.WriteLine(lambda(nums.Single())); return lambda(nums.Single()); }

            var firstHalf = nums.Take(nums.Count() / 2);
            var secondHalf = nums.Skip(nums.Count() / 2);
            var sum1 = lowPrecisionLossSum(firstHalf, lambda);
            var sum2 = lowPrecisionLossSum(secondHalf, lambda);

            return sum1 + sum2;
        }
    }
}
