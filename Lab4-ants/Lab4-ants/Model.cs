using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_ants
{
    internal class Model
    {
        public int[,] distance_matrix;
        public double[,] pheromone_matrix;

        public int MIN_DISTANCE = 1;
        public int MAX_DISTANCE = 40;
        public double START_PHEREMONE = 0.1;
        public Model(int number_of_nodes)
        {
            distance_matrix = new int[number_of_nodes, number_of_nodes];
            pheromone_matrix = new double[number_of_nodes, number_of_nodes];
            __set_random(number_of_nodes);
        }

        private void __set_random(int number_of_nodes)
        {
            for (int i = 0; i < number_of_nodes; i++)
            {
                for (int j = 0; j < number_of_nodes; j++)
                {
                    distance_matrix[i, j] = 0;
                    pheromone_matrix[i, j] = 0;
                }
            }

            for (int i = 0; i < number_of_nodes; i++)
            {
                for (int j = i + 1; j < number_of_nodes; j++)
                {
                    Random rnd = new Random();
                    distance_matrix[i, j] = rnd.Next(MIN_DISTANCE, MAX_DISTANCE);
                    distance_matrix[j, i] = distance_matrix[i , j];
                    pheromone_matrix[i, j] = START_PHEREMONE;
                    pheromone_matrix[j, i] = START_PHEREMONE;
                }
            }
        }

        //жадібний алгоритм, що шукає цикл починаючи з current_node
        public int greedy_search(int current_node)
        {
            List<int> visited_nodes = new List<int>();
            int full_dist = 0;
            for (int i = 0; i < distance_matrix.Length - 1; i++)
            {
                int next_node = __find_min(distance_matrix, current_node, visited_nodes.ToArray());
                full_dist += distance_matrix[current_node , next_node];
                visited_nodes.Add(current_node);
                current_node = next_node;
            }

            full_dist += distance_matrix[current_node , visited_nodes[0]];
            visited_nodes.Add(current_node);
            return full_dist;
        }

        public int __find_min(int[,] distances, int column, int[] visited_nodes)
        {
            int min_value = MAX_DISTANCE + 1;
            int min_index = 0;
            for (int i = 0; i < distances.GetLength(1); i++)
            {
                if (min_value > distances[column, i] && distances[column, i] > 0 && !visited_nodes.Contains(i))
                {
                    min_value = distances[column, i];
                    min_index = i;
                }
            }
            return min_index;
        }

        public void print_graph()
        {
            Console.WriteLine("Distance matrix:");
            for (int i = 0; i < distance_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < distance_matrix.GetLength(1); j++)
                {
                    Console.Write(distance_matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void print_pheromone()
        {
            Console.WriteLine("The pheromone matrix is");
            for (int i = 0; i < pheromone_matrix.GetLength(0); i++)
            {
                for (int j = 0; j < pheromone_matrix.GetLength(1); j++)
                {
                    Console.Write(pheromone_matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
