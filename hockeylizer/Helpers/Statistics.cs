using System.Collections.Generic;
using hockeylizer.Models;
using System.Linq;
using System;

namespace hockeylizer.Helpers
{
    public static class Statistics
    {
        /*o=================================================================o
          |                    PUCK CALCULATION METHODS                     |
          o=================================================================o*/

        public static Dictionary<int, Point2d> TargetCoords { get; }
            = new Dictionary<int, Point2d> {{ 1, new Point2d(10, 91)  },
                                            { 2, new Point2d(10, 18)  },
                                            { 3, new Point2d(173, 18) },
                                            { 4, new Point2d(173, 91) },
                                            { 5, new Point2d(91.5, 101)} };

        public static IEnumerable<Point2d1T> offsetToAbsolute(IEnumerable<Point2d1T> offsets)
        {
            foreach (var p in offsets) {
                if (p.target < 1 || 5 < p.target) throw new InvalidOperationException("A offset target is not in the range 1-5.");
            }
            return offsets.Select(p =>  offsetToAbsolute(p) );
        }

        public static Point2d1T offsetToAbsolute(Point2d1T offset)
        {
            if (offset.target < 1 || 5 < offset.target) throw new InvalidOperationException("offset.target is not in the range 1-5.");
            return new Point2d1T(TargetCoords[offset.target].x + offset.x, TargetCoords[offset.target].y - offset.y, offset.target);
        }

        public static double norm(double x, double y) {
            return System.Math.Sqrt(x * x + y * y);
        }

        /*o=================================================================o
          |                       STATISTICS METHODS                        |
          o=================================================================o*/

        /// <summary></summary>
        /// <param name="sorted_nums">Must be sorted and non-empty.</param>
        /// <returns>The median of the values in sorted_nums</returns>
        public static double medianOnSorted(List<double> sorted_nums)
        {
            if (sorted_nums == null) throw new ArgumentNullException("sorted_nums is null.");
            if (!sorted_nums.Any()) throw new InvalidOperationException("sorted_nums cannot be empty.");
            int len = sorted_nums.Count;
            if (len % 2 == 1) return sorted_nums[(len - 1) / 2];
            else return (sorted_nums[len / 2 - 1] + sorted_nums[len / 2]) / 2;
        }

