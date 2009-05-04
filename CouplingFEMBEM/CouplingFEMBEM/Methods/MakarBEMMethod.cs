using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SbB.Diploma.Methods
{
    public class MakarBEMMethod:MethodBase
    {
        #region Fields
        private Matrix H, G;
        private List<Vertex> vertexes;
        private List<BoundEdge>[] boundaries;
        private BoundaryClass[] boundaryClasses;
        private int elementsPerSegment = 8;

        private Matrix A;
        private Vector b;
        #endregion

        #region Constructors
        public MakarBEMMethod(Polygon polygon)
        {
            this.polygon = polygon;
        }
        #endregion

        #region Properties
        public List<Vertex> Vertexes
        {
            get { return vertexes; }
        }
        public BoundaryClass[] BoundaryClasses
        {
            get { return boundaryClasses; }
            set { boundaryClasses = value; }
        }

        public List<BoundEdge>[] Boundaries
        {
            get { return boundaries; }
        }

        public int ElementsPerSegment
        {
            get { return elementsPerSegment; }
            set { elementsPerSegment = value; }
        }

        #endregion

        #region Methods
        #region Private
        private void createVertexes()
        {
            vertexes = new List<Vertex>();
            for (int i = 0; i < polygon.Count; i++)
            {
                for (int j = 0; j < ElementsPerSegment; j++)
                {
                    Vertex v = ((double) j/ElementsPerSegment)*(polygon[i + 1] - polygon[i]);
                    vertexes.Add(polygon[i] + v);
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
            sw.WriteLine("{0}", youngModulus);
            sw.WriteLine("{0}", poissonRatio);
            sw.WriteLine("$#");
            sw.Close();
        }
        private void writeDomain()
        {
            StreamWriter sw = new StreamWriter("Domain.txt");
            // кількість точок
            sw.WriteLine("{0}", 2*polygon.Count);
            for (int i = 0; i < polygon.Count; i++)
            {
                // вершина
                sw.WriteLine("{0} {1} {2} {3}", 2*i+1, polygon[i].X, polygon[i].Y, /*показник подвоєності*/0);
                Vertex middle = (polygon[i] + polygon[i + 1])*0.5;
                sw.WriteLine("{0} {1} {2} {3}", 2 * i + 2, middle.X, middle.Y, /*показник подвоєності*/0);
                // середина відрізка
            }

            // кількість сегментів
            sw.WriteLine("{0}", polygon.Count);
            for (int i = 0; i < polygon.Count; i++)
            {
                // інформація про сегмент
                sw.WriteLine("{0} {1} {2} {3} {4} {5} {6}", i+1, 2*i+1, 2*i+2, (2*i+3)%(2*polygon.Count), 0, 0, 0);
            }

            // кількість сегментів для FEM
            sw.WriteLine("{0}", 0);
            // номери сегментів для FEM
            sw.WriteLine("{0}", 0);

            // кількість сегментів для BEM
            sw.WriteLine("{0}", polygon.Count);
            for(int i=0; i<polygon.Count; i++)
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
            newp.StartInfo.FileName = "NSGBEM.exe";
            newp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            newp.Start();
            newp.WaitForExit();

            readFiles();

            deleteFiles();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(currentCulture);
        }
        public override void Run()
        {
            //Create A an b from G and H
            int mDim = 2*vertexes.Count;
            int nDim = 0;
            foreach (Vertex vertex in vertexes)
            {
                nDim += vertex.Dofu.Length;
                nDim += vertex.Doft.Length;
            }
            b = new Vector(mDim);
            A = new Matrix(mDim, nDim);

            int counter = 0;
            for (int i = 0; i < vertexes.Count; i++)
            {
                int k = 0;
                for (int j = 0; j < polygon.Count; j++)
                    if (polygon.edge(j).hasVertex(vertexes[i]))
                    {
                        k = j;
                        break;
                    }

                if (vertexes[i].Dofu.Length == 0)
                { /* Kinenatic boundary type. U=0 */ }
                else
                {
                    for (int j = 0; j < A.Size.m; j++)
                    {
                        A[j][counter] = H[j][2*i];
                        A[j][counter + 1] = H[j][2*i + 1];
                    }
                    counter += 2;
                }

                if (vertexes[i].Doft.Length==0)
                {
                    b += ((StaticBoundary) boundaryClasses[i]).P.X*G[2*i];
                    b += ((StaticBoundary) boundaryClasses[i]).P.Y*G[2*i + 1];
                }
                else
                {
                    for (int j = 0; j < A.Size.m; j++)
                    {
                        A[j][counter] = G[j][2 * i];
                        A[j][counter + 1] = G[j][2*i + 1];
                    }
                    counter += 2;
                }
            }
        }
        public override void FillGlobalmatrix(Matrix global)
        {
            int[] indexes = new int[A.Size.m];
            int counter = 0;
            foreach (Vertex vertex in vertexes)
                for (int i = 0; i < vertex.Dofu.Length; i++)
                    indexes[counter++] = vertex.Dofu[i];

            counter = 0;
            for (int i = 0; i < vertexes.Count; i++)
            {
                for (int j = 0; j < vertexes[i].Dofu.Length; j++)
                {
                    for (int k = 0; k < indexes.Length; k++)
                        global[indexes[k]][vertexes[i].Dofu[j]] += A[k][counter + j];
                }
                counter += vertexes[i].Dofu.Length;

                for (int j = 0; j < vertexes[i].Doft.Length; j++)
                {
                    for (int k = 0; k < indexes.Length; k++)
                        global[indexes[k]][vertexes[i].Doft[j]] += A[k][counter + j];
                }
                counter += vertexes[i].Doft.Length;
            }
        }
        public override void FillGlobalvector(Vector global)
        {
            int[] indexes = new int[b.Length];
            int counter = 0;
            foreach (Vertex vertex in vertexes)
                for (int i = 0; i < vertex.Dofu.Length; i++)
                    indexes[counter++] = vertex.Dofu[i];

            for (int i = 0; i < indexes.Length; i++)
                global[indexes[i]] += b[i];
        } 
        #endregion
        #endregion
    }
}
