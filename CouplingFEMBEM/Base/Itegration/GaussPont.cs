namespace SbB.Diploma
{
    public class GaussPont
    {
        #region Fields
        private double[][] Lk = new double[2][] { 
                new double[] {1.0/3, 1, 0, 0, 0, 0.5, 0.5},
                new double[] {1.0/3, 0, 1, 0, 0.5, 0, 0.5}};
        private double[] wk = new double[] { 0.225, 0.025, 0.025, 0.025, 8.0 / 120, 8.0 / 120, 8.0 / 120 };
        #endregion

        #region Properties
        public int Count
        {
            get { return wk.Length; }
        }
        #endregion

        #region Methods
        public double L1(int i)
        {
            return Lk[0][i];
        }
        public double L2(int i)
        {
            return Lk[1][i];
        }
        public double w(int i)
        {
            return wk[i];
        }
        #endregion
    }
}