using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    public abstract class BoundEdge:Edge
    {
        public BoundEdge(Vertex a, Vertex b): base(a,b){}
        protected BoundEdge() { }
        public abstract void mortar(Mortar mortarvisitor);
    }
}
