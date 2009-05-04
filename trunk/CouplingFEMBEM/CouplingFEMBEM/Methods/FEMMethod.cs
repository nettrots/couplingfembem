using System;
using System.Collections.Generic;

namespace SbB.Diploma
{
    public class FEMMethod:MethodBase
    {
        #region Fields

        private Matrix d = new Matrix(3,3);
        private List<Vertex> vertexes;
        private List<FEMElement> elements;
        private List<FEMEdge>[] boundaries;
        private BoundaryClass[] boundaryClasses;
        private int n;

        private Matrix Af;
        private Matrix A;
        private Vector b;

        private Triangulation triangulation;
        //private Polygon polygon;
        private double angle;
        private double area;

        #endregion

        #region Constructors
        public FEMMethod(object data)
        {
            //create read data
            refreshD();
        }
        #endregion

        #region Properties
 
    
        public override double YoungModulus
        {
            get { return base.YoungModulus; }
            set
            {
                base.YoungModulus = value;
                refreshD();
            }
        }
        public override double PoissonRatio
        {
            get { return base.PoissonRatio; }
            set
            {
                base.PoissonRatio = value;
                refreshD();
            }
        }
        public List<Vertex> Vertexes
        {
            get
            {
                if (Vertexes == null) throw new Exception("Triangulation did not complete");
                return Vertexes;
            }
            set { vertexes = value; }

        }
        public List<FEMElement> Elements
        {
            get
            {
                if (elements == null) throw new Exception("Triangulation did not complete");
                return elements;
            }
            set { elements = value; }

        }
        public List<FEMEdge>[] Boundaries
        {
            get
            {
                if (boundaries == null) throw new Exception("Triangulation did not complete");
                return boundaries;
            }
            set { boundaries = value; }

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

        public Triangulation Triangulation
        {
            get { return triangulation; }
            set { triangulation = value; }
        }

        public double Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public double Area
        {
            get { return area; }
            set { area = value; }
        }

        #endregion

        #region Methods
        #region Private
        private void refreshD()
        {
            double D1 = (1 - base.PoissonRatio) * base.YoungModulus / ((1 + base.PoissonRatio) * (1 - 2 * base.PoissonRatio));
            double D2 = D1 * (1 - 2 * base.PoissonRatio) / (2 - 2 * base.PoissonRatio);
            double D3 = D1 * base.PoissonRatio / (1 - base.PoissonRatio);
            D[0][0] = D1;
            D[0][1] = D[1][0] = D3;
            D[1][1] = D1;
            D[2][2] = D2;
        }
        #endregion

        #region Public

        public override void Initialize()
        {
            //triangulate
            triangulation.triangulate(Angle, Area);
            vertexes = Triangulation.Vertexes;
            elements = Triangulation.Elements;
            boundaries = Triangulation.Boundaries;
            
        }

        public override void Run()
        {
            A = new Matrix(2*Vertexes.Count, 2*Vertexes.Count);

            foreach (FEMElement element in elements)
                element.FEM(A, D);

            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.KINEMATIC)
                {
                    foreach (FEMEdge edge in boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            for (int k = 0; k < edge[j].Dofu.Length; k++)
                                A[edge[j].Dofu[k]][edge[j].Dofu[k]] = 1.0/Constants.EPS;
                        }
                }
            //
            b = new Vector(2*Vertexes.Count);
            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.STATIC)
                {
                    Vertex p = ((StaticBoundary) BoundaryClasses[i]).P;
                    foreach (FEMEdge edge in boundaries[i])
                        edge.FEM(b, p);
                }
            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.KINEMATIC)
                    foreach (FEMEdge edge in boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            for (int k = 0; k < edge[j].Dofu.Length; k++)
                                b[edge[j].Dofu[k]] = 0.0;
                        }
            //Somehow create AF
            int dim = 0;
            foreach (Vertex vertex in vertexes)
            {
                dim += vertex.Doft.Length;
            }
            Af = new Matrix(dim, dim);

            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.MORTAR)
                {
                    int min = int.MaxValue;
                    foreach (FEMEdge edge in boundaries[i])
                    {
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            for (int k = 0; k < edge[j].Doft.Length; k++)
                            {
                                if (min > edge[j].Doft[k]) min = edge[j].Doft[k];
                            }
                        }
                    }
                    foreach (FEMEdge edge in boundaries[i])
                        edge.FEM(Af, min);
                }
        }

        public override void FillGlobalmatrix(Matrix global)
        {
            //every vertex has few rows in matrix. iterating all of them and we fill GM
            // fill matrix A
            for (int i = 0; i < A.Size.m; i++)
                for (int j = 0; j < A.Size.n; j++)
                    global[i][j] = A[i][j];

            // fill matrix Af
            for (int i = 0; i < Af.Size.m; i++)
                for (int j = 0; j < A.Size.n; j++)
                    global[i + A.Size.m][j + A.Size.n] = Af[i][j];
        }

        public override void FillGlobalvector(Vector global)
        {
            for (int i = 0; i < b.Length; i++)
                global[i] = b[i];
        }

        public override void GetResultsFrom(Vector vector)
        {
            results = new Vector(2*vertexes.Count);
            int counter = 0;
            for (int i = 0; i < vertexes.Count; i++)
                for (int j = 0; j < vertexes[i].Dofu.Length; j++)
                    results[counter++] = vector[vertexes[i].Dofu[j]]; 
        }
        #endregion
        #endregion

        
    }
}