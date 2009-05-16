namespace SbB.Diploma
{
    public delegate double FunctionX(double x);

    public abstract class Integration
    {
        #region Fields
        protected FunctionX f;
        #endregion

        #region Methods
        public abstract double defineIntegral(double a, double b);
        #endregion
    }
}