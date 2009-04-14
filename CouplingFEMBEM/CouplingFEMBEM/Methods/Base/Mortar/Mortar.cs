using System.Collections.Generic;

namespace SbB.Diploma
{
    public abstract class Mortar
    {
        #region Fields
        protected Matrix matrix;
        protected List<Vertex> vertexes; 
        protected bool sign = true;
        #endregion

        #region Properties
        public bool Sign
        {
            get { return sign; }
            set { sign = value; }
        }
        public Matrix D
        {
            get { return matrix; }
        }
        #endregion

        #region Methods
        public abstract void visitFunction(int nlocal, int nglobal, Phi phi);
        #endregion


    }
}