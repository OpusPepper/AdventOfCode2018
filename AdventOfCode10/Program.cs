using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode10.Models;

namespace AdventOfCode10
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            // Part I
            Log.InfoFormat($"** Part I Begin **");
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);
            int partOneAnswer = 0;
            string delimited = @"(\d+)";
            List<LightNode> lightNodes = new List<LightNode>();

            // Find beginning and end nodes
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                var lightNode = new LightNode();
                lightNode.TuplePosition =
                    new Tuple<int, int>(Convert.ToInt32(matches[0].Value), Convert.ToInt32(matches[1].Value));
                lightNode.TupleVelocity = 
                    new Tuple<int, int>(Convert.ToInt32(matches[2].Value), Convert.ToInt32(matches[3].Value));

                lightNodes.Add(lightNode);
            }

            Log.InfoFormat($"Total lightNodes: " + lightNodes.Count);

            Log.InfoFormat($"** Part I End **");
            // Part II
            int partTwoAnswer = 0;

            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 9");
            Log.InfoFormat($"Part I: " + partOneAnswer);
            Log.InfoFormat($"Part II: " + partTwoAnswer);
            Log.InfoFormat($"******************");
            Log.InfoFormat($"Game ends");

            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }
    }
}
