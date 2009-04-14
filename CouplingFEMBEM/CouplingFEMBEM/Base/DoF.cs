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

        private int number;

        public DoF(int number)
        {
            this.number = number;
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
