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
using SbB.Diploma;

class trinverse
{
    /*************************************************************************
    Обращение треугольной матрицы

    Подпрограмма обращает следующие типы матриц:
        * верхнетреугольные
        * верхнетреугольные с единичной диагональю
        * нижнетреугольные
        * нижнетреугольные с единичной диагональю

    В случае, если матрица верхне(нижне)треугольная, то  матрица,  обратная  к
    ней, тоже верхне(нижне)треугольная, и после  завершения  работы  алгоритма
    обратная матрица замещает переданную. При этом элементы расположенные ниже
    (выше) диагонали не меняются в ходе работы алгоритма.

    Если матрица с единичной  диагональю, то обратная к  ней  матрица  тоже  с
    единичной  диагональю.  В  алгоритм  передаются  только    внедиагональные
    элементы. При этом в результате работы алгоритма диагональные элементы  не
    меняются.

    Входные параметры:
        A           -   матрица. Массив с нумерацией элементов [0..N-1,0..N-1]
        N           -   размер матрицы
        IsUpper     -   True, если матрица верхнетреугольная
        IsUnitTriangular-   True, если матрица с единичной диагональю.

    Выходные параметры:
        A           -   матрица, обратная к входной, если задача не вырождена.

    Результат:
        True, если матрица не вырождена
        False, если матрица вырождена

      -- LAPACK routine (version 3.0) --
         Univ. of Tennessee, Univ. of California Berkeley, NAG Ltd.,
         Courant Institute, Argonne National Lab, and Rice University
         February 29, 1992
    *************************************************************************/
    public static bool rmatrixtrinverse(Matrix a,
        int n,
        bool isupper,
        bool isunittriangular)
    {
        bool result = new bool();
        bool nounit = new bool();
        int i = 0;
        int j = 0;
        double v = 0;
        double ajj = 0;
        double[] t = new double[0];
        int i_ = 0;

        result = true;
        t = new double[n-1+1];
        
        //
        // Test the input parameters.
        //
        nounit = !isunittriangular;
        if( isupper )
        {
            
            //
            // Compute inverse of upper triangular matrix.
            //
            for(j=0; j<=n-1; j++)
            {
                if( nounit )
                {
                    if( a[j][j]==0 )
                    {
                        result = false;
                        return result;
                    }
                    a[j][j] = 1/a[j][j];
                    ajj = -a[j][j];
                }
                else
                {
                    ajj = -1;
                }
                
                //
                // Compute elements 1:j-1 of j-th column.
                //
                if( j>0 )
                {
                    for(i_=0; i_<=j-1;i_++)
                    {
                        t[i_] = a[i_][j];
                    }
                    for(i=0; i<=j-1; i++)
                    {
                        if( i<j-1 )
                        {
                            v = 0.0;
                            for(i_=i+1; i_<=j-1;i_++)
                            {
                                v += a[i][i_]*t[i_];
                            }
                        }
                        else
                        {
                            v = 0;
                        }
                        if( nounit )
                        {
                            a[i][j] = v+a[i][i]*t[i];
                        }
                        else
                        {
                            a[i][j] = v+t[i];
                        }
                    }
                    for(i_=0; i_<=j-1;i_++)
                    {
                        a[i_][j] = ajj*a[i_][j];
                    }
                }
            }
        }
        else
        {
            
            //
            // Compute inverse of lower triangular matrix.
            //
            for(j=n-1; j>=0; j--)
            {
                if( nounit )
                {
                    if( a[j][j]==0 )
                    {
                        result = false;
                        return result;
                    }
                    a[j][j] = 1/a[j][j];
                    ajj = -a[j][j];
                }
                else
                {
                    ajj = -1;
                }
                if( j<n-1 )
                {
                    
                    //
                    // Compute elements j+1:n of j-th column.
                    //
                    for(i_=j+1; i_<=n-1;i_++)
                    {
                        t[i_] = a[i_][j];
                    }
                    for(i=j+1; i<=n-1; i++)
                    {
                        if( i>j+1 )
                        {
                            v = 0.0;
                            for(i_=j+1; i_<=i-1;i_++)
                            {
                                v += a[i][i_]*t[i_];
                            }
                        }
                        else
                        {
                            v = 0;
                        }
                        if( nounit )
                        {
                            a[i][j] = v+a[i][i]*t[i];
                        }
                        else
                        {
                            a[i][j] = v+t[i];
                        }
                    }
                    for(i_=j+1; i_<=n-1;i_++)
                    {
                        a[i_][j] = ajj*a[i_][j];
                    }
                }
            }
        }
        return result;
    }


