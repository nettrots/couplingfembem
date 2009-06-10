using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SbB.Diploma.Yaml.Custom;

namespace SbB.Diploma.Methods
{
    public class MakarBEMMethod:BPMethod, IProblem, IDiscretization
    {
        #region Fields
        private Matrix H, G;
        private Vector t;
        #endregion

        #region Constructors
        public MakarBEMMethod(Dictionary<string, HashValue> data)
        {
            Vertex[] bemPolygon = new Vertex[data["Vertex"].eHash.Count];

            for (int i = 0; i < bemPolygon.Length; i++)
            {
                double x = data["Vertex"].eHash[i.ToString()].eHash["0"].eDouble;
                double y = data["Vertex"].eHash[i.ToString()].eHash["1"].eDouble;
                bemPolygon[i] = new Vertex(x, y);
            }
            Polygon = new Polygon(bemPolygon);
            ElementsPerSegment = data["elementsPerSegment"].eInt;
            YoungModulus = data["youngModulus"].eDouble;
            PoissonRatio = data["poissonRatio"].eDouble;

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
                Name = "BEM" + this.ElementsPerSegment;
            }
        }
        #endregion

        #region Properties
        public List<Vertex> Vertexes { get; private set; }
        public List<BoundEdge>[] Boundaries { get; private set; }

        public double YoungModulus { get; set; }

        public double PoissonRatio { get; set; }

        public BoundaryClass[] BoundaryClasses { get; set; }

        public int ElementsPerSegment { get; set; }
        public List<Func<double, double, double>> FuncList { get; set; }
        public void caltT()
        {
           // inv.rmatrixinverse(G);
            t = G * H * Results;
        }

        public override Vector Results { get; set; }
        #endregion


        #region Methods
        #region Private
        private void createVertexes()
        {
            Vertexes = new List<Vertex>();
            for (int i = 0; i < Polygon.Count; i++)
            {
                for (int j = 0; j < ElementsPerSegment; j++)
                {
                    Vertex v = ((double) j/ElementsPerSegment)*(Polygon[i + 1] - Polygon[i]);
                    Vertexes.Add(Polygon[i] + v);
                }
            }

            int counter = 0;
            foreach (Vertex vertex in Vertexes)
            {
                vertex.Dofu = new int[] { counter++, counter++ };
            }
            
            Boundaries = new List<BoundEdge>[Polygon.Count];
            for (int i = 0; i < Polygon.Count; i++)
            {
                Boundaries[i] = new List<BoundEdge>();
                for (int j = 0; j < ElementsPerSegment; j++)
                {
                    int ai = ElementsPerSegment*i + j;
                    int bi = (i == Polygon.Count - 1) && (j == ElementsPerSegment - 1)
                                 ? 0
                                 : ElementsPerSegment*i + j + 1;
                    BoundEdge edge = new LinearBEMEdge(Vertexes[ai], Vertexes[bi]);
                    Boundaries[i].Add(edge);
                }
            }
        }
        private void writeFiles()
        {
            writeApproximation();
            writeMaterial();
            writeDomain();
        }
        private void writeApproximation()
        {
            StreamWriter sw = new StreamWriter("ApproxBEM.txt");
            sw.WriteLine("1");
            sw.WriteLine("$#");
            sw.Close();
        }
        private void writeMaterial()
        {
            StreamWriter sw = new StreamWriter("ConstBEM.txt");
            sw.WriteLine("{0}", YoungModulus);
            sw.WriteLine("{0}", PoissonRatio);
            sw.WriteLine("$#");
            sw.Close();
        }
        private void writeDomain()
        {
            StreamWriter sw = new StreamWriter("Domain.txt");
            // кількість точок
            sw.WriteLine("{0}", 2*Polygon.Count);
            for (int i = 0; i < Polygon.Count; i++)
            {
                // вершина
                sw.WriteLine("{0} {1} {2} {3}", 2*i+1, Polygon[i].X, Polygon[i].Y, /*показник подвоєності*/0);
                Vertex middle = (Polygon[i] + Polygon[i + 1])*0.5;
                sw.WriteLine("{0} {1} {2} {3}", 2 * i + 2, middle.X, middle.Y, /*показник подвоєності*/0);
                // середина відрізка
            }

            // кількість сегментів
            sw.WriteLine("{0}", Polygon.Count);
            for (int i = 0; i < Polygon.Count; i++)
            {
                // інформація про сегмент
                sw.WriteLine("{0} {1} {2} {3} {4} {5} {6}", i+1, 2*i+1, 2*i+2, (2*i+3)%(2*Polygon.Count), 0, 0, 0);
            }

            // кількість сегментів для FEM
            sw.WriteLine("{0}", 0);
            // номери сегментів для FEM
            sw.WriteLine("{0}", 0);

            // кількість сегментів для BEM
            sw.WriteLine("{0}", Polygon.Count);
            for(int i=0; i<Polygon.Count; i++)
            {
                // номери сегментів BEM, кількість елементів
                sw.WriteLine("{0} {1}", i+1, ElementsPerSegment);
            }

            // кількість спільних сегментів
            sw.WriteLine("{0}", 1);
            // номери спільних сегментів
            sw.WriteLine("{0}", 1);

            sw.WriteLine("$#");
            sw.Close();
        }
        private void readFiles()
        {
            readH();
            readG();
        }
        private void readH()
        {
            StreamReader sr = new StreamReader("H.txt");
            H = new Matrix(2*Vertexes.Count, 2*Vertexes.Count);
            for (int i = 0; i < H.Size.m; i++)
            {
                string line = sr.ReadLine();
                while (line.Contains("  ")) line = line.Replace("  ", " ");
                if (line[0] == ' ') line = line.Substring(1);
                string[] cells = line.Split(' ');
                for (int j = 0; j < H.Size.n; j++)
                    H[i][j] = double.Parse(cells[j]);
            }
            sr.Close();
        }
        private void readG()
        {
            StreamReader sr = new StreamReader("G.txt");
            G = new Matrix(2*Vertexes.Count, 2*Vertexes.Count);
            for (int i = 0; i < G.Size.m; i++)
            {
                string line = sr.ReadLine();
                while (line.Contains("  ")) line = line.Replace("  ", " ");
                if (line[0] == ' ') line = line.Substring(1);
                string[] cells = line.Split(' ');
                for (int j = 0; j < G.Size.n; j++)
                    G[i][j] = double.Parse(cells[j]);
            }
            sr.Close();
        }
        private void deleteFiles()
        {
            File.Delete("ApproxBEM.txt");
            File.Delete("ConstBEM.txt");
            File.Delete("Domain.txt");
            File.Delete("H.txt");
            File.Delete("G.txt");
            File.Delete("Points.txt");
        }
        #endregion

