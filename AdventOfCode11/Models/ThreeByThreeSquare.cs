using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode11.Models
{
    public class ThreeByThreeSquare
    {
        public int xStart { get; set; }
        public int yStart { get; set; }

        public int xEnd { get; set; }
        public int yEnd { get; set; }
        public int totalEnergy { get; set; }

        public ThreeByThreeSquare(int xstart, int ystart, int xend, int yend, int totalenergy)
        {
            xStart = xstart;
            yStart = ystart;
            xEnd = xend;
            yEnd = yend;
            totalEnergy = totalenergy;
        }
    }
}
