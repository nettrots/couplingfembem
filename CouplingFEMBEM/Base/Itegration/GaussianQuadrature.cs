using System;

namespace SbB.Diploma
{
    public class GaussianQuadrature: Integration
    {
        #region Fields
        private const int n = 2;
        private static double[] x;
        private static double[] w;
        #endregion

        #region Constructors
        static GaussianQuadrature()
        {
            x = new double[n];
            w = new double[n];

            for(int i = 0; i <= (n+1)/2-1; i++)
            {
                double r = Math.Cos(Math.PI*(4*i+3)/(4*n+2));
                double dp3, r1;
                do
                {
                    double p2 = 0, p3 = 1;
                    for(int j = 0; j <= n-1; j++)
                    {
                        double p1 = p2;
                        p2 = p3;
                        p3 = ((2*j+1)*r*p2-j*p1)/(j+1);
                    }
                    dp3 = n*(r*p3-p2)/(r*r-1);
                    r1 = r;
                    r = r-p3/dp3;
                } while(Math.Abs(r-r1)>=Constants.EPS*(1+Math.Abs(r))*100);

                x[i] = r;
                x[n-1-i] = -r;
                w[i] = 2.0/((1-r*r)*dp3*dp3);
                w[n-1-i] = 2.0/((1-r*r)*dp3*dp3);
            }

        }
        public GaussianQuadrature(FunctionX f)
        {
            this.f = f;
        }
        #endregion

        #region Methods
        public override double defineIntegral(double a, double b)
        {
            double k = (b - a)/2, d = (a + b)/2;
            double rez = 0.0;
            for (int i = 0; i < n; i++)
                rez += w[i]*f(d + k*x[i]);
            rez *= k;
            return rez;
        }
        #endregion
    }
}