        #region Public
        public override void Initialize()
        {
            string currentCulture = CultureInfo.CurrentCulture.Name;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            createVertexes();

            writeFiles();

            Process newp = new Process();
            newp.StartInfo.FileName = "NSGBEMPP.exe";
            newp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            newp.Start();
            newp.WaitForExit();

            readFiles();

           // deleteFiles();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(currentCulture);
        }
        public override void Run()
        {
            // Create M
            Matrix M = new Matrix(H.Size);
            foreach (List<BoundEdge> boundary in Boundaries)
            {
                foreach (LinearBEMEdge edge in boundary)
                {
                    edge.Converting(M);
                }
            }

            // Inverse G
            inv.rmatrixinverse(G);

            // Create t
            Vector t = new Vector(2*Vertexes.Count);
            for (int i = 0; i < BoundaryClasses.Length; i++)
            {
                if (BoundaryClasses[i].type()==BoundaryType.STATIC)
                {
                    Vertex p = ((StaticBoundary) BoundaryClasses[i]).P;
                    for (int j = ElementsPerSegment*i; j <  ElementsPerSegment*(i+1); j++)
                    {
                        t[Vertexes[j].Dofu[0]] += p.X;
                        t[Vertexes[j].Dofu[1]] += p.Y;
                    }
                }
            }

            // Create K
            K = M*G*H;

            // Create F
            F = M*t;

            for (int i = 0; i < BoundaryClasses.Length; i++)
            {
                if (BoundaryClasses[i].type() == BoundaryType.KINEMATIC)
                {
                    for (int j = ElementsPerSegment * i; j < ElementsPerSegment * (i + 1); j++)
                    {
                        K[Vertexes[j].Dofu[0]][Vertexes[j].Dofu[0]] = 1.0 / Constants.EPS;
                        K[Vertexes[j].Dofu[1]][Vertexes[j].Dofu[1]] = 1.0 / Constants.EPS;
                        F[Vertexes[j].Dofu[1]] = 0.0;
                    }
                }
            }
        }
        public override void Solve()
        {
            LUSolve.Solve(K, F, out results);
            Results = results;
            caltT();
        }

        //Formate files for postproccessing
        #region PostProccesing private zone

