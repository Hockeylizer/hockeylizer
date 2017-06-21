using Microsoft.VisualStudio.TestTools.UnitTesting;
using static hockeylizer.Helpers.Statistics;
using System.Collections.Generic;
using hockeylizer.Models;
using System.Linq;
using System;

namespace HockeylizerUnitTests.Helpers
{
    [TestClass]
    public class puckCalculationMethodsTest {

        // TODO Not sure if y-coords go upp or down. Fixate this after testing main app.
        static int k = -1;

        [TestMethod]
        public void targetCoordsTest() {
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
        public void offsetToAbsolute_Test1() {

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
        public void offsetToAbsolute_Test2() {
            

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
        public void medianOnSorted_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() => medianOnSorted(null));
            Assert.ThrowsException<InvalidOperationException>(() => medianOnSorted(new List<double> { }));

            Assert.AreEqual(0.1, medianOnSorted(new List<double> { 0.1 }));
            Assert.AreEqual(-0.1, medianOnSorted(new List<double> { -0.1, -0.1, -0.1, -0.1 }));
            Assert.AreEqual(2.45, medianOnSorted(new List<double> { -0.1, -0.1, 5, 8 }));
            Assert.AreEqual(5.7, medianOnSorted(new List<double> { -0.1, -0.1, 5.7, 8, 1000 }));
        }

        [TestMethod]
        public void median_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() => median(null));
            Assert.ThrowsException<InvalidOperationException>(() => median(new List<double> { }));

            Assert.AreEqual(0.1, median(new List<double> { 0.1 }));
            Assert.AreEqual(-0.1, median(new List<double> { -0.1, -0.1, -0.1, -0.1 }));
            Assert.AreEqual(2.45, median(new List<double> { 5, 8, -0.1, -0.1 }));
            Assert.AreEqual(5.7, median(new List<double> { -0.1, 1000, 8, 5.7, -0.1 }));
        }

        [TestMethod]
        public void mean_Test()
        {
            Assert.AreEqual(0, mean(new List<double> { 0 }));
            Assert.AreEqual(0, mean(new List<double> { -1, 1 }));
            Assert.AreEqual(-197.3, mean(new List<double> { -0.1, -1000, 8, 5.7, -0.1 }));
        }

        [TestMethod]
        public void squaredErrorSum_Test1()
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
        public void squaredErrorSum_Test2()
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
        public void variance_Test()
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
        public void standardDeviation_Test()
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
        public void quantileIndexLeft_Test()
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
        public void quantileIndexRight_Test()
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
        public void cmToString_Test()
        {
            var test1 = cmToString(0);
            var ans1 = "0.0";
            var test2 = cmToString(12.12345);
            var ans2 = "12.1";
            var test3 = cmToString(1.17);
            var ans3 = "1.2";
            var test4 = cmToString(-0.345);
            var ans4 = "-0.3";
            var test5 = cmToString(123456789);
            var ans5 = "123456789.0";
            Assert.AreEqual(ans1, test1);
            Assert.AreEqual(ans2, test2);
            Assert.AreEqual(ans3, test3);
            Assert.AreEqual(ans4, test4);
            Assert.AreEqual(ans5, test5);
        }

        [TestMethod]
        public void matrixToCsv_Test1()
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
        public void matrixToCsv_Test2()
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
        public void matrixToCsv_Test3()
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
        public void matrixToCsv_Test4()
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
        public void numsToStrings_Test1()
        {
            Assert.AreEqual(0, numsToStrings(new int[0]).Length);
            var retArray = numsToStrings(new int[] { -3, 122, 0 });
            var ansArray = new string[] { "-3", "122", "0" };
            for (int i = 0; i < ansArray.Length; i++) Assert.AreEqual(ansArray[i], retArray[i]);
        }
        [TestMethod]
        public void numsToStrings_Test2()
        {
            Assert.AreEqual(0, numsToStrings(new double[0]).Length);
            var retArray = numsToStrings(new double[] { -3, 122, 0, -0.12, 8.888, 0.7 });
            var ansArray = new string[] { "-3", "122", "0", "-0.12", "8.888", "0.7" };
            for (int i = 0; i < ansArray.Length; i++) Assert.AreEqual(ansArray[i], retArray[i]);
        }

        private Target mockTarget(int targetNumber, int order, int sessionId, double? xOffset, double? yOffset)
        {
            var ret = new Target(targetNumber, order, -1, -1, -1, -1, -1, -1);
            ret.SessionId = sessionId;
            ret.XOffset = xOffset;
            ret.YOffset = yOffset;
            ret.HitGoal = true;
            return ret;
        }

