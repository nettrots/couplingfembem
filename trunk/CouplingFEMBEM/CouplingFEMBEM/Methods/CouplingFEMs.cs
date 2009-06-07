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
    using MyHash = Dictionary<string, object>;

    public class CouplingFEMs : BPMethod
    {
        #region Fields
        private MortarSide mortarSide;
        private Mortar Mortar;
        #endregion

        #region Constructors
        public CouplingFEMs(Dictionary<string, HashValue> data)
        {
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

            FEMs = new FEMMethod[2];

            //DONE: Create FEM
            if (data.ContainsKey("FEM1"))
            {
                FEMs[0] = new FEMMethod(data["FEM1"].eHash);
            }
            
            //DONE: Create BEM
            if (data.ContainsKey("FEM2"))
            {
                FEMs[1] = new FEMMethod(data["FEM2"].eHash);
            }

            //DONE: Create mortar
            List<int> mortarsides = new List<int>();
            for (int i = 0; i < FEMs[0].BoundaryClasses.Length; i++)
            {
                if (FEMs[0].BoundaryClasses[i].type() == BoundaryType.MORTAR) mortarsides.Add(i);
            }
            mortarSide = new MortarSide(FEMs[0], mortarsides);

            Name = "FEM(" + FEMs[0].Area + ")-FEM(" + FEMs[1].Area + ")";

        }
        #endregion

        #region Properties
        public FEMMethod[] FEMs{ get; set;}
        public List<Func<double, double, double>> FuncList { get; set; }
        public override string Name { get; set; }
        #endregion

        #region Methods

        public override void Initialize()
        {
            for (int i = 0; i < FEMs.Length; i++)
               FEMs[i].Initialize(); 
            mortarSide.createMortarNodes();
            Mortar = mortarSide.createMortar();
        }

        public override void Run()
        {
            for (int i = 0; i < FEMs.Length; i++)
                FEMs[i].Run();

            Matrix[] D = new Matrix[FEMs.Length];

            List<BoundEdge>[] boundaries = new List<BoundEdge>[mortarSide.MortarSides.Count];

            for (int i = 0; i < boundaries.Length; i++)
                boundaries[i] = FEMs[0].Boundaries[mortarSide.MortarSides[i]];
            D[0] = Mortar.createD(2*FEMs[0].Vertexes.Count, boundaries);

            for (int i = 0; i < boundaries.Length; i++)
            {
                int k = FEMs[1].Polygon.isEdgeOnPolygon(FEMs[0].Polygon.edge(mortarSide.MortarSides[i]));
                boundaries[i] = FEMs[1].Boundaries[k];
            }
            D[1] = -1*Mortar.createD(2*FEMs[1].Vertexes.Count, boundaries);


            // Create k and F
            int size = FEMs[0].K.Size.m + FEMs[1].K.Size.m + D[0].Size.n;
            int sizeK = FEMs[0].K.Size.m + FEMs[1].K.Size.m;
            K = new Matrix(size, size);
            F = new Vector(size);

            for (int i = 0; i < FEMs.Length; i++)
            {
                size = 0;
                for (int l = 0; l < i; l++)
                {
                    size += FEMs[l].K.Size.m;
                }
                for (int j = 0; j < FEMs[i].K.Size.m; j++)
                {
                    F[size + j] = FEMs[i].F[j];
                    for (int k = 0; k < FEMs[i].K.Size.n; k++)
                        K[size + j][size + k] = FEMs[i].K[j][k];
                    for (int k = 0; k < D[i].Size.n; k++)
                    {
                        K[size + j][sizeK + k] = D[i][j][k];
                        K[sizeK + k][size + j] = D[i][j][k];
                    }
                }
            }
        }

        public override void Solve()
        {
            LUSolve.Solve(K, F, out results);

            for (int i = 0; i < FEMs.Length; i++)
            {
                FEMs[i].Results = new Vector(FEMs[i].K.Size.m);
                int offset = 0;
                for (int j = 0; j < i; j++)
                    offset += FEMs[j].F.Length;
                for (int j = 0; j < FEMs[i].Results.Length; j++)
                {
                    FEMs[i].Results[j] = results[j + offset];
                }
            }
        }

        public override double U(double x, double y)
        {
            for (int i = 0; i < FEMs.Length; i++)
                if (FEMs[i].Polygon.hasVertex(new Vertex(x, y))) return FEMs[i].U(x, y);
            return 0.0;
        }

        public override double V(double x, double y)
        {
            for (int i = 0; i < FEMs.Length; i++)
                if (FEMs[i].Polygon.hasVertex(new Vertex(x, y))) return FEMs[i].V(x, y);
            return 0.0;
        }

        public override double[] U(Vertex[] vertices)
        {
            List<double> rez = new List<double>();
            foreach (Vertex vertex in vertices)
            {
                rez.Add(U(vertex.X, vertex.Y));
            }
            return rez.ToArray();
        }

        public override double[] V(Vertex[] vertices)
        {
            List<double> rez = new List<double>();
            foreach (Vertex vertex in vertices)
            {
                rez.Add(U(vertex.X, vertex.Y));
            }
            return rez.ToArray();
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
