using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma.Methods
{
    public class MakarBEMMethod:MethodBase
    {
        private Matrix H, G;
        private Matrix A;
        private Vector b;
        private List<Vertex> vertexes;
        //private List<FEMElement> elements;
        private List<BoundEdge>[] boundaries;

        public MakarBEMMethod(object somedata)
        {
            
        }

        public override void FillGlobalmatrix(Matrix global)
        {
            throw new NotImplementedException();
        }

        public override void FillGlobalvector(Vector global)
        {
            throw new NotImplementedException();
        }

        public override void Run()
        {
            //Create A an b from G and H
            throw new NotImplementedException();
        }
    }
}
