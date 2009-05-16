using System;
using System.Collections.Generic;

namespace SbB.Diploma
{
    public abstract class Triangulation
    {
        #region Fields
        protected Polygon polygon;
        protected List<Vertex> vertexes;
        protected List<FEMElement> elements;
        protected List<BoundEdge>[] boundaries;
        #endregion

        #region Properties
        public List<Vertex> Vertexes
        {
            get
            {
                if (vertexes == null) throw new Exception("Triangulation did not complete");
                return vertexes;
            }
        }
        public List<FEMElement> Elements
        {
            get
            {
                if (elements == null) throw new Exception("Triangulation did not complete");
                return elements;
            }
 
        }
        public List<BoundEdge>[] Boundaries
        {
            get
            {
                if (boundaries == null) throw new Exception("Triangulation did not complete");
                return boundaries;
            }
        }
        #endregion

        #region Methods
        public abstract void triangulate(double minAngle, double maxArea);
        #endregion
    }
}