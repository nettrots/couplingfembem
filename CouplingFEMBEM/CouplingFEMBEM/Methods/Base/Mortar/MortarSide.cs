using System;
using System.Collections.Generic;
using System.Reflection;

namespace SbB.Diploma
{
    public class MortarSide
    {
        #region Fields
        private List<int> mortarsides;
        private IDiscretization descritization;
        private List<Vertex> vertexes;
        #endregion

        #region Constructors
        public MortarSide(IDiscretization descritization, List<int> mortarsides)
        {
            this.descritization = descritization;
            this.mortarsides = mortarsides;
        }
        #endregion

        #region Methods
        public void createMortarNodes()
        {
            vertexes = new List<Vertex>();
            foreach (int i in mortarsides)
            {
                foreach (BoundEdge edge in descritization.Boundaries[i])
                {
                    for (int j = 0; j < edge.NodesCount; j++)
                    {
                        if (!vertexes.Contains(edge[j])) vertexes.Add(edge[j]);
                    }
                }
            }
        }
        public Mortar createMortar()
        {
            return new LinearMortar(vertexes);
        }
        #endregion
    }
}