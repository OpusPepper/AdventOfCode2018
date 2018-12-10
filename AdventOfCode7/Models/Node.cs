using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode7.Models
{
    public class Node
    {
        public char Name { get; set; }

        public List<Node> DependsOn { get; set; } = new List<Node>();

        public bool isReady { get; set; }

        public bool isWaiting { get; set; }

        public bool isComplete { get; set; }

        public Node(char name)
        {
            Name = name;
            isReady = false;
            isComplete = false;
            isWaiting = false;
        }
    }
}
