using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    public class DoF
    {
        private int[] globalDofsNumbers;
        private int[] localDofsNumbers;
        private Vertex v;

        public DoF(Vertex v)
        {
            this.v = v;
        }

        public int[] GlobalDofsNumbers
        {
            get { return globalDofsNumbers; }
            set { globalDofsNumbers = value; }
        }

        public int[] LocalDofsNumbers
        {
            get { return localDofsNumbers; }
            set { localDofsNumbers = value; }
        }
    }
}
