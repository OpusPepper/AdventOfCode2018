using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode7.Models;

namespace AdventOfCode7
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part I
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);
            int partOneAnswer = 0;
            string delimited = @"(\w+)";
            List<Node> listOfNodes = new List<Node>();
            LinkedList<char> linkedListNodes = new LinkedList<char>();

            //Build List of points                        
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                if (matches.Count != 10)
                {
                    Console.WriteLine("Error with line: " + line);
                }
                else
                {
                    listOfNodes.Add(new Node(matches[1].Value[0], matches[7].Value[0]));
                }
            }

            //Write out list
            Console.WriteLine("Current list: ");
            foreach (var n in listOfNodes)
            {
                Console.WriteLine("Name: " + n.Name + ", depends on: " + n.DependsOn);
            }
            Console.WriteLine();
            Console.WriteLine();

            //Find beginning
            var beginning = listOfNodes.First(x => listOfNodes.Any(y => y.DependsOn != x.Name));
            Console.WriteLine("Beginning: " + beginning.Name);

            //Find ending
            char ending = ' ';
            char findMe = ' ';
            for (int i = 0; i < listOfNodes.Count; i++)
            {
                findMe = listOfNodes[i].DependsOn;
                if (listOfNodes.All(x => x.Name != findMe))
                {
                    ending = findMe;
                }
            }            
            Console.WriteLine("ending: " + ending);

            // Now let's build the list
            //linkedListNodes.AddFirst(beginning);
            //linkedListNodes.AddLast(ending);

            TreeNode myTree = new TreeNode(beginning.Name);
          
            foreach(var c in listOfNodes)
{
                if (c.Name == myTree.Name)
                    {
                    var newNode = new TreeNode(c.DependsOn);
                    myTree.ChildNodes.Add(newNode);
}
                else
                    {
                    var currentNode = myTree;
                    var childNode = recursiveNodeFind(c, myTree);
                    if (childNode != null)
                    {
                        childNode.ChildNodes.Add(new TreeNode(c.Name));
                    }
                }
            }

            //Display
            DisplayTree(myTree);

            // Part II
            int partTwoAnswer = 0;

            // Results
            Console.WriteLine("******************");
            Console.WriteLine("AdventOfCode Day 7");
            Console.WriteLine("Part I: " + partOneAnswer);
            Console.WriteLine("Part II: " + partTwoAnswer);
            Console.WriteLine("******************");
            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }


        private static TreeNode recursiveNodeFind(Node inNode, TreeNode tree)
        {
             if (tree.Name == inNode.Name)
                return tree;
             else 
             {
                var foundNode = tree.ChildNodes.FirstOrDefault(x => x.Name == inNode.Name);
                bool isFoundNode = false;
                foreach(var cn in tree.ChildNodes)
                {
                    if (isFoundNode)
                        break;
                    var childNodeFind = recursiveNodeFind(inNode, cn);
                    if (childNodeFind != null)
                    {
                        isFoundNode = true;
                        return childNodeFind;
                        break;
                    }
                    else
                    {
                        return null;
                    }
                }
                if (!isFoundNode)
                    return tree;
             }
            return null;
        }

        private static void DisplayTree(TreeNode myTree)
        {
            Console.Write(myTree.Name);
            foreach(var node in myTree.ChildNodes.OrderBy(x => x.Name))
            {
                DisplayTree(node);
            }
        }
    }
}
