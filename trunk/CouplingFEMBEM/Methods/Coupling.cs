using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    class CouplingMethod
    {
        private MethodBase[] methods;
        private Matrix GlobalMatrix;
        private Vector GlobalVector;
        private Vector Result;

        public CouplingMethod(string filename)
        {
            //Create methods and fill with data

        }

        public void assamble()
        {
            //Assamble vertexes (dofs) in all methods
        }

        public void fillSystem()
        {
            //Create system from all methods
            //FEM+BEM+Mortar
        }

        public void solveSystem()
        {
            LUSolve.Solve(GlobalMatrix, GlobalVector, Result);
        }
    }
}
