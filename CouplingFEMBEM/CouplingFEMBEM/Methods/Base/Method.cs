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
        public virtual Vector Results
        {
            get { return results; }
            set { results = value; }
        }
        public Matrix K { get; protected set;}
        public Vector F { get; protected set; }
        public Polygon Polygon { get; set; }
        public virtual string Name { get; set; }
        #endregion

        #region Methods
        public abstract void Initialize();
        public abstract void Run();
        public abstract void Solve();
        public abstract double U(double x, double y);
        public abstract double V(double x, double y);
        public abstract double Sxx(double x, double y);
        public abstract double Syy(double x, double y);
        public abstract double Sxy(double x, double y);
        public abstract double[] U(Vertex[] vertices);
        public abstract double[] V(Vertex[] vertices);
        public abstract double[] Sxx(Vertex[] vertices);
        public abstract double[] Syy(Vertex[] vertices);
        public abstract double[] Sxy(Vertex[] vertices);

        public override string ToString()

        {
            return Name;
        }
        #endregion
    }
}
