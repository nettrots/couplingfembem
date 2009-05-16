using System;
using System.Collections.Generic;

namespace SbB.Diploma
{
    public class FEMMethod:BPMethod, IProblem, IDiscretization 
    {
        #region Fields

        private double youngModulus = 21000;
        private double poissonRatio = 0.3;

        private Matrix d = new Matrix(3,3);
        private List<FEMElement> elements;
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
        public FEMMethod()
        {
            //create read data
            refreshD();
        }
        #endregion

        #region Properties
 
    
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
        public Polygon Polygon { get; set;}
        public BoundaryClass[] BoundaryClasses { get; set; }

        public List<Vertex> Vertexes { get; private set; }
        public List<BoundEdge>[] Boundaries { get; private set; }
        public List<FEMElement> Elements
        {
            get
            {
                if (elements == null) throw new Exception("Triangulation did not complete");
                return elements;
            }
            set { elements = value; }

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
        public List<Func<double, double, double>> FuncList { get; set; }



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
            double D1 = (1 - poissonRatio)*youngModulus/((1 + poissonRatio)*(1 - 2*poissonRatio));
            double D2 = D1*(1 - 2*poissonRatio)/(2 - 2*poissonRatio);
            double D3 = D1*poissonRatio/(1 - poissonRatio);
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
            Vertexes = Triangulation.Vertexes;
            elements = Triangulation.Elements;
            Boundaries = Triangulation.Boundaries;
            
        }

        public override void Run()
        {
            A = new Matrix(2*Vertexes.Count, 2*Vertexes.Count);

            foreach (FEMElement element in elements)
                element.FEM(A, D);

            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.KINEMATIC)
                {
                    foreach (FEMEdge edge in Boundaries[i])
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
                    foreach (FEMEdge edge in Boundaries[i])
                        edge.FEM(b, p);
                }
            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.KINEMATIC)
                    foreach (FEMEdge edge in Boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            for (int k = 0; k < edge[j].Dofu.Length; k++)
                                b[edge[j].Dofu[k]] = 0.0;
                        }
            //Somehow create AF
            int dim = 0;
            foreach (Vertex vertex in Vertexes)
            {
                dim += vertex.Doft.Length;
            }
            Af = new Matrix(dim, dim);

            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.MORTAR)
                {
                    int min = int.MaxValue;
                    foreach (FEMEdge edge in Boundaries[i])
                    {
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            for (int k = 0; k < edge[j].Doft.Length; k++)
                            {
                                if (min > edge[j].Doft[k]) min = edge[j].Doft[k];
                            }
                        }
                    }
                    foreach (FEMEdge edge in Boundaries[i])
                        edge.FEM(Af, min);
                }
        }

        public override void Solve()
        {
            throw new NotImplementedException();
        }

        #endregion
        #endregion

        
    }
}