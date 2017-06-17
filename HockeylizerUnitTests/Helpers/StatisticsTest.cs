using Microsoft.VisualStudio.TestTools.UnitTesting;
using static hockeylizer.Helpers.Statistics;
using System.Collections.Generic;
using hockeylizer.Models;
using System.Linq;
using System;

namespace HockeylizerUnitTests.Helpers
{
    [TestClass]
    public class puckCalculationMethodsTest
    {

        [TestMethod]
        public void targetCoordsTest()
        {
            Assert.AreEqual(10, TargetCoords[1].x, "target1.x is wrong.");
            Assert.AreEqual(91, TargetCoords[1].y, "target1.y is wrong.");
            Assert.AreEqual(10, TargetCoords[2].x, "target2.x is wrong.");
            Assert.AreEqual(18, TargetCoords[2].y, "target2.y is wrong.");
            Assert.AreEqual(173, TargetCoords[3].x, "target3.x is wrong.");
            Assert.AreEqual(18, TargetCoords[3].y, "target3.y is wrong.");
            Assert.AreEqual(173, TargetCoords[4].x, "target4.x is wrong.");
            Assert.AreEqual(91, TargetCoords[4].y, "target4.y is wrong.");
            Assert.AreEqual(91.5, TargetCoords[5].x, "target5.x is wrong.");
            Assert.AreEqual(101, TargetCoords[5].y, "target5.y is wrong.");
        }

        [TestMethod]
        public void offsetToAbsoluteTest1()
        {
            // TODO Not sure if y-coords go upp or down. Fixate this after testing main app.
            var k = 1;

            var pdt = new Point2d1T(0, 0, 1);
            var testPdt = offsetToAbsolute(pdt);
            Assert.AreEqual(TargetCoords[1].x, testPdt.x);
            Assert.AreEqual(TargetCoords[1].y, testPdt.y);
            Assert.AreEqual(1, testPdt.target);

            pdt.target = 2;
            pdt.x = 1;
            pdt.y = 1;
            testPdt = offsetToAbsolute(pdt);
            Assert.AreEqual(TargetCoords[2].x + 1, testPdt.x);
            Assert.AreEqual(TargetCoords[2].y + k*1, testPdt.y);
            Assert.AreEqual(2, testPdt.target);

            pdt.target = 3;
            pdt.x = -12;
            pdt.y = 0.12;
            testPdt = offsetToAbsolute(pdt);
            Assert.AreEqual(TargetCoords[3].x - 12, testPdt.x);
            Assert.AreEqual(TargetCoords[3].y + k * 0.12, testPdt.y);
            Assert.AreEqual(3, testPdt.target);

            pdt.target = 4;
            pdt.x = 78.521;
            pdt.y = -0.000112;
            testPdt = offsetToAbsolute(pdt);
            Assert.AreEqual(TargetCoords[4].x + 78.521, testPdt.x);
            Assert.AreEqual(TargetCoords[4].y - k * 0.000112, testPdt.y);
            Assert.AreEqual(4, testPdt.target);

            pdt.target = 5;
            pdt.x = -78.521;
            pdt.y = 0.000112;
            testPdt = offsetToAbsolute(pdt);
            Assert.AreEqual(TargetCoords[5].x - 78.521, testPdt.x);
            Assert.AreEqual(TargetCoords[5].y + k * 0.000112, testPdt.y);
            Assert.AreEqual(5, testPdt.target);


            pdt.target = 6;
            Assert.ThrowsException<InvalidOperationException>(() => offsetToAbsolute(pdt));
        }

