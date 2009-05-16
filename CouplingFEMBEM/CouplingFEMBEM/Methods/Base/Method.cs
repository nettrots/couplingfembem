using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    public abstract class BPMethod
    {
        #region Fields
        protected Vector results;
        #endregion

        #region Properties
        public Vector Results
        {
            get { return results; }
        }
        public Matrix K { get; protected set;}
        public Vector F { get; protected set; }
        public Polygon Polygon { get; set; }
        #endregion

        #region Methods
        public abstract void Initialize();
        public abstract void Run();
        public abstract void Solve();
        #endregion
    }
}
