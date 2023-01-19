using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4_ants
{
    internal class Ant
    {
        public bool _is_wild {get; }
        public int _position { get; }

        public List<int> way;

        public Ant(bool ant_type, int position)
        {
            _is_wild = ant_type;
            _position = position;
            way = new List<int>();
        }

        public int get_way_length(Model world)
        {
            int length = 0;
            for (int i = 0; i < way.Count - 1; i++)
            {
                length += world.distance_matrix[way[i], way[i + 1]];
            }
            length += world.distance_matrix[way[^1], way[0]];
            return length;
        }
    }
}
