using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        private Stopwatch _stopwatch;
        private Dictionary<long, List<long>> _parallelTestStatistics;
        private Dictionary<long, List<long>> _serialTestStatistics;

        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            const int testAmount = 10;
            const long rangeAmount = 250;

            _stopwatch = new Stopwatch();
            _parallelTestStatistics = new Dictionary<long, List<long>>();
            _serialTestStatistics = new Dictionary<long, List<long>>();

            var parallelMultiplier = new MatricesMultiplierParallel();
            var serialMultiplier = new MatricesMultiplier();

            for (var test = 0; test < testAmount; test++)
            {
                for (long range = 1; range < rangeAmount; range++)
                {
                    var m1 = new Matrix(range, range, true);
                    var m2 = new Matrix(range, range, true);

                    CollectStatistics(m1, m2, range, parallelMultiplier, _parallelTestStatistics);
                    CollectStatistics(m1, m2, range, serialMultiplier, _serialTestStatistics);
                }
            }

            using (var streamWriter = new StreamWriter("C:\\Workspace\\performanceTesting.dat"))
            {
                for (long range = 1; range < rangeAmount; range++)
                {
                    streamWriter.Write($"{range} ");
                    streamWriter.Write($"{_parallelTestStatistics[range].TakeLast(testAmount - 1).Average()} ");
                    streamWriter.Write($"{_serialTestStatistics[range].TakeLast(testAmount - 1).Average()}\n");
                }
            }
        }

        void CollectStatistics(IMatrix m1, IMatrix m2, long range, IMatricesMultiplier multiplier, Dictionary<long, List<long>> statistics)
        {
            _stopwatch.Restart();
            multiplier.Multiply(m1, m2);
            _stopwatch.Stop();

            if (statistics.ContainsKey(range))
            {
                statistics[range].Add(_stopwatch.ElapsedTicks);
            }
            else
            {
                statistics.Add(range, new List<long> { _stopwatch.ElapsedTicks });
            }
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        #endregion
    }
}
