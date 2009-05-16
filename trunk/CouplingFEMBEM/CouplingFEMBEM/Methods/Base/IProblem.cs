using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    public interface IProblem
    {
        double YoungModulus { get; set; }
        double PoissonRatio { get; set; }
        Polygon Polygon { get; set; }
        BoundaryClass[] BoundaryClasses { get; set; }
    }
}