        [TestMethod]
        public void offsetToAbsoluteTest2()
        {
            // TODO Not sure if y-coords go upp or down. Fixate this after testing main app.
            var k = 1;

            var emptyList = new List<Point2d1T> { };
            Assert.AreEqual(0, offsetToAbsolute(emptyList).Count());

            var p = new Point2d1T(0, 0, 1);
            p.target = -1;
            var testList = new List<Point2d1T> { p };
            //offsetToAbsolute(testList);
            Assert.ThrowsException<InvalidOperationException>(() => offsetToAbsolute(testList));

            var p1 = new Point2d1T(0, 0, 1);
            var p2 = new Point2d1T(1, 1, 2);
            var p3 = new Point2d1T(-12, 0.12, 3);
            var p4 = new Point2d1T(78.521, -0.000112, 4);
            var p5 = new Point2d1T(-78.521, 0.000112, 5);
            var q1 = new Point2d1T(TargetCoords[1].x, TargetCoords[1].y, 1);
            var q2 = new Point2d1T(TargetCoords[2].x + 1, TargetCoords[2].y + k * 1, 2);
            var q3 = new Point2d1T(TargetCoords[3].x - 12, TargetCoords[3].y + k * 0.12, 3);
            var q4 = new Point2d1T(TargetCoords[4].x + 78.521, TargetCoords[4].y - k * 0.000112, 4);
            var q5 = new Point2d1T(TargetCoords[5].x - 78.521, TargetCoords[5].y + k * 0.000112, 5);

            testList = new List<Point2d1T> { p1, p2, p3, p4, p5 };
            var retList = offsetToAbsolute(testList).ToList();
            var ansList = new List<Point2d1T> { q1, q2, q3, q4, q5 };

            for (int i=0; i<5; i++) {
                Assert.AreEqual(ansList[i].x, retList[i].x, "Error for retList["+i.ToString()+"].x");
                Assert.AreEqual(ansList[i].y, retList[i].y, "Error for retList[" + i.ToString() + "].y");
                Assert.AreEqual(ansList[i].target, retList[i].target, "Error for retList[" + i.ToString() + "].target");
            }

        }
    }

    [TestClass]
    public class StatisticsMethodsTest
    {
        [TestMethod]
        public void medianOnSortedTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => medianOnSorted(null));
            Assert.ThrowsException<InvalidOperationException>(() => medianOnSorted(new List<double> { }));

