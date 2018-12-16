using AdventOfCode12.Models;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode12
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
            List<Plant> plants = new List<Plant>();
            List<Rule> rules = new List<Rule>();

            //string delimited = @"(-?\d+)";
            int gridSerialNumber = 0;

            var ruleNumber = 0;

            // Read input
            foreach (var line in allLines)
            {
                //var matches = Regex.Matches(line, delimited);
                
                //gridSerialNumber = Convert.ToInt32(matches[0].Value);
                if (line.Contains("initial state: "))
                {
                    var counter = 0;
                    foreach(var c in line.Replace("initial state: ",""))
                    {
                        var newPlant = new Plant(counter, c);
                        plants.Add(newPlant);
                        counter++;
                    }

                    // create negative pots, same size as total pots to the right
                    for (int i = counter; i >= 0; i--)
                    {
                        var newPlant = new Plant 
                    }
                }
                else if (line.Length != 0)
                {
                    var newRule = new Rule(ruleNumber, line);
                    ruleNumber++;
                    rules.Add(newRule);
                }

            }
            Log.InfoFormat($"Grid serial number: " + gridSerialNumber);

            Log.InfoFormat($"*** PART II Ends ***");

            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 12");
            Log.InfoFormat($"Part I: Energy: " + partOneAnswer);
            Log.InfoFormat($"Part II: Energy: ");
            Log.InfoFormat($"******************");

            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }
    }
}
