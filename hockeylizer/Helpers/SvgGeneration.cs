using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System;

namespace hockeylizer.Helpers
{
    public static class SvgGeneration
    {

        private static readonly double[,] _targetCoords = new double[5, 2] { { 10, 91 }, { 10, 18 }, { 173, 18 }, { 173, 91 }, { 91.5, 101 } };
        
        // A black circle to mark indivdual hits.
        private static XElement svgCircle(XNamespace ns, double cx, double cy)
        {
            var fill = new XAttribute("fill", "black");
            var radius = new XAttribute("r", 4);
            var xCoord = new XAttribute("cx", cx);
            var yCoord = new XAttribute("cy", cy);
            return new XElement(ns + "circle", fill, radius, xCoord, yCoord);
        }

        private static XElement svgLine(XNamespace ns, double x1, double y1, double x2, double y2)
        {
            var stroke = new XAttribute("stroke", "black");
            var stroke_width = new XAttribute("stroke-width", "1");
            var xStart = new XAttribute("x1", x1);
            var xEnd = new XAttribute("x2", x2);
            var yStart = new XAttribute("y1", y1);
            var yEnd = new XAttribute("y2", y2);
            return new XElement(ns + "line", stroke, stroke_width, xStart, xEnd, yStart, yEnd);
        }

        // A semi-see through rectangle. Puts the "box" in "boxplot".
        private static XElement svgRect(XNamespace ns, double x1, double y1, double x2, double y2)
        {
            var fill = new XAttribute("fill", "#e8e83a");
            var fill_opacity = new XAttribute("fill-opacity", 0.5);
            var stroke = new XAttribute("stroke", "black");
            var stroke_width = new XAttribute("stroke-width", 1);
            var fill_rule = new XAttribute("fill-rule", "nonzero");
            var xCoord = new XAttribute("x", x1);
            var yCoord = new XAttribute("y", y1);
            var width = new XAttribute("width", x2 - x1);
            var height = new XAttribute("height", y2 - y1);
            return new XElement(ns + "rect", stroke, stroke_width, fill, fill_opacity, fill_rule, xCoord, yCoord, width, height);
        }

        // Like a 2D boxplot but without the box.
        private static List<XElement> svgCrossPlot(XNamespace ns, double x0, double x1, double x2, double y0, double y1, double y2)
        {
            var maxLineLen = (double)Math.Max(Math.Abs(y2 - y0), (double)Math.Abs(x2 - x0));
            double whiskerRadius = Math.Min(4, 0.25 * maxLineLen);

            var crossPlot = new List<XElement>();
            var horizontalLine = svgLine(ns, x0, y1, x2, y1);
            var verticalLine = svgLine(ns, x1, y0, x1, y2);
            var leftWhisker = svgLine(ns, x0, y1 - whiskerRadius, x0, y1 + whiskerRadius);
            var rightWhisker = svgLine(ns, x2, y1 - whiskerRadius, x2, y1 + whiskerRadius);
            var topWhisker = svgLine(ns, x1 - whiskerRadius, y0, x1 + whiskerRadius, y0);
            var bottomWhisker = svgLine(ns, x1 - whiskerRadius, y2, x1 + whiskerRadius, y2);

            return new List<XElement> { horizontalLine, verticalLine, leftWhisker, rightWhisker, topWhisker, bottomWhisker };
        }

        private static List<XElement> svgBoxPlot(XNamespace ns, double x0, double x1, double x2, double x3, double x4, double y0, double y1, double y2, double y3, double y4)
        {
            var rect = svgRect(ns, x1, y1, x3, y3);
            var cross = svgCrossPlot(ns, x0, x2, x4, y0, y2, y4);
            //cross.Insert(0, rect);
            cross.Prepend(rect);
            return cross.Prepend(rect).ToList();
        }

        private static List<double[]> selectPointsByTarget(List<double[]> points, List<int> targets, int target)
        {
            int lim = Math.Min(points.Count, targets.Count);
            var ret = new List<double[]> { };
            for (int ii = 0; ii < lim; ii++) if (targets[ii] == target) ret.Add(points[ii]);
            return ret;
        }

        // Assumes a sorted list.
        private static double median(List<double> sorted_nums)
        {
            int len = sorted_nums.Count;
            if (len % 2 == 1) return sorted_nums[(len - 1) / 2];
            else return (sorted_nums[len / 2 - 1] + sorted_nums[len / 2]) / 2;
        }

        private static string svgToString(XDocument svgCode)
        {
            var strBuilder = new System.Text.StringBuilder();
            using (TextWriter writer = new StringWriter(strBuilder)) svgCode.Save(writer, SaveOptions.DisableFormatting);
            return strBuilder.ToString();
        }

        // Creates and saves svg-file to disk, and returns link.
        private static string svgToLink(XDocument svgCode, String whereToPutFile)
        {
            // Setup unique filename and write to it
            var timeStr = DateTime.Now.ToString("ddhhmmss");
            var guidStr = Guid.NewGuid().ToString().Substring(0, 7);
            var fileName = "date" + timeStr + "rnd" + guidStr + ".svg";

            var fs = new FileStream(whereToPutFile + @"/" + fileName, FileMode.Create);
            svgCode.Save(fs);

            return @"http://hockeylizer.azurewebsites.net/images/" + fileName;
        }

