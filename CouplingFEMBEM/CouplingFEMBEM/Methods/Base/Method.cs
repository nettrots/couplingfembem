using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    public abstract class MethodBase
    {
        #region Fields
        protected Polygon polygon;
        protected double youngModulus = 21000;
        protected double poissonRatio = 0.3;


        #endregion

        #region Properties
        public virtual double YoungModulus
        {
            get { return youngModulus; }
            set { youngModulus = value; }
        }
        public virtual double PoissonRatio
        {
            get { return poissonRatio; }
            set { poissonRatio = value; }
        }
        public Polygon Polygon
        {
            get { return polygon; }
        }
        #endregion

        #region Methods
        public abstract void Initialize();
        public abstract void Run();
        public abstract void FillGlobalmatrix(Matrix global);
        public abstract void FillGlobalvector(Vector global);
        #endregion
    }
}
