using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode8.Models;

namespace AdventOfCode8
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part I
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);
            int partOneAnswer = 0;
            string delimited = @"(\d+)";
            List<int> inputNumbers = new List<int>();
            List<Node> allNodes = new List<Node>();
            int counter = 0;

            // Find beginning and end nodes
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                foreach (Match m in matches)
                {
                    inputNumbers.Add(Convert.ToInt32(m.Value));
                }
            }

            Node newNode;

            newNode = getNode(inputNumbers);

            partOneAnswer = getTotalOfMetaData(newNode);

            // Part II
            int partTwoAnswer = getRootNodeValue(newNode);

            // Results
            Console.WriteLine("******************");
            Console.WriteLine("AdventOfCode Day 8");
            Console.WriteLine("Part I: " + partOneAnswer);
            Console.WriteLine("Part II: " + partTwoAnswer);
            Console.WriteLine("******************");
            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }

        private static Node getNode(List<int> inList)
        {
            Node returnValue = null;

            if (inList.Any())
            {
                returnValue = new Node(new Tuple<int, int>(inList[0], inList[1]));
                inList.RemoveRange(0, 2);

                if (returnValue.Header.Item1 > 0)
                {
                    // Check if there is a childnode
                    for (int i = 0; i < returnValue.Header.Item1; i++)
                    {                        
                        returnValue.ChildNodes.Add(getNode(inList));
                    }
                }

                if (returnValue.Header.Item2 > 0)
                {
                    for (int i = 0; i < returnValue.Header.Item2; i++)
                    {
                        returnValue.MetaData.Add(inList[i]);
                    }
                    inList.RemoveRange(0, returnValue.Header.Item2);
                }

            }

        return returnValue;
        }

        private static int getTotalOfMetaData(Node inNode)
        {
            int returnValue = 0;

            foreach (var i in inNode.MetaData)
            {
                returnValue += i;
            }

            if (inNode.ChildNodes.Any())
            {
                foreach (var cn in inNode.ChildNodes)
                {
                    returnValue += getTotalOfMetaData(cn);
                }                
            }

            return returnValue;
        }

        private static int getRootNodeValue(Node inNode)
        {
            int returnValue = 0;

            if (inNode.ChildNodes.Any())
            {
                foreach (var i in inNode.MetaData)
                {
                    if (inNode.ChildNodes.Count() > (i - 1))
                    {
                        returnValue += getRootNodeValue(inNode.ChildNodes[i - 1]);
                    }
                }
            }
            else
            {
                foreach (var i in inNode.MetaData)
                {
                    returnValue += i;
                }
            }

            return returnValue;
        }

    }
}
