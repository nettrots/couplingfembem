using System;

namespace SbB.Diploma
{
    public class Edge: IComparable
    {
        #region Fields
        protected Vertex[] nodes;
        #endregion

        #region Constructors
        public Edge(Vertex a, Vertex b)
        {
            nodes = new Vertex[] {a, b};
        }
        protected Edge(){}
        #endregion

        #region Properties
        public Vertex A
        {
            get { return nodes[0]; }
            set { nodes[0] = value; }
        }
        public Vertex B
        {
            get { return nodes[1]; }
            set { nodes[1] = value; }
        }
        public double Length
        {
            get { return (A - B).Length; }
        }
        public Vertex this[int index]
        {
            get { return nodes[index]; }
            set { nodes[index] = value; }
        }
        public int NodesCount
        {
            get { return nodes.Length; }
        }
        #endregion

        #region Methods
        public static bool operator ==(Edge eLeft, Edge eRight)
        {
            if ((object)eLeft == null || (object)eRight == null) return (object)eLeft == (object)eRight;
            return ((eLeft.A == eRight.A) && (eLeft.B == eRight.B)) ||
                   ((eLeft.A == eRight.B) && (eLeft.B == eRight.A));
        }
        public static bool operator !=(Edge eLeft, Edge eRight)
        {
            return !(eLeft == eRight);
        }
        public static Edge operator &(Edge eLeft, Edge eRight)
        {
//            if (eLeft.classify(eRight.a) == VertexPos.LEFT ||
//                eLeft.classify(eRight.a) == VertexPos.RIGHT ||
//                eLeft.classify(eRight.b) == VertexPos.BEHIND ||
//                eLeft.classify(eRight.b) == VertexPos.BEYOND)
//                return null;
//            if (eRight.classify(eLeft.a) == VertexPos.LEFT ||
//               eRight.classify(eLeft.a) == VertexPos.RIGHT ||
//               eRight.classify(eLeft.b) == VertexPos.BEHIND ||
//               eRight.classify(eLeft.b) == VertexPos.BEYOND)
//                return null;
//            Vertex[] varray = new Vertex[] { eRight.a, eRight.b, eLeft.a, eLeft.b };
//            Array.Sort(varray);
//            if (varray[1] == varray[2]) return null;
//            return new Edge(varray[1], varray[2]);
            if (eLeft == eRight) return eLeft;

            if (eLeft.classify(eRight.A) == VertexPos.BETWEEN && eLeft.classify(eRight.B) == VertexPos.BETWEEN)
                return eRight;
            if (eRight.classify(eLeft.A) == VertexPos.BETWEEN && eRight.classify(eLeft.B) == VertexPos.BETWEEN)
                return eLeft;
            if(eRight.classify(eLeft.A)==VertexPos.BETWEEN&&eLeft.classify(eRight.B)==VertexPos.BETWEEN)
                return new Edge(eLeft.A, eRight.B);
            if (eRight.classify(eLeft.B) == VertexPos.BETWEEN && eLeft.classify(eRight.A) == VertexPos.BETWEEN)
                return new Edge(eRight.A, eLeft.B);

            if (eRight.classify(eLeft.A) == VertexPos.BETWEEN && eLeft.B==eRight.B)
                return eLeft;
            if (eRight.classify(eLeft.B) == VertexPos.BETWEEN && eLeft.A == eRight.A)
                return eLeft;

            if (eLeft.classify(eRight.A) == VertexPos.BETWEEN && eLeft.B == eRight.B)
                return eRight;
            if (eLeft.classify(eRight.B) == VertexPos.BETWEEN && eLeft.A == eRight.A)
                return eRight;
            return null;
        }
        public static Edge operator |(Edge eLeft, Edge eRight)
        {
            if (eLeft.classify(eRight.A) == VertexPos.LEFT ||
                eLeft.classify(eRight.A) == VertexPos.RIGHT ||
                eLeft.classify(eRight.B) == VertexPos.LEFT ||
                eLeft.classify(eRight.B) == VertexPos.RIGHT)
                return null;
            Vertex[] varray = new Vertex[] { eRight.A, eRight.B, eLeft.A, eLeft.B };
            Array.Sort(varray);
            if (varray[0] == varray[3]) return null;
            return new Edge(varray[0], varray[3]);
        }

        public VertexPos classify(Vertex p)
        {
            return p.classify(A, B);
        }
        public bool hasVertex(Vertex p)
        {
            if (classify(p) == VertexPos.ORIGIN ||
                classify(p) == VertexPos.BETWEEN ||
                classify(p) == VertexPos.DESTINATION) return true;
            return false;
        }

        public override string ToString()
        {
            string result = string.Format("A({0}; {1})", A.X, A.Y);
            result = string.Format("{0}\tB({1}; {2})", result, B.X, B.Y);
            return "{ " + result + " }";
        }
        public override bool Equals(object obj)
        {
            return this == (Edge)obj;
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public int CompareTo(object obj)
        {
            Edge e = (Edge) obj;
            Vertex v1 = A.CompareTo(B) < 0 ? A : B;
            Vertex v2 = e.A.CompareTo(e.B) < 0 ? e.A : e.B;
            return v1.CompareTo(v2);
        }
        #endregion
    }
}
