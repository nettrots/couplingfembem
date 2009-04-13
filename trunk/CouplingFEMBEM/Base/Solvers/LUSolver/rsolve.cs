
using System;
using SbB.Diploma;

public class LUSolve
{
    /*************************************************************************
    Решение системы  линейных  уравнений  с  матрицей  системы,  заданной  LU-
    разложением.

    Алгоритм решает систему линейных уравнений,  матрица  которой  задана  LU-
    разложением. В случае, если  в  процессе  решения  произойдет  деление  на
    ноль, возвращается значение  False,  обозначающее,  что  система  является
    вырожденной.

    Алгоритм решает только системы уравнений с квадратной матрицей.

    Входные параметры:
        A   -   LU-разложение матрицы системы в упакованной  форме  (результат
                работы подпрограммы RMatrixLU).
        Pivots- таблица  перестановок  строк  (результат  работы  подпрограммы
                RMatrixLU).
        B   -   правая часть системы. Массив с нумерацией элементов [0..N-1]
        N   -   размерность системы.

    Выходные параметры:
        X   -   решение системы. Массив с нумерацией элементов [0..N-1]

    Результат:
        True, если система не вырождена (но, возможно, близка к вырожденной).
        False, если система вырождена. В таком случае X не содержит решение.

      -- ALGLIB --
         Copyright 2005-2008 by Bochkanov Sergey
    *************************************************************************/
    private static bool rmatrixlusolve(Matrix a,
        ref int[] pivots,
        double[] b,
        int n,
        ref double[] x)
    {
        bool result = new bool();
        double[] y = new double[0];
        int i = 0;
        int j = 0;
        double v = 0;
        int i_ = 0;

        b = (double[])b.Clone();

        y = new double[n-1+1];
        x = new double[n-1+1];
        result = true;
        for(i=0; i<=n-1; i++)
        {
            if( a[i][i]==0 )
            {
                result = false;
                return result;
            }
        }
        
        //
        // pivots
        //
        for(i=0; i<=n-1; i++)
        {
            if( pivots[i]!=i )
            {
                v = b[i];
                b[i] = b[pivots[i]];
                b[pivots[i]] = v;
            }
        }
        
        //
        // Ly = b
        //
        y[0] = b[0];
        for(i=1; i<=n-1; i++)
        {
            v = 0.0;
            for(i_=0; i_<=i-1;i_++)
            {
                v += a[i][i_]*y[i_];
            }
            y[i] = b[i]-v;
        }
        
        //
        // Ux = y
        //
        x[n-1] = y[n-1]/a[n-1][n-1];
        for(i=n-2; i>=0; i--)
        {
            v = 0.0;
            for(i_=i+1; i_<=n-1;i_++)
            {
                v += a[i][i_]*x[i_];
            }
            x[i] = (y[i]-v)/a[i][i];
        }
        return result;
    }


    /*************************************************************************
    Решение системы  линейных  уравнений

    Алгоритм решает систему линейных уравнений с использованием LU-разложения.
    Алгоритм решает только системы уравнений с квадратной матрицей.

    Входные параметры:
        A   -   Матрица системы.
                Массив с нумерацией элементов [0..N-1, 0..N-1].
        B   -   Правая часть.
                Массив с нумерацией элементов [0..N-1].
        N   -   размерность системы.

    Выходные параметры:
        X   -   решение системы. Массив с нумерацией элементов [0..N-1]

    Результат:
        True, если система не вырождена (но, возможно, близка к вырожденной).
        False, если система вырождена. В таком случае X не содержит решение.

      -- ALGLIB --
         Copyright 2005-2008 by Bochkanov Sergey
    *************************************************************************/
    public static bool Solve(Matrix a,
        Vector b,
        Vector x)
    {
        int n = a.Size.n;
        bool result = new bool();
        int[] pivots = new int[0];
        int i = 0;

        a = (Matrix)a.Clone();
        double[] bb=b.ToArray();
        
        lu.rmatrixlu(a, n, n, ref pivots);
        double[] xx=new double[n];
        result = rmatrixlusolve(a, ref pivots, bb, n, ref xx);
        x=new Vector(xx);
        return result;
    }


    /*************************************************************************
    Obsolete 1-based subroutine
    *************************************************************************/
    private static bool solvesystemlu(ref double[,] a,
        ref int[] pivots,
        double[] b,
        int n,
        ref double[] x)
    {
        bool result = new bool();
        double[] y = new double[0];
        int i = 0;
        int j = 0;
        double v = 0;
        int ip1 = 0;
        int im1 = 0;
        int i_ = 0;

        b = (double[])b.Clone();

        y = new double[n+1];
        x = new double[n+1];
        result = true;
        for(i=1; i<=n; i++)
        {
            if( a[i,i]==0 )
            {
                result = false;
                return result;
            }
        }
        
        //
        // pivots
        //
        for(i=1; i<=n; i++)
        {
            if( pivots[i]!=i )
            {
                v = b[i];
                b[i] = b[pivots[i]];
                b[pivots[i]] = v;
            }
        }
        
        //
        // Ly = b
        //
        y[1] = b[1];
        for(i=2; i<=n; i++)
        {
            im1 = i-1;
            v = 0.0;
            for(i_=1; i_<=im1;i_++)
            {
                v += a[i,i_]*y[i_];
            }
            y[i] = b[i]-v;
        }
        
        //
        // Ux = y
        //
        x[n] = y[n]/a[n,n];
        for(i=n-1; i>=1; i--)
        {
            ip1 = i+1;
            v = 0.0;
            for(i_=ip1; i_<=n;i_++)
            {
                v += a[i,i_]*x[i_];
            }
            x[i] = (y[i]-v)/a[i,i];
        }
        return result;
    }


    /*************************************************************************
    Obsolete 1-based subroutine
    *************************************************************************/
    private static bool solvesystem(double[,] a,
        double[] b,
        int n,
        ref double[] x)
    {
        bool result = new bool();
        int[] pivots = new int[0];
        int i = 0;

        a = (double[,])a.Clone();
        b = (double[])b.Clone();

        lu.ludecomposition(ref a, n, n, ref pivots);
        result = solvesystemlu(ref a, ref pivots, b, n, ref x);
        return result;
    }
}
