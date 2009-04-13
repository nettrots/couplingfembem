using System;
using System.Collections.Generic;

namespace SbB.Diploma
{
    public class LinearTriangle: Triangle
    {
        #region Constructors
        public LinearTriangle(Vertex a, Vertex b, Vertex c) : this(a, b, c, 0) {}
        public LinearTriangle(Vertex a, Vertex b, Vertex c, int number)
        {
            this.number = number;
            nodes = new Vertex[] {a,b,c};
        }
        public LinearTriangle(Vertex[] vertexes) : this(vertexes, 0) {}
        public LinearTriangle(Vertex[] vertexes, int number)
        {
            if (vertexes.Length>3) throw new Exception("Too much elements in 'vertexes'");
            if (vertexes.Length<3) throw new Exception("Not enought elements in 'vertexes'");
            nodes = new Vertex[] {vertexes[0], vertexes[1], vertexes[2]};
            this.number = number;
        }
        #endregion

        #region Methods
        public override void FEM(Matrix K, Matrix D)
        {
            Matrix B = new Matrix(3, 6);
            for (int i = 0; i < 3; i++)
            {
                B[0][i] = B[2][i+3] = (this[i+1].Y - this[i+2].Y) / 2;
                B[1][i+3] = B[2][i] = (this[i+2].X - this[i+1].X) / 2;
            }
            Matrix local = 1.0/S*B.Transposed()*D*B;
            for (int k = 0; k < 2; k++)
                for (int l = 0; l < 2; l++)
                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                            K[2*this[i].Number + k][2*this[j].Number + l] += local[3*k + i][3*l + j];
        }
        public override double phi(int i, Vertex v)
        {
            if (!hasVertex(v)) return 0;
            double ai = this[i + 1].X*this[i + 2].Y - this[i + 2].X*this[i + 1].Y;
            double bi = this[i + 1].Y - this[i + 2].Y;
            double ci = this[i + 2].X - this[i + 1].X;
            return (ai + bi*v.X + ci*v.Y)/(2*S);
            Triangle tr = new LinearTriangle(v, this[i + 1], this[i + 2]);
            return tr.S / S;
        }
        public override double[] dphi(int i, Vertex v)
        {
            double[] dphixy = new double[2];
            dphixy[0] = (this[i + 1].Y - this[i + 2].Y)/(2*S);
            dphixy[1] = (this[i + 2].X - this[i + 1].X)/(2*S);
            return dphixy;
        }
        #endregion
    }
}