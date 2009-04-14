using System.Collections.Generic;

namespace SbB.Diploma
{
    public class LinearMortarMethod: MortarMethod
    {
        #region Fields
        private FunctionX Nu;
        private Phi phi;
        private int nlocal;
        private Edge gamma;
        //private List<MethodBase> methods;//dont know if needed
        private MortarSide[] mortarSides;//all mortars
        protected List<Vertex> vertexes;//temp vertexes
        #endregion

        #region Constructors
        public LinearMortarMethod(object data)
        {
           mortarSides=new MortarSide[1];
            mortarSides[1]=new MortarSide(null,null,null,typeof(LinearMortar));
        }
        #endregion

      
       

        public  void Run()
        {
            //create Ds
//                    public LinearMortar(int femnodescount, List<Vertex> vertexes)
//        {
//            matrix = new Matrix(2*femnodescount, 2*(vertexes.Count-2));
//            this.vertexes = vertexes;
//        }
            //BoundEdge
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
            throw new System.NotImplementedException();
        }
    }
}