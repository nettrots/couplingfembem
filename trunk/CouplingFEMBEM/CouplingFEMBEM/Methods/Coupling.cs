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
        #region Fields
        private BPMethod[] methods;
        public FEMMethod FEM;
        public MakarBEMMethod BEM;
        private MortarSide mortarSide;
        private Mortar Mortar;
        #endregion

        #region Constructors
        public CouplingMethod(Dictionary<string, HashValue> data)
        {
            Name = "Coupling FEM-BEM";
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
        }
        #endregion

        #region Properties
        public List<Func<double, double, double>> FuncList { get; set; }
        public string Name { get; set; }
        #endregion

        #region Methods

        public override void Initialize()
        {
            FEM.Initialize();
            BEM.Initialize();
            mortarSide.createMortarNodes();
            Mortar = mortarSide.createMortar();
        }

        public override void Run()
        {
            FEM.Run();
            BEM.Run();

            Matrix KFEM = FEM.K;
            Matrix KBEM = BEM.K;
            Vector FFEM = FEM.F;
            Vector FBEM = BEM.F;

            List<BoundEdge>[] boundaries = new List<BoundEdge>[mortarSide.MortarSides.Count];
            for (int i = 0; i < boundaries.Length; i++)
            {
                boundaries[i] = FEM.Boundaries[mortarSide.MortarSides[i]];
            }
            Matrix DFEM = Mortar.createD(2*FEM.Vertexes.Count, boundaries);

            for (int i = 0; i < boundaries.Length; i++)
            {
                int k = BEM.Polygon.isEdgeOnPolygon(FEM.Polygon.edge(mortarSide.MortarSides[i]));
                boundaries[i] = BEM.Boundaries[k];
            }
            Matrix DBEM = -1*Mortar.createD(2*BEM.Vertexes.Count, boundaries);

            

            // Create k and F
            int size = KFEM.Size.m + KBEM.Size.m + DFEM.Size.n;
            K = new Matrix(size, size);
            F = new Vector(size);

            for (int i = 0; i < KFEM.Size.m; i++)
            {
                F[i] = FFEM[i];
                for (int j = 0; j < KFEM.Size.n; j++)
                {
                    K[i][j] = KFEM[i][j];
                }
            }

            size = KFEM.Size.m;
            for (int i = 0; i < KBEM.Size.m; i++)
            {
                F[size + i] = FBEM[i];
                for (int j = 0; j < KBEM.Size.n; j++)
                {
                    K[size + i][size + j] = KBEM[i][j];
                }   

            }

            size += KBEM.Size.n;
            for (int i = 0; i < DFEM.Size.m; i++)
            {
                for (int j = 0; j < DFEM.Size.n; j++)
                {
                    K[i][size + j] = DFEM[i][j];
                    K[size + j][i] = DFEM[i][j];
                }
            }

            for (int i = 0; i < DBEM.Size.m; i++)
            {
                for (int j = 0; j < DBEM.Size.n; j++)
                {
                    K[KFEM.Size.m + i][size + j] = DBEM[i][j];
                    K[size + j][KFEM.Size.m + i] = DBEM[i][j];
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
        }

        public override double U(double x, double y)
        {
            return FEM.U(x, y);
        }

        public override double V(double x, double y)
        {
            return FEM.V(x, y);
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}
