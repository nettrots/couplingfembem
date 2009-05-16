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

class ldlt
{
    /*************************************************************************
    LDL^T ���������� ������������ �������

    �������� ������������ ������������ �������  (��  �����������  ������������
    ������������) � ����  A = L*D*L'  ���  A = U*D*U', ��� ������� D - ������-
    ������������ � �������  ��������  1x1  ���  2x2,  �������  L  (�������  U)
    - ������������ ����������������  (�����������������)  ������  �  ���������
    ���������� � ������ ������������.

    ������� ���������:
        A   -   ������������� �������. ������ � ���������� ��������� [0..N-1, 0..N-1].
                ���� IsUpper=True, �� � ������� ������������ ��������� ��������
                ������������ ������� A, � ������  ����������� ��  ������������
                (����������, ���� IsUpper=False).
        N   -   ������ ������������� �������
        IsUpper-��������, �����������  ������  �������  �������  (�������  ���
                ������ �������������).

    �������� ���������:
        A   -   ������� D � U, ���� IsUpper=True, ��� L, ���� IsUpper=False, �
                ����������  �����,  ����������  �������  (������)  �����������
                ������� A. ��� ���� ��������, ������������� ���� (����) �������
                ��������� �� ������������ � �� ��������������.
        Pivots- ������� ������������� ������������ (��. ����).



    ���� IsUpper=True, �� A = U*D*U', U=P(n)*U(n)*...*P(k)*U(k),  ���  �������
    P(k) - ������� ������������, ������� U(k) - ����������������� �  ���������
    ����������, � ������ k �����������, ������� � n-1, � ����� s ������ 1 ���  2
    (�������� �������� ������ ������� D).

            (   I    v    0   )   k-s+1
    U(k) =  (   0    I    0   )   s
            (   0    0    I   )   n-k-1
               k-s+1 s   n-k-1

    ���� Pivots[k]>=0, �� s=1, P(k) - ������������ ����� k � Pivots[k],  ������
    v,  �����������  �������  U(k),  ��������  �  ���������  A(0:k-1,k), D(k)
    �������� A(k,k). ���� Pivots[k]=Pivots[k-1]<0, �� s=2, P(k) - ������������
    ����� k-1 � N+Pivots[k-1], ������ v, ����������� �������  U(k), ��������  �
    A(0:k-1,k:k+1), ������� ����������� �����  D(k)  ��������  �   A(k,k),
    A(k,k+1) � A(k+1,k+1).



    ���� IsUpper=False, �� A = L*D*L', L=P(0)*L(0)*...*P(k)*L(k),  ���  �������
    P(k) - ������� ������������, ������� L(k) - ����������������  �  ���������
    ����������, � ������ k ������, ������� � 1, �  �����  s  ������  1  ���  2
    (�������� �������� ������ ������� D).

            (   I    0     0   )  k-1
    L(k) =  (   0    I     0   )  s
            (   0    v     I   )  n-k-s+1
               k-1   s  n-k-s+1

    ���� Pivots[k]>=0, �� s=1, P(k) - ������������ ����� k � Pivots[k],  ������
    v,  �����������  �������  L(k),  ��������  �  ���������  A(k+1:n-1,k), D(k)
    �������� A(k,k). ���� Pivots[k]=Pivots[k+1]<0, �� s=2, P(k) - ������������
    ����� k+1 � N+Pivots[k+1], ������ v, ����������� �������  L(k), �������� �
    A(k+2:n-1,k:k+1), ������ ����������� �����  D(k) ��������  � A(k,k), A(k+1,k)
    � A(k+1,k+1).



      -- LAPACK routine (version 3.0) --
         Univ. of Tennessee, Univ. of California Berkeley, NAG Ltd.,
         Courant Institute, Argonne National Lab, and Rice University
         June 30, 1999
    *************************************************************************/
    public static void smatrixldlt(ref double[,] a,
        int n,
        bool isupper,
        ref int[] pivots)
    {
        int i = 0;
        int imax = 0;
        int j = 0;
        int jmax = 0;
        int k = 0;
        int kk = 0;
        int kp = 0;
        int kstep = 0;
        double absakk = 0;
        double alpha = 0;
        double colmax = 0;
        double d11 = 0;
        double d12 = 0;
        double d21 = 0;
        double d22 = 0;
        double r1 = 0;
        double rowmax = 0;
        double t = 0;
        double wk = 0;
        double wkm1 = 0;
        double wkp1 = 0;
        int ii = 0;
        int i1 = 0;
        int i2 = 0;
        double vv = 0;
        double[] temp = new double[0];
        int i_ = 0;

        pivots = new int[n-1+1];
        temp = new double[n-1+1];
        
        //
        // Initialize ALPHA for use in choosing pivot block size.
        //
        alpha = (1+Math.Sqrt(17))/8;
        if( isupper )
        {
            
            //
            // Factorize A as U*D*U' using the upper triangle of A
            //
            //
            // K is the main loop index, decreasing from N to 1 in steps of
            // 1 or 2
            //
            k = n-1;
            while( k>=0 )
            {
                kstep = 1;
                
                //
                // Determine rows and columns to be interchanged and whether
                // a 1-by-1 or 2-by-2 pivot block will be used
                //
                absakk = Math.Abs(a[k,k]);
                
                //
                // IMAX is the row-index of the largest off-diagonal element in
                // column K+1, and COLMAX is its absolute value
                //
                if( k>0 )
                {
                    imax = 1;
                    for(ii=2; ii<=k; ii++)
                    {
                        if( Math.Abs(a[ii-1,k])>Math.Abs(a[imax-1,k]) )
                        {
                            imax = ii;
                        }
                    }
                    colmax = Math.Abs(a[imax-1,k]);
                }
                else
                {
                    colmax = 0;
                }
                if( Math.Max(absakk, colmax)==0 )
                {
                    
                    //
                    // Column K is zero
                    //
                    kp = k;
                }
                else
                {
                    if( absakk>=alpha*colmax )
                    {
                        
                        //
                        // no interchange, use 1-by-1 pivot block
                        //
                        kp = k;
                    }
                    else
                    {
                        
                        //
                        // JMAX is the column-index of the largest off-diagonal
                        // element in row IMAX, and ROWMAX is its absolute value
                        //
                        jmax = imax+1;
                        for(ii=imax+2; ii<=k+1; ii++)
                        {
                            if( Math.Abs(a[imax-1,ii-1])>Math.Abs(a[imax-1,jmax-1]) )
                            {
                                jmax = ii;
                            }
                        }
                        rowmax = Math.Abs(a[imax-1,jmax-1]);
                        if( imax>1 )
                        {
                            jmax = 1;
                            for(ii=2; ii<=imax-1; ii++)
                            {
                                if( Math.Abs(a[ii-1,imax-1])>Math.Abs(a[jmax-1,imax-1]) )
                                {
                                    jmax = ii;
                                }
                            }
                            rowmax = Math.Max(rowmax, Math.Abs(a[jmax-1,imax-1]));
                        }
                        vv = colmax/rowmax;
                        if( absakk>=alpha*colmax*vv )
                        {
                            
                            //
                            // no interchange, use 1-by-1 pivot block
                            //
                            kp = k;
                        }
                        else
                        {
                            if( Math.Abs(a[imax-1,imax-1])>=alpha*rowmax )
                            {
                                
                                //
                                // interchange rows and columns K and IMAX, use 1-by-1
                                // pivot block
                                //
                                kp = imax-1;
                            }
                            else
                            {
                                
                                //
                                // interchange rows and columns K-1 and IMAX, use 2-by-2
                                // pivot block
                                //
                                kp = imax-1;
                                kstep = 2;
                            }
                        }
                    }
                    kk = k+1-kstep;
                    if( kp+1!=kk+1 )
                    {
                        
                        //
                        // Interchange rows and columns KK and KP+1 in the leading
                        // submatrix A(0:K,0:K)
                        //
                        for(i_=0; i_<=kp-1;i_++)
                        {
                            temp[i_] = a[i_,kk];
                        }
                        for(i_=0; i_<=kp-1;i_++)
                        {
                            a[i_,kk] = a[i_,kp];
                        }
                        for(i_=0; i_<=kp-1;i_++)
                        {
                            a[i_,kp] = temp[i_];
                        }
                        for(i_=kp+1; i_<=kk-1;i_++)
                        {
                            temp[i_] = a[i_,kk];
                        }
                        for(i_=kp+1; i_<=kk-1;i_++)
                        {
                            a[i_,kk] = a[kp,i_];
                        }
                        for(i_=kp+1; i_<=kk-1;i_++)
                        {
                            a[kp,i_] = temp[i_];
                        }
                        t = a[kk,kk];
                        a[kk,kk] = a[kp,kp];
                        a[kp,kp] = t;
                        if( kstep==2 )
                        {
                            t = a[k-1,k];
                            a[k-1,k] = a[kp,k];
                            a[kp,k] = t;
                        }
                    }
                    
                    //
                    // Update the leading submatrix
                    //
                    if( kstep==1 )
                    {
                        
                        //
                        // 1-by-1 pivot block D(k): column k now holds
                        //
                        // W(k) = U(k)*D(k)
                        //
                        // where U(k) is the k-th column of U
                        //
                        // Perform a rank-1 update of A(1:k-1,1:k-1) as
                        //
                        // A := A - U(k)*D(k)*U(k)' = A - W(k)*1/D(k)*W(k)'
                        //
                        r1 = 1/a[k,k];
                        for(i=0; i<=k-1; i++)
                        {
                            vv = -(r1*a[i,k]);
                            for(i_=i; i_<=k-1;i_++)
                            {
                                a[i,i_] = a[i,i_] + vv*a[i_,k];
                            }
                        }
                        
                        //
                        // Store U(K+1) in column K+1
                        //
                        for(i_=0; i_<=k-1;i_++)
                        {
                            a[i_,k] = r1*a[i_,k];
                        }
                    }
                    else
                    {
                        
                        //
                        // 2-by-2 pivot block D(k): columns k and k-1 now hold
                        //
                        // ( W(k-1) W(k) ) = ( U(k-1) U(k) )*D(k)
                        //
                        // where U(k) and U(k-1) are the k-th and (k-1)-th columns
                        // of U
                        //
                        // Perform a rank-2 update of A(1:k-2,1:k-2) as
                        //
                        // A := A - ( U(k-1) U(k) )*D(k)*( U(k-1) U(k) )'
                        //    = A - ( W(k-1) W(k) )*inv(D(k))*( W(k-1) W(k) )'
                        //
                        if( k>1 )
                        {
                            d12 = a[k-1,k];
                            d22 = a[k-1,k-1]/d12;
                            d11 = a[k,k]/d12;
                            t = 1/(d11*d22-1);
                            d12 = t/d12;
                            for(j=k-2; j>=0; j--)
                            {
                                wkm1 = d12*(d11*a[j,k-1]-a[j,k]);
                                wk = d12*(d22*a[j,k]-a[j,k-1]);
                                for(i_=0; i_<=j;i_++)
                                {
                                    a[i_,j] = a[i_,j] - wk*a[i_,k];
                                }
                                for(i_=0; i_<=j;i_++)
                                {
                                    a[i_,j] = a[i_,j] - wkm1*a[i_,k-1];
                                }
                                a[j,k] = wk;
                                a[j,k-1] = wkm1;
                            }
                        }
                    }
                }
                
                //
                // Store details of the interchanges in IPIV
                //
                if( kstep==1 )
                {
                    pivots[k] = kp;
                }
                else
                {
                    pivots[k] = kp-n;
                    pivots[k-1] = kp-n;
                }
                
                //
                // Decrease K+1 and return to the start of the main loop
                //
                k = k-kstep;
            }
        }
        else
        {
            
            //
            // Factorize A as L*D*L' using the lower triangle of A
            //
            // K+1 is the main loop index, increasing from 1 to N in steps of
            // 1 or 2
            //
            k = 0;
            while( k<=n-1 )
            {
                kstep = 1;
                
                //
                // Determine rows and columns to be interchanged and whether
                // a 1-by-1 or 2-by-2 pivot block will be used
                //
                absakk = Math.Abs(a[k,k]);
                
                //
                // IMAX is the row-index of the largest off-diagonal element in
                // column K+1, and COLMAX is its absolute value
                //
                if( k<n-1 )
                {
                    imax = k+1+1;
                    for(ii=k+1+2; ii<=n; ii++)
                    {
                        if( Math.Abs(a[ii-1,k])>Math.Abs(a[imax-1,k]) )
                        {
                            imax = ii;
                        }
                    }
                    colmax = Math.Abs(a[imax-1,k]);
                }
                else
                {
                    colmax = 0;
                }
                if( Math.Max(absakk, colmax)==0 )
                {
                    
                    //
                    // Column K+1 is zero
                    //
                    kp = k;
                }
                else
                {
                    if( absakk>=alpha*colmax )
                    {
                        
                        //
                        // no interchange, use 1-by-1 pivot block
                        //
                        kp = k;
                    }
                    else
                    {
                        
                        //
                        // JMAX is the column-index of the largest off-diagonal
                        // element in row IMAX, and ROWMAX is its absolute value
                        //
                        jmax = k+1;
                        for(ii=k+1+1; ii<=imax-1; ii++)
                        {
                            if( Math.Abs(a[imax-1,ii-1])>Math.Abs(a[imax-1,jmax-1]) )
                            {
                                jmax = ii;
                            }
                        }
                        rowmax = Math.Abs(a[imax-1,jmax-1]);
                        if( imax<n )
                        {
                            jmax = imax+1;
                            for(ii=imax+2; ii<=n; ii++)
                            {
                                if( Math.Abs(a[ii-1,imax-1])>Math.Abs(a[jmax-1,imax-1]) )
                                {
                                    jmax = ii;
                                }
                            }
                            rowmax = Math.Max(rowmax, Math.Abs(a[jmax-1,imax-1]));
                        }
                        vv = colmax/rowmax;
                        if( absakk>=alpha*colmax*vv )
                        {
                            
                            //
                            // no interchange, use 1-by-1 pivot block
                            //
                            kp = k;
                        }
                        else
                        {
                            if( Math.Abs(a[imax-1,imax-1])>=alpha*rowmax )
                            {
                                
                                //
                                // interchange rows and columns K+1 and IMAX, use 1-by-1
                                // pivot block
                                //
                                kp = imax-1;
                            }
                            else
                            {
                                
                                //
                                // interchange rows and columns K+1+1 and IMAX, use 2-by-2
                                // pivot block
                                //
                                kp = imax-1;
                                kstep = 2;
                            }
                        }
                    }
                    kk = k+kstep-1;
                    if( kp!=kk )
                    {
                        
                        //
                        //              Interchange rows and columns KK+1 and KP+1 in the trailing
                        //              submatrix A(K+1:n,K+1:n)
                        //
                        if( kp+1<n )
                        {
                            for(i_=kp+1; i_<=n-1;i_++)
                            {
                                temp[i_] = a[i_,kk];
                            }
                            for(i_=kp+1; i_<=n-1;i_++)
                            {
                                a[i_,kk] = a[i_,kp];
                            }
                            for(i_=kp+1; i_<=n-1;i_++)
                            {
                                a[i_,kp] = temp[i_];
                            }
                        }
                        for(i_=kk+1; i_<=kp-1;i_++)
                        {
                            temp[i_] = a[i_,kk];
                        }
                        for(i_=kk+1; i_<=kp-1;i_++)
                        {
                            a[i_,kk] = a[kp,i_];
                        }
                        for(i_=kk+1; i_<=kp-1;i_++)
                        {
                            a[kp,i_] = temp[i_];
                        }
                        t = a[kk,kk];
                        a[kk,kk] = a[kp,kp];
                        a[kp,kp] = t;
                        if( kstep==2 )
                        {
                            t = a[k+1,k];
                            a[k+1,k] = a[kp,k];
                            a[kp,k] = t;
                        }
                    }
                    
                    //
                    // Update the trailing submatrix
                    //
                    if( kstep==1 )
                    {
                        
                        //
                        // 1-by-1 pivot block D(K+1): column K+1 now holds
                        //
                        // W(K+1) = L(K+1)*D(K+1)
                        //
                        // where L(K+1) is the K+1-th column of L
                        //
                        if( k+1<n )
                        {
                            
                            //
                            // Perform a rank-1 update of A(K+1+1:n,K+1+1:n) as
                            //
                            // A := A - L(K+1)*D(K+1)*L(K+1)' = A - W(K+1)*(1/D(K+1))*W(K+1)'
                            //
                            d11 = 1/a[k+1-1,k+1-1];
                            for(ii=k+1; ii<=n-1; ii++)
                            {
                                vv = -(d11*a[ii,k]);
                                for(i_=k+1; i_<=ii;i_++)
                                {
                                    a[ii,i_] = a[ii,i_] + vv*a[i_,k];
                                }
                            }
                            
                            //
                            // Store L(K+1) in column K+1
                            //
                            for(i_=k+1; i_<=n-1;i_++)
                            {
                                a[i_,k] = d11*a[i_,k];
                            }
                        }
                    }
                    else
                    {
                        
                        //
                        // 2-by-2 pivot block D(K+1)
                        //
                        if( k<n-2 )
                        {
                            
                            //
                            // Perform a rank-2 update of A(K+1+2:n,K+1+2:n) as
                            //
                            // A := A - ( (A(K+1) A(K+1+1))*D(K+1)**(-1) ) * (A(K+1) A(K+1+1))'
                            //
                            // where L(K+1) and L(K+1+1) are the K+1-th and (K+1+1)-th
                            // columns of L
                            //
                            d21 = a[k+1,k];
                            d11 = a[k+1,k+1]/d21;
                            d22 = a[k,k]/d21;
                            t = 1/(d11*d22-1);
                            d21 = t/d21;
                            for(j=k+2; j<=n-1; j++)
                            {
                                wk = d21*(d11*a[j,k]-a[j,k+1]);
                                wkp1 = d21*(d22*a[j,k+1]-a[j,k]);
                                for(i_=j; i_<=n-1;i_++)
                                {
                                    a[i_,j] = a[i_,j] - wk*a[i_,k];
                                }
                                for(i_=j; i_<=n-1;i_++)
                                {
                                    a[i_,j] = a[i_,j] - wkp1*a[i_,k+1];
                                }
                                a[j,k] = wk;
                                a[j,k+1] = wkp1;
                            }
                        }
                    }
                }
                
                //
                // Store details of the interchanges in IPIV
                //
                if( kstep==1 )
                {
                    pivots[k+1-1] = kp+1-1;
                }
                else
                {
                    pivots[k+1-1] = kp+1-1-n;
                    pivots[k+1+1-1] = kp+1-1-n;
                }
                
                //
                // Increase K+1 and return to the start of the main loop
                //
                k = k+kstep;
            }
        }
    }


