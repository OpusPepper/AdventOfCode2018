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
            Node newNode;

            // Build first node;  We know this is "D" by observing the data
            newNode = new Node('D');
            listOfNodes.Add(newNode);

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
                    var nodeName = matches[7].Value[0];
                    var dependsOn = matches[1].Value[0];

                    var nodeFound = listOfNodes.FirstOrDefault(x => x.Name == nodeName);
                    if (nodeFound != null)
                    {
                        var dependsOnNode = listOfNodes.FirstOrDefault(x => x.Name == dependsOn);

                        if (dependsOnNode != null)
                        {
                            nodeFound.DependsOn.Add(dependsOnNode);
                        }
                        else
                        {
                            nodeFound.DependsOn.Add(new Node(dependsOn));
                        }
                    }
                    else
                    {
                        newNode = new Node(nodeName);

                        var dependsOnNode = listOfNodes.FirstOrDefault(x => x.Name == dependsOn);

                        if (dependsOnNode != null)
                        {
                            newNode.DependsOn.Add(dependsOnNode);
                        }
                        else
                        {
                            newNode.DependsOn.Add(new Node(dependsOn));
                        }
                        listOfNodes.Add(newNode);
                    }
                }
            }

            string finalString = "";
            do
            {
                List<Node> nodesThatAreReady = new List<Node>();

                // Let's see if any are all "ready" and just waiting for their turn..
                

                foreach (var n in listOfNodes)
                {
                    if (!n.isComplete && (n.DependsOn.Count == 0 || n.DependsOn.All(x => x.isReady)))
                    {
                        nodesThatAreReady.Add(n);
                    }

                }

                // Process one node at a time, in order
                var nextNodeToProcess = nodesThatAreReady.OrderBy(y => y.Name).FirstOrDefault();

                if (nextNodeToProcess != null)
                {
                    finalString += nextNodeToProcess.Name;
                    nextNodeToProcess.isComplete = true;
                }

                //Process all nodes that are ready in order
                //foreach(var n in nodesThatAreReady.OrderBy(x => x.Name))
                //{
                //    finalString += n.Name;
                //    var node = nodesThatAreReady.FirstOrDefault(x => x.Name == n.Name);
                //    if (node != null)
                //        node.isComplete = true;
                //}

                foreach(var n in listOfNodes.Where(x => !x.isComplete))
                {
                    foreach(var sn in n.DependsOn.Where(x => x.Name == nextNodeToProcess.Name))
                    {
                        sn.isReady = true;
                    }
                }

                //foreach (var n in listOfNodes.Where(x => x.isReady == false))
                //{
                //    if (n.DependsOn.All(x => x.isReady))
                //    {
                //        n.isReady = true;
                //    }
                //}
                Console.WriteLine("Final string: " + finalString);
            } while (listOfNodes.Any(x => !x.isComplete));



            //Display
            //DisplayTree(myTree, ending);
            Console.WriteLine(finalString);

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



    }
}
