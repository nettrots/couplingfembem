using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SbB.Diploma
{
    public interface IDiscretization
    {
        List<Vertex> Vertexes { get; }
        List<BoundEdge>[] Boundaries { get; } 
    }
}
