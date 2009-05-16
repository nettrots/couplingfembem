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
        public LinearMortar(List<Vertex> vertexes)
        {
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
        private void visitFunction(int indexLocal, int indexU, int indexV, Phi phi)
        {
            Integration integration = new GaussianQuadrature(integratedFunction);
            double integral;

            this.phi = phi;
            this.nlocal = indexLocal;

            gamma = new Edge(vertexes[0], vertexes[1]);
            Nu = Nu0;
            integral = integration.defineIntegral(0.0, 1.0);
            matrix[indexU][0] += integral;
            matrix[indexV][1] += integral;

            for (int i = 1; i < vertexes.Count - 2; i++)
            {
                gamma = new Edge(vertexes[i], vertexes[i + 1]);

                Nu = Nu1;
                integral = integration.defineIntegral(0.0, 1.0);
                matrix[indexU][2*(i - 1)] += integral;
                matrix[indexV][2*(i - 1) + 1] += integral;

                Nu = Nu2;
                integral = integration.defineIntegral(0.0, 1.0);
                matrix[indexU][2*i] += integral;
                matrix[indexV][2*i + 1] += integral;
            }

            gamma = new Edge(vertexes[vertexes.Count - 2], vertexes[vertexes.Count - 1]);
            Nu = Nu0;
            integral = integration.defineIntegral(0.0, 1.0);
            matrix[indexU][2*(vertexes.Count - 2) - 2] += integral;
            matrix[indexV][2*(vertexes.Count - 2) - 1] += integral;
        }

        public override Matrix createD(int DoFsCount, List<BoundEdge>[] boundaries)
        {
            matrix = new Matrix(DoFsCount, 2*(vertexes.Count-2));

            foreach (List<BoundEdge> boundary in boundaries)
            {
                foreach (BoundEdge edge in boundary)
                {
                    for (int i = 0; i < edge.NodesCount; i++)
                    {
                        visitFunction(i, edge[i].Dofu[0], edge[i].Dofu[1], edge.phi);               
                    }
                }
            }

            return matrix;
        }

        #endregion
        #endregion
    }
}
