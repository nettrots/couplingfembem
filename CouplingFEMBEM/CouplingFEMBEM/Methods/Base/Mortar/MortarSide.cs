using System;
using System.Collections.Generic;
using System.Reflection;

namespace SbB.Diploma
{
    public class MortarSide
    {
        #region Fields
        private Vertex[] nodes;
        private MethodBase mortar;
        private MethodBase nonmortar;
        private List<Vertex> vertexes;
        //private Mortar mortar;
        private readonly Type mortarType;
        #endregion

        #region Constructors
        public MortarSide(MethodBase mortar, MethodBase nonmortar,Type mortarType)
        {
            this.mortar = mortar;
            this.nonmortar = nonmortar;
            this.mortarType = mortarType;
        }
        #endregion

        #region Properties
        public MethodBase Mortar
        {
            get { return mortar; }
            set { mortar = value; }

        }
        public MethodBase Nonmortar
        {
            get { return nonmortar; }
            set { nonmortar = value; }
        }
        public int NodesCount
        {
            get { return nodes.Length; }
        }

        public List<Vertex> Vertexes
        {
            get { return vertexes; }
            set { vertexes = value; }
        }

        #endregion

        #region Methods
        public Edge edge(int i)
        { 
            return new Edge(nodes[i], nodes[i+1]);
        }
        public void createMortarNodes()
        {
//            vertexes = new List<Vertex>();
//            for (int i = 0; i < nodes.Length-1; i++)
//            {
//                Edge e = new Edge(nodes[i], nodes[i+1]);
//                for (int j = 0; j < Mortars.Length; j++)
//                {
//                    int k = Mortars[j].Polygon.isEdgeOnPolygon(e);
//                    if (k >= 0)
//                    {
//                        foreach (Edge medge in Mortars[j].Boundaries[k])
//                        {
//                            for (int m = 0; m < medge.NodesCount; m++)
//                                if (!vertexes.Contains(medge[m])) vertexes.Add(medge[m]);
//                        }
//                        break;
//                    }
//                }
//            }
//            vertexes.Sort();
        }
        public Mortar createMortar(int femnodescount)
        {
           return (Mortar)(mortarType.GetConstructor(new Type[] { typeof(int), typeof(List<Vertex>) })).Invoke(new object[] { femnodescount, Vertexes });
        }
        #endregion
    }
}