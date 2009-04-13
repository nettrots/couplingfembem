using System;
using System.Collections.Generic;

namespace SbB.Diploma
{
    public class FEMMethod:MethodBase
    {
        #region Fields
        private Polygon polygon;
        
        private double youngModulus;
        private double poissonRatio;
        private Matrix d = new Matrix(3,3);
        private List<Vertex> vertexes;
        private List<FEMElement> elements;
        private List<FEMEdge>[] boundaries;
        private BoundaryClass[] boundaryClasses;
        private int n;

        private Matrix A;
        private Vector b;
        private Matrix Af;

        #endregion

        #region Constructors
        public FEMMethod(object data)
        {
            //create read data
            refreshD();
        }
        #endregion

        #region Properties
        public Polygon Polygon
        {
            get { return polygon; }
        }
 
    
        public double YoungModulus
        {
            get { return youngModulus; }
            set
            {
                youngModulus = value;
                refreshD();
            }
        }
        public double PoissonRatio
        {
            get { return poissonRatio; }
            set
            {
                poissonRatio = value;
                refreshD();
            }
        }
        public List<Vertex> Vertexes
        {
            get
            {
                if (vertexes == null) throw new Exception("Triangulation did not complete");
                return vertexes;
            }
        }
        public List<FEMElement> Elements
        {
            get
            {
                if (elements == null) throw new Exception("Triangulation did not complete");
                return elements;
            }

        }
        public List<FEMEdge>[] Boundaries
        {
            get
            {
                if (boundaries == null) throw new Exception("Triangulation did not complete");
                return boundaries;
            }
        }
        public Matrix D
        {
            get { return d; }
        }

        public int N
        {
            get { return n; }
            set { n = value; }
        }

        public BoundaryClass[] BoundaryClasses
        {
            get { return boundaryClasses; }
            set { boundaryClasses = value; }
        }

        #endregion

        #region Methods
        #region Private
        private void refreshD()
        {
            double D1 = (1 - poissonRatio) * youngModulus / ((1 + poissonRatio) * (1 - 2 * poissonRatio));
            double D2 = D1 * (1 - 2 * poissonRatio) / (2 - 2 * poissonRatio);
            double D3 = D1 * poissonRatio / (1 - poissonRatio);
            D[0][0] = D1;
            D[0][1] = D[1][0] = D3;
            D[1][1] = D1;
            D[2][2] = D2;
        }
        #endregion

        #region Public

        public override void Run()
        {

            A = new Matrix(2 * vertexes.Count, 2 * vertexes.Count);

            foreach (FEMElement element in elements)
                element.FEM(A, D);

            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.KINEMATIC)
                {
                    foreach (FEMEdge edge in boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            A[2 * edge[j].Number][2 * edge[j].Number] = 1.0 / Constants.EPS;
                            A[2 * edge[j].Number + 1][2 * edge[j].Number + 1] = 1.0 / Constants.EPS;
                        }
                }
            //
            b = new Vector(2 * vertexes.Count);
            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.STATIC)
                {
                    Vertex p = ((StaticBoundary)BoundaryClasses[i]).P;
                    foreach (FEMEdge edge in boundaries[i])
                        edge.FEM(b, p);
                }
            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.KINEMATIC)
                    foreach (FEMEdge edge in boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            b[2 * edge[j].Number] = 0.0;
                            b[2 * edge[j].Number + 1] = 0.0;
                        }
            //Somehow create AF
            Af = new Matrix();
            throw new NotImplementedException();
        }

        public override void FillGlobalmatrix(Matrix global)
        {
            //every vertex has few rows in matrix. iterating all of them and we fill GM
            throw new NotImplementedException();
        }

        public override void FillGlobalvector(Vector global)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

       
    }
}