using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SbB.Diploma;
using SbB.Diploma.Methods;

namespace TestingCouplingClesses
{
    class Program
    {
        static void Main(string[] args)
        {
            Vertex[] vertexes = new Vertex[]
                { 
                  new Vertex(1,1),
                  new Vertex(1,0),
                  new Vertex(2,0),
                  new Vertex(2,1) 
                };

            Polygon polygon = new Polygon(vertexes);

            MakarBEMMethod makar = new MakarBEMMethod(polygon);
            makar.Initialize();
            
        }
    }
}