    /*************************************************************************
    Obsolete 1-based subroutine.
    See RMatrixTRInverse for 0-based replacement.
    *************************************************************************/
    public static bool invtriangular(ref double[,] a,
        int n,
        bool isupper,
        bool isunittriangular)
    {
        bool result = new bool();
        bool nounit = new bool();
        int i = 0;
        int j = 0;
        int nmj = 0;
        int jm1 = 0;
        int jp1 = 0;
        double v = 0;
        double ajj = 0;
        double[] t = new double[0];
        int i_ = 0;

        result = true;
        t = new double[n+1];
        
        //
        // Test the input parameters.
        //
        nounit = !isunittriangular;
        if( isupper )
        {
            
            //
            // Compute inverse of upper triangular matrix.
            //
            for(j=1; j<=n; j++)
            {
                if( nounit )
                {
                    if( a[j,j]==0 )
                    {
                        result = false;
                        return result;
                    }
                    a[j,j] = 1/a[j,j];
                    ajj = -a[j,j];
                }
                else
                {
                    ajj = -1;
                }
                
                //
                // Compute elements 1:j-1 of j-th column.
                //
                if( j>1 )
                {
                    jm1 = j-1;
                    for(i_=1; i_<=jm1;i_++)
                    {
                        t[i_] = a[i_,j];
                    }
                    for(i=1; i<=j-1; i++)
                    {
                        if( i<j-1 )
                        {
                            v = 0.0;
                            for(i_=i+1; i_<=jm1;i_++)
                            {
                                v += a[i,i_]*t[i_];
                            }
                        }
                        else
                        {
                            v = 0;
                        }
                        if( nounit )
                        {
                            a[i,j] = v+a[i,i]*t[i];
                        }
                        else
                        {
                            a[i,j] = v+t[i];
                        }
                    }
                    for(i_=1; i_<=jm1;i_++)
                    {
                        a[i_,j] = ajj*a[i_,j];
                    }
                }
            }
        }
        else
        {
            
            //
            // Compute inverse of lower triangular matrix.
            //
            for(j=n; j>=1; j--)
            {
                if( nounit )
                {
                    if( a[j,j]==0 )
                    {
                        result = false;
                        return result;
                    }
                    a[j,j] = 1/a[j,j];
                    ajj = -a[j,j];
                }
                else
                {
                    ajj = -1;
                }
                if( j<n )
                {
                    
                    //
                    // Compute elements j+1:n of j-th column.
                    //
                    nmj = n-j;
                    jp1 = j+1;
                    for(i_=jp1; i_<=n;i_++)
                    {
                        t[i_] = a[i_,j];
                    }
                    for(i=j+1; i<=n; i++)
                    {
                        if( i>j+1 )
                        {
                            v = 0.0;
                            for(i_=jp1; i_<=i-1;i_++)
                            {
                                v += a[i,i_]*t[i_];
                            }
                        }
                        else
                        {
                            v = 0;
                        }
                        if( nounit )
                        {
                            a[i,j] = v+a[i,i]*t[i];
                        }
                        else
                        {
                            a[i,j] = v+t[i];
                        }
                    }
                    for(i_=jp1; i_<=n;i_++)
                    {
                        a[i_,j] = ajj*a[i_,j];
                    }
                }
            }
        }
        return result;
    }
}
