using System.Threading.Tasks;
using MultiThreading.Task3.MatrixMultiplier.Matrices;

namespace MultiThreading.Task3.MatrixMultiplier.Multipliers
{
    public class MatricesMultiplierParallel : IMatricesMultiplier
    {
        public IMatrix Multiply(IMatrix m1, IMatrix m2)
        {
            var resultMatrix = new Matrix(m1.RowCount, m2.ColCount);

            Parallel.For(0, m1.RowCount, i =>
                {
                    Parallel.For(0, m2.ColCount, j =>
                    {
                        var element = 0L;
                        for (var k = 0; k < m1.ColCount; k++)
                        {
                            element += m1.GetElement(i, k) * m2.GetElement(k, j);
                        }
                        resultMatrix.SetElement(i, j, element);
                    });
                }
            );

            return resultMatrix;
        }
    }
}