        [TestMethod]
        public void generatePlayerMailString_Test() {
            var p = new Player("Foo Fooson");
            p.PlayerId = 1;
            var s1 = new PlayerSession("whatevs", "whatevs", p.PlayerId, -1, -1, 5, 2);
            s1.Analyzed = true;
            s1.SessionId = 1;
            var s2 = new PlayerSession("whatevs", "whatevs", p.PlayerId, -1, -1, 5, 2);
            s2.Analyzed = true;
            s2.SessionId = 2;
            var dummyTarget = mockTarget(0, 1, 99, 0, 0);
            DateTime dateTimeOut;

            // Test of empty data list.
            var targs = new List<Target>();
            var ansStr = generatePlayerMailString(p, new List<PlayerSession> { s1 }, targs);
            var ans = ansStr.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            Assert.AreEqual(8, ans.Length, ansStr);

            var line0 = ans[0].Split(';');
            Assert.AreEqual(p.Name, line0[0]);
            Assert.IsTrue(DateTime.TryParse(line0[1], out dateTimeOut), "Trying to parse "+line0[1]+" as DateTime.");
            Assert.AreEqual("Shot No.;Target No.;x difference (cm);y difference (cm);Distance to target (cm)", ans[1]);
            Assert.AreEqual("", ans[2]);
            Assert.IsTrue(DateTime.TryParse(ans[3], out dateTimeOut), "Trying to parse date as DateTime");

            Assert.AreEqual("No hits found.", ans[4]);
            Assert.AreEqual("", ans[5]);
            Assert.AreEqual("Mean;;N/A;N/A;N/A", ans[6]);
            Assert.AreEqual("Standard deviation (unbiased);;N/A;N/A;N/A", ans[7]);

            // 2nd Test set.

            var t1 = mockTarget(1, 1, s1.SessionId, 0, 0);
            var t2 = mockTarget(1, 14, s1.SessionId, 1, 1);
            var t3 = mockTarget(2, 3, s1.SessionId, null, null);
            var t4 = mockTarget(2, 5, s1.SessionId, 100, 100);
            t4.HitGoal = false;
            var t5 = mockTarget(3, 2, s1.SessionId, -100.1, -100.1);

            var t6 = mockTarget(5, 1, s2.SessionId, 0, 0);
            var t7 = mockTarget(2, 2, s2.SessionId, -74.90200118, 115.4000926);

            targs = new List<Target>() { t1, t2, t3, t4, t5, t6, t7, dummyTarget };
            ansStr = generatePlayerMailString(p, new List<PlayerSession> { s1, s2 }, targs);
            ans = ansStr.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            Assert.AreEqual(16, ans.Length, ansStr);

            line0 = ans[0].Split(';');
            Assert.AreEqual(p.Name, line0[0]);
            Assert.IsTrue(DateTime.TryParse(line0[1], out dateTimeOut), "Trying to parse " + line0[1] + " as DateTime.");
            Assert.AreEqual("Shot No.;Target No.;x difference (cm);y difference (cm);Distance to target (cm)", ans[1]);
            Assert.AreEqual("", ans[2]);

            Assert.IsTrue(DateTime.TryParse(ans[3], out dateTimeOut), "Trying to parse date as DateTime");
            Assert.AreEqual("1;1;0.0;0.0;0.0", ans[4]);
            Assert.AreEqual("2;3;-100.1;-100.1;" + cmToString(141.563), ans[5]);
            Assert.AreEqual("3;2;N/A;N/A;N/A", ans[6]);
            Assert.AreEqual("5;2;miss;miss;miss", ans[7]);
            Assert.AreEqual("14;1;1.0;1.0;" + cmToString(norm(1, 1)), ans[8]);

            Assert.AreEqual("", ans[9]);
            Assert.IsTrue(DateTime.TryParse(ans[10], out dateTimeOut), "Trying to parse line10 '"+ans[10]+"' as DateTime.");

            Assert.AreEqual("1;5;0.0;0.0;0.0", ans[11]);
            Assert.AreEqual("2;2;" + cmToString(-74.90200118) + ";" + cmToString(115.4000926) + ";" + cmToString(137.57721887), ans[12]);

            Assert.AreEqual("", ans[13]);

            var xMean = cmToString(-34.8004);
            var yMean = cmToString(3.26001852);
            var normMean = cmToString(56.110842);
            Assert.AreEqual("Mean;;" + xMean + ";" + yMean + ";" + normMean, ans[14]);
            var xStd = cmToString(48.928469);
            var yStd = cmToString(76.2972030);
            var normStd = cmToString(76.202654);
            Assert.AreEqual("Standard deviation (unbiased);;" + xStd + ";" + yStd + ";" + normStd, ans[15]);
        }
        
