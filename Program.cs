using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoModelos
{
    internal class Program
    {
        static void Main(string[] args)
        {
             Ejecucion();
        }

        static void ImprimirGrafo(Grafo graph)
        {
            Console.WriteLine("Graph representation:");
            foreach (var edge in graph.Bordes)
            {
                Console.WriteLine($"{edge.Nodo1} -- {edge.Nodo2}");
            }
        }

        static void Ejecucion()
        {
            Console.WriteLine("Problema del minimizado del cutwidth");

            // Ruta del archivo de grafo dentro del proyecto
            string filePath = "graph.txt";

            // Cargar grafo
            Grafo grafo = CargarGrafo(filePath);

             ImprimirGrafo(grafo);
             Console.WriteLine();

            // Solicitar detalles de la muestra
            Console.WriteLine("Ingrese el número de permutaciones a generar:");


            if (!int.TryParse(Console.ReadLine(), out int permutations))
            {
                Console.WriteLine("Entrada no válida para permutaciones. Por favor ingrese un número entero válido.");
                return;
            }

            Console.WriteLine("Ingrese el número de iteraciones:");
            if (!int.TryParse(Console.ReadLine(), out int iterations))
            {
                Console.WriteLine("Entrada no válida para iteraciones. Por favor ingrese un número entero válido.");
                return;
            }

            // Procesar el grafo
            ProcessGraph(grafo, permutations, iterations);
            Console.ReadKey();
        }

        static Grafo CargarGrafo(string filePath)
        {
            List<Arista> edges = new List<Arista>();
            using (StreamReader Lectura = new StreamReader(filePath))
            {
                string line;
                bool isFirstLine = true;
                while ((line = Lectura.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    var nodes = line.Split(' ').Select(int.Parse).ToArray();
                    edges.Add(new Arista(nodes[0], nodes[1]));
                }
            }
            return new Grafo(edges);
        }

        static void ProcessGraph(Grafo grafo, int permutations, int iterations)
        {
            List<int> nodes = grafo.Bordes.SelectMany(e => new[] { e.Nodo1, e.Nodo2 }).Distinct().ToList();
            Random random = new Random(DateTime.Now.Millisecond);

            for (int iter = 0; iter < iterations; iter++)
            {
                int iterationMinCutwidth = int.MaxValue;

                Console.WriteLine($"\nIteration {iter + 1}:");
                for (int i = 0; i < permutations; i++)
                {
                    List<int> currentOrder = nodes.OrderBy(n => random.Next()).ToList();
                    int currentCutwidth = CalcularCutwidth(grafo, currentOrder);

                    Console.WriteLine($"Permutation {i + 1}: {string.Join(", ", currentOrder)} - Cutwidth: {currentCutwidth}");

                    if (currentCutwidth < iterationMinCutwidth)
                    {
                        iterationMinCutwidth = currentCutwidth;
                    }
                }

                Console.WriteLine($"Iteration {iter + 1}, Best Cutwidth: {iterationMinCutwidth}");
            }
        }

        static int CalcularCutwidth(Grafo grafo, List<int> order, bool verbose = true)
        {
            int maximoCruce = 0;

            // Iterar sobre cada punto de corte posible en la permutación
            for (int i = 0; i < order.Count - 1; i++)
            {
                int cruces = 0;
                List<string> bordeCruce = new List<string>(); // Lista para guardar las aristas que cruzan

                // Comparar cada arista con el punto de corte actual
                foreach (var edge in grafo.Bordes)
                {
                    if ((order.IndexOf(edge.Nodo1) <= i && order.IndexOf(edge.Nodo2) > i) ||
                        (order.IndexOf(edge.Nodo2) <= i && order.IndexOf(edge.Nodo1) > i))
                    {
                        cruces++; // Incrementar el conteo de cruces
                        bordeCruce.Add($"({edge.Nodo1}-{edge.Nodo2})"); // Agregar la arista a la lista de cruces
                    }
                }

                // Actualizar el máximo de cruces encontrado
                if (cruces > maximoCruce)
                {
                    maximoCruce = cruces;
                }

                // Si verbose es true, imprimir detalles de este punto de corte
                if (verbose)
                {
                    Console.WriteLine($" Entre {order[i]} y {order[i + 1]}: {cruces} cruces ({String.Join(", ", bordeCruce)})");
                }
            }

            return maximoCruce; // Retornar el número máximo de cruces encontrado en esta permutación
        }


    }
}
