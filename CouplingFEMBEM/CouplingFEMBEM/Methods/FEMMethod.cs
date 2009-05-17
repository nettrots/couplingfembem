using System;
using System.Collections.Generic;
using SbB.Diploma.Yaml.Custom;

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

        #endregion

        #region Constructors
        public FEMMethod(Dictionary<string, HashValue> data)
        {
           
            //create read data
            Vertex[] femPolygon = new Vertex[data["Vertex"].eHash.Count];

            for (int i = 0; i < femPolygon.Length; i++)
            {
                double x = data["Vertex"].eHash[i.ToString()].eHash["0"].eDouble;
                double y = data["Vertex"].eHash[i.ToString()].eHash["1"].eDouble;
                femPolygon[i] = new Vertex(x, y);
            }

            Polygon = new Polygon(femPolygon);

            Angle = data["Mesh"].eHash["angle"].eDouble;
            Area = data["Mesh"].eHash["area"].eDouble;
            youngModulus = data["youngModulus"].eDouble;
            poissonRatio = data["poissonRatio"].eDouble;
            refreshD();

            Triangulation = new LinialTriangleTriangulation(new Polygon(femPolygon));

            BoundaryClasses = new BoundaryClass[data["BoundaryType"].eHash.Count];
            for (int i = 0; i < BoundaryClasses.Length; i++)
            {
                string[] ss = data["BoundaryType"].eHash[i.ToString()].eString.Split('@');
                switch (ss[0])
                {
                    case "STATIC":
                        if (ss.Length > 1)
                        {
                            ss[1] = ss[1].Substring(1, ss[1].Length - 2);
                            ss = ss[1].Split(',');
                            BoundaryClasses[i] = new StaticBoundary(double.Parse(ss[0]), double.Parse(ss[1]));
                        }
                        else BoundaryClasses[i] = new StaticBoundary(0, 0);
                        break;
                    case "KINEMATIC":
                        BoundaryClasses[i] = new KinematicBoundary();
                        break;
                    case "MORTAR":
                        BoundaryClasses[i] = new MortarBoundary();
                        break;
                    case "NONMORTAR":
                        BoundaryClasses[i] = new NonMortarBoundary();
                        break;
                    default:
                        throw new Exception("A-ya-yaj!!!");
                }
                Name = "FEM("+this.Area+")";
            }
        }
        #endregion

        #region Properties
        public override string Name{get;set;}
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



        public Triangulation Triangulation { get; set; }

        public double Angle { get; set; }

        public double Area { get; set; }

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
            Triangulation.triangulate(Angle, Area);
            Vertexes = Triangulation.Vertexes;
            elements = Triangulation.Elements;
            Boundaries = Triangulation.Boundaries;

            int counter = 0;
            foreach (Vertex vertex in Vertexes)
            {
                vertex.Dofu = new int[] {counter++, counter++};
            }
        }

        public override void Run()
        {
            // Create K
            K = new Matrix(2*Vertexes.Count, 2*Vertexes.Count);

            foreach (FEMElement element in elements)
                element.FEM(K, D);

            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.KINEMATIC)
                {
                    foreach (FEMEdge edge in Boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            for (int k = 0; k < edge[j].Dofu.Length; k++)
                                K[edge[j].Dofu[k]][edge[j].Dofu[k]] = 1.0/Constants.EPS;
                        }
                }

            // Create F
            F = new Vector(2*Vertexes.Count);
            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.STATIC)
                {
                    Vertex p = ((StaticBoundary) BoundaryClasses[i]).P;
                    foreach (FEMEdge edge in Boundaries[i])
                        edge.FEM(F, p);
                }
            for (int i = 0; i < BoundaryClasses.Length; i++)
                if (BoundaryClasses[i].type() == BoundaryType.KINEMATIC)
                    foreach (FEMEdge edge in Boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            for (int k = 0; k < edge[j].Dofu.Length; k++)
                                F[edge[j].Dofu[k]] = 0.0;
                        }
        }

        public override void Solve()
        {
            LUSolve.Solve(K, F, out results);
        }

        public override double U(double x, double y)
        {
            double rez = 0.0;
            Vertex v = new Vertex(x,y);
            foreach (FEMElement element in elements)
            {
                if (element.hasVertex(v))
                {
                    for (int i = 0; i < element.NodesCount; i++)
                        rez += results[element[i].Dofu[0]]*element.phi(i, v);
                    return rez;
                }
            }
            return rez;
        }

        public override double V(double x, double y)
        {
            double rez = 0.0;
            Vertex v = new Vertex(x, y);
            foreach (FEMElement element in elements)
            {
                if (element.hasVertex(v))
                {
                    for (int i = 0; i < element.NodesCount; i++)
                        rez += results[element[i].Dofu[1]]*element.phi(i, v);
                    return rez;
                }
            }
            return rez;
        }

        #endregion
        #endregion
    }
}