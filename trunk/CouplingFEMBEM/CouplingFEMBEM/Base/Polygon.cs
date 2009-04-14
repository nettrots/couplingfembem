using System;
using System.Collections;

namespace SbB.Diploma
{
    public class Polygon
    {
        #region Fields
        private ArrayList vertexArray = new ArrayList();
        private Vertex min, max;
        #endregion

        #region Constructors
        public Polygon(Vertex[] points)
        {
            vertexArray.AddRange(points);
        }
        #endregion

        #region Properties
        public int Count
        {
            get { return vertexArray.Count; }
        }
        public Vertex this[int index]
        {
            get 
            {
                if (Count == 0) return null;
                while (index >= Count) index -= Count;
                return (Vertex)vertexArray[index]; 
            }
            set 
            {
                if (Count == 0) return;
                while (index >= Count) index -= Count;
                vertexArray[index] = value; 
            }
        }
        public Vertex MinVertex
        {
            get
            {
                if (min!=null) return min;
                min = new Vertex(double.PositiveInfinity, double.PositiveInfinity);
                for (int i = 0; i < Count; i++)
                {
                    if (min.X>this[i].X) min.X = this[i].X;
                    if (min.Y>this[i].Y) min.Y = this[i].Y;
                }
                return min;
            }
        }
        public Vertex MaxVertex
        {
            get
            {
                if (max != null) return max;
                max = new Vertex(double.NegativeInfinity, double.NegativeInfinity);
                for (int i = 0; i < Count; i++)
                {
                    if (max.X < this[i].X) max.X = this[i].X;
                    if (max.Y < this[i].Y) max.Y = this[i].Y;
                }
                return max;
            }
        }
        public double S
        {
            get
            {
                double s = 0.0;
                for (int i = 0; i < Count; i++)
                    s += (this[i].X + this[i + 1].X) * (this[i].Y - this[i + 1].Y);
                return Math.Abs(s) / 2;
            }
        }
        #endregion

        #region Methods
        public Edge edge(int i)
        {
            return new Edge(this[i], this[i + 1]);
        }
        public int isEdgeOnPolygon(Edge e)
        {
            for (int i = 0; i < Count; i++)
                if (null != (Object)(edge(i) & e)) return i;
            return -1;
        }
        public bool isVertexInPolygon(Vertex v)
        {
            if (isVertexOnPolygon(v)) return false;
            bool result;
            int i = 0;
            result = false;
            do
            {
                if (!((v.Y > this[i].Y) ^ (v.Y <= this[i + 1].Y)))
                    if (v.X - this[i].X < (v.Y - this[i].Y) * (this[i + 1].X - this[i].X) / (this[i + 1].Y - this[i].Y))
                        result = !result;
                i++;
            }
            while (i <= Count);
            return result;
        }
        public bool isVertexOnPolygon(Vertex v)
        {
            if (vertexArray.Contains(v)) return true;
            for (int i = 0; i < Count; i++)
                if (edge(i).classify(v) == VertexPos.BETWEEN)
                    return true;
            return false;
        }
        public bool hasVertex(Vertex v)
        {
            return isVertexOnPolygon(v) || isVertexInPolygon(v);
        }
        public bool hasElement(FEMElement femElement)
        {
            Vertex v = new Vertex(0.0, 0.0);
            for (int i = 0; i < femElement.NodesCount; i++)
            {
                if (!hasVertex(femElement[i])) return false;
                if (i<3) v += femElement[i];
            }
            v *= 1.0/3.0;

            return isVertexInPolygon(v);
        }
        public override string ToString()
        {
            String s = "";
            for (int i=0; i<Count; i++)
                s += (Vertex)vertexArray[i] + "\n";
            return s;
        }
        #endregion
    }
}
