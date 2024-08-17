using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoModelos
{
    class Arista
    {
        public int Node1 { get; private set; }
        public int Node2 { get; private set; }

        public Arista(int node1, int node2)
        {
            Node1 = node1;
            Node2 = node2;
        }
    }
}
