using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SbB.Diploma.Methods.Base;
using SbB.Diploma.Methods.Base.LinearTriangleFEM;


namespace SbB.Diploma
{
    public class LinialTriangleTriangulation: Triangulation
    {
        #region Constructors
        public LinialTriangleTriangulation(Polygon polygon)
        {
            this.polygon = polygon;
        }
        #endregion

        #region Methods
        private void writeFile()
        {
            try
            {
                FileInfo f = new FileInfo("C.poly");
                StreamWriter writer = f.CreateText();
                writer.WriteLine("{0} 2 0 0", polygon.Count);
                for (int i = 0; i < polygon.Count; i++)
                {
                    string str = string.Format("{0} {1} {2} 0", i + 1, polygon[i].X, polygon[i].Y);
                    str = str.Replace(",", ".");
                    writer.WriteLine(str);
                }
                writer.WriteLine("{0} 1", polygon.Count);
                for (int i = 0; i < polygon.Count; i++)
                {
                    int j = (i + 1)%polygon.Count;
                    writer.WriteLine("{0} {1} {2} {0}", i + 1, i + 1, j + 1, i + 1);
                }
                writer.WriteLine("0");
                writer.Close();
            }
            catch
            {
                throw new Exception("Disk error. Could not create file for triangulation.");
            }
        }
        private void executeTriangle(double minAngle, double maxArea)
        {
            String option = " -pqc" + minAngle + 'a' + maxArea + "C c.poly";
            option = option.Replace(",", ".");
            Process newp = new Process();

            try
            {
                File.WriteAllBytes("triangle.exe", TriangleExe.triangle);

                newp.StartInfo.FileName = "triangle.exe";
                newp.StartInfo.Arguments = option;
                newp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                newp.Start();
                newp.WaitForExit();

                File.Delete("triangle.exe");
                if (!File.Exists("c.1.node") && File.Exists("c.1.ele")) throw new Exception("Triangulation failed");
            }
            catch
            {
                throw new Exception("Triangulation failed");
            }
        }
        private void readFile()
        {
            File.GetAccessControl("c.1.node");
            StreamReader reader;

            reader = File.OpenText("c.1.node");
            string line = reader.ReadLine();
            string[] number = line.Split(new char[] {' '});
            int vertexNum = int.Parse(number[0]);

            vertexes = new List<Vertex>();
            boundaries = new List<BoundEdge>[polygon.Count];
            List<Vertex>[] temp = new List<Vertex>[polygon.Count];
            for (int i = 0; i < polygon.Count; i++)
            {
                boundaries[i] = new List<BoundEdge>();
                temp[i] = new List<Vertex>();
            }

            for (int i = 0; i < vertexNum; i++)
            {
                line = reader.ReadLine();
                while (line.Contains("  ")) line = line.Replace("  ", " ");
                if (line[0] == ' ') line = line.Remove(0, 1);
                //line = line.Replace(".", ",");
                number = line.Split(new char[] {' '});
                Vertex vertex =
                    new Vertex(double.Parse(number[1]), double.Parse(number[2]));
                vertexes.Add(vertex);
                int boundNum = int.Parse(number[3]);
                if (boundNum != 0)
                    temp[boundNum - 1].Add(vertex);
            }
            reader.Close();

            reader = File.OpenText("c.1.ele");
            line = reader.ReadLine();
            number = line.Split(new char[] {' '});
            int triangleNum = int.Parse(number[0]);
            elements = new List<FEMElement>();
            for (int i = 0; i < triangleNum; i++)
            {
                line = reader.ReadLine();
                while (line.Contains("  ")) line = line.Replace("  ", " ");
                if (line[0] == ' ') line = line.Remove(0, 1);
                number = line.Split(new char[] {' '});
                Vertex vi = vertexes[int.Parse(number[1]) - 1];
                Vertex vj = vertexes[int.Parse(number[2]) - 1];
                Vertex vm = vertexes[int.Parse(number[3]) - 1];
                elements.Add(new LinearTriangle(vi, vj, vm, int.Parse(number[0]) - 1));
            }
            reader.Close();

            for (int i = 0; i < polygon.Count; i++)
            {
                int j = (i + 1)%polygon.Count;
                if (!temp[i].Contains(vertexes[i]))
                    temp[i].Add(vertexes[i]);
                if (!temp[i].Contains(vertexes[j]))
                    temp[i].Add(vertexes[j]);
                temp[i].Sort();
            }
            for (int i = 0; i < boundaries.Length; i++)
            {
                for (int j = 0; j < temp[i].Count - 1; j++)
                    boundaries[i].Add(new LinearEdge(temp[i][j], temp[i][j + 1]));
            }

            for (int i = 0; i < boundaries.Length; i++)
                for (int j = 0; j < boundaries[i].Count; j++)
                    if (!polygon.hasVertex(boundaries[i][j].A) || !polygon.hasVertex(boundaries[i][j].B))
                        boundaries[i].RemoveAt(j--);

            for (int i = 0; i < elements.Count; i++)
                if (!polygon.hasElement(elements[i]))
                    elements.RemoveAt(i--);

            for (int i = 0; i < vertexes.Count; i++)
                if (!polygon.hasVertex(vertexes[i]))
                    vertexes.RemoveAt(i--);
        }

        public override void triangulate(double minAngle, double maxArea)
        {
            writeFile();
            executeTriangle(minAngle, maxArea);
            readFile();

            File.Delete("C.poly");
            File.Delete("c.1.ele");
            File.Delete("c.1.node");
            File.Delete("c.1.poly");
        }
        #endregion
    }
}