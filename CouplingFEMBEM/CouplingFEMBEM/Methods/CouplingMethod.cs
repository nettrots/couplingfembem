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

    public class CouplingFEMBEM: BPMethod
    {
        #region Fields
        private BPMethod[] methods;
        public FEMMethod FEM;
        public MakarBEMMethod BEM;
        private MortarSide mortarSide;
        private Mortar Mortar;
        #endregion

        #region Constructors
        public CouplingFEMBEM(Dictionary<string, HashValue> data)
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

            //DONE: Create mortar
            List<int> mortarsides = new List<int>();
            for (int i = 0; i < FEM.BoundaryClasses.Length; i++)
            {
                if (FEM.BoundaryClasses[i].type()==BoundaryType.MORTAR) mortarsides.Add(i);
            }
            mortarSide = new MortarSide(FEM, mortarsides);

            Name = "FEM(" + FEM.Area+ ")-BEM("+BEM.ElementsPerSegment+")";
        }
        #endregion

        #region Properties
        public List<Func<double, double, double>> FuncList { get; set; }
        public override string Name { get; set; }
        #endregion

        #region Methods

        public override void Initialize()
        {
            for (int i = 0; i < methods.Length; i++)
                methods[i].Initialize();
            mortarSide.createMortarNodes();
            Mortar = mortarSide.createMortar();
        }

        public override void Run()
        {
            for (int i = 0; i < methods.Length; i++)
                methods[i].Run();

            Matrix[] D = new Matrix[methods.Length];

            List<BoundEdge>[] boundaries = new List<BoundEdge>[mortarSide.MortarSides.Count];

            for (int i = 0; i < boundaries.Length; i++)
                boundaries[i] = (methods[0] as IDiscretization).Boundaries[mortarSide.MortarSides[i]];
            D[0] = Mortar.createD(2 * (methods[0] as IDiscretization).Vertexes.Count, boundaries);

            for (int i = 0; i < boundaries.Length; i++)
            {
                int k = methods[1].Polygon.isEdgeOnPolygon(methods[0].Polygon.edge(mortarSide.MortarSides[i]));
                boundaries[i] = (methods[1] as IDiscretization).Boundaries[k];
            }
            D[1] = -1 * Mortar.createD(2 * (methods[1] as IDiscretization).Vertexes.Count, boundaries);


            // Create k and F
            int size = methods[0].K.Size.m + methods[1].K.Size.m + D[0].Size.n;
            int sizeK = methods[0].K.Size.m + methods[1].K.Size.m;
            K = new Matrix(size, size);
            F = new Vector(size);

            for (int i = 0; i < methods.Length; i++)
            {
                size = 0;
                for (int l = 0; l < i; l++)
                {
                    size += methods[l].K.Size.m;
                }
                for (int j = 0; j < methods[i].K.Size.m; j++)
                {
                    F[size + j] = methods[i].F[j];
                    for (int k = 0; k < methods[i].K.Size.n; k++)
                        K[size + j][size + k] = methods[i].K[j][k];
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

            FEM.Results = new Vector(FEM.K.Size.m);
            for (int i = 0; i < FEM.Results.Length; i++)
                FEM.Results[i] = results[i];

            BEM.Results = new Vector(BEM.K.Size.m);
            for (int i = 0; i < BEM.Results.Length; i++)
                BEM.Results[i] = results[FEM.Results.Length + i];

            BEM.caltT();
        }

        public override double U(double x, double y)
        {
            return FEM.U(x, y);
        }

        public override double V(double x, double y)
        {
            return FEM.V(x, y);
        }

        public override double[] U(Vertex[] vertices)
        {
            List<Vertex> rezF = new List<Vertex>();
            List<Vertex> rezB = new List<Vertex>();
            foreach (Vertex vertex in vertices)
            {
                if(FEM.Polygon.hasVertex(vertex))
                    rezF.Add(vertex);
                else
                    rezB.Add(vertex);

            }
            List<double> rez = new List<double>();
            rez.AddRange(FEM.U(rezF.ToArray()));
            rez.AddRange(BEM.U(rezB.ToArray()));
            return rez.ToArray();
        }

        public override double[] V(Vertex[] vertices)
        {
            List<Vertex> rezF = new List<Vertex>();
            List<Vertex> rezB = new List<Vertex>();
            foreach (Vertex vertex in vertices)
            {
                if (FEM.Polygon.hasVertex(vertex))
                    rezF.Add(vertex);
                else
                    rezB.Add(vertex);

            }
            List<double> rez = new List<double>();
            rez.AddRange(FEM.V(rezF.ToArray()));
            rez.AddRange(BEM.V(rezB.ToArray()));
            return rez.ToArray();
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
