using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using hockeylizer.Helpers;
using System;

namespace HockeylizerUnitTests.Helpers
{
    [TestClass]
    public class constantsTest
    {

        [TestMethod]
        public void targetCoordsTest()
        {
            Assert.AreEqual(10, Statistics.TargetCoords[1].x, "target1.x");
            Assert.AreEqual(91, Statistics.TargetCoords[1].y, "target1.y");
            Assert.AreEqual(10, Statistics.TargetCoords[2].x, "target2.x");
            Assert.AreEqual(18, Statistics.TargetCoords[2].y, "target2.y");
            Assert.AreEqual(173, Statistics.TargetCoords[3].x, "target3.x");
            Assert.AreEqual(18, Statistics.TargetCoords[3].y, "target3.y");
            Assert.AreEqual(173, Statistics.TargetCoords[4].x, "target4.x");
            Assert.AreEqual(91, Statistics.TargetCoords[4].y, "target4.y");
            Assert.AreEqual(91.5, Statistics.TargetCoords[5].x, "target5.x");
            Assert.AreEqual(101, Statistics.TargetCoords[5].y, "target5.y");
        }
    }

    [TestClass]
    public class StatisticsTest
    {
        [TestMethod]
        public void medianOnSortedTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Statistics.medianOnSorted(null));
            Assert.ThrowsException<InvalidOperationException>(() => Statistics.medianOnSorted(new List<double> { }));

