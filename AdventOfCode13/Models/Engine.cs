using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode13.Models
{
    public class Engine
    {
        public char EnginePic { get; set; }
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }

        public bool Crashed { get; set; }

        public List<string> Directions { get; set; }

        public int NextDirection { get; set; }

        public Engine(char enginePic, int currentRow, int currentColumn)
        {
            EnginePic = enginePic;
            CurrentRow = currentRow;
            CurrentColumn = currentColumn;
            Directions = new List<string>();
            Directions.Add("Left");
            Directions.Add("Straight");
            Directions.Add("Right");
            NextDirection = 0;
        }
    }
}
