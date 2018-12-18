using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode13.Models
{
    public class Rail
    {
        public char[,] Track { get; set; }

        public int NumRows { get; set; }
        public int NumColumns { get; set; }


        public Rail(char[,] ingrid, int numRows, int numColumns)
        {
            Track = ingrid;
            NumRows = numRows;
            NumColumns = numColumns;
        }
    }
}
