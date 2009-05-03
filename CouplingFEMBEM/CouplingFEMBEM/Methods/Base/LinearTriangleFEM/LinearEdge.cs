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
                for (int j = 0; j < this[i].Dofu.Length; j++)
                    V[2 * this[i].Dofu[j]] += p.X * Length / 2;    
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

        public override void FEM(Matrix Af, int offset)
        {
            Matrix local = new Matrix(4,4);
            local[0][0] = local[1][1] = local[2][2] = local[3][3] = 2;
            local[2][0] = local[0][2] = local[3][1] = local[1][3] = 1;
            local *= this.Length/6;

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < this[i].Doft.Length; k++)
                        for (int l = 0; l < this[j].Doft.Length; l++)
                            Af[this[i].Doft[k] - offset][this[j].Doft[l] - offset] += local[2*i + k][2*j + l];
        }
    }
}