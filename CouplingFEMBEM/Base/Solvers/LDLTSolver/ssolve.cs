/*************************************************************************
Copyright (c) 1992-2007 The University of Tennessee.  All rights reserved.

Contributors:
    * Sergey Bochkanov (ALGLIB project). Translation from FORTRAN to
      pseudocode.

See subroutines comments for additional copyrights.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are
met:

- Redistributions of source code must retain the above copyright
  notice, this list of conditions and the following disclaimer.

- Redistributions in binary form must reproduce the above copyright
  notice, this list of conditions and the following disclaimer listed
  in this license in the documentation and/or other materials
  provided with the distribution.

- Neither the name of the copyright holders nor the names of its
  contributors may be used to endorse or promote products derived from
  this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*************************************************************************/

using System;

class ssolve
{
    /*************************************************************************
    Решение системы линейных уравнений с матрицей системы, заданной
    LDLT-разложением.

    Алгоритм решает только системы уравнений с квадратной матрицей.

    Входные параметры:
        A   -   LDLT-разложение матрицы системы (результат работы подпрограммы
                SMatrixLDLT).
        Pivots- таблица перестановок (результат работы подпрограммы SMatrixLDLT).
        B   -   правая часть системы. Массив с нумерацией элементов [0..N-1]
        N   -   размерность системы.
        IsUpper-указывает треугольник матрицы A, в котором хранится разложение
                Если IsUpper=True, то разложение имеет вид U*D*U', и в верхнем
                треугольнике матрицы A хранится  матрица U  (при  этом  нижний
                треугольник не используется и  не  меняется подпрограммой).
                Аналогично,  если  IsUpper=False, разложение имеет вид L*D*L',
                и передается матрица L (в нижнем треугольнике).

    Выходные параметры:
        X   -   решение системы. Массив с нумерацией элементов [0..N-1]

    Результат:
        True    -   если система не вырождена. X содержит решение.
        False   -   если система вырождена (определитель матрицы D строго равен
                    нолю). В  таком  случае X не содержит решение.
    *************************************************************************/
    public static bool smatrixldltsolve(ref double[,] a,
        ref int[] pivots,
        double[] b,
        int n,
        bool isupper,
        ref double[] x)
    {
        bool result = new bool();
        int i = 0;
        int j = 0;
        int k = 0;
        int kp = 0;
        double ak = 0;
        double akm1 = 0;
        double akm1k = 0;
        double bk = 0;
        double bkm1 = 0;
        double denom = 0;
        double v = 0;
        int i_ = 0;

        b = (double[])b.Clone();

        
        //
        // Quick return if possible
        //
        result = true;
        if( n==0 )
        {
            return result;
        }
        
        //
        // Check that the diagonal matrix D is nonsingular
        //
        if( isupper )
        {
            
            //
            // Upper triangular storage: examine D from bottom to top
            //
            for(i=n-1; i>=0; i--)
            {
                if( pivots[i]>=0 & a[i,i]==0 )
                {
                    result = false;
                    return result;
                }
            }
        }
        else
        {
            
            //
            // Lower triangular storage: examine D from top to bottom.
            //
            for(i=0; i<=n-1; i++)
            {
                if( pivots[i]>=0 & a[i,i]==0 )
                {
                    result = false;
                    return result;
                }
            }
        }
        
        //
        // Solve Ax = b
        //
        if( isupper )
        {
            
            //
            // Solve A*X = B, where A = U*D*U'.
            //
            // First solve U*D*X = B, overwriting B with X.
            //
            // K+1 is the main loop index, decreasing from N to 1 in steps of
            // 1 or 2, depending on the size of the diagonal blocks.
            //
            k = n-1;
            while( k>=0 )
            {
                if( pivots[k]>=0 )
                {
                    
                    //
                    // 1 x 1 diagonal block
                    //
                    // Interchange rows K+1 and IPIV(K+1).
                    //
                    kp = pivots[k];
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    
                    //
                    // Multiply by inv(U(K+1)), where U(K+1) is the transformation
                    // stored in column K+1 of A.
                    //
                    v = b[k];
                    for(i_=0; i_<=k-1;i_++)
                    {
                        b[i_] = b[i_] - v*a[i_,k];
                    }
                    
                    //
                    // Multiply by the inverse of the diagonal block.
                    //
                    b[k] = b[k]/a[k,k];
                    k = k-1;
                }
                else
                {
                    
                    //
                    // 2 x 2 diagonal block
                    //
                    // Interchange rows K+1-1 and -IPIV(K+1).
                    //
                    kp = pivots[k]+n;
                    if( kp!=k-1 )
                    {
                        v = b[k-1];
                        b[k-1] = b[kp];
                        b[kp] = v;
                    }
                    
                    //
                    // Multiply by inv(U(K+1)), where U(K+1) is the transformation
                    // stored in columns K+1-1 and K+1 of A.
                    //
                    v = b[k];
                    for(i_=0; i_<=k-2;i_++)
                    {
                        b[i_] = b[i_] - v*a[i_,k];
                    }
                    v = b[k-1];
                    for(i_=0; i_<=k-2;i_++)
                    {
                        b[i_] = b[i_] - v*a[i_,k-1];
                    }
                    
                    //
                    // Multiply by the inverse of the diagonal block.
                    //
                    akm1k = a[k-1,k];
                    akm1 = a[k-1,k-1]/akm1k;
                    ak = a[k,k]/akm1k;
                    denom = akm1*ak-1;
                    bkm1 = b[k-1]/akm1k;
                    bk = b[k]/akm1k;
                    b[k-1] = (ak*bkm1-bk)/denom;
                    b[k] = (akm1*bk-bkm1)/denom;
                    k = k-2;
                }
            }
            
            //
            // Next solve U'*X = B, overwriting B with X.
            //
            // K+1 is the main loop index, increasing from 1 to N in steps of
            // 1 or 2, depending on the size of the diagonal blocks.
            //
            k = 0;
            while( k<=n-1 )
            {
                if( pivots[k]>=0 )
                {
                    
                    //
                    // 1 x 1 diagonal block
                    //
                    // Multiply by inv(U'(K+1)), where U(K+1) is the transformation
                    // stored in column K+1 of A.
                    //
                    v = 0.0;
                    for(i_=0; i_<=k-1;i_++)
                    {
                        v += b[i_]*a[i_,k];
                    }
                    b[k] = b[k]-v;
                    
                    //
                    // Interchange rows K+1 and IPIV(K+1).
                    //
                    kp = pivots[k];
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    k = k+1;
                }
                else
                {
                    
                    //
                    // 2 x 2 diagonal block
                    //
                    // Multiply by inv(U'(K+1+1)), where U(K+1+1) is the transformation
                    // stored in columns K+1 and K+1+1 of A.
                    //
                    v = 0.0;
                    for(i_=0; i_<=k-1;i_++)
                    {
                        v += b[i_]*a[i_,k];
                    }
                    b[k] = b[k]-v;
                    v = 0.0;
                    for(i_=0; i_<=k-1;i_++)
                    {
                        v += b[i_]*a[i_,k+1];
                    }
                    b[k+1] = b[k+1]-v;
                    
                    //
                    // Interchange rows K+1 and -IPIV(K+1).
                    //
                    kp = pivots[k]+n;
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    k = k+2;
                }
            }
        }
        else
        {
            
            //
            // Solve A*X = B, where A = L*D*L'.
            //
            // First solve L*D*X = B, overwriting B with X.
            //
            // K+1 is the main loop index, increasing from 1 to N in steps of
            // 1 or 2, depending on the size of the diagonal blocks.
            //
            k = 0;
            while( k<=n-1 )
            {
                if( pivots[k]>=0 )
                {
                    
                    //
                    // 1 x 1 diagonal block
                    //
                    // Interchange rows K+1 and IPIV(K+1).
                    //
                    kp = pivots[k];
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    
                    //
                    // Multiply by inv(L(K+1)), where L(K+1) is the transformation
                    // stored in column K+1 of A.
                    //
                    if( k+1<n )
                    {
                        v = b[k];
                        for(i_=k+1; i_<=n-1;i_++)
                        {
                            b[i_] = b[i_] - v*a[i_,k];
                        }
                    }
                    
                    //
                    // Multiply by the inverse of the diagonal block.
                    //
                    b[k] = b[k]/a[k,k];
                    k = k+1;
                }
                else
                {
                    
                    //
                    // 2 x 2 diagonal block
                    //
                    // Interchange rows K+1+1 and -IPIV(K+1).
                    //
                    kp = pivots[k]+n;
                    if( kp!=k+1 )
                    {
                        v = b[k+1];
                        b[k+1] = b[kp];
                        b[kp] = v;
                    }
                    
                    //
                    // Multiply by inv(L(K+1)), where L(K+1) is the transformation
                    // stored in columns K+1 and K+1+1 of A.
                    //
                    if( k+1<n-1 )
                    {
                        v = b[k];
                        for(i_=k+2; i_<=n-1;i_++)
                        {
                            b[i_] = b[i_] - v*a[i_,k];
                        }
                        v = b[k+1];
                        for(i_=k+2; i_<=n-1;i_++)
                        {
                            b[i_] = b[i_] - v*a[i_,k+1];
                        }
                    }
                    
                    //
                    // Multiply by the inverse of the diagonal block.
                    //
                    akm1k = a[k+1,k];
                    akm1 = a[k,k]/akm1k;
                    ak = a[k+1,k+1]/akm1k;
                    denom = akm1*ak-1;
                    bkm1 = b[k]/akm1k;
                    bk = b[k+1]/akm1k;
                    b[k] = (ak*bkm1-bk)/denom;
                    b[k+1] = (akm1*bk-bkm1)/denom;
                    k = k+2;
                }
            }
            
            //
            // Next solve L'*X = B, overwriting B with X.
            //
            // K+1 is the main loop index, decreasing from N to 1 in steps of
            // 1 or 2, depending on the size of the diagonal blocks.
            //
            k = n-1;
            while( k>=0 )
            {
                if( pivots[k]>=0 )
                {
                    
                    //
                    // 1 x 1 diagonal block
                    //
                    // Multiply by inv(L'(K+1)), where L(K+1) is the transformation
                    // stored in column K+1 of A.
                    //
                    if( k+1<n )
                    {
                        v = 0.0;
                        for(i_=k+1; i_<=n-1;i_++)
                        {
                            v += b[i_]*a[i_,k];
                        }
                        b[k] = b[k]-v;
                    }
                    
                    //
                    // Interchange rows K+1 and IPIV(K+1).
                    //
                    kp = pivots[k];
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    k = k-1;
                }
                else
                {
                    
                    //
                    // 2 x 2 diagonal block
                    //
                    // Multiply by inv(L'(K+1-1)), where L(K+1-1) is the transformation
                    // stored in columns K+1-1 and K+1 of A.
                    //
                    if( k+1<n )
                    {
                        v = 0.0;
                        for(i_=k+1; i_<=n-1;i_++)
                        {
                            v += b[i_]*a[i_,k];
                        }
                        b[k] = b[k]-v;
                        v = 0.0;
                        for(i_=k+1; i_<=n-1;i_++)
                        {
                            v += b[i_]*a[i_,k-1];
                        }
                        b[k-1] = b[k-1]-v;
                    }
                    
                    //
                    // Interchange rows K+1 and -IPIV(K+1).
                    //
                    kp = pivots[k]+n;
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    k = k-2;
                }
            }
        }
        x = new double[n-1+1];
        for(i_=0; i_<=n-1;i_++)
        {
            x[i_] = b[i_];
        }
        return result;
    }


