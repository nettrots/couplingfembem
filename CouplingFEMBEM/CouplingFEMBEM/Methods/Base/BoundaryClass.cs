namespace SbB.Diploma
{
    public abstract class BoundaryClass
    {
        #region Methods
        public abstract BoundaryType type();
        #endregion
    }

    public class KinematicBoundary : BoundaryClass
    {
        #region Methods
        public override BoundaryType type()
        {
            return BoundaryType.KINEMATIC;
        }
        #endregion
    }

    public class StaticBoundary : BoundaryClass
    {
        #region Fields
        private Vertex p = new Vertex();
        #endregion

        #region Constructors
        public StaticBoundary() { }
        public StaticBoundary(double px, double py)
        {
            p.X = px;
            p.Y = py;
        }
        public StaticBoundary(Vertex p) : this(p.X, p.Y) { }
        #endregion

        #region Properties
        public Vertex P
        {
            get { return p; }
            set { p = value; }
        }
        #endregion

        #region Methods
        public override BoundaryType type()
        {
            return BoundaryType.STATIC;
        }
        #endregion
    }

    public class MortarBoundary: BoundaryClass
    {
        #region Methods
        public override BoundaryType type()
        {
            return BoundaryType.MORTAR;
        }
        #endregion
    }

    public class NonMortarBoundary: BoundaryClass
    {
        #region Methods
        public override BoundaryType type()
        {
            return BoundaryType.NONMORTAR;
        }
        #endregion
    }
}