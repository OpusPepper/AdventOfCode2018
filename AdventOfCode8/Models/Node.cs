using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode8.Models
{
    public class Node
    {
        public Tuple<int, int> Header { get; set; }

        public List<Node> ChildNodes { get; set; } = new List<Node>();

        public List<int> MetaData { get; set; } = new List<int>();

        public Node(Tuple<int, int> tupleIn)
        {
            Header = tupleIn;
        }
    }
}