            Assert.AreEqual(0.1, Statistics.medianOnSorted(new List<double> { 0.1 }));
            Assert.AreEqual(-0.1, Statistics.medianOnSorted(new List<double> { -0.1, -0.1, -0.1, -0.1 }));
            Assert.AreEqual(2.45, Statistics.medianOnSorted(new List<double> { -0.1, -0.1, 5, 8 }));
            Assert.AreEqual(5.7, Statistics.medianOnSorted(new List<double> { -0.1, -0.1, 5.7, 8, 1000 }));
        }

        [TestMethod]
        public void medianTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Statistics.median(null));
            Assert.ThrowsException<InvalidOperationException>(() => Statistics.median(new List<double> { }));

            Assert.AreEqual(0.1, Statistics.median(new List<double> { 0.1 }));
            Assert.AreEqual(-0.1, Statistics.median(new List<double> { -0.1, -0.1, -0.1, -0.1 }));
            Assert.AreEqual(2.45, Statistics.median(new List<double> { 5, 8, -0.1, -0.1 }));
            Assert.AreEqual(5.7, Statistics.median(new List<double> { -0.1, 1000, 8, 5.7, -0.1 }));
        }

        [TestMethod]
        public void meanTest()
        {
            Assert.AreEqual(0, Statistics.mean(new List<double> { 0 }));
            Assert.AreEqual(0, Statistics.mean(new List<double> { -1, 1 }));
            Assert.AreEqual(-197.3, Statistics.mean(new List<double> { -0.1, -1000, 8, 5.7, -0.1 }));
        }

        [TestMethod]
        public void squaredErrorSumTest1()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Statistics.squaredErrorSum(null));
            Assert.ThrowsException<InvalidOperationException>(() => Statistics.squaredErrorSum(new List<double> { }));

            // Expected answers, paremeters etc. calculated with Mathematica. Touch at your own risk. Or ask Daniel.
            Assert.AreEqual(0, Statistics.squaredErrorSum(new List<double> { 0 }, 0));
            Assert.AreEqual(25, Statistics.squaredErrorSum(new List<double> { 0 }, -5));
            Assert.AreEqual(12500, Statistics.squaredErrorSum(new List<double> { -50, -50, -50, -50, -50 }, 0));
            double tolerance = 0.00000000001;
            Assert.AreEqual(8600.06, Statistics.squaredErrorSum(new List<double> { -0.1, -100, 8, 5.7, -0.1 }, -17.3), tolerance);
        }
        [TestMethod]
        public void squaredErrorSumTest2()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Statistics.squaredErrorSum(null, 0));
            Assert.ThrowsException<InvalidOperationException>(() => Statistics.squaredErrorSum(new List<double> { }, 0));

            // Expected answers, paremeters etc. calculated with Mathematica. Touch at your own risk. Or ask Daniel.
            Assert.AreEqual(0, Statistics.squaredErrorSum(new List<double> { 0 }));
            Assert.AreEqual(4.5, Statistics.squaredErrorSum(new List<double> { -1, 2 }));
            Assert.AreEqual(0, Statistics.squaredErrorSum(new List<double> { -50, -50, -50, -50, -50 }));
            double tolerance = 0.00000000001;
            Assert.AreEqual(8600.06, Statistics.squaredErrorSum(new List<double> { -0.1, -100, 8, 5.7, -0.1 }), tolerance);
        }

        [TestMethod]
        public void varianceTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Statistics.variance(null));
            Assert.ThrowsException<InvalidOperationException>(() => Statistics.variance(new List<double> { }));

            Assert.AreEqual(0, Statistics.variance(new List<double> { -3 }), "Test A");
            Assert.AreEqual(0, Statistics.variance(new List<double> { 0 }), "Test B");
            Assert.AreEqual(0, Statistics.variance(new List<double> { 1 }), "Test C");

            Assert.AreEqual(0, Statistics.variance(new List<double> { -0.1, -0.1, -0.1, -0.1 }), "Test D");
            double tolerance = 0.000000000001;
            Assert.AreEqual(25.5025, Statistics.variance(new List<double> { 10, -0.1, -0.1, -0.1 }), tolerance, "Test E");
            Assert.AreEqual(2150.015, Statistics.variance(new List<double> { -0.1, -100, 8, 5.7, -0.1 }), tolerance, "Test F");
            Assert.AreEqual(50000000, Statistics.variance(new List<double> { 10000, 20000 }), "Test G");
        }

        [TestMethod]
        public void standardDeviationTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Statistics.standardDeviation(null));
            Assert.ThrowsException<InvalidOperationException>(() => Statistics.standardDeviation(new List<double> { }));

            Assert.AreEqual(0, Statistics.standardDeviation(new List<double> { -3 }), "Test A");
            Assert.AreEqual(0, Statistics.standardDeviation(new List<double> { 0 }), "Test B");
            Assert.AreEqual(0, Statistics.standardDeviation(new List<double> { 1 }), "Test C");

            Assert.AreEqual(0, Statistics.standardDeviation(new List<double> { -0.1, -0.1, -0.1, -0.1 }), "Test D");
            double tolerance = 0.000000000001;
            Assert.AreEqual(5.05, Statistics.standardDeviation(new List<double> { 10, -0.1, -0.1, -0.1 }), tolerance, "Test E");
            Assert.AreEqual(46.36825422635621, Statistics.standardDeviation(new List<double> { -0.1, -100, 8, 5.7, -0.1 }), tolerance, "Test F");
            Assert.AreEqual(7071.067811865476, Statistics.standardDeviation(new List<double> { 10000, 20000 }), tolerance, "Test G");
        }

        [TestMethod]
        public void quantileIndexLeftTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.quantileIndexLeft(0, 0.5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.quantileIndexLeft(0 - 1, 0.5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.quantileIndexLeft(5, -0.001));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.quantileIndexLeft(5, 1.001));

            // Lotsa tests cause its quite a tricky concept with lotsa fringe cases.
            Assert.AreEqual(0, Statistics.quantileIndexLeft(1, 0));
            Assert.AreEqual(0, Statistics.quantileIndexLeft(1, 0.5));
            Assert.AreEqual(0, Statistics.quantileIndexLeft(1, 1));

            Assert.AreEqual(0, Statistics.quantileIndexLeft(2, 0));
            Assert.AreEqual(0, Statistics.quantileIndexLeft(2, 0.5));
            Assert.AreEqual(1, Statistics.quantileIndexLeft(2, 1));

            Assert.AreEqual(0, Statistics.quantileIndexLeft(4, 0));
            Assert.AreEqual(0, Statistics.quantileIndexLeft(4, 0.25));
            Assert.AreEqual(1, Statistics.quantileIndexLeft(4, 0.5));
            Assert.AreEqual(2, Statistics.quantileIndexLeft(4, 0.75));
            Assert.AreEqual(2, Statistics.quantileIndexLeft(4, 0.8));
            Assert.AreEqual(3, Statistics.quantileIndexLeft(4, 1));

            Assert.AreEqual(0, Statistics.quantileIndexLeft(5, 0));
            Assert.AreEqual(1, Statistics.quantileIndexLeft(5, 0.25));
            Assert.AreEqual(2, Statistics.quantileIndexLeft(5, 0.5));
            Assert.AreEqual(3, Statistics.quantileIndexLeft(5, 0.75));
            Assert.AreEqual(3, Statistics.quantileIndexLeft(5, 0.8));
            Assert.AreEqual(4, Statistics.quantileIndexLeft(5, 1));
        }

        [TestMethod]
        public void quantileIndexRightTest()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.quantileIndexRight(0, 0.5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.quantileIndexRight(0 - 1, 0.5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.quantileIndexRight(5, -0.001));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Statistics.quantileIndexRight(5, 1.001));

            // Lotsa tests cause its quite a tricky concept with lotsa fringe cases.
            Assert.AreEqual(0, Statistics.quantileIndexRight(1, 0));
            Assert.AreEqual(0, Statistics.quantileIndexRight(1, 0.5));
            Assert.AreEqual(0, Statistics.quantileIndexRight(1, 1));

            Assert.AreEqual(1, Statistics.quantileIndexRight(2, 0));
            Assert.AreEqual(1, Statistics.quantileIndexRight(2, 0.5));
            Assert.AreEqual(0, Statistics.quantileIndexRight(2, 1));

            Assert.AreEqual(2, Statistics.quantileIndexRight(3, 0));
            Assert.AreEqual(2, Statistics.quantileIndexRight(3, 0.25));
            Assert.AreEqual(1, Statistics.quantileIndexRight(3, 0.5));
            Assert.AreEqual(1, Statistics.quantileIndexRight(3, 0.8));
            Assert.AreEqual(0, Statistics.quantileIndexRight(3, 1));

            Assert.AreEqual(3, Statistics.quantileIndexRight(4, 0));
            Assert.AreEqual(3, Statistics.quantileIndexRight(4, 0.25));
            Assert.AreEqual(2, Statistics.quantileIndexRight(4, 0.5));
            Assert.AreEqual(1, Statistics.quantileIndexRight(4, 0.75));
            Assert.AreEqual(1, Statistics.quantileIndexRight(4, 0.8));
            Assert.AreEqual(0, Statistics.quantileIndexRight(4, 1));

            Assert.AreEqual(4, Statistics.quantileIndexRight(5, 0));
            Assert.AreEqual(3, Statistics.quantileIndexRight(5, 0.25));
            Assert.AreEqual(2, Statistics.quantileIndexRight(5, 0.5));
            Assert.AreEqual(1, Statistics.quantileIndexRight(5, 0.75));
            Assert.AreEqual(1, Statistics.quantileIndexRight(5, 0.8));
            Assert.AreEqual(0, Statistics.quantileIndexRight(5, 1));
        }

        [TestMethod]
        public void matrixToCsvTest1()
        {
            Assert.ThrowsException<ArgumentException>(() => Statistics.matrixToCSV(new string[1][] { new string[] { "a", "b" } }, ""));
            Assert.ThrowsException<ArgumentNullException>(() => Statistics.matrixToCSV(new string[1][] { new string[] { "a", "b" } }, null));

            Assert.AreEqual("", Statistics.matrixToCSV(null, "!"));
            Assert.AreEqual("", Statistics.matrixToCSV(new string[0][] { }, "!"));
            Assert.AreEqual("a.bb.ccc", Statistics.matrixToCSV(new string[1][] { new string[] { "a", "bb", "ccc" } }, "."));

            var n = Environment.NewLine;
            var ansStr = string.Format("a{0}b", n);
            var testMatrix = new string[2][] { new string[] { "a" }, new string[] { "b" } };
            Assert.AreEqual(ansStr, Statistics.matrixToCSV(testMatrix, "!"));

            ansStr = string.Format("a!a!a{0}b!b{0}{0}ccc", n);
            testMatrix = new string[4][] { new string[] { "a", "a", "a" }, new string[] { "b", "b" }, new string[] { }, new string[] { "ccc" } };
            Assert.AreEqual(ansStr, Statistics.matrixToCSV(testMatrix, "!"));

            ansStr = string.Format("ax{0}bx{0}cx", n);
            testMatrix = new string[3][] { new string[] { "ax", "bx", "cx" }, new string[] { n }, new string[] { } };
            Assert.AreEqual(ansStr, Statistics.matrixToCSV(testMatrix, n));
        }

        [TestMethod]
        public void matrixToCsvTest2()
        {
            Assert.AreEqual("", Statistics.matrixToCSV(null));
            Assert.AreEqual("", Statistics.matrixToCSV(new string[0][] { }));
            Assert.AreEqual("a;bb;ccc", Statistics.matrixToCSV(new string[1][] { new string[] { "a", "bb", "ccc" } }));

            var n = Environment.NewLine;
            var ansStr = string.Format("a{0}b", n);
            var testMatrix = new string[2][] { new string[] { "a" }, new string[] { "b" } };
            Assert.AreEqual(ansStr, Statistics.matrixToCSV(testMatrix));

            ansStr = string.Format("a;a;a{0}b;b{0}{0}ccc", n);
            testMatrix = new string[4][] { new string[] { "a", "a", "a" }, new string[] { "b", "b" }, new string[] { }, new string[] { "ccc" } };
            Assert.AreEqual(ansStr, Statistics.matrixToCSV(testMatrix));
        }
    }
}