    /*************************************************************************
    Решение системы линейных уравнений с симметричной матрицей системы.

    Входные параметры:
        A   -   матрица системы (верхний или нижний треугольник).
                Массив с нумерацией элементов [0..N-1, 0..N-1]
        B   -   правая часть системы.
                Массив с нумерацией элементов [0..N-1]
        N   -   размерность системы.
        IsUpper-Если IsUpper=True, то задан верхний треугольник матрицы, иначе
                переменная A содержит нижний треугольник.

    Выходные параметры:
        X   -   решение системы. Массив с нумерацией элементов [0..N-1]

    Результат:
        True    -   если система не вырождена. X содержит решение.
        False   -   если система вырождена (определитель матрицы строго равен
                    нолю). В  таком  случае X не содержит решение.

      -- ALGLIB --
         Copyright 2005 by Bochkanov Sergey
    *************************************************************************/
    public static bool smatrixsolve(double[,] a,
        ref double[] b,
        int n,
        bool isupper,
        ref double[] x)
    {
        bool result = new bool();
        int[] pivots = new int[0];

        a = (double[,])a.Clone();

        ldlt.smatrixldlt(ref a, n, isupper, ref pivots);
        result = smatrixldltsolve(ref a, ref pivots, b, n, isupper, ref x);
        return result;
    }


