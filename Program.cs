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

        static long Factorial(int n)
        {
            long result = 1;
            for (int i = 1; i <= n; i++)
                result *= i;
            return result;
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
            string filePath = "graph.txt";
            Grafo grafo = CargarGrafo(filePath);
            ImprimirGrafo(grafo);
            Console.WriteLine();
            // Mostrar el número total de permutaciones posibles
            int numNodos = grafo.Bordes.SelectMany(e => new[] { e.Nodo1, e.Nodo2 }).Distinct().Count();
            Console.WriteLine($"La poblacion total es (n factorial): {numNodos}! = {Factorial(numNodos)}");
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
                Console.WriteLine($"\nIteracion {iter + 1}:");
                List<List<int>> allPermutations = new List<List<int>>();
                Permutar(nodes, 0, nodes.Count - 1, allPermutations);
                var selectedPermutations = allPermutations.Take(permutations);
                foreach (var order in selectedPermutations)
                {
                    int currentCutwidth = CalcularCutwidth(grafo, order);

                    Console.WriteLine($"Permutation: {string.Join(", ", order)} - Cutwidth: {currentCutwidth}");

                    if (currentCutwidth < iterationMinCutwidth)
                    {
                        iterationMinCutwidth = currentCutwidth;
                    }
                }

                Console.WriteLine($"Iteracion {iter + 1}, Mejor Cutwidth: {iterationMinCutwidth}");
            }
        }



        static int CalcularCutwidth(Grafo grafo, List<int> order, bool verbose = true)
        {
            int maximoCruce = 0;
            for (int i = 0; i < order.Count - 1; i++)
            {
                int cruces = 0;
                List<string> bordeCruce = new List<string>();

                foreach (var edge in grafo.Bordes)
                {
                    if ((order.IndexOf(edge.Nodo1) <= i && order.IndexOf(edge.Nodo2) > i) ||
                        (order.IndexOf(edge.Nodo2) <= i && order.IndexOf(edge.Nodo1) > i))
                    {
                        cruces++;
                        bordeCruce.Add($"({edge.Nodo1}-{edge.Nodo2})");
                    }
                }
                if (cruces > maximoCruce)
                {
                    maximoCruce = cruces;
                }
                if (verbose)
                {
                    Console.WriteLine($" Entre {order[i]} y {order[i + 1]}: {cruces} cruces ({String.Join(", ", bordeCruce)})");
                }
            }
            return maximoCruce;
        }
       
        
        static void Permutar(List<int> list, int k, int m, List<List<int>> allPermutations)
        {
            if (k == m)
            {
                allPermutations.Add(new List<int>(list));
            }
            else
            {
                for (int i = k; i <= m; i++)
                {
                    Intercambio(list, k, i);
                    Permutar(list, k + 1, m, allPermutations);
                    Intercambio(list, k, i);
                }
            }
        }

        static void Intercambio(List<int> list, int index1, int index2)
        {
            int temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }
    }

}

