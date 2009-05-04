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

        public override void mortar(Mortar mortarvisitor)
        {
            throw new NotImplementedException();
        }
    }
}
