using System;
using System.Collections.Generic;

namespace SbB.Diploma
{
    public abstract class FEMElement: IComparable  

    {
        #region Fields
        protected Vertex[] nodes;
        protected int number = 0;
        #endregion

        #region Properties
        public int N
        {
            get { return number; }
            set { number = value; }
        }
        public int NodesCount
        {
            get { return nodes.Length; }
        }
        public Vertex this[int i]
        {
            get { return nodes[i%NodesCount]; }
            set { nodes[i%NodesCount] = value; }
        }
        #endregion

        #region Methods
        public abstract void FEM(Matrix K, Matrix D);
        public abstract double phi(int i, Vertex v);
        public double phi(int i, double x, double y)
        {
            return phi(i, new Vertex(x, y));
        }
        public abstract double[] dphi(int i, Vertex v);
        public double[] dphi(int i, double x, double y)
        {
            return dphi(i, new Vertex(x, y));
        }

        public double Exx(Vertex vertex, Vector U)
        {
            double exx = 0.0;
            for (int i = 0; i < NodesCount; i++)
                exx += U[i]*dphi(i, vertex)[0];
            return exx;
        }
        public double Eyy(Vertex vertex, Vector V)
        {
            double eyy = 0.0;
            for (int i = 0; i < NodesCount; i++)
                eyy += V[i]*dphi(i, vertex)[1];
            return eyy;
        }
        public double Exy(Vertex vertex, Vector U, Vector V)
        {
            double exy = 0.0;
            for (int i = 0; i < NodesCount; i++)
            {
                double[] dN = dphi(i, vertex);
                exy+=U[i]*dN[1] + V[i]*dN[0];
            }
            return exy;
        }
        public double Sxx(Vertex vertex, Vector U, Vector V, Matrix D)
        {
            return D[0][0]*Exx(vertex, U) + D[0][1]*Eyy(vertex, V);
        }
        public double Syy(Vertex vertex, Vector U, Vector V, Matrix D)
        {
            return D[0][1]*Exx(vertex, U) + D[0][0]*Eyy(vertex, V);
        }
        public double Sxy(Vertex vertex, Vector U, Vector V, Matrix D)
        {
            return D[2][2]*Exy(vertex, U, V);
        }

        public abstract bool hasVertex(Vertex v);
        public int CompareTo(object obj)
        {
            FEMElement temp = (FEMElement) obj;
            Vertex v1 = nodes[0], v2 = temp.nodes[0];
            for (int i = 1; i < nodes.Length; i++)
                if (nodes[i] < v1) v1 = nodes[i];
            for (int i = 1; i < temp.nodes.Length; i++)
            {
                if (temp.nodes[i] < v2) v2 = temp.nodes[i];
            }
            return v1.CompareTo(v2);
        }
        #endregion
    }
}