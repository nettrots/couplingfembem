using System.Collections.Generic;

namespace SbB.Diploma
{
    public class LinearEdge: FEMEdge
    {
        #region Constructors
        public LinearEdge(Vertex a, Vertex b): base(a,b) {}
        #endregion

        #region Properties
        public override int Rank
        {
            get { return 2; }
        }
        #endregion

        #region Methods
        public override void FEM(Vector V, Vertex p)
        {
            for (int i = 0; i < 2; i++)
            {
                V[2*this[i].Number] += p.X*Length/2;
                V[2*this[i].Number + 1] += p.Y*Length/2;
            }
        }
        public override double phi(int i, double x, double y)
        {
            Vertex v = new Vertex(x,y);
            if (!hasVertex(v)) return 0;
            if (i != 0 && i != 1) return 0;
            return (v - this[(i + 1) % 2]).Length / Length;
        }
        #endregion
    }
}