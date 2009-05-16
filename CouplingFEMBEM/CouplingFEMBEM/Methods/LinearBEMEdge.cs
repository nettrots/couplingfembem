using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma.Methods
{
    public class LinearBEMEdge:BoundEdge
    {
        #region Constructors
        public LinearBEMEdge(Vertex a, Vertex b) : base(a, b){}
        #endregion

        public void Converting(Matrix M)
        {
            Matrix local = new Matrix(4, 4);
            local[0][0] = local[1][1] = local[2][2] = local[3][3] = 2;
            local[2][0] = local[0][2] = local[3][1] = local[1][3] = 1;
            local *= this.Length / 6;

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    for (int k = 0; k < this[i].Dofu.Length; k++)
                        for (int l = 0; l < this[j].Dofu.Length; l++)
                            M[this[i].Dofu[k]][this[j].Dofu[l]] += local[2*i + k][2*j + l];
            ;
        }

        public override double phi(int i, double x, double y)
        {
            Vertex v = new Vertex(x, y);
            if (!hasVertex(v)) return 0;
            if (i != 0 && i != 1) return 0;
            return (v - this[(i + 1) % 2]).Length / Length;
        }

        public override void mortar(Mortar mortarvisitor)
        {
            throw new NotImplementedException();
        }
    }
}