    /*************************************************************************
    Obsolete 1-based subroutine.
    *************************************************************************/
    public static bool solvesystemldlt(ref double[,] a,
        ref int[] pivots,
        double[] b,
        int n,
        bool isupper,
        ref double[] x)
    {
        bool result = new bool();
        int i = 0;
        int j = 0;
        int k = 0;
        int kp = 0;
        int km1 = 0;
        int km2 = 0;
        int kp1 = 0;
        int kp2 = 0;
        double ak = 0;
        double akm1 = 0;
        double akm1k = 0;
        double bk = 0;
        double bkm1 = 0;
        double denom = 0;
        double v = 0;
        int i_ = 0;

        b = (double[])b.Clone();

        
        //
        // Quick return if possible
        //
        result = true;
        if( n==0 )
        {
            return result;
        }
        
        //
        // Check that the diagonal matrix D is nonsingular
        //
        if( isupper )
        {
            
            //
            // Upper triangular storage: examine D from bottom to top
            //
            for(i=n; i>=1; i--)
            {
                if( pivots[i]>0 & a[i,i]==0 )
                {
                    result = false;
                    return result;
                }
            }
        }
        else
        {
            
            //
            // Lower triangular storage: examine D from top to bottom.
            //
            for(i=1; i<=n; i++)
            {
                if( pivots[i]>0 & a[i,i]==0 )
                {
                    result = false;
                    return result;
                }
            }
        }
        
        //
        // Solve Ax = b
        //
        if( isupper )
        {
            
            //
            // Solve A*X = B, where A = U*D*U'.
            //
            // First solve U*D*X = B, overwriting B with X.
            //
            // K is the main loop index, decreasing from N to 1 in steps of
            // 1 or 2, depending on the size of the diagonal blocks.
            //
            k = n;
            while( k>=1 )
            {
                if( pivots[k]>0 )
                {
                    
                    //
                    // 1 x 1 diagonal block
                    //
                    // Interchange rows K and IPIV(K).
                    //
                    kp = pivots[k];
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    
                    //
                    // Multiply by inv(U(K)), where U(K) is the transformation
                    // stored in column K of A.
                    //
                    km1 = k-1;
                    v = b[k];
                    for(i_=1; i_<=km1;i_++)
                    {
                        b[i_] = b[i_] - v*a[i_,k];
                    }
                    
                    //
                    // Multiply by the inverse of the diagonal block.
                    //
                    b[k] = b[k]/a[k,k];
                    k = k-1;
                }
                else
                {
                    
                    //
                    // 2 x 2 diagonal block
                    //
                    // Interchange rows K-1 and -IPIV(K).
                    //
                    kp = -pivots[k];
                    if( kp!=k-1 )
                    {
                        v = b[k-1];
                        b[k-1] = b[kp];
                        b[kp] = v;
                    }
                    
                    //
                    // Multiply by inv(U(K)), where U(K) is the transformation
                    // stored in columns K-1 and K of A.
                    //
                    km2 = k-2;
                    km1 = k-1;
                    v = b[k];
                    for(i_=1; i_<=km2;i_++)
                    {
                        b[i_] = b[i_] - v*a[i_,k];
                    }
                    v = b[k-1];
                    for(i_=1; i_<=km2;i_++)
                    {
                        b[i_] = b[i_] - v*a[i_,km1];
                    }
                    
                    //
                    // Multiply by the inverse of the diagonal block.
                    //
                    akm1k = a[k-1,k];
                    akm1 = a[k-1,k-1]/akm1k;
                    ak = a[k,k]/akm1k;
                    denom = akm1*ak-1;
                    bkm1 = b[k-1]/akm1k;
                    bk = b[k]/akm1k;
                    b[k-1] = (ak*bkm1-bk)/denom;
                    b[k] = (akm1*bk-bkm1)/denom;
                    k = k-2;
                }
            }
            
            //
            // Next solve U'*X = B, overwriting B with X.
            //
            // K is the main loop index, increasing from 1 to N in steps of
            // 1 or 2, depending on the size of the diagonal blocks.
            //
            k = 1;
            while( k<=n )
            {
                if( pivots[k]>0 )
                {
                    
                    //
                    // 1 x 1 diagonal block
                    //
                    // Multiply by inv(U'(K)), where U(K) is the transformation
                    // stored in column K of A.
                    //
                    km1 = k-1;
                    v = 0.0;
                    for(i_=1; i_<=km1;i_++)
                    {
                        v += b[i_]*a[i_,k];
                    }
                    b[k] = b[k]-v;
                    
                    //
                    // Interchange rows K and IPIV(K).
                    //
                    kp = pivots[k];
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    k = k+1;
                }
                else
                {
                    
                    //
                    // 2 x 2 diagonal block
                    //
                    // Multiply by inv(U'(K+1)), where U(K+1) is the transformation
                    // stored in columns K and K+1 of A.
                    //
                    km1 = k-1;
                    kp1 = k+1;
                    v = 0.0;
                    for(i_=1; i_<=km1;i_++)
                    {
                        v += b[i_]*a[i_,k];
                    }
                    b[k] = b[k]-v;
                    v = 0.0;
                    for(i_=1; i_<=km1;i_++)
                    {
                        v += b[i_]*a[i_,kp1];
                    }
                    b[k+1] = b[k+1]-v;
                    
                    //
                    // Interchange rows K and -IPIV(K).
                    //
                    kp = -pivots[k];
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    k = k+2;
                }
            }
        }
        else
        {
            
            //
            // Solve A*X = B, where A = L*D*L'.
            //
            // First solve L*D*X = B, overwriting B with X.
            //
            // K is the main loop index, increasing from 1 to N in steps of
            // 1 or 2, depending on the size of the diagonal blocks.
            //
            k = 1;
            while( k<=n )
            {
                if( pivots[k]>0 )
                {
                    
                    //
                    // 1 x 1 diagonal block
                    //
                    // Interchange rows K and IPIV(K).
                    //
                    kp = pivots[k];
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    
                    //
                    // Multiply by inv(L(K)), where L(K) is the transformation
                    // stored in column K of A.
                    //
                    if( k<n )
                    {
                        kp1 = k+1;
                        v = b[k];
                        for(i_=kp1; i_<=n;i_++)
                        {
                            b[i_] = b[i_] - v*a[i_,k];
                        }
                    }
                    
                    //
                    // Multiply by the inverse of the diagonal block.
                    //
                    b[k] = b[k]/a[k,k];
                    k = k+1;
                }
                else
                {
                    
                    //
                    // 2 x 2 diagonal block
                    //
                    // Interchange rows K+1 and -IPIV(K).
                    //
                    kp = -pivots[k];
                    if( kp!=k+1 )
                    {
                        v = b[k+1];
                        b[k+1] = b[kp];
                        b[kp] = v;
                    }
                    
                    //
                    // Multiply by inv(L(K)), where L(K) is the transformation
                    // stored in columns K and K+1 of A.
                    //
                    if( k<n-1 )
                    {
                        kp1 = k+1;
                        kp2 = k+2;
                        v = b[k];
                        for(i_=kp2; i_<=n;i_++)
                        {
                            b[i_] = b[i_] - v*a[i_,k];
                        }
                        v = b[k+1];
                        for(i_=kp2; i_<=n;i_++)
                        {
                            b[i_] = b[i_] - v*a[i_,kp1];
                        }
                    }
                    
                    //
                    // Multiply by the inverse of the diagonal block.
                    //
                    akm1k = a[k+1,k];
                    akm1 = a[k,k]/akm1k;
                    ak = a[k+1,k+1]/akm1k;
                    denom = akm1*ak-1;
                    bkm1 = b[k]/akm1k;
                    bk = b[k+1]/akm1k;
                    b[k] = (ak*bkm1-bk)/denom;
                    b[k+1] = (akm1*bk-bkm1)/denom;
                    k = k+2;
                }
            }
            
            //
            // Next solve L'*X = B, overwriting B with X.
            //
            // K is the main loop index, decreasing from N to 1 in steps of
            // 1 or 2, depending on the size of the diagonal blocks.
            //
            k = n;
            while( k>=1 )
            {
                if( pivots[k]>0 )
                {
                    
                    //
                    // 1 x 1 diagonal block
                    //
                    // Multiply by inv(L'(K)), where L(K) is the transformation
                    // stored in column K of A.
                    //
                    if( k<n )
                    {
                        kp1 = k+1;
                        v = 0.0;
                        for(i_=kp1; i_<=n;i_++)
                        {
                            v += b[i_]*a[i_,k];
                        }
                        b[k] = b[k]-v;
                    }
                    
                    //
                    // Interchange rows K and IPIV(K).
                    //
                    kp = pivots[k];
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    k = k-1;
                }
                else
                {
                    
                    //
                    // 2 x 2 diagonal block
                    //
                    // Multiply by inv(L'(K-1)), where L(K-1) is the transformation
                    // stored in columns K-1 and K of A.
                    //
                    if( k<n )
                    {
                        kp1 = k+1;
                        km1 = k-1;
                        v = 0.0;
                        for(i_=kp1; i_<=n;i_++)
                        {
                            v += b[i_]*a[i_,k];
                        }
                        b[k] = b[k]-v;
                        v = 0.0;
                        for(i_=kp1; i_<=n;i_++)
                        {
                            v += b[i_]*a[i_,km1];
                        }
                        b[k-1] = b[k-1]-v;
                    }
                    
                    //
                    // Interchange rows K and -IPIV(K).
                    //
                    kp = -pivots[k];
                    if( kp!=k )
                    {
                        v = b[k];
                        b[k] = b[kp];
                        b[kp] = v;
                    }
                    k = k-2;
                }
            }
        }
        x = new double[n+1];
        for(i_=1; i_<=n;i_++)
        {
            x[i_] = b[i_];
        }
        return result;
    }


    /*************************************************************************
    Obsolete 1-based subroutine
    *************************************************************************/
    public static bool solvesymmetricsystem(double[,] a,
        double[] b,
        int n,
        bool isupper,
        ref double[] x)
    {
        bool result = new bool();
        int[] pivots = new int[0];

        a = (double[,])a.Clone();
        b = (double[])b.Clone();

        ldlt.ldltdecomposition(ref a, n, isupper, ref pivots);
        result = solvesystemldlt(ref a, ref pivots, b, n, isupper, ref x);
        return result;
    }
}
