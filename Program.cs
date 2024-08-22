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
            string filePath = "graph.txt";

            // Cargar grafo
            Graph graph = LoadGraph(filePath);

            // Solicitar detalles de la muestra
            Console.WriteLine("Enter the number of permutations to generate:");
            if (!int.TryParse(Console.ReadLine(), out int permutations))
            {
                Console.WriteLine("Invalid input for permutations. Please enter a valid integer.");
                return;
            }

            Console.WriteLine("Enter the number of iterations:");
            if (!int.TryParse(Console.ReadLine(), out int iterations))
            {
                Console.WriteLine("Invalid input for iterations. Please enter a valid integer.");
                return;
            }

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
                bool isFirstLine = true;
                while ((line = reader.ReadLine()) != null)
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
            return new Graph(edges);
        }

        static void ProcessGraph(Graph graph, int permutations, int iterations)
        {
            List<int> nodes = graph.Edges.SelectMany(e => new[] { e.Node1, e.Node2 }).Distinct().ToList();
            Random random = new Random();

            for (int iter = 0; iter < iterations; iter++)
            {
                int iterationMinCutwidth = int.MaxValue;

                Console.WriteLine($"\nIteration {iter + 1}:");
                for (int i = 0; i < permutations; i++)
                {
                    List<int> currentOrder = nodes.OrderBy(n => random.Next()).ToList();
                    int currentCutwidth = CalculateCutwidth(graph, currentOrder);

                    Console.WriteLine($"Permutation {i + 1}: {string.Join(", ", currentOrder)} - Cutwidth: {currentCutwidth}");

                    if (currentCutwidth < iterationMinCutwidth)
                    {
                        iterationMinCutwidth = currentCutwidth;
                    }
                }

                Console.WriteLine($"Iteration {iter + 1}, Best Cutwidth: {iterationMinCutwidth}");
            }
        }

        static int CalculateCutwidth(Graph graph, List<int> order, bool verbose = true)
        {
            int maxCrossing = 0;

            // Iterar sobre cada punto de corte posible en la permutación
            for (int i = 0; i < order.Count - 1; i++)
            {
                int crossings = 0;
                List<string> crossingEdges = new List<string>(); // Lista para guardar las aristas que cruzan

                // Comparar cada arista con el punto de corte actual
                foreach (var edge in graph.Edges)
                {
                    if ((order.IndexOf(edge.Node1) <= i && order.IndexOf(edge.Node2) > i) ||
                        (order.IndexOf(edge.Node2) <= i && order.IndexOf(edge.Node1) > i))
                    {
                        crossings++; // Incrementar el conteo de cruces
                        crossingEdges.Add($"({edge.Node1}-{edge.Node2})"); // Agregar la arista a la lista de cruces
                    }
                }

                // Actualizar el máximo de cruces encontrado
                if (crossings > maxCrossing)
                {
                    maxCrossing = crossings;
                }

                // Si verbose es true, imprimir detalles de este punto de corte
                if (verbose)
                {
                    Console.WriteLine($" Entre {order[i]} y {order[i + 1]}: {crossings} cruces ({String.Join(", ", crossingEdges)})");
                }
            }

            return maxCrossing; // Retornar el número máximo de cruces encontrado en esta permutación
        }



        static void ShowAllCombinations(Graph graph)
        {
            List<int> nodes = graph.Edges.SelectMany(e => new[] { e.Node1, e.Node2 }).Distinct().ToList();
            var permutations = GetPermutations(nodes, nodes.Count);

            foreach (var perm in permutations)
            {
                Console.WriteLine($"Permutation: {string.Join(", ", perm)}");
            }
        }

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                            (t1, t2) => t1.Concat(new T[] { t2 }));
        }


    }
}
