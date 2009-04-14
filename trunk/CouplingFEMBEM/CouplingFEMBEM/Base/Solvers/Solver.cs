using System;

namespace SbB.Diploma
{
    public class Solver
    {
        public static Vector Eval(Matrix a, Vector b)
        {
            int n = a.Size.n;

            int m = 0;
            for (int i = 0; i < n; i++)
            {
                int max = a.Size.n - 1;
                while (a[i][max] == 0.0) max--;
                max = max - i + 1;
                m = max > m ? max : m;
            }

            Vector vector = Line(a, n, m);
            Decomp(vector,n,m);
            return Solv(vector,b,n,m);
        }
        private static Vector Line(Matrix matrix, int n, int m)
        {
            Vector line = new Vector(n*m);
	        for (int i=0; i<n; i++)
		        for (int j=0; j<m; j++)
			        if (i+j<n)
                        line[m * i + j] = matrix[i][i + j];
            return line;
        }
        private static void Decomp(Vector vector, int n, int m)
        {
            int l, im, jm, km;
            double r, q, eps = 0.0;
            im = -m;
            for (int i = 0; i < n; i++)
            {
                im += m;
                l = m;
                if (l > n - i) 
                    l = n - i;
                q = 1.0 / vector[im];
                vector[im] = q;
                jm = im;
                for (int j = 1; j < l; j++)
                {
                    jm += m;
                    r = -vector[im + j] * q;
                    if (Math.Abs(r) >= eps)
                    {
                        km = jm - j;
                        for (int k = j; k < l; k++) 
                            vector[km + k] += r * vector[im + k];
                    }
                    vector[im + j] = r;
                }
            }
        }
        private static Vector Solv(Vector a, Vector b, int n, int m)
        {
            Vector f = (Vector) b.Clone();
            double eps = 0.0;
            int i, j, l, im;
            double r, u;
            im = -m;
            for (i = 0; i < n; i++)
            {
                im += m;
                l = m;
                if (l > n - i) l = n - i;
                for (j = 1; j < l; j++)
                {
                    r = a[im + j];
                    if (Math.Abs(r) >= eps)
                        f[i + j] += r * f[i];
                }
                f[i] *= a[im];
            }
            im += m;
            for (i = n - 1; i >= 0; i--)
            {
                im -= m;
                l = m;
                if (l > n - i) l = n - i;
                u = f[i];
                for (j = 1; j < l; j++) u += a[im + j] * f[i + j];
                f[i] = u;
            }
            return f;
        }
    }
}
