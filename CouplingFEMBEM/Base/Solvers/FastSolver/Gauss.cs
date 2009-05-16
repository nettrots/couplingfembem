using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma.Base.Solvers.FastSolver
{
    public class Gauss
    {

        // матрица

        private IMatrix matrix;



        // конструктор, принимает созданную матрицу коэффициентов

        public Gauss(IMatrix matrix)
        {

            this.matrix = matrix;

        }



        // главный метод, возвращающий решение, принимает вектор свободных членов

        public double[] calculate(double[] B)
        {

            // обнуляем нижнюю полуматрицу, перебирая сверху вниз все строки

            // и складывая каждую со всеми нижележащими

            for (int row = 0; row < (matrix.getN() - 1); row++)
            {

                // получаем все ненулевые значения обнуляемого столбца

                int[] colIndexes = new int[0];

                double[] colValues = new double[0];

                matrix.getJCol(row, ref colIndexes, ref colValues);



                // получаем индексы и значения ячеек строки, правее главной диагонали

                // ячейки левее главной диагонали к этому моменту уже обнулены

                int[] rowIndexes = new int[0];

                double[] rowValues = new double[0];

                matrix.getJRow(row, ref rowIndexes, ref rowValues);



                // получаем элемент главной диагонали, которым будем обнулять столбец

                double dd = matrix.getValue(row, row);

                for (int i = 0; i < colIndexes.Length; i++)
                {

                    // высчитываем коэффициент 

                    double k = colValues[i] / dd;



                    // k подобран таким образом чтобы обнулить ячейку столбца,

                    matrix.setValue(colIndexes[i], row, 0);



                    // складываем строки

                    for (int ii = 0; ii < rowIndexes.Length; ii++)
                    {

                        matrix.addValue(colIndexes[i], rowIndexes[ii], -rowValues[ii] * k);

                    }



                    // складываем соответствующие свободные члены

                    B[colIndexes[i]] -= B[row] * k;

                }

            }



            // создаем вектор неизвестных

            double[] x = new double[matrix.getN()];



            // используя обратный ход находим неизвестные

            for (int row = (matrix.getN() - 1); row >= 0; row--)
            {

                double e = 0;

                int[] indexes = new int[0];

                double[] values = new double[0];

                matrix.getJRow(row, ref indexes, ref values);

                for (int i = 0; i < indexes.Length; i++) e += x[indexes[i]] * values[i];

                x[row] = (B[row] - e) / matrix.getValue(row, row);

            }



            return x;

        }

    } 

}