        [TestMethod]
        public void generateSessionMailString_Test() {
            var p = new Player("Foo Fooson");
            p.PlayerId = 1;
            var s = new PlayerSession("whatevs", "whatevs", p.PlayerId, -1, -1, 5, 2);
            s.SessionId = 1;
            var dummyTarget = mockTarget(0, 1, 99, 0, 0);

            var targs = new List<Target>();
            var ansStr = generateSessionMailString(p, s, targs);
            var ans = ansStr.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            Assert.AreEqual(6, ans.Length, ansStr);

            var line0 = ans[0].Split(';');
            Assert.AreEqual(2, line0.Length);
            Assert.AreEqual(p.Name, line0[0]);
            DateTime foo;
            Assert.IsTrue(DateTime.TryParse(line0[1], out foo), "Trying to parse date as DateTime");

            var line1 = ans[1];
            Assert.AreEqual("Shot No.;Target No.;x difference (cm);y difference (cm);Distance to target (cm)", line1);
            var line2 = ans[2];
            Assert.AreEqual("No hits found.", line2);
            var line3 = ans[3];
            Assert.AreEqual("", line3);
            var line4 = ans[4];
            Assert.AreEqual("Mean;;N/A;N/A;N/A", line4);
            var line5 = ans[5];
            Assert.AreEqual("Standard deviation (unbiased);;N/A;N/A;N/A", line5);

            var t1 = mockTarget(1, 1, s.SessionId, 0, 0);
            var t2 = mockTarget(1, 14, s.SessionId, 1, 1);
            var t3 = mockTarget(2, 3, s.SessionId, null, null);
            var t4 = mockTarget(2, 5, s.SessionId, 100, 100);
            var t5 = mockTarget(3, 2, s.SessionId, -100.1, -100.1);

            targs = new List<Target>() { t1, t2, t3, t4, t5, dummyTarget};
            ansStr = generateSessionMailString(p, s, targs);
            ans = ansStr.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            Assert.AreEqual(10, ans.Length, ansStr);

            line0 = ans[0].Split(';');
            Assert.AreEqual(2, line0.Length);
            Assert.AreEqual(p.Name, line0[0]);
            Assert.IsTrue(DateTime.TryParse(line0[1], out foo), "Trying to parse date as DateTime");

            line1 = ans[1];
            Assert.AreEqual("Shot No.;Target No.;x difference (cm);y difference (cm);Distance to target (cm)", line1);
            line2 = ans[2];
            Assert.AreEqual("1;1;0.0;0.0;0.0", line2);
            line3 = ans[3];
            Assert.AreEqual("2;3;-100.1;-100.1;" + cmToString(141.563), line3);
            line4 = ans[4];
            Assert.AreEqual("3;2;N/A;N/A;N/A", line4);
            line5 = ans[5];
            Assert.AreEqual("5;2;100.0;100.0;" + cmToString(141.421), line5);
            var line6 = ans[6];
            Assert.AreEqual("14;1;1.0;1.0;" + cmToString(norm(1, 1)), line6);
            var line7 = ans[7];
            Assert.AreEqual("", line7);
            var line8 = ans[8];
            var xyMean = cmToString(0.225);
            var normMean = cmToString(71.0996);
            Assert.AreEqual("Mean;;" + xyMean + ";" + xyMean + ";" + normMean, line8);
            var line9 = ans[9];
            var xyStd = cmToString(standardDeviation(new double[] { 0, -100.1, 100, 1 }));
            var normStd = cmToString(standardDeviation(new double[] { 0, norm(-100.1, -100.1), norm(100, 100), norm(1, 1) }));
            Assert.AreEqual("Standard deviation (unbiased);;" + xyStd + ";" + xyStd + ";" + normStd, line9);

            // Samma som innan, men sätt t4 till miss och se så att allt stämmer.
            t4.HitGoal = false;
            ansStr = generateSessionMailString(p, s, targs);
            ans = ansStr.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            Assert.AreEqual(10, ans.Length, ansStr);
            line5 = ans[5];
            line8 = ans[8];
            line9 = ans[9];
            Assert.AreEqual("5;2;miss;miss;miss", line5);
            Assert.AreEqual("Mean;;" + cmToString(-33.0333333) + ";" + cmToString(-33.0333333) + ";" + cmToString(47.658997), line8, ansStr);
            Assert.AreEqual("Standard deviation (unbiased);;" + cmToString(58.083589) + ";" + cmToString(58.083589) + ";" + cmToString(81.32613356), line9);

            // Buggen som Jonas hittade, issue #121. Berodde på att jag hade skrivit XOffset när det skulle vara YOffset.
            t1 = mockTarget(5, 1, s.SessionId, 0, 0);
            t2 = mockTarget(2, 2, s.SessionId, -74.90200118, 115.4000926);
            targs = new List<Target> { t1, t2 };
            ansStr = generateSessionMailString(p, s, targs);
            ans = ansStr.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            Assert.AreEqual(7, ans.Length, ansStr);

            line2 = ans[2];
            Assert.AreEqual("1;5;0.0;0.0;0.0", line2);
            line3 = ans[3];
            Assert.AreEqual("2;2;" + cmToString(-74.90200118) + ";" + cmToString(115.4000926) + ";" + cmToString(137.57721887), line3);
            line5 = ans[5];
            Assert.AreEqual("Mean;;" + cmToString(-37.45100059) + ";" + cmToString(57.7000463) + ";" + cmToString(68.7886094), line5);
            line6 = ans[6];
            Assert.AreEqual("Standard deviation (unbiased);;" + cmToString(52.963713) + ";" + cmToString(81.600188) + ";" + cmToString(97.2817844), line6);
        }

    }
}