    /*************************************************************************
    Obsolete subroutine.
    *************************************************************************/
    public static void ldltdecomposition(ref double[,] a,
        int n,
        bool isupper,
        ref int[] pivots)
    {
        int i = 0;
        int imax = 0;
        int j = 0;
        int jmax = 0;
        int k = 0;
        int kk = 0;
        int kp = 0;
        int kstep = 0;
        double absakk = 0;
        double alpha = 0;
        double colmax = 0;
        double d11 = 0;
        double d12 = 0;
        double d21 = 0;
        double d22 = 0;
        double r1 = 0;
        double rowmax = 0;
        double t = 0;
        double wk = 0;
        double wkm1 = 0;
        double wkp1 = 0;
        int ii = 0;
        int i1 = 0;
        int i2 = 0;
        double vv = 0;
        double[] temp = new double[0];
        int i_ = 0;

        pivots = new int[n+1];
        temp = new double[n+1];
        
        //
        // Initialize ALPHA for use in choosing pivot block size.
        //
        alpha = (1+Math.Sqrt(17))/8;
        if( isupper )
        {
            
            //
            // Factorize A as U*D*U' using the upper triangle of A
            //
            //
            // K is the main loop index, decreasing from N to 1 in steps of
            // 1 or 2
            //
            k = n;
            while( k>=1 )
            {
                kstep = 1;
                
                //
                // Determine rows and columns to be interchanged and whether
                // a 1-by-1 or 2-by-2 pivot block will be used
                //
                absakk = Math.Abs(a[k,k]);
                
                //
                // IMAX is the row-index of the largest off-diagonal element in
                // column K, and COLMAX is its absolute value
                //
                if( k>1 )
                {
                    imax = 1;
                    for(ii=2; ii<=k-1; ii++)
                    {
                        if( Math.Abs(a[ii,k])>Math.Abs(a[imax,k]) )
                        {
                            imax = ii;
                        }
                    }
                    colmax = Math.Abs(a[imax,k]);
                }
                else
                {
                    colmax = 0;
                }
                if( Math.Max(absakk, colmax)==0 )
                {
                    
                    //
                    // Column K is zero
                    //
                    kp = k;
                }
                else
                {
                    if( absakk>=alpha*colmax )
                    {
                        
                        //
                        // no interchange, use 1-by-1 pivot block
                        //
                        kp = k;
                    }
                    else
                    {
                        
                        //
                        // JMAX is the column-index of the largest off-diagonal
                        // element in row IMAX, and ROWMAX is its absolute value
                        //
                        jmax = imax+1;
                        for(ii=imax+2; ii<=k; ii++)
                        {
                            if( Math.Abs(a[imax,ii])>Math.Abs(a[imax,jmax]) )
                            {
                                jmax = ii;
                            }
                        }
                        rowmax = Math.Abs(a[imax,jmax]);
                        if( imax>1 )
                        {
                            jmax = 1;
                            for(ii=2; ii<=imax-1; ii++)
                            {
                                if( Math.Abs(a[ii,imax])>Math.Abs(a[jmax,imax]) )
                                {
                                    jmax = ii;
                                }
                            }
                            rowmax = Math.Max(rowmax, Math.Abs(a[jmax,imax]));
                        }
                        vv = colmax/rowmax;
                        if( absakk>=alpha*colmax*vv )
                        {
                            
                            //
                            // no interchange, use 1-by-1 pivot block
                            //
                            kp = k;
                        }
                        else
                        {
                            if( Math.Abs(a[imax,imax])>=alpha*rowmax )
                            {
                                
                                //
                                // interchange rows and columns K and IMAX, use 1-by-1
                                // pivot block
                                //
                                kp = imax;
                            }
                            else
                            {
                                
                                //
                                // interchange rows and columns K-1 and IMAX, use 2-by-2
                                // pivot block
                                //
                                kp = imax;
                                kstep = 2;
                            }
                        }
                    }
                    kk = k-kstep+1;
                    if( kp!=kk )
                    {
                        
                        //
                        // Interchange rows and columns KK and KP in the leading
                        // submatrix A(1:k,1:k)
                        //
                        i1 = kp-1;
                        for(i_=1; i_<=i1;i_++)
                        {
                            temp[i_] = a[i_,kk];
                        }
                        for(i_=1; i_<=i1;i_++)
                        {
                            a[i_,kk] = a[i_,kp];
                        }
                        for(i_=1; i_<=i1;i_++)
                        {
                            a[i_,kp] = temp[i_];
                        }
                        i1 = kp+1;
                        i2 = kk-1;
                        for(i_=i1; i_<=i2;i_++)
                        {
                            temp[i_] = a[i_,kk];
                        }
                        for(i_=i1; i_<=i2;i_++)
                        {
                            a[i_,kk] = a[kp,i_];
                        }
                        for(i_=i1; i_<=i2;i_++)
                        {
                            a[kp,i_] = temp[i_];
                        }
                        t = a[kk,kk];
                        a[kk,kk] = a[kp,kp];
                        a[kp,kp] = t;
                        if( kstep==2 )
                        {
                            t = a[k-1,k];
                            a[k-1,k] = a[kp,k];
                            a[kp,k] = t;
                        }
                    }
                    
                    //
                    // Update the leading submatrix
                    //
                    if( kstep==1 )
                    {
                        
                        //
                        // 1-by-1 pivot block D(k): column k now holds
                        //
                        // W(k) = U(k)*D(k)
                        //
                        // where U(k) is the k-th column of U
                        //
                        // Perform a rank-1 update of A(1:k-1,1:k-1) as
                        //
                        // A := A - U(k)*D(k)*U(k)' = A - W(k)*1/D(k)*W(k)'
                        //
                        r1 = 1/a[k,k];
                        for(i=1; i<=k-1; i++)
                        {
                            i2 = k-1;
                            vv = -(r1*a[i,k]);
                            for(i_=i; i_<=i2;i_++)
                            {
                                a[i,i_] = a[i,i_] + vv*a[i_,k];
                            }
                        }
                        
                        //
                        // Store U(k) in column k
                        //
                        i2 = k-1;
                        for(i_=1; i_<=i2;i_++)
                        {
                            a[i_,k] = r1*a[i_,k];
                        }
                    }
                    else
                    {
                        
                        //
                        // 2-by-2 pivot block D(k): columns k and k-1 now hold
                        //
                        // ( W(k-1) W(k) ) = ( U(k-1) U(k) )*D(k)
                        //
                        // where U(k) and U(k-1) are the k-th and (k-1)-th columns
                        // of U
                        //
                        // Perform a rank-2 update of A(1:k-2,1:k-2) as
                        //
                        // A := A - ( U(k-1) U(k) )*D(k)*( U(k-1) U(k) )'
                        //    = A - ( W(k-1) W(k) )*inv(D(k))*( W(k-1) W(k) )'
                        //
                        if( k>2 )
                        {
                            d12 = a[k-1,k];
                            d22 = a[k-1,k-1]/d12;
                            d11 = a[k,k]/d12;
                            t = 1/(d11*d22-1);
                            d12 = t/d12;
                            for(j=k-2; j>=1; j--)
                            {
                                wkm1 = d12*(d11*a[j,k-1]-a[j,k]);
                                wk = d12*(d22*a[j,k]-a[j,k-1]);
                                for(i_=1; i_<=j;i_++)
                                {
                                    a[i_,j] = a[i_,j] - wk*a[i_,k];
                                }
                                i1 = k-1;
                                for(i_=1; i_<=j;i_++)
                                {
                                    a[i_,j] = a[i_,j] - wkm1*a[i_,i1];
                                }
                                a[j,k] = wk;
                                a[j,k-1] = wkm1;
                            }
                        }
                    }
                }
                
                //
                // Store details of the interchanges in IPIV
                //
                if( kstep==1 )
                {
                    pivots[k] = kp;
                }
                else
                {
                    pivots[k] = -kp;
                    pivots[k-1] = -kp;
                }
                
                //
                // Decrease K and return to the start of the main loop
                //
                k = k-kstep;
            }
        }
        else
        {
            
            //
            // Factorize A as L*D*L' using the lower triangle of A
            //
            // K is the main loop index, increasing from 1 to N in steps of
            // 1 or 2
            //
            k = 1;
            while( k<=n )
            {
                kstep = 1;
                
                //
                // Determine rows and columns to be interchanged and whether
                // a 1-by-1 or 2-by-2 pivot block will be used
                //
                absakk = Math.Abs(a[k,k]);
                
                //
                // IMAX is the row-index of the largest off-diagonal element in
                // column K, and COLMAX is its absolute value
                //
                if( k<n )
                {
                    imax = k+1;
                    for(ii=k+2; ii<=n; ii++)
                    {
                        if( Math.Abs(a[ii,k])>Math.Abs(a[imax,k]) )
                        {
                            imax = ii;
                        }
                    }
                    colmax = Math.Abs(a[imax,k]);
                }
                else
                {
                    colmax = 0;
                }
                if( Math.Max(absakk, colmax)==0 )
                {
                    
                    //
                    // Column K is zero
                    //
                    kp = k;
                }
                else
                {
                    if( absakk>=alpha*colmax )
                    {
                        
                        //
                        // no interchange, use 1-by-1 pivot block
                        //
                        kp = k;
                    }
                    else
                    {
                        
                        //
                        // JMAX is the column-index of the largest off-diagonal
                        // element in row IMAX, and ROWMAX is its absolute value
                        //
                        jmax = k;
                        for(ii=k+1; ii<=imax-1; ii++)
                        {
                            if( Math.Abs(a[imax,ii])>Math.Abs(a[imax,jmax]) )
                            {
                                jmax = ii;
                            }
                        }
                        rowmax = Math.Abs(a[imax,jmax]);
                        if( imax<n )
                        {
                            jmax = imax+1;
                            for(ii=imax+2; ii<=n; ii++)
                            {
                                if( Math.Abs(a[ii,imax])>Math.Abs(a[jmax,imax]) )
                                {
                                    jmax = ii;
                                }
                            }
                            rowmax = Math.Max(rowmax, Math.Abs(a[jmax,imax]));
                        }
                        vv = colmax/rowmax;
                        if( absakk>=alpha*colmax*vv )
                        {
                            
                            //
                            // no interchange, use 1-by-1 pivot block
                            //
                            kp = k;
                        }
                        else
                        {
                            if( Math.Abs(a[imax,imax])>=alpha*rowmax )
                            {
                                
                                //
                                // interchange rows and columns K and IMAX, use 1-by-1
                                // pivot block
                                //
                                kp = imax;
                            }
                            else
                            {
                                
                                //
                                // interchange rows and columns K+1 and IMAX, use 2-by-2
                                // pivot block
                                //
                                kp = imax;
                                kstep = 2;
                            }
                        }
                    }
                    kk = k+kstep-1;
                    if( kp!=kk )
                    {
                        
                        //
                        //              Interchange rows and columns KK and KP in the trailing
                        //              submatrix A(k:n,k:n)
                        //
                        if( kp<n )
                        {
                            i1 = kp+1;
                            for(i_=i1; i_<=n;i_++)
                            {
                                temp[i_] = a[i_,kk];
                            }
                            for(i_=i1; i_<=n;i_++)
                            {
                                a[i_,kk] = a[i_,kp];
                            }
                            for(i_=i1; i_<=n;i_++)
                            {
                                a[i_,kp] = temp[i_];
                            }
                        }
                        i1 = kk+1;
                        i2 = kp-1;
                        for(i_=i1; i_<=i2;i_++)
                        {
                            temp[i_] = a[i_,kk];
                        }
                        for(i_=i1; i_<=i2;i_++)
                        {
                            a[i_,kk] = a[kp,i_];
                        }
                        for(i_=i1; i_<=i2;i_++)
                        {
                            a[kp,i_] = temp[i_];
                        }
                        t = a[kk,kk];
                        a[kk,kk] = a[kp,kp];
                        a[kp,kp] = t;
                        if( kstep==2 )
                        {
                            t = a[k+1,k];
                            a[k+1,k] = a[kp,k];
                            a[kp,k] = t;
                        }
                    }
                    
                    //
                    // Update the trailing submatrix
                    //
                    if( kstep==1 )
                    {
                        
                        //
                        // 1-by-1 pivot block D(k): column k now holds
                        //
                        // W(k) = L(k)*D(k)
                        //
                        // where L(k) is the k-th column of L
                        //
                        if( k<n )
                        {
                            
                            //
                            // Perform a rank-1 update of A(k+1:n,k+1:n) as
                            //
                            // A := A - L(k)*D(k)*L(k)' = A - W(k)*(1/D(k))*W(k)'
                            //
                            d11 = 1/a[k,k];
                            for(ii=k+1; ii<=n; ii++)
                            {
                                i1 = k+1;
                                i2 = ii;
                                vv = -(d11*a[ii,k]);
                                for(i_=i1; i_<=i2;i_++)
                                {
                                    a[ii,i_] = a[ii,i_] + vv*a[i_,k];
                                }
                            }
                            
                            //
                            // Store L(k) in column K
                            //
                            i1 = k+1;
                            for(i_=i1; i_<=n;i_++)
                            {
                                a[i_,k] = d11*a[i_,k];
                            }
                        }
                    }
                    else
                    {
                        
                        //
                        // 2-by-2 pivot block D(k)
                        //
                        if( k<n-1 )
                        {
                            
                            //
                            // Perform a rank-2 update of A(k+2:n,k+2:n) as
                            //
                            // A := A - ( (A(k) A(k+1))*D(k)**(-1) ) * (A(k) A(k+1))'
                            //
                            // where L(k) and L(k+1) are the k-th and (k+1)-th
                            // columns of L
                            //
                            d21 = a[k+1,k];
                            d11 = a[k+1,k+1]/d21;
                            d22 = a[k,k]/d21;
                            t = 1/(d11*d22-1);
                            d21 = t/d21;
                            for(j=k+2; j<=n; j++)
                            {
                                wk = d21*(d11*a[j,k]-a[j,k+1]);
                                wkp1 = d21*(d22*a[j,k+1]-a[j,k]);
                                ii = k+1;
                                for(i_=j; i_<=n;i_++)
                                {
                                    a[i_,j] = a[i_,j] - wk*a[i_,k];
                                }
                                for(i_=j; i_<=n;i_++)
                                {
                                    a[i_,j] = a[i_,j] - wkp1*a[i_,ii];
                                }
                                a[j,k] = wk;
                                a[j,k+1] = wkp1;
                            }
                        }
                    }
                }
                
                //
                // Store details of the interchanges in IPIV
                //
                if( kstep==1 )
                {
                    pivots[k] = kp;
                }
                else
                {
                    pivots[k] = -kp;
                    pivots[k+1] = -kp;
                }
                
                //
                // Increase K and return to the start of the main loop
                //
                k = k+kstep;
            }
        }
    }
}
