using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    public class MortarMethod:MethodBase
    {
        protected List<MortarSide> mortarSides;
        public MortarMethod()
        {
            //List<MortarSide> mortarSides;
            //MortarSide ms=new MortarSide();
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

//            int m = vertexes.Count;
//            Matrix[] D = new Matrix[MortarSides.Length];
//            for (int i = 0; i < MortarSides.Length; i++)
//            {
//                Mortar mortar = MortarSides[i].createMortar(m);
//                mortar.Sign = true;
//                foreach (SubDomain subDomain in MortarSides[i].Mortars)
//                    for (int j = 0; j < MortarSides[i].NodesCount - 1; j++)
//                    {
//                        int k = subDomain.Polygon.isEdgeOnPolygon(MortarSides[i].edge(j));
//                        if (k >= 0)
//                            foreach (FEMEdge femEdge in subDomain.Boundaries[k])
//                                femEdge.mortar(mortar);
//                    }
//                mortar.Sign = false;
//                foreach (SubDomain subDomain in MortarSides[i].Nonmortars)
//                    for (int j = 0; j < MortarSides[i].NodesCount - 1; j++)
//                    {
//                        int k = subDomain.Polygon.isEdgeOnPolygon(MortarSides[i].edge(j));
//                        if (k >= 0)
//                            foreach (FEMEdge femEdge in subDomain.Boundaries[k])
//                                femEdge.mortar(mortar);
//                    }
//                D[i] = mortar.D;
//            }
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
