using System;

namespace SbB.Diploma
{
    public abstract class Triangle: FEMElement
    {
        #region Propeties
        public double S
        {
            get
            {
                Vertex a = nodes[0], b = nodes[1], c = nodes[2];
                return 0.5 * Math.Abs((b.X * c.Y - c.X * b.Y) - (a.X * c.Y - c.X * a.Y) + (a.X * b.Y - b.X * a.Y));
            }
        }
        #endregion

        #region Methods
        public bool isVertexInTriangle(Vertex v)
        {

            if (isVertexOnTriangle(v)) return false;
            Polygon p = new Polygon(new Vertex[] { nodes[0], nodes[1], nodes[2] });
            return p.isVertexInPolygon(v);
            Triangle a1, a2, a3;
            Vertex a = nodes[0], b = nodes[1], c = nodes[2];
            a1 = new LinearTriangle(a, b, v);
            a2 = new LinearTriangle(a, c, v);
            a3 = new LinearTriangle(c, b, v);

            return (Math.Abs(S - (a1.S + a2.S + a3.S))<=Constants.EPS);
        }
        public bool isVertexOnTriangle(Vertex v)
        {

            Vertex a = nodes[0], b = nodes[1], c = nodes[2];
            Edge Em = new Edge(a,b), Ej=new Edge(a,c), Ei = new Edge(b,c);
            return Em.hasVertex(v) || Ej.hasVertex(v) || Ei.hasVertex(v);
            Edge e1 = new Edge(a, v), e2 = new Edge(v, b);
            if (e1.Length + e2.Length == Em.Length) return true;

            e1 = new Edge(c, v); e2 = new Edge(v, b);
            if (e1.Length + e2.Length == Ei.Length) return true;

            e1 = new Edge(a, v); e2 = new Edge(v, c);
            if (e1.Length + e2.Length == Ej.Length) return true;

            return false;
        }
        public override bool hasVertex(Vertex v)
        {
            return isVertexOnTriangle(v) || isVertexInTriangle(v);
        }
        #endregion
    }
}
