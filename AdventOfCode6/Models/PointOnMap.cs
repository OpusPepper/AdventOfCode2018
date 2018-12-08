using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode6.Models
{
    public class PointOnMap
    {
        public char Name { get; set; }

        public Tuple<int,int> coordinate { get; set; }

        public int Distance { get; set; }

        public int area { get; set; }
        public bool isInfinite { get; set; }

        public PointOnMap(char name, Tuple<int,int> coord)
        {
            Name = name;
            coordinate = coord;
        }
    }
}
