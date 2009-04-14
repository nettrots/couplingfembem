using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    class LinearMortar:Mortar
    {
        #region Fields
        private FunctionX Nu;
        private Phi phi;
        private int nlocal;
        private Edge gamma;
        #endregion

        #region Constructors
        public LinearMortar(int femnodescount, List<Vertex> vertexes)
        {
            matrix = new Matrix(2 * femnodescount, 2 * (vertexes.Count - 2));
            this.vertexes = vertexes;
        }
        #endregion

        #region Methods
        #region Private
        private double Nu0(double t)
        {
            return 1;
        }
        private double Nu1(double t)
        {
            return (1 - t);
        }
        private double Nu2(double t)
        {
            return t;
        }
        private double integratedFunction(double t)
        {
            double x = gamma.A.X + (gamma.B.X - gamma.A.X) * t;
            double y = gamma.A.Y + (gamma.B.Y - gamma.A.Y) * t;
            return Nu(t) * phi(nlocal, x, y) * gamma.Length;
        }
        #endregion

        #region Public
        public override void visitFunction(int nlocal, int nglobal, Phi phi)
        {
            Integration integration = new GaussianQuadrature(integratedFunction);
            double integral;

            this.phi = phi;
            this.nlocal = nlocal;

            gamma = new Edge(vertexes[0], vertexes[1]);
            Nu = Nu0;
            integral = sign ? integration.defineIntegral(0.0, 1.0) : -integration.defineIntegral(0.0, 1.0);
            matrix[2 * nglobal][0] += integral;
            matrix[2 * nglobal + 1][1] += integral;

            for (int i = 1; i < vertexes.Count - 2; i++)
            {
                gamma = new Edge(vertexes[i], vertexes[i + 1]);

                Nu = Nu1;
                integral = sign ? integration.defineIntegral(0.0, 1.0) : -integration.defineIntegral(0.0, 1.0);
                matrix[2 * nglobal][2 * (i - 1)] += integral;
                matrix[2 * nglobal + 1][2 * (i - 1) + 1] += integral;

                Nu = Nu2;
                integral = sign ? integration.defineIntegral(0.0, 1.0) : -integration.defineIntegral(0.0, 1.0);
                matrix[2 * nglobal][2 * i] += integral;
                matrix[2 * nglobal + 1][2 * i + 1] += integral;
            }

            gamma = new Edge(vertexes[vertexes.Count - 2], vertexes[vertexes.Count - 1]);
            Nu = Nu0;
            integral = sign ? integration.defineIntegral(0.0, 1.0) : -integration.defineIntegral(0.0, 1.0);
            matrix[2 * nglobal][2 * (vertexes.Count - 2) - 2] += integral;
            matrix[2 * nglobal + 1][2 * (vertexes.Count - 2) - 1] += integral;
        }
        #endregion
        #endregion
    }
}
