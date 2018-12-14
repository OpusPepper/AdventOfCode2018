using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode10.Models
{
    public class LightNode
    {
        public (int x, int y) TuplePosition;
        public (int x, int y) TupleVelocity;

        public int PointsTouching { get; set; }

    }
}
