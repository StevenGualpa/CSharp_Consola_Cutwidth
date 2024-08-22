using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoModelos
{
    class Arista
    {
        public int Nodo1 { get; private set; }
        public int Nodo2 { get; private set; }

        public Arista(int nodo1, int nodo2)
        {
            Nodo1 = nodo1;
            Nodo2 = nodo2;
        }
    }
}
