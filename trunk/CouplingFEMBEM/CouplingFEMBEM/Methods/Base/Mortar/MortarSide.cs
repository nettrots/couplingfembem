using System;
using System.Collections.Generic;
using System.Reflection;

namespace SbB.Diploma
{
    public class MortarSide
    {
        #region Fields
        private Vertex[] nodes;
        private MethodBase[] mortars;
        private MethodBase[] nonmortars;
        private List<Vertex> vertexes;
        private Type mortarType;
        #endregion

        #region Constructors
        public MortarSide(Vertex[] nodes, MethodBase[] mortars, MethodBase[] nonmortars,Type mortarType)
        {
            this.nodes = nodes;
            this.mortars = mortars;
            this.nonmortars = nonmortars;
            this.mortarType = mortarType;
        }
        #endregion

        #region Properties
        public MethodBase[] Mortars
        {
            get { return mortars; }
            set { mortars = value; }

        }
        public MethodBase[] Nonmortars
        {
            get { return nonmortars; }
            set { nonmortars = value; }
        }
        public int NodesCount
        {
            get { return nodes.Length; }
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
           return (Mortar)(mortarType.GetConstructor(new Type[] { typeof(int), typeof(List<Vertex>) })).Invoke(new object[] { femnodescount, vertexes });
        }
        #endregion
    }
}