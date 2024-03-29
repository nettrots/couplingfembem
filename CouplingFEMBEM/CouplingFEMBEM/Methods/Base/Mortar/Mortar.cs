using System.Collections.Generic;

namespace SbB.Diploma
{
    public abstract class Mortar
    {
        #region Fields
        protected Matrix matrix;
        protected List<Vertex> vertexes; 
        #endregion

        #region Methods
        public abstract Matrix createD(int DoFsCount, List<BoundEdge>[] boundaries);
        #endregion


    }
}