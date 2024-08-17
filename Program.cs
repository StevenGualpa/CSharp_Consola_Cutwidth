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
            Console.WriteLine("Cutwidth Problem Solver");

            // Ruta del archivo de grafo dentro del proyecto
            string filePath = "graph.txt"; // Asegúrate de que la ruta sea correcta según tu configuración

            // Cargar grafo
            Graph graph = LoadGraph(filePath);

            // Solicitar detalles de la muestra
            Console.WriteLine("Enter the number of permutations to generate:");
            int permutations = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the number of iterations:");
            int iterations = int.Parse(Console.ReadLine());

            // Procesar el grafo
            ProcessGraph(graph, permutations, iterations);
            Console.ReadKey();
        }

        static Graph LoadGraph(string filePath)
        {
            List<Arista> edges = new List<Arista>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var nodes = line.Split(' ').Select(int.Parse).ToArray();
                    edges.Add(new Arista(nodes[0], nodes[1]));
                }
            }
            return new Graph(edges);
        }

        static void ProcessGraph(Graph graph, int permutations, int iterations)
        {
            List<int> nodes = graph.Edges.SelectMany(e => new[] { e.Node1, e.Node2 }).Distinct().ToList();
            Random random = new Random(DateTime.Now.Millisecond);
            int globalMinCutwidth = int.MaxValue;
            List<int> bestGlobalOrder = null;

            for (int iter = 0; iter < iterations; iter++)
            {
                int minCutwidth = int.MaxValue;
                List<int> bestOrder = null;
                Console.WriteLine($"Iteration {iter + 1}:");

                for (int i = 0; i < permutations; i++)
                {
                    List<int> currentOrder = nodes.OrderBy(n => random.Next()).ToList();
                    int currentCutwidth = CalculateCutwidth(graph, currentOrder, true); // Modified to include verbose output
                    if (currentCutwidth < minCutwidth)
                    {
                        minCutwidth = currentCutwidth;
                        bestOrder = new List<int>(currentOrder);
                    }
                    Console.WriteLine($"Permutation {i + 1}: {string.Join(", ", currentOrder)} - Cutwidth: {currentCutwidth}");
                }
                // Comparar el mínimo de esta iteración con el mínimo global
                if (minCutwidth < globalMinCutwidth)
                {
                    globalMinCutwidth = minCutwidth;
                    bestGlobalOrder = bestOrder;
                }

                Console.WriteLine($"Iteration {iter + 1}, Best Cutwidth: {minCutwidth}");
            }
            // Mostrar el mejor resultado global después de todas las iteraciones
            Console.WriteLine($"Global Best Cutwidth: {globalMinCutwidth}");
            Console.WriteLine("Best Node Order: " + string.Join(", ", bestGlobalOrder));
        }

        
        
        
        // Modified to include optional verbose output
        static int CalculateCutwidth(Graph graph, List<int> order, bool verbose = false)
        {
            int maxCrossing = 0;
            for (int i = 0; i < order.Count - 1; i++)
            {
                var crossingEdges = new List<string>();
                int crossings = 0;
                foreach (var edge in graph.Edges)
                {
                    if (order.IndexOf(edge.Node1) <= i && order.IndexOf(edge.Node2) > i || order.IndexOf(edge.Node2) <= i && order.IndexOf(edge.Node1) > i)
                    {
                        crossings++;
                        crossingEdges.Add($"({edge.Node1}-{edge.Node2})");
                    }
                }
                maxCrossing = Math.Max(maxCrossing, crossings);
                if (verbose)
                {
                    Console.WriteLine($" Between {order[i]} and {order[i + 1]}: {crossings} crossings ({string.Join(", ", crossingEdges)})");
                }
            }
            return maxCrossing;
        }
    }
}
