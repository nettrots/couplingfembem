using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QiHe.Yaml.Grammar;
using QiHe.Yaml.YamlUtility.UI;
using SbB.Diploma.Methods;
using SbB.Diploma.Yaml.Custom;

namespace SbB.Diploma
{
    using MyHash=Dictionary<string, object> ; 

    public class CouplingMethod: BPMethod
    {
        public Polygon Polygon { get; set; }
        private BPMethod[] methods;
        public FEMMethod FEM;
        public Polygon FemPolygon { get; set; }
        public MakarBEMMethod BEM;
        public Polygon BemPolygon { get; set; }
        private MortarMethod Mortar;
        private List<Vertex> vertexes;
        public List<Func<double,double, double>> FuncList { get; set; }

        private Matrix GlobalMatrix;
        private Vector GlobalVector;
        private Vector Result;

        public CouplingMethod(string filename)
        {
            //Create methods and fill with data

            //load yaml file
            //traverse tree to arraylist
            //ArrayList data=new ArrayList();
            Dictionary<string, HashValue> data = new Dictionary<string, HashValue>();
            if (!File.Exists(filename))
            {
                MessageBox.Show(filename + " does not exist.");
                return;
            }
            YamlParser parser = new YamlParser();
            TextInput input = new TextInput(File.ReadAllText(filename));
            bool success;
            YamlStream yamlStream = parser.ParseYamlStream(input, out success);
            if (success)
            {
                data.Clear();
                foreach (YamlDocument doc in yamlStream.Documents)
                {
                    Dictionary<string, HashValue> a1 = YamlEmittor.CreateNode(doc.Root).eHash;
                    foreach (KeyValuePair<string, HashValue> pair in a1)
                    {
                        data.Add(pair.Key, pair.Value);
                    }
                }
            }
            else
            {
             //   MessageBox.Show(parser.GetEorrorMessages());
            }

            if (data.ContainsKey("Polygon"))
            {
                Vertex[] poly = new Vertex[data["Polygon"].eHash["Vertex"].eHash.Count];

                for (int i = 0; i < poly.Length; i++)
                {
                    double x = data["Polygon"].eHash["Vertex"].eHash[i.ToString()].eHash["0"].eDouble;
                    double y = data["Polygon"].eHash["Vertex"].eHash[i.ToString()].eHash["1"].eDouble;
                    poly[i] = new Vertex(x, y);
                }
                Polygon = new Polygon(poly);
            }

            methods = new BPMethod[2];

            //DONE: Create FEM
            if (data.ContainsKey("FEM"))
            {
                FEM = new FEMMethod(data["FEM"].eHash);
            }
            methods[0] = FEM;

            //DONE: Create BEM
            if (data.ContainsKey("BEM"))
            {
                BEM = new MakarBEMMethod(data["BEM"].eHash);
            }
            methods[1] = BEM;

            //TODO: Create mortar

            Mortar=new MortarMethod();
            MortarSide ms=new MortarSide(FEM,BEM,typeof(LinearMortar) );
            Mortar.MortarSides = new List<MortarSide>();
            Mortar.MortarSides.Add(ms);
        }

        public string Name
        {
            get; set;
        }

        public void assemble()
        {
            FEM.Initialize();
            BEM.Initialize();
            //Mortar.Initialize();

            //Assamble vertexes (dofs) in all methods
            vertexes = new List<Vertex>();

            foreach (Vertex vertex in FEM.Vertexes)
                vertexes.Add(vertex);

            for (int i = 0; i < BEM.Vertexes.Count; i++)
            {
                int k = vertexes.IndexOf(BEM.Vertexes[i]);
                if (k >= 0) BEM.Vertexes[i] = vertexes[k];
                else vertexes.Add(BEM.Vertexes[i]);
            }

            vertexes.Sort();


            int counter = 0;

            foreach (Vertex vertex in FEM.Vertexes)
            {
                vertex.Dofu = new int[] {counter++, counter++};
            }

            for (int i = 0; i < FEM.BoundaryClasses.Length; i++)
            {
                if (FEM.BoundaryClasses[i].type()==BoundaryType.MORTAR)
                {
                    List<Vertex> boundarylist = new List<Vertex>();
                    foreach (FEMEdge edge in FEM.Boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                            if (!boundarylist.Contains(edge[j])) boundarylist.Add(edge[j]);
                    boundarylist.Sort();
                    for (int j = 1; j < boundarylist.Count-1; j++)
                    {
                        boundarylist[i].Doft = new int[] { counter++, counter++ };
                    }
                }
            }

            for (int i = 0; i < BEM.BoundaryClasses.Length; i++)
            {
                if (BEM.BoundaryClasses[i].type() == BoundaryType.NONMORTAR)
                {
                    List<Vertex> boundarylist = new List<Vertex>();
                    foreach (BoundEdge edge in BEM.Boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            edge[j] = vertexes[vertexes.IndexOf(edge[j])];
                            if (!boundarylist.Contains(edge[j])) boundarylist.Add(edge[j]);
                        }
                    boundarylist.Sort();
                    for (int j = 1; j < boundarylist.Count - 1; j++)
                    {
                        if (boundarylist[j].Doft.Length == 0)
                            boundarylist[j].Doft = new int[] {counter++,counter++};
                    }
                    for (int j = 1; j < boundarylist.Count - 1; j++)
                    {
                        if (boundarylist[j].Dofu.Length == 0)
                            boundarylist[j].Dofu = new int[] { counter++,counter++ };
                    }
                }
            }

            for (int i = 0; i < BEM.BoundaryClasses.Length; i++)
            {
                if (BEM.BoundaryClasses[i].type()==BoundaryType.STATIC)
                {
                    List<Vertex> boundarylist = new List<Vertex>();
                    foreach (BoundEdge edge in BEM.Boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            edge[j] = vertexes[vertexes.IndexOf(edge[j])];
                            if (!boundarylist.Contains(edge[j])) boundarylist.Add(edge[j]);
                        }
                    boundarylist.Sort();
                    for (int j = 0; j < boundarylist.Count; j++)
                    {
                        if (boundarylist[j].Dofu.Length == 0)
                            boundarylist[j].Dofu = new int[] { counter++, counter++ };
                    }
                }

                if (BEM.BoundaryClasses[i].type() == BoundaryType.KINEMATIC)
                {
                    List<Vertex> boundarylist = new List<Vertex>();
                    foreach (BoundEdge edge in BEM.Boundaries[i])
                        for (int j = 0; j < edge.NodesCount; j++)
                        {
                            edge[j] = vertexes[vertexes.IndexOf(edge[j])];
                            if (!boundarylist.Contains(edge[j])) boundarylist.Add(edge[j]);
                        }
                    boundarylist.Sort();
                    for (int j = 0; j < boundarylist.Count; j++)
                    {
                        if (boundarylist[j].Doft.Length == 0)
                            boundarylist[j].Doft = new int[] { counter++,counter++ };
                    }
                }
            }

            /*foreach (MortarSide side in Mortar.MortarSides)
            {
                for (int i = 1; i < side.Vertexes.Count-1; i++)
                    side.Vertexes[i].Dofm = new int[] { counter++, counter++ };
            }*/
        }


        public override string ToString()
        {
            return Name;
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }

        public override void Solve()
        {
            throw new NotImplementedException();
        }
    }
}
