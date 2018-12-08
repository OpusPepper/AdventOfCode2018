using System;
using System.Collections.Generic;

namespace AdventOfCode7.Models
{
    public class TreeNode
    {
        public char Name { get; set; }

        public List<TreeNode> ChildNodes { get; set; } = new List<TreeNode>();

        public TreeNode(char name)
        {
            Name = name;
        }
    }
}