        private static List<double[]> offsetToAbsolute(List<double[]> offsets, List<int> targets)
        {
            var absoluteCoords = new List<double[]> { };

            for (int ii = 0; ii < offsets.Count; ii++)
            {
                double x = _targetCoords[targets[ii] - 1, 0] + offsets[ii][0];
                double y = _targetCoords[targets[ii] - 1, 1] + offsets[ii][1];
                absoluteCoords.Add(new double[] { x, y });
            }

            return absoluteCoords;
        }

        /// <summary>
        /// Takes a list of hit coordinates, and puts them as black dots on a simple svg of the goal.
        /// </summary>
        /// <param name="points">List of elements {x_double, y_double}.</param>
        /// <param name="svg_template_path">Path to the template that the back dots are added to.</param>
        public static string generateAllHitsSVG(List<double[]> offsets, List<int> targets, string svg_dir, bool return_link)
        {
            if (!offsets.Any()) return emptyGoalSvg(svg_dir, return_link);

            // Calculate absolute cm coords from offset + target coords.
            List<double[]> points = offsetToAbsolute(offsets, targets);
            
            var svg_template_path = svg_dir + "/goal_template.svg";
            XDocument svgCode = XDocument.Load(svg_template_path);
            XNamespace ns = svgCode.Root.Name.Namespace;
           
            foreach (double[] point in points) svgCode.Root.Add(svgCircle(ns, point[0], point[1]));
            
            return (return_link ? svgToLink(svgCode, svg_dir) : svgToString(svgCode));
        }

        /// <summary>
        /// Takes a list of hits, generates crossplots and/or boxplots depending on the number
        /// of hits per targets, and puts those on the svg template.
        /// </summary>
        /// <param name="points">Elements on the form {double x, double y}.</param>
        /// <param name="targets">
        /// A list of the same length as List points, where each int denotes the target point of the
        /// corresponding point in points, on the same index.
        /// </param>
        /// <param name="svg_template_path">
        /// File path to the svg goal template.
        /// </param>
        public static string generateBoxplotsSVG(List<double[]> offsets, List<int> targets, string svg_dir, bool return_link)
        {
            if (!offsets.Any()) return emptyGoalSvg(svg_dir, return_link);

            // Calculate absolute cm coords from offset + target coords.
            List<double[]> points = offsetToAbsolute(offsets, targets);

            var svg_template_path = svg_dir + "/goal_template.svg";
            // Hardcoded switch to change between average and median as mean for the cross.
            bool use_median_not_average = false;
            // This is the minimal number of shots against a target that will trigger
            // the generation of a boxplot. Fewer will generate a crossplot instead.
            int boxplot_limit = 7;

            XDocument svgCode = XDocument.Load(svg_template_path);
            XNamespace ns = svgCode.Root.Name.Namespace;

            for (int target = 1; target <= 5; target++) {
                var target_pts = selectPointsByTarget(points, targets, target);
                int count = target_pts.Count();

                if (count > 0) {
                    List<double> xs = target_pts.Select((double[] pt) => { return pt[0]; }).ToList();
                    List<double> ys = target_pts.Select((double[] pt) => { return pt[1]; }).ToList();
                    xs.Sort();
                    ys.Sort();

                    // Cross boundaries
                    double xMin = xs.First();
                    double xMax = xs.Last();
                    double yMin = ys.First();
                    double yMax = ys.Last();

                    // Cross center
                    double xMean = (use_median_not_average ? median(xs) : xs.Average());
                    double yMean = (use_median_not_average ? median(ys) : ys.Average());

                    // Change the factors to whatever quantile you want to be the box boundaries
                    int quantileLowerIndex = (int)Math.Ceiling(0.25 * (count - 1));
                    int quantileUpperIndex = (int)Math.Floor(0.75 * (count - 1));

                    if (count == 1) svgCode.Root.Add(svgCircle(ns, xs[0], ys[0]));
                    else if (2 <= count && count < boxplot_limit) {
                        var crossPlot = svgCrossPlot(ns, xMin, xMean, xMax, yMin, yMean, yMax);
                        foreach (XElement svgLine in crossPlot)
                        {
                            svgCode.Root.Add(svgLine);
                        }
                    } else if (boxplot_limit <= count)
                    {
                        var boxPlot = svgBoxPlot(ns, xMin, xs[quantileLowerIndex], xMean, xs[quantileUpperIndex], xMax, yMin, ys[quantileLowerIndex], yMean, ys[quantileUpperIndex], yMax);
                        foreach (XElement svgLine in boxPlot) {
                            svgCode.Root.Add(svgLine);
                        }
                    }
                }
            }
            return (return_link ? svgToLink(svgCode, svg_dir) : svgToString(svgCode));
        }

        public static string emptyGoalSvg(string svg_dir, bool return_link)
        {
            var svg_template_path = svg_dir + "/goal_template.svg";
            if (return_link) return @"http://hockeylizer.azurewebsites.net/images/goal_template.svg";
            else return svgToString(XDocument.Load(svg_template_path));
        }

    }
}
