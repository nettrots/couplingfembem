using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    public class MortarMethod:MethodBase
    {
        private List<MortarSide> mortarSides;

        public List<MortarSide> MortarSides
        {
            get { return mortarSides; }
            set { mortarSides = value; }
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
            foreach (MortarSide side in mortarSides)
            {
                side.createMortarNodes();
                side.createMortar(side.Vertexes.Count);
            }
            throw new NotImplementedException();
        }

        public override void FillGlobalmatrix(Matrix global)
        {
            foreach (MortarSide side in mortarSides)
            {
                //side.Mortar.
            }
            throw new NotImplementedException();
        }

        public override void FillGlobalvector(Vector global)
        {
            throw new NotImplementedException();
        }
    }
}
