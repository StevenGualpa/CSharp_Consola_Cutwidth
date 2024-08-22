using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProyectoModelos.Program;

namespace ProyectoModelos
{
    class Grafo
    {
        public List<Arista> Bordes { get; private set; }

        public Grafo(List<Arista> bordes)
        {
            Bordes = bordes;
        }

    }
}
