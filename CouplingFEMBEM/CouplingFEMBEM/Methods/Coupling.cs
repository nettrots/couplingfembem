﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SbB.Diploma.Methods;

namespace SbB.Diploma
{
    class CouplingMethod
    {
        private MethodBase[] methods;
        private FEMMethod FEM;
        private MakarBEMMethod BEM;
        private MortarMethod Mortar;
        private List<Vertex> vertexes;

        private Matrix GlobalMatrix;
        private Vector GlobalVector;
        private Vector Result;

        public CouplingMethod(string filename)
        {
            //Create methods and fill with data

            //load yaml file
            //traverse tree to arraylist
            ArrayList data;

            //TODO: Create FEM
            Polygon FEMPolygon=new Polygon(new Vertex[]{new Vertex(1,2), });
            double Angle=1;
            double Area=1;
            double YoungModulus=1;
            double PoissonRatio=1;


            FEM = new FEMMethod(new object());

            FEM.BoundaryClasses = new BoundaryClass[0];
            FEM.PoissonRatio = PoissonRatio;
            FEM.YoungModulus = YoungModulus;

            LinialTriangleTriangulation ltt=new LinialTriangleTriangulation(FEMPolygon);
            FEM.Triangulation = ltt;
            FEM.Angle = Angle;
            FEM.Area = Area;
            
            //TODO: Create BEM
            Polygon BEMPolygon=new Polygon(new Vertex[0]);
            int ElementsPerSegment = 1;
            YoungModulus = 1;
            PoissonRatio = 1;

            BEM=new MakarBEMMethod(BEMPolygon);

            BEM.PoissonRatio = PoissonRatio;
            BEM.YoungModulus = YoungModulus;
            BEM.ElementsPerSegment = ElementsPerSegment;
            BEM.BoundaryClasses = new BoundaryClass[0];

            //TODO: Create mortar

            Mortar=new MortarMethod();
            MortarSide ms=new MortarSide(FEM,BEM,typeof(LinearMortar) );
            Mortar.MortarSides.Add(ms);


        }

        public void assamble()
        {
            FEM.Initialize();
            BEM.Initialize();
            Mortar.Initialize();








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
                        if (boundarylist[i].Doft.Length != 0)
                            boundarylist[i].Doft = new int[] {counter++,counter++};
                    }
                    for (int j = 1; j < boundarylist.Count - 1; j++)
                    {
                        if (boundarylist[i].Dofu.Length != 0)
                            boundarylist[i].Dofu = new int[] { counter++,counter++ };
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
                        if (boundarylist[i].Dofu.Length != 0)
                            boundarylist[i].Dofu = new int[] { counter++, counter++ };
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
                        if (boundarylist[i].Doft.Length != 0)
                            boundarylist[i].Doft = new int[] { counter++,counter++ };
                    }
                }
            }
        }

        public void fillSystem()
        {
            //Create system from all methods
            //FEM+BEM+Mortar
        }

        public void solveSystem()
        {
            LUSolve.Solve(GlobalMatrix, GlobalVector, Result);
        }
    }
}
