using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProyectoModelos.Program;

namespace ProyectoModelos
{
    class Graph
    {
        public List<Arista> Edges { get; private set; }

        public Graph(List<Arista> edges)
        {
            Edges = edges;
        }

    }
}
