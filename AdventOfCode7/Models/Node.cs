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

        public char DependsOn { get; set; }

        public Node(char name, char dependsOn)
        {
            Name = name;
            DependsOn = dependsOn;
        }
    }
}