            Assert.AreEqual(0.1, medianOnSorted(new List<double> { 0.1 }));
            Assert.AreEqual(-0.1, medianOnSorted(new List<double> { -0.1, -0.1, -0.1, -0.1 }));
            Assert.AreEqual(2.45, medianOnSorted(new List<double> { -0.1, -0.1, 5, 8 }));
            Assert.AreEqual(5.7, medianOnSorted(new List<double> { -0.1, -0.1, 5.7, 8, 1000 }));
        }

        [TestMethod]
        public void medianTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => median(null));
            Assert.ThrowsException<InvalidOperationException>(() => median(new List<double> { }));

            Assert.AreEqual(0.1, median(new List<double> { 0.1 }));
            Assert.AreEqual(-0.1, median(new List<double> { -0.1, -0.1, -0.1, -0.1 }));
            Assert.AreEqual(2.45, median(new List<double> { 5, 8, -0.1, -0.1 }));
            Assert.AreEqual(5.7, median(new List<double> { -0.1, 1000, 8, 5.7, -0.1 }));
        }

        [TestMethod]
        public void meanTest()
        {
            Assert.AreEqual(0, mean(new List<double> { 0 }));
            Assert.AreEqual(0, mean(new List<double> { -1, 1 }));
            Assert.AreEqual(-197.3, mean(new List<double> { -0.1, -1000, 8, 5.7, -0.1 }));
        }

        [TestMethod]
        public void squaredErrorSumTest1()
        {
            Assert.ThrowsException<ArgumentNullException>(() => squaredErrorSum(null));
            Assert.ThrowsException<InvalidOperationException>(() => squaredErrorSum(new List<double> { }));

            // Expected answers, paremeters etc. calculated with Mathematica. Touch at your own risk. Or ask Daniel.
            Assert.AreEqual(0, squaredErrorSum(new List<double> { 0 }, 0));
            Assert.AreEqual(25, squaredErrorSum(new List<double> { 0 }, -5));
            Assert.AreEqual(12500, squaredErrorSum(new List<double> { -50, -50, -50, -50, -50 }, 0));
            double tolerance = 0.00000000001;
            Assert.AreEqual(8600.06, squaredErrorSum(new List<double> { -0.1, -100, 8, 5.7, -0.1 }, -17.3), tolerance);
        }
        [TestMethod]
        public void squaredErrorSumTest2()
        {
            Assert.ThrowsException<ArgumentNullException>(() => squaredErrorSum(null, 0));
            Assert.ThrowsException<InvalidOperationException>(() => squaredErrorSum(new List<double> { }, 0));

            // Expected answers, paremeters etc. calculated with Mathematica. Touch at your own risk. Or ask Daniel.
            Assert.AreEqual(0, squaredErrorSum(new List<double> { 0 }));
            Assert.AreEqual(4.5, squaredErrorSum(new List<double> { -1, 2 }));
            Assert.AreEqual(0, squaredErrorSum(new List<double> { -50, -50, -50, -50, -50 }));
            double tolerance = 0.00000000001;
            Assert.AreEqual(8600.06, squaredErrorSum(new List<double> { -0.1, -100, 8, 5.7, -0.1 }), tolerance);
        }

        [TestMethod]
        public void varianceTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => variance(null));
            Assert.ThrowsException<InvalidOperationException>(() => variance(new List<double> { }));

            Assert.AreEqual(0, variance(new List<double> { -3 }), "Test A");
            Assert.AreEqual(0, variance(new List<double> { 0 }), "Test B");
            Assert.AreEqual(0, variance(new List<double> { 1 }), "Test C");

            Assert.AreEqual(0, variance(new List<double> { -0.1, -0.1, -0.1, -0.1 }), "Test D");
            double tolerance = 0.000000000001;
            Assert.AreEqual(25.5025, variance(new List<double> { 10, -0.1, -0.1, -0.1 }), tolerance, "Test E");
            Assert.AreEqual(2150.015, variance(new List<double> { -0.1, -100, 8, 5.7, -0.1 }), tolerance, "Test F");
            Assert.AreEqual(50000000, variance(new List<double> { 10000, 20000 }), "Test G");
        }

        [TestMethod]
        public void standardDeviationTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => standardDeviation(null));
            Assert.ThrowsException<InvalidOperationException>(() => standardDeviation(new List<double> { }));

            Assert.AreEqual(0, standardDeviation(new List<double> { -3 }), "Test A");
            Assert.AreEqual(0, standardDeviation(new List<double> { 0 }), "Test B");
            Assert.AreEqual(0, standardDeviation(new List<double> { 1 }), "Test C");

            Assert.AreEqual(0, standardDeviation(new List<double> { -0.1, -0.1, -0.1, -0.1 }), "Test D");
            double tolerance = 0.000000000001;
            Assert.AreEqual(5.05, standardDeviation(new List<double> { 10, -0.1, -0.1, -0.1 }), tolerance, "Test E");
            Assert.AreEqual(46.36825422635621, standardDeviation(new List<double> { -0.1, -100, 8, 5.7, -0.1 }), tolerance, "Test F");
            Assert.AreEqual(7071.067811865476, standardDeviation(new List<double> { 10000, 20000 }), tolerance, "Test G");
        }

        [TestMethod]
        public void quantileIndexLeftTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => quantileIndexLeft(0, 0.5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => quantileIndexLeft(0 - 1, 0.5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => quantileIndexLeft(5, -0.001));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => quantileIndexLeft(5, 1.001));

            // Lotsa tests cause its quite a tricky concept with lotsa fringe cases.
            Assert.AreEqual(0, quantileIndexLeft(1, 0));
            Assert.AreEqual(0, quantileIndexLeft(1, 0.5));
            Assert.AreEqual(0, quantileIndexLeft(1, 1));

            Assert.AreEqual(0, quantileIndexLeft(2, 0));
            Assert.AreEqual(0, quantileIndexLeft(2, 0.5));
            Assert.AreEqual(1, quantileIndexLeft(2, 1));

            Assert.AreEqual(0, quantileIndexLeft(4, 0));
            Assert.AreEqual(0, quantileIndexLeft(4, 0.25));
            Assert.AreEqual(1, quantileIndexLeft(4, 0.5));
            Assert.AreEqual(2, quantileIndexLeft(4, 0.75));
            Assert.AreEqual(2, quantileIndexLeft(4, 0.8));
            Assert.AreEqual(3, quantileIndexLeft(4, 1));

            Assert.AreEqual(0, quantileIndexLeft(5, 0));
            Assert.AreEqual(1, quantileIndexLeft(5, 0.25));
            Assert.AreEqual(2, quantileIndexLeft(5, 0.5));
            Assert.AreEqual(3, quantileIndexLeft(5, 0.75));
            Assert.AreEqual(3, quantileIndexLeft(5, 0.8));
            Assert.AreEqual(4, quantileIndexLeft(5, 1));
        }

        [TestMethod]
        public void quantileIndexRightTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => quantileIndexRight(0, 0.5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => quantileIndexRight(0 - 1, 0.5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => quantileIndexRight(5, -0.001));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => quantileIndexRight(5, 1.001));

            // Lotsa tests cause its quite a tricky concept with lotsa fringe cases.
            Assert.AreEqual(0, quantileIndexRight(1, 0));
            Assert.AreEqual(0, quantileIndexRight(1, 0.5));
            Assert.AreEqual(0, quantileIndexRight(1, 1));

            Assert.AreEqual(1, quantileIndexRight(2, 0));
            Assert.AreEqual(1, quantileIndexRight(2, 0.5));
            Assert.AreEqual(0, quantileIndexRight(2, 1));

            Assert.AreEqual(2, quantileIndexRight(3, 0));
            Assert.AreEqual(2, quantileIndexRight(3, 0.25));
            Assert.AreEqual(1, quantileIndexRight(3, 0.5));
            Assert.AreEqual(1, quantileIndexRight(3, 0.8));
            Assert.AreEqual(0, quantileIndexRight(3, 1));

            Assert.AreEqual(3, quantileIndexRight(4, 0));
            Assert.AreEqual(3, quantileIndexRight(4, 0.25));
            Assert.AreEqual(2, quantileIndexRight(4, 0.5));
            Assert.AreEqual(1, quantileIndexRight(4, 0.75));
            Assert.AreEqual(1, quantileIndexRight(4, 0.8));
            Assert.AreEqual(0, quantileIndexRight(4, 1));

            Assert.AreEqual(4, quantileIndexRight(5, 0));
            Assert.AreEqual(3, quantileIndexRight(5, 0.25));
            Assert.AreEqual(2, quantileIndexRight(5, 0.5));
            Assert.AreEqual(1, quantileIndexRight(5, 0.75));
            Assert.AreEqual(1, quantileIndexRight(5, 0.8));
            Assert.AreEqual(0, quantileIndexRight(5, 1));
        }
    }

    [TestClass]
    public class ReportGenerationMethodsTest
    {
        [TestMethod]
        public void matrixToCsvTest1()
        {
            Assert.ThrowsException<ArgumentException>(() => matrixToCSV(new string[1][] { new string[] { "a", "b" } }, ""));
            Assert.ThrowsException<ArgumentNullException>(() => matrixToCSV(new string[1][] { new string[] { "a", "b" } }, null));

            Assert.AreEqual("", matrixToCSV(null, "!"));
            Assert.AreEqual("", matrixToCSV(new string[0][] { }, "!"));
            Assert.AreEqual("a.bb.ccc", matrixToCSV(new string[1][] { new string[] { "a", "bb", "ccc" } }, "."));

            var n = Environment.NewLine;
            var ansStr = string.Format("a{0}b", n);
            var testMatrix = new string[2][] { new string[] { "a" }, new string[] { "b" } };
            Assert.AreEqual(ansStr, matrixToCSV(testMatrix, "!"));

            ansStr = string.Format("a!a!a{0}b!b{0}{0}ccc", n);
            testMatrix = new string[4][] { new string[] { "a", "a", "a" }, new string[] { "b", "b" }, new string[] { }, new string[] { "ccc" } };
            Assert.AreEqual(ansStr, matrixToCSV(testMatrix, "!"));

            ansStr = string.Format("ax{0}bx{0}cx", n);
            testMatrix = new string[3][] { new string[] { "ax", "bx", "cx" }, new string[] { n }, new string[] { } };
            Assert.AreEqual(ansStr, matrixToCSV(testMatrix, n));
        }

        [TestMethod]
        public void matrixToCsvTest2()
        {
            Assert.AreEqual("", matrixToCSV(null));
            Assert.AreEqual("", matrixToCSV(new string[0][] { }));
            Assert.AreEqual("a;bb;ccc", matrixToCSV(new string[1][] { new string[] { "a", "bb", "ccc" } }));

            var n = Environment.NewLine;
            var ansStr = string.Format("a{0}b", n);
            var testMatrix = new string[2][] { new string[] { "a" }, new string[] { "b" } };
            Assert.AreEqual(ansStr, matrixToCSV(testMatrix));

            ansStr = string.Format("a;a;a{0}b;b{0}{0}ccc", n);
            testMatrix = new string[4][] { new string[] { "a", "a", "a" }, new string[] { "b", "b" }, new string[] { }, new string[] { "ccc" } };
            Assert.AreEqual(ansStr, matrixToCSV(testMatrix));
        }

        [TestMethod]
        public void matrixToCsvTest3()
        {
            Assert.ThrowsException<ArgumentException>(() => matrixToCSV(new List<string[]>() { new string[] { "a", "b" } }, ""));
            Assert.ThrowsException<ArgumentNullException>(() => matrixToCSV(new List<string[]>() { new string[] { "a", "b" } }, null));

            Assert.AreEqual("", matrixToCSV(null, "!"));
            Assert.AreEqual("", matrixToCSV(new List<string[]>() { }, "!"));
            Assert.AreEqual("a.bb.ccc", matrixToCSV(new List<string[]>() { new string[] { "a", "bb", "ccc" } }, "."));

            var n = Environment.NewLine;
            var ansStr = string.Format("a{0}b", n);
            var testMatrix = new List<string[]>() { new string[] { "a" }, new string[] { "b" } };
            Assert.AreEqual(ansStr, matrixToCSV(testMatrix, "!"));

            ansStr = string.Format("a!a!a{0}b!b{0}{0}ccc", n);
            testMatrix = new List<string[]>() { new string[] { "a", "a", "a" }, new string[] { "b", "b" }, new string[] { }, new string[] { "ccc" } };
            Assert.AreEqual(ansStr, matrixToCSV(testMatrix, "!"));

            ansStr = string.Format("ax{0}bx{0}cx", n);
            testMatrix = new List<string[]>() { new string[] { "ax", "bx", "cx" }, new string[] { n }, new string[] { } };
            Assert.AreEqual(ansStr, matrixToCSV(testMatrix, n));
        }

        [TestMethod]
        public void matrixToCsvTest4()
        {
            Assert.AreEqual("", matrixToCSV(null));
            Assert.AreEqual("", matrixToCSV(new List<string[]>() { }));
            Assert.AreEqual("a;bb;ccc", matrixToCSV(new List<string[]>() { new string[] { "a", "bb", "ccc" } }));

            var n = Environment.NewLine;
            var ansStr = string.Format("a{0}b", n);
            var testMatrix = new List<string[]>() { new string[] { "a" }, new string[] { "b" } };
            Assert.AreEqual(ansStr, matrixToCSV(testMatrix));

            ansStr = string.Format("a;a;a{0}b;b{0}{0}ccc", n);
            testMatrix = new List<string[]>() { new string[] { "a", "a", "a" }, new string[] { "b", "b" }, new string[] { }, new string[] { "ccc" } };
            Assert.AreEqual(ansStr, matrixToCSV(testMatrix));
        }

        [TestMethod]
        public void numsToStringsTest1()
        {
            Assert.AreEqual(0, numsToStrings(new int[0]).Length);
            var retArray = numsToStrings(new int[] { -3, 122, 0 });
            var ansArray = new string[] { "-3", "122", "0" };
            for (int i = 0; i < ansArray.Length; i++) Assert.AreEqual(ansArray[i], retArray[i]);
        }
        [TestMethod]
        public void numsToStringsTest2()
        {
            Assert.AreEqual(0, numsToStrings(new double[0]).Length);
            var retArray = numsToStrings(new double[] { -3, 122, 0, -0.12, 8.888, 0.7 });
            var ansArray = new string[] { "-3", "122", "0", "-0.12", "8.888", "0.7" };
            for (int i = 0; i < ansArray.Length; i++) Assert.AreEqual(ansArray[i], retArray[i]);
        }

    }
}