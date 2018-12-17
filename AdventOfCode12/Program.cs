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
            int numberOfGenerations = 20;
            int currentGeneration = 0;
            int numberOfNegativePots = 3;

            //string delimited = @"(-?\d+)";
            int gridSerialNumber = 0;

            var ruleNumber = 0;

            for(int i = -3; i < 0; i++)
            {
                var newPlant = new Plant(i, '.');
                plants.Add(newPlant);
            }

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
                }
                else if (line.Length != 0)
                {
                    var newRule = new Rule(ruleNumber, line);
                    ruleNumber++;
                    rules.Add(newRule);
                }
            }

            // Go through the generations
            DisplayRow(currentGeneration, plants);
            for (var g = 1; g <= numberOfGenerations; g++)
            {
                ApplyRules(plants, rules);
                currentGeneration = g;
                DisplayRow(currentGeneration, plants);
            }

            var sumOfPots = plants.Count(x => x.PotContents == '#');

            Log.InfoFormat($"Total Pots: " + sumOfPots);

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

        public static void DisplayRow(int displayRow, List<Plant> plants)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(displayRow.ToString() + ": ");

            foreach(var p in plants)
            {
                sb.Append(p.PotContents);
            }
            Log.InfoFormat(sb.ToString());
        }

        public static void ApplyRules(List<Plant> plants, List<Rule> rules)
        {
            var beginPosition = 0;
            var maxPosition = plants.Max(x => x.Location);
            for (int i = beginPosition; i <= maxPosition; i++  )
            {
                var currentPot = plants.FirstOrDefault(x => x.Location == i).PotContents;
                
                    string previousTwoPots;
                    string postTwoPots;
                    //if (i == beginPosition)
                    //{
                    //    previousTwoPots = "..";
                    //    postTwoPots = String.Concat(plants.FirstOrDefault(x => x.Location == (i +1)).PotContents, plants.FirstOrDefault(x => x.Location == (i + 2)).PotContents);
                    //}
                    //else if (i == beginPosition + 1)
                    //{
                    //    previousTwoPots = String.Concat(".", plants.FirstOrDefault(x => x.Location == i).PotContents);
                    //    postTwoPots = String.Concat(plants.FirstOrDefault(x => x.Location == (i + 1)).PotContents, plants.FirstOrDefault(x => x.Location == (i + 2)).PotContents);
                    //}
                    //else 
if (i == maxPosition - 1)
                    {
                        previousTwoPots = String.Concat(plants.FirstOrDefault(x => x.Location == (i - 2)).PotContents, plants.FirstOrDefault(x => x.Location == (i - 1)).PotContents);
                        postTwoPots = String.Concat(plants.FirstOrDefault(x => x.Location == i).PotContents, "."); 
                    }
                    else if (i == maxPosition)
                    {
                        previousTwoPots = String.Concat(plants.FirstOrDefault(x => x.Location == (i - 2)).PotContents, plants.FirstOrDefault(x => x.Location == (i - 1)).PotContents);
                        postTwoPots = "..";
                    }
                    else
                    {
                        previousTwoPots = String.Concat(plants.FirstOrDefault(x => x.Location == (i - 2)).PotContents, plants.FirstOrDefault(x => x.Location == (i - 1)).PotContents);
                        postTwoPots = String.Concat(plants.FirstOrDefault(x => x.Location == (i + 1)).PotContents, plants.FirstOrDefault(x => x.Location == (i + 2)).PotContents);
                    }

                var rulePatternToFind = String.Concat(previousTwoPots, currentPot, postTwoPots);

                var rulesfound = rules.Where(x => x.RulePatten.Substring(0, 5) == rulePatternToFind);

                if (rulesfound.Count() == 1)
                {
                    Log.InfoFormat($"******************");
                    Log.InfoFormat($" String position we are looking at:  " + i);
                    Log.InfoFormat($" String in input: " + rulePatternToFind);
                    Log.InfoFormat($" Rule found: " + rulesfound.First().RulePatten);
                    Log.InfoFormat($"******************");
                    plants.FirstOrDefault(x => x.Location == i).PotContents = rulesfound.First().RulePatten.Substring(9, 1)[0];
                }
                else if (rulesfound.Count() > 1)
                {
                    Log.ErrorFormat($"More than 1 rule match found for pattern: " + rulePatternToFind);
                }
                else
                {
                    plants.FirstOrDefault(x => x.Location == i).PotContents = '.';
                }

              
                
            }
        }

        public static bool DoesRuleMatch(Rule rule, char pot, string previousTwo, string postTwo)
        {
            bool matchesPrevious2 = false;
            bool matchesPost2 = false;
            var rulePotContents = rule.RulePatten.Substring(2, 1)[0];

            if (rulePotContents == pot)
            {
                if (rule.RulePatten.Substring(0, 2) == previousTwo)
                {
                    matchesPrevious2 = true;
                }
                if (rule.RulePatten.Substring(3, 2) == postTwo)
                {
                    matchesPost2 = true;
                }
            }

            return matchesPrevious2 && matchesPost2;
        }

    }
}
