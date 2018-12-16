using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode12.Models
{
    public class Plant
    {
        public int Location { get; set; }
        public char PotContents { get; set; }

        public Plant(int inLocation, char inPotContents)
        {
            Location = inLocation;
            PotContents = inPotContents;
        }
    }
}
