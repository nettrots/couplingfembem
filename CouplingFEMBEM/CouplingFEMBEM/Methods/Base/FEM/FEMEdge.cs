using System.Collections.Generic;

namespace SbB.Diploma
{
    public abstract class FEMEdge : BoundEdge
    {
        #region Constructors
        public FEMEdge(Vertex a, Vertex b): base(a,b) { }
        protected FEMEdge(){}
        #endregion

        #region Properties
        public abstract int Rank
        { get; }
        #endregion

        #region Methods
        public abstract void FEM(Vector V, Vertex p);
        public abstract void FEM(Matrix Af, int offset);
        #endregion
    }
}