        private double[] runPP(Vertex[] vertices,bool isU)
        {
            
            string currentCulture = CultureInfo.CurrentCulture.Name;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            writeFiles();
            writePP(vertices);
            Process newp = new Process();
            newp.StartInfo.FileName =Directory.GetCurrentDirectory()+"\\NSGBEMPP.exe";
            newp.StartInfo.Arguments = "1";
            newp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
//            newp.StartInfo.RedirectStandardError = true;

            newp.Start();
            newp.WaitForExit();
            double[] rez= readPP(isU,vertices.Length);
            //clearPP();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(currentCulture);
            return rez;
        }
        private double[] runPPS(Vertex[] vertices, int isU)
        {

            string currentCulture = CultureInfo.CurrentCulture.Name;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            writeFiles();
            writePP(vertices);
            Process newp = new Process();
            newp.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\NSGBEMPP.exe";
            newp.StartInfo.Arguments = "1";
            newp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //            newp.StartInfo.RedirectStandardError = true;

            newp.Start();
            newp.WaitForExit();
            double[] rez = readPPS(isU, vertices.Length);
            //clearPP();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(currentCulture);
            return rez;
        }
        private void writePP(Vertex[] vertices)
        {
            innerPointsPP(vertices);
            seiPP();
        }
        private void seiPP()
        {
            List<List<Vertex>> temp=new List<List<Vertex>>();
            foreach (List<BoundEdge> list in Boundaries)
            {
                temp.Add(new List<Vertex>());
                foreach (BoundEdge edge in list)
                {
                    if (!temp[temp.Count - 1].Contains(edge.A))
                        temp[temp.Count - 1].Add(edge.A);
                    if (!temp[temp.Count - 1].Contains(edge.B))
                        temp[temp.Count - 1].Add(edge.B);
                }
            }
            int counter=1;
           
            foreach (List<Vertex> list in temp)
            {
                StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\se_"+(counter++)+".txt");
                foreach (Vertex vertex in list)
                {
                    sw.WriteLine("{0} {1} {2} {3} {4} {5}", vertex.X, vertex.Y, Results[vertex.Dofu[0]], Results[vertex.Dofu[1]], t[vertex.Dofu[0]], t[vertex.Dofu[1]]);
                }
                sw.Close();
            }
        }
        private void innerPointsPP(Vertex[] vertices)
        {
            StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory()+"\\inner_points.txt");
            sw.WriteLine("{0}", vertices.Length);
            foreach (Vertex vertex in vertices)
            {
                sw.WriteLine("{0} {1}", vertex.X,vertex.Y);
            }
            sw.Close();
        }
      
        private double[] readPPS(int sn,int n)
        {
            List<double> rez = new List<double>();

            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "\\inner_stress.txt");

            for (int i = 0; i < n; i++)
            {
                string line = sr.ReadLine();
                while (line.Contains("  ")) line = line.Replace("  ", " ");
                if (line[0] == ' ') line = line.Substring(1);
                string[] cells = line.Split(' ');

//                Vertex v = new Vertex(double.Parse(cells[0]), double.Parse(cells[1]));

                 rez.Add(double.Parse(cells[sn + 2]));
            }
            sr.Close();
            return rez.ToArray();
        }

        private double[] readPP(bool isU,int n)
        {
            List<double> rez=new List<double>();
            
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory()+ "\\inner_disp.txt");

            for (int i = 0; i < n; i++)
            {
                string line = sr.ReadLine();
                while (line.Contains("  ")) line = line.Replace("  ", " ");
                if (line[0] == ' ') line = line.Substring(1);
                string[] cells = line.Split(' ');

                int dofn = (isU) ? 0 : 1;

                Vertex v = new Vertex(double.Parse(cells[0]), double.Parse(cells[1]));
                if (Polygon.isVertexOnPolygon(v))
                {
                    double r = 0;
                    foreach (List<BoundEdge> list in Boundaries)
                    {
                        foreach (BoundEdge edge in list)
                        {
                            if (edge.hasVertex(v))
                            {
                                for (int k = 0; k < 2; k++)
                                    r += Results[edge[k].Dofu[dofn]]*edge.phi(k, v.X,v.Y);
                                break;
                            }
                        }
                        if(r!=0)break;
                    }
                    rez.Add(r);
                }
                else  rez.Add(double.Parse(cells[dofn+2]));
            }
            sr.Close();
            return rez.ToArray();
        }
        private void clearPP()
        {
            File.Delete("inner_stress.txt");
            File.Delete("inner_points.txt");
            File.Delete("inner_disp.txt");
       
        }
        #endregion

        public override double Sxy(double x, double y)
        {
            throw new NotImplementedException();
        }

        public override double[] U(Vertex[] vertices)
        {
            return runPP(vertices, true);
            return new double[vertices.Length];
        }

        public override double[] V(Vertex[] vertices)
        {
            return runPP(vertices, false);
            //throw new NotImplementedException();
            return new double[vertices.Length];
        }

        public override double[] Sxx(Vertex[] vertices)
        {
            return runPPS(vertices, 0);
        }

        public override double[] Syy(Vertex[] vertices)
        {
            return runPPS(vertices, 1);
        }

        public override double[] Sxy(Vertex[] vertices)
        {
            return runPPS(vertices, 2);
        }

        public override double U(double x, double y)
        {
            throw new NotImplementedException();
        }

        public override double V(double x, double y)
        {
            throw new NotImplementedException();
        }

        public override double Sxx(double x, double y)
        {
            throw new NotImplementedException();
        }

        public override double Syy(double x, double y)
        {
            throw new NotImplementedException();
        }

        #endregion
        #endregion
    }
}
