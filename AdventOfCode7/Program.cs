using System;
using System.Collections.Generic;
using System.Data;
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
            string partOneAnswer = "";
            string delimited = @"(\w+)";
            List<Node> listOfNodes = new List<Node>();
            LinkedList<char> linkedListNodes = new LinkedList<char>();
            List<char> haveFirstChar = new List<char>();
            List<char> haveSecondChar = new List<char>();
            Node newNode;

            // Find beginning and end nodes
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                haveSecondChar.Add(matches[7].Value[0]);
                haveFirstChar.Add(matches[1].Value[0]);
            }

            var notInFirst = haveFirstChar.Where(x => !haveSecondChar.Contains(x)).GroupBy(y => y);
            var notInSecond = haveSecondChar.Where(x => !haveFirstChar.Contains(x)).GroupBy(y => y);

            //Build first nodes
            foreach (var node in notInFirst)
            {
                newNode = new Node(node.Key);
                listOfNodes.Add(newNode);
            }
            

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

                    foreach (var n in listOfNodes.Where(x => !x.isComplete))
                    {
                        foreach (var sn in n.DependsOn.Where(x => x.Name == nextNodeToProcess.Name))
                        {
                            sn.isReady = true;
                        }
                    }
                }                
            } while (listOfNodes.Any(x => !x.isComplete));

            partOneAnswer = finalString;

            /////////////////////////////////////////
            // Part II
            int partTwoAnswer = 0;

            // Can we use a datatable to hold the data?
            int numberOfWorkers = 5;
            int numberOfAdditionalSecondsPerTask = 60;
            foreach(var node in listOfNodes)
            {
                node.isComplete = false;
                node.isReady = false;
                node.SecondsLeftToProcess = (((int)node.Name) - 64) + numberOfAdditionalSecondsPerTask;

                foreach (var subnode in node.DependsOn)
                {
                    subnode.isComplete = false;
                    subnode.isReady = false;
                    subnode.SecondsLeftToProcess = (((int)subnode.Name) - 64) + numberOfAdditionalSecondsPerTask;
                }
            }


            int counter = 0;
            DataRow newDataRow;
            DataTable mytable = new DataTable("TimeTable");
            mytable.Columns.Add(new DataColumn("Seconds", typeof(int)));
            for (int i = 0; i < numberOfWorkers; i++)
            {
                mytable.Columns.Add(new DataColumn("Worker " + i, typeof(char)));
            }
            mytable.Columns.Add(new DataColumn("Done"));



            finalString = "";
            do
            {
                newDataRow = mytable.NewRow();
                newDataRow["Seconds"] = counter;
                for (int i = 0; i < numberOfWorkers; i++)
                {
                    newDataRow["Worker " + i] = '.';
                }

                List<Node> nodesThatAreReady = new List<Node>();

                foreach (var n in listOfNodes)
                {
                    if (!n.isComplete && (n.DependsOn.Count == 0 || n.DependsOn.All(x => x.SecondsLeftToProcess == 0)))
                    {
                        if (n.SecondsLeftToProcess > 0)
                        {
                            nodesThatAreReady.Add(n);                            
                        }
                        else
                        {
                            
                            finalString += n.Name;
                            n.isComplete = true;
                            

                            foreach (var n2 in listOfNodes.Where(x => !x.isComplete))
                            {
                                foreach (var sn in n2.DependsOn.Where(x => x.Name == n.Name))
                                {
                                    sn.isReady = true;
                                    sn.SecondsLeftToProcess = 0;
                                }
                            }

                        }
                    }

                }

                newDataRow["Done"] = finalString;

                // Process one node at a time, in order
                var nextNodeToProcess = nodesThatAreReady.Where(x => x.SecondsLeftToProcess > 0).OrderBy(y => y.Name).ToList();
                var numNodesToProcess = nextNodeToProcess.Count;
                    for (int i = 0; i < numNodesToProcess; i++)
                    {
                        bool assignedToWorker = false;
                        if (mytable.Rows.Count > 0)
                        {
                            var lastRow = mytable.Rows[mytable.Rows.Count - 1];
                            
                            //Now go through the workers and see if someone was working on this node before
                            for (int j = 0; j < numberOfWorkers; j++)
                            {
                                if (Convert.ToChar(newDataRow["Worker " + j]) == '.')
                                {
                                    char lastNodeByWorker = (char) lastRow["Worker " + j];

                                    for (int k = 0; k < nextNodeToProcess.Count(); k++)
                                    {
                                        if (lastNodeByWorker == nextNodeToProcess[k].Name)
                                        {
                                            newDataRow["Worker " + j] = nextNodeToProcess[k].Name;
                                            nextNodeToProcess[k].SecondsLeftToProcess -= 1;
                                            nextNodeToProcess.Remove(nextNodeToProcess[k]);
                                            assignedToWorker = true;
                                        }
                                    }
                                }
                            }
                        }

                        var numberOfWorkersAvailable = 0;
                        for (int j = 0; j < numberOfWorkers; j++)
                        {
                            if (Convert.ToChar(newDataRow["Worker " + j]) == '.')
                            {
                                numberOfWorkersAvailable++;
                            }
                        }

                        if ((!assignedToWorker) && (nextNodeToProcess.Count > 0) && (numberOfWorkersAvailable > 0))
                        {
                            for (int j = 0; j < numberOfWorkers; j++)
                            {
                                if (Convert.ToChar(newDataRow["Worker " + j]) == '.')
                                {
                                    newDataRow["Worker " + j] = nextNodeToProcess.First().Name;
                                    nextNodeToProcess.First().SecondsLeftToProcess -= 1;
                                    nextNodeToProcess.Remove(nextNodeToProcess.First());
                                    assignedToWorker = true;
                                    break;
                                }
                            }                            
                        }                             
                    }

                mytable.Rows.Add(newDataRow);

                
                counter++;
            } while (listOfNodes.Any(x => !x.isComplete));

            foreach (DataRow r in mytable.Rows)
            {

                Console.Write(r["Seconds"] + ", ");

                for (int i = 0; i < numberOfWorkers; i++)
                {
                    Console.Write(r["Worker " + i] + ", ");
                }
                Console.WriteLine(r["Done"]);
            }

            partTwoAnswer = mytable.Rows.Count - 1;  // last row just shows it's all done

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
