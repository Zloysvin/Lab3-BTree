using System;

namespace Lab4_ants
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter number of nodes:");
            int number_of_nodes = Convert.ToInt32(Console.ReadLine());
            Model world = new Model(number_of_nodes);
            world.print_graph();

            Algorithm algorithm = new Algorithm(world, number_of_nodes);
            algorithm.run();
        }
    }
}
