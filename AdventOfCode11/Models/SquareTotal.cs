using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode11.Models
{
    public class SquareTotal
    {
        public int xStart { get; set; }
        public int yStart { get; set; }

        public int xEnd { get; set; }
        public int yEnd { get; set; }
        public int SquareSize { get; set; }
        public int totalEnergy { get; set; }

        public SquareTotal(int xstart, int ystart, int xend, int yend, int squaresize, int totalenergy)
        {
            xStart = xstart;
            yStart = ystart;
            xEnd = xend;
            yEnd = yend;
            SquareSize = squaresize;
            totalEnergy = totalenergy;
        }
    }
}