        /// <summary></summary>
        /// <param name="nums">Cannot be empty.</param>
        /// <returns></returns>
        public static double median(List<double> nums)
        {
            if (nums == null) throw new ArgumentNullException("nums is null.");
            if (!nums.Any()) throw new InvalidOperationException("nums cannot be empty.");
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
            if (nums == null) throw new ArgumentNullException("nums is null.");
            if (!nums.Any()) throw new InvalidOperationException("nums cannot be empty.");
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
            if (nums == null) throw new ArgumentNullException("nums is null.");
            if (!nums.Any()) throw new InvalidOperationException("nums cannot be empty.");
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
            if (nums == null) throw new ArgumentNullException("nums is null.");
            if (!nums.Any()) throw new InvalidOperationException("nums cannot be empty.");

            if (nums.Count() == 1) return 0;

            return squaredErrorSum(nums) / (nums.Count() - 1);
        }
        public static double standardDeviation(IEnumerable<double> nums)
        {
            if (nums == null) throw new ArgumentNullException("nums is null.");
            if (!nums.Any()) throw new InvalidOperationException("nums cannot be empty.");
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

        /*o=================================================================o
          |                    REPORT GENERATION METHODS                    |
          o=================================================================o*/

        /// <summary>
        /// Returns the double as a string, formatted with exactly one decimal, dot (not comma) as
        /// decimal delimiter, and no separator between groups of three digits.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string cmToString(double num)
        {
            return num.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates a CSV-string (comma separated values, but semicolons ';' are used, not commas.)
        /// from the matrix. Each row is  a line and each column on a row is one value.
        /// </summary>
        /// <param name="matrix">You probably don't want the entries to contain semicolons.</param>
        /// <returns></returns>
        public static string matrixToCSV(IEnumerable<IEnumerable<string>> matrix)
        {
            return matrixToCSV(matrix, ";");
        }

        /// <summary>
        /// Creates a CSV-string (comma separated values, but the separator paremeter is used, not commas.)
        /// from the matrix. Each row is  a line and each column on a row is one value.
        /// </summary>
        /// <param name="matrix">You probably don't want the entries to contain the separator.</param>
        /// <returns></returns>
        public static string matrixToCSV(IEnumerable<IEnumerable<string>> matrix, string separator)
        {
            if (matrix == null) return "";
            if (separator == null) throw new ArgumentNullException("separator");
            if (separator == "") throw new ArgumentException("separator", "Cannot be an empty string.");

            var csv = new System.Text.StringBuilder();
            foreach (IEnumerable<string> row in matrix) csv.AppendLine(String.Join(separator, row));
            return csv.ToString().TrimEnd('\r', '\n');
        }

        public static string[] numsToStrings(IEnumerable<int> nums) {
            return nums.Select(n => n.ToString()).ToArray();
        }
        public static string[] numsToStrings(IEnumerable<double> nums)
        {
            // Invariant culture uses '.' as decimal sign instead of ','
            return nums.Select(n => n.ToString(System.Globalization.CultureInfo.InvariantCulture)).ToArray();
        }

        // A wrapper to add norms to targets, and also a nice to string[] function.
        private class TargetAndNorm {
            public TargetAndNorm(Target t) {
                this.target = t;
                if (t.XOffset.HasValue && t.YOffset.HasValue) this.norm = norm(t.XOffset.Value, t.YOffset.Value);
                else this.norm = null;
            }
            public string[] toStringRow() {
                var shotNo = target.Order.ToString();
                var targetNo = target.TargetNumber.ToString();
                var xOffset = "miss";
                var yOffset = "miss";
                var deltaNorm = "miss";
                if (target.HitGoal)
                {
                    var xExists = target.XOffset.HasValue;
                    var yExists = target.YOffset.HasValue;
                    xOffset = xExists ? cmToString(target.XOffset.Value) : "N/A";
                    yOffset = yExists ? cmToString(target.YOffset.Value) : "N/A";
                    deltaNorm = norm.HasValue ? cmToString(norm.Value) : "N/A";
                }
                return new string[] { shotNo, targetNo, xOffset, yOffset, deltaNorm };
            }
            public Target target { get; set; }
            public double? norm { get; set; }
        }

        /// <summary>
        /// Generates a nice looking CSV string (semicolon-separated) for one player.
        /// </summary>
        /// <param name="player">The player for which the report is to be generated.</param>
        /// <param name="sessions">Sessions will be selected from here, but only those for the correct player.</param>
        /// <param name="playerTargets">Targets are selected from there, but only those for the correct sessions.</param>
        /// <returns>A ';'-separated CSV.</returns>
        public static string generatePlayerMailString(Player player, IEnumerable<PlayerSession> sessions, IEnumerable<Target> playerTargets) {
            var pSessions = sessions.Where(s => s.PlayerId == player.PlayerId && s.Analyzed);

            // titleLine
            var titleLine = new string[] { player.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm") };
            // Captions for the per-shot data lines
            var captionsLine = new string[] { "Shot No.", "Target No.", "X difference (cm)", "Y difference (cm)", "Distance to target (cm)" };
            // empty Line
            var emptyLine = new string[] { };

            var sessionIds = sessions.Select(s => s.SessionId);
            var pTargsTemp = playerTargets.Where(t => sessionIds.Contains(t.SessionId));
            var pTargets = pTargsTemp.Select(targ => new TargetAndNorm(targ));
            
            // dataLines
            var dataLines = new List<string[]>();
            foreach (PlayerSession sess in pSessions) {
                dataLines.Add(emptyLine);
                var targs = pTargets.Where(t => t.target.SessionId == sess.SessionId).OrderBy(t => t.target.Order);
                var sessionTitleLine = new string[] { sess.Created.ToString("yyyy-MM-dd HH:mm") };
                dataLines.Add(sessionTitleLine);
                if (targs == null || !targs.Any()) dataLines.Add(new string[] { "No hits found." });
                else foreach (TargetAndNorm t in targs) dataLines.Add(t.toStringRow());
            }

            var meanStdLines = targetAndNormsToMeanAndStdRows(pTargets);

            // Generate the table, in matrix form.
            var table = new List<string[]>() { titleLine, captionsLine };
            table = table.Concat(dataLines).ToList();
            table.Add(emptyLine);
            table.Add(meanStdLines.First());
            table.Add(meanStdLines.Last());

            return matrixToCSV(table);
        }

        /// <summary>
        /// Generates a nice looking CSV string (semicolon-separated) for one session.
        /// </summary>
        /// <param name="player">Only needed for the name.</param>
        /// <param name="session">The session for which the report is to be generated.</param>
        /// <param name="playerTargets">Targets are selected from there, but only those for the correct session.</param>
        /// <returns>A ';'-separated CSV.</returns>
        public static string generateSessionMailString(Player player, PlayerSession session, IEnumerable<Target> targets)
        {

            var titleLine = new string[] { player.Name, session.Created.ToString() };
            var sessT = targets.Where(t => t.SessionId == session.SessionId);
            var sessionTargets = sessT.Select(t => new TargetAndNorm(t)).OrderBy(t => t.target.Order);

            // Captions for the per-shot data lines
            var captionsLine = new string[] { "Shot No.", "Target No.", "X difference (cm)", "Y difference (cm)", "Distance to target (cm)" };

            // Lines of per-shot data
            var dataLines = new List<string[]>();
            if (sessionTargets == null || !sessionTargets.Any()) dataLines.Add(new string[] { "No hits found." });
            else foreach (TargetAndNorm t in sessionTargets) dataLines.Add(t.toStringRow());

            var emptyLine = new string[] { };

            var meanStdLines = targetAndNormsToMeanAndStdRows(sessionTargets);

            // Generate the table, in matrix form.
            var table = new List<string[]>() { titleLine, captionsLine };
            table = table.Concat(dataLines).ToList();
            table.Add(emptyLine);
            table.Add(meanStdLines.First());
            table.Add(meanStdLines.Last());

            return matrixToCSV(table);
        }

        private static List<string[]> targetAndNormsToMeanAndStdRows(IEnumerable<TargetAndNorm> ts) {
            var hitTargets = ts.Where(t => t.target.HitGoal);
            var xOffs = hitTargets.Where(t => t.target.XOffset.HasValue).Select(t => t.target.XOffset.Value);
            var yOffs = hitTargets.Where(t => t.target.YOffset.HasValue).Select(t => t.target.YOffset.Value);
            var norms = hitTargets.Where(t => t.target.XOffset.HasValue && t.target.YOffset.HasValue).Select(t => norm(t.target.XOffset.Value, t.target.YOffset.Value));

            return new List<string[]>() { numsToMeanRow(xOffs, yOffs, norms), numsToStdRow(xOffs, yOffs, norms) };
        }

        private static string[] numsToMeanRow(IEnumerable<double> xOffsets, IEnumerable<double> yOffsets, IEnumerable<double> norms) {
            var xMeanStr = xOffsets.Any() ? cmToString(mean(xOffsets)) : "N/A";
            var yMeanStr = yOffsets.Any() ? cmToString(mean(yOffsets)) : "N/A";
            var normMeanStr = norms.Any() ? cmToString(mean(norms)) : "N/A";
            return new string[] { "Mean", "", xMeanStr, yMeanStr, normMeanStr };
        }

        private static string[] numsToStdRow(IEnumerable<double> xOffsets, IEnumerable<double> yOffsets, IEnumerable<double> norms) {
            var xStdStr = xOffsets.Any() ? cmToString(standardDeviation(xOffsets)) : "N/A";
            var yStdStr = yOffsets.Any() ? cmToString(standardDeviation(yOffsets)) : "N/A";
            var normStdStr = norms.Any() ? cmToString(standardDeviation(norms)) : "N/A";
            return new string[] { "Standard deviation (unbiased)", "", xStdStr, yStdStr, normStdStr };
        }

        private static string[] targetToStringRow(Target t)
        {
            return (new TargetAndNorm(t)).toStringRow();
        }

        private static IEnumerable<string[]> targetsToStringTable(IEnumerable<Target> ts) {
            return ts.Select(t => targetToStringRow(t));
        }
    }
}
