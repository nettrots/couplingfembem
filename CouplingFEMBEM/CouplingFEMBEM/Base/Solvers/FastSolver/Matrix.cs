using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma.Base.Solvers.FastSolver
{
    public abstract class Matrix : IMatrix
    {

        // порядок матрицы

        private int N;

        public Matrix(int N)
        {

            this.N = N;

        }

        // возвращает порядок матрицы

        public int getN()
        {

            return N;

        }

        // методы интерфеса IMatrix имплементируемые в реализациях

        public abstract void setValue(int row, int col, double value);

        public abstract void addValue(int row, int col, double value);

        public abstract double getValue(int row, int col);

        public abstract void getJRow(int d, ref int[] indexes, ref double[] values);

        public abstract void getJCol(int d, ref int[] indexes, ref double[] values);



        // сервисные методы для преобразования координат внутри матрицы



        // возвращает 0 при x<=0, x при x>0

        protected static int sigma(int x)
        {

            return (x > 0) ? x : 0;

        }

        // преобразование row,col в координаты полуматриц (d,j)

        // для верхней полуматрицы: d=0..(n-1), j=-(n-d-1)..-1

        // для нижней полуматрицы:  d=0..(n-1), j=1..(n-d-1)

        protected static int getD(int row, int col)
        {

            return row - sigma(row - col);

        }

        protected static int getJ(int row, int col)
        {

            return col - row;

        }



        // обратное преобразование координат полуматриц (d,j) в row,col

        protected static int getRow(int d, int j)
        {

            return d + sigma(-j);

        }

        protected static int getCol(int d, int j)
        {

            return d + sigma(j);

        }

    }
 

}
