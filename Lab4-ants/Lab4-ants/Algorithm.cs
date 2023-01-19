using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_ants
{
    internal class Algorithm
    {
        Model _model;
        int _num_of_nodes;
        int _l_min;
        int _l_best;
        int _best_ant;

        public int MIN_DISTANCE = 1;
        public int MAX_DISTANCE = 40;
        public int T_MAX = 1000;
        public int T_ITER = 20;
        public int ALPHA = 2;
        public int BETA = 4;
        public double R = 0.7;
        public int NUM_OF_ANTS = 10;
        public int NUM_OF_WILD_ANTS = 5;

        public Algorithm(Model world, int num)
        {
            _model = world;
            _num_of_nodes = num;
            // L min
            _l_min = MAX_DISTANCE * _num_of_nodes;
            __find_l_min();
            // L*
            _l_best = MAX_DISTANCE * this._num_of_nodes;
            //індекс мурахи з найкоротшим шляхом
            _best_ant = 0;
        }

        public void __find_l_min()
        {
            for (int i = 0; i < _num_of_nodes; i++)
            {
                int new_value = _model.greedy_search(i);
                if (new_value < _l_min)
                {
                    _l_min = new_value;
                }
            }
            Console.WriteLine("L-min = "+ _l_min);
        }
        public void run()
        {
            List<Ant> ants = __set_ants();
            for (int t = 1; t < T_MAX + 1; t++)
            {
                //обчислюємо шляху для всіх мурах
                foreach (Ant ant in ants)
                {
                    ant.way = new List<int>();
                    ant.way.Add(ant._position);
                    while (ant.way.Count < _num_of_nodes)
                    {
                        int new_node = this.__find_new_node(ant);
                        ant.way.Add(new_node);
                    }
                }
                //знаходимо найкортоший шлях серед всіх мурах
                (int best_length, int best_ant) = __find_min_way(ants);
                if (best_length < _l_best)
                {
                    _l_best = best_length;
                    _best_ant = best_ant;
                }
                __update_pheromone(ants);
                if (t % T_ITER == 0)
                {
                    Console.WriteLine("On the {0} iteration, the best lenght is: {1}", t, _l_best);
                    //convert ants[_best_ant].way to line 
                    string line = "";
                    foreach (int node in ants[_best_ant].way)
                    {
                        line += node + " ";
                    }
                    Console.WriteLine("And way is [{0}]", line);
                    //self.__model.print_pheromone()
                }
            }
        }
        private int __find_new_node(Ant ant)
        {
            if (ant._is_wild)
            {
                List<int> possible_node = new List<int>();
                for (int i = 0; i < _num_of_nodes; i++)
                {
                    if (!ant.way.Contains(i))
                    {
                        possible_node.Add(i);
                    }
                }
                Random random = new Random();
                int new_node = possible_node[random.Next(possible_node.Count)];
                return new_node;
            }
            else
            {
                int cur = ant.way[ant.way.Count - 1];
                List<double> probabilities = new List<double>();
                for (int i = 0; i < _num_of_nodes; i++)
                {
                    if (ant.way.Contains(i))
                    {
                        probabilities.Add(0);
                    }
                    else
                    {
                        probabilities.Add(Math.Pow(_model.pheromone_matrix[cur, i], ALPHA) * Math.Pow(1 / _model.distance_matrix[cur, i], BETA));
                    }
                }
                double prob_sum = probabilities.Sum();
                for (int i = 0; i < probabilities.Count; i++)
                {
                    probabilities[i] /= prob_sum;
                }
                Random random = new Random();
                double rand_num = random.NextDouble();
                double sum_num = 0;
                for (int i = 0; i < probabilities.Count; i++)
                {
                    sum_num += probabilities[i];
                    if (rand_num < sum_num)
                    {
                        return i;
                    }
                }
                return probabilities.Count - 1;
            }
        }
        private (int, int) __find_min_way(List<Ant> ants)
        {
            int min_length = MAX_DISTANCE * _num_of_nodes;
            int ant_index = 0;
            foreach (Ant ant in ants)
            {
                int way_len = ant.get_way_length(_model);
                if (way_len < min_length)
                {
                    min_length = way_len;
                    ant_index = ants.IndexOf(ant);
                }
            }
            return (min_length, ant_index);
        }
        private List<Ant> __set_ants()
        {
            List<int> nodes_in_use = new List<int>();
            List<Ant> ants = new List<Ant>();
            int num_of_wild = 0;
            while (ants.Count < NUM_OF_ANTS)
            {
                Random rnd = new Random();
                int new_position = rnd.Next(0, _num_of_nodes);
                if (!nodes_in_use.Contains(new_position))
                {
                    if (num_of_wild < NUM_OF_WILD_ANTS)
                    {
                        ants.Add(new Ant(true, new_position));
                        num_of_wild++;
                    }
                    else
                    {
                        ants.Add(new Ant(false, new_position));
                    }
                    nodes_in_use.Add(new_position);
                }
            }
            return ants;
        }
        private void __update_pheromone(List<Ant> ants)
        {
            for (int i = 0; i < _num_of_nodes; i++)
            {
                for (int j = 0; j < _num_of_nodes; j++)
                {
                    _model.pheromone_matrix[i, j] *= (1 - R);
                }
            }
            foreach (Ant ant in ants)
            {
                double pheromone_of_ant = _l_min / ant.get_way_length(_model);
                for (int i = 0; i < ant.way.Count - 1; i++)
                {
                    _model.pheromone_matrix[ant.way[i], ant.way[i + 1]] += pheromone_of_ant;
                    _model.pheromone_matrix[ant.way[i + 1], ant.way[i]] += pheromone_of_ant;
                }
                _model.pheromone_matrix[ant.way[ant.way.Count - 1], ant.way[0]] += pheromone_of_ant;
                _model.pheromone_matrix[ant.way[0], ant.way[ant.way.Count - 1]] += pheromone_of_ant;
            }
        }
    }
}
