using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    public abstract class MethodBase
    {
        public abstract void FillGlobalmatrix(Matrix global);
        public abstract void FillGlobalvector(Vector global);
        public abstract void Run();

    }
}
