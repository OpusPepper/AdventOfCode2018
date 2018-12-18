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
            List<Rule> rules = new List<Rule>();
            long numberOfGenerations = 50000000000;
            //long numberOfGenerations = 20;
            //long numberOfGenerations = 2000;
            long currentGeneration = 0;
            string currentGenerationString = "";
            string nextGenerationString = "";
            List<Plant> plants = new List<Plant>();
            var ruleNumber = 0;


            // Read input
            foreach (var line in allLines)
            {
                //var matches = Regex.Matches(line, delimited);
                
                //gridSerialNumber = Convert.ToInt32(matches[0].Value);
                if (line.Contains("initial state: "))
                {
                    var counter = 0;
                    foreach (var c in line.Replace("initial state: ", ""))
                    {
                        var newPlant = new Plant(counter, c);
                        plants.Add(newPlant);
                        counter++;
                    }
                    //currentGenerationString = line.Replace("initial state: ", "");
                }
                else if (line.Length != 0)
                {
                    var newRule = new Rule(ruleNumber, line);
                    ruleNumber++;
                    rules.Add(newRule);
                }
            }

            Log.InfoFormat($"Potential Plants read in: " + plants.Count);
            Log.InfoFormat($"Rules read in: " + rules.Count);

            // Go through the generations            
            DisplayRowShort(currentGeneration, plants);
            for (long g = 1; g <= numberOfGenerations; g++)
            {
                ApplyRules(plants, rules);                
                currentGeneration = g;                

                if (g % 1000 == 0)
                    DisplayRowShort(currentGeneration, plants);

            }
            long sumOfPots = plants.Where(x => x.PotContents == '#').Sum(x => x.Location);
            Log.InfoFormat($"Total Pots: " + sumOfPots);
            Log.InfoFormat($"*** PART I Ends ***");


            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 12");
            Log.InfoFormat($"Part I: Total Pots: " + sumOfPots);            
            Log.InfoFormat($"******************");

            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }

        public static void DisplayRow(long displayRow, string currentGenerationString)
        {
            Log.InfoFormat(displayRow + ": " + currentGenerationString);
        }
        public static void DisplayRow(long displayRow, List<Plant> plants)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(displayRow.ToString() + ": ");

            foreach (var p in plants)
            {
                sb.Append(p.PotContents);
            }
            Log.InfoFormat(sb.ToString());
        }

        public static void DisplayRowShort(long displayRow, List<Plant> plants)
        {
            StringBuilder sb = new StringBuilder();
            long sumOfPots = plants.Where(x => x.PotContents == '#').Sum(x => x.Location);
            sb.Append(displayRow.ToString() + ": Sum of pots: " + sumOfPots);

            Log.InfoFormat(sb.ToString());
        }

        public static string ApplyRulesOnString(string currentGenerationString, List<Rule> rules)
        {
            string returnNewGeneration = "";

            //Let's go backwards first to make sure we don't have any changes in the negative pots
            //Calculate -3 previous pots
            string prevCurrentString = "..." + currentGenerationString;
            var beginPosition = 0;
            var maxPosition = prevCurrentString.Length - 1;
            char currentPot;
            string tempGeneration = "";
            
            for (int i = beginPosition; i < maxPosition; i++)
            {
                currentPot = prevCurrentString[i];

                tempGeneration += ReturnNewGeneration(prevCurrentString, rules, i, maxPosition, beginPosition, currentPot);
            }

            if (tempGeneration.Substring(0, 3) != "...")
            {
                returnNewGeneration += tempGeneration.Substring(0, 3);
            }

            // Now go through the regular "middle" part of the string
            beginPosition = 0;
            maxPosition = currentGenerationString.Length - 1;
            for (int i = beginPosition; i <= maxPosition; i++)
            {
                currentPot = currentGenerationString[i];

                returnNewGeneration += ReturnNewGeneration(currentGenerationString, rules, i, maxPosition, beginPosition, currentPot);                
            }

            //Let's go forwards last to make sure we don't have any changes in the end pots
            //Calculate -3 previous pots
            prevCurrentString = currentGenerationString + "...";
            beginPosition = 0;
            maxPosition = prevCurrentString.Length - 1;
            tempGeneration = "";
            for (int i = beginPosition; i <= maxPosition; i++)
            {
                currentPot = prevCurrentString[i];

                tempGeneration += ReturnNewGeneration(prevCurrentString, rules, i, maxPosition, beginPosition, currentPot);
            }

            if (tempGeneration.Substring(tempGeneration.Length - 3, 3) != "...")
            {
                returnNewGeneration += tempGeneration.Substring(tempGeneration.Length - 3, 3);
            }

            return returnNewGeneration;
        }

        public static string ApplyRules(List<Plant> plants, List<Rule> rules)
        {
            var plantsForString = plants.Select(x => x.PotContents);
            var currentGenerationString = new String(plantsForString.ToArray());
            //string returnNewGeneration = "";

            //Let's go backwards first to make sure we don't have any changes in the negative pots
            //Calculate -3 previous pots
            var currentFirstPlant = plants.First().Location;
            string prevCurrentString = "..." + currentGenerationString + "...";
            var beginPosition = 0;
            var maxPosition = prevCurrentString.Length - 1;
            char currentPot;
            //string tempGeneration = "";
            StringBuilder tempGenerationSb = new StringBuilder();
            StringBuilder returnNewGenerationSb = new StringBuilder();
            for (int i = beginPosition; i <= maxPosition; i++)
            {
                currentPot = prevCurrentString[i];

                tempGenerationSb.Append(ReturnNewGeneration(prevCurrentString, rules, i, maxPosition, beginPosition, currentPot));
            }

            if (tempGenerationSb.ToString().Substring(0, 3) != "...")
            {
                var firstPlant = plants.First();
                returnNewGenerationSb.Append(tempGenerationSb.ToString().Substring(0, 3));
                for (int i = -1; i >= -3; i--)
                {
                    plants.Insert(0, new Plant(firstPlant.Location + i, tempGenerationSb.ToString().Substring(Math.Abs(i) - 1, 1)[0]));
                }
            }
            else
            {
                tempGenerationSb = new StringBuilder(tempGenerationSb.ToString().Substring(3, tempGenerationSb.Length - 3));                
            }
            if (tempGenerationSb.ToString().Substring(tempGenerationSb.Length - 3, 3) != "...")
            {
                returnNewGenerationSb.Append(tempGenerationSb.ToString().Substring(tempGenerationSb.Length - 3, 3));
                var lastPlant = plants.Last();
                var tempInt = 0;
                for (int i = 1; i <= 3; i++)
                {
                    plants.Add(new Plant(lastPlant.Location + i, tempGenerationSb.ToString().Substring(tempGenerationSb.Length - (3 + tempInt), 1)[0]));
                    tempInt++;
                }
            }
            else
            {
                tempGenerationSb = new StringBuilder(tempGenerationSb.ToString().Substring(0, tempGenerationSb.Length - 3));
            }

            var currentPlant = plants.First().Location;
            var currentLastPlant = plants.Last().Location;

            if (tempGenerationSb.Length != plants.Count())
            {
                Log.InfoFormat($"******************");
                Log.InfoFormat($"* Error: tempGeneration not equal to plants.count *");
                Log.InfoFormat($"******************");
            }

            for (int i = 0; i < tempGenerationSb.Length; i++)
            {
                var plantToUpdate = plants.FirstOrDefault(x => x.Location == currentPlant);
                if (plantToUpdate != null)
                {
                    plantToUpdate.PotContents = tempGenerationSb[i];
                }
                else
                {
                    Log.InfoFormat($"******************");
                    Log.InfoFormat($"* Error: too many plants on update *");
                    Log.InfoFormat($"******************");
                }

                currentPlant++;
            }

            return returnNewGenerationSb.ToString();
        }


        private static string ReturnNewGeneration(string currentGenerationString, List<Rule> rules, int i, int maxPosition,
            int beginPosition, char currentPot)
        {
            string previousTwoPots;
            string postTwoPots;

            if (i == maxPosition - 1)
            {
                previousTwoPots = String.Concat(currentGenerationString[i - 2], currentGenerationString[i - 1]);
                postTwoPots = String.Concat(currentGenerationString[i + 1], ".");
            }
            else if (i == maxPosition)
            {
                previousTwoPots = String.Concat(currentGenerationString[i - 2], currentGenerationString[i - 1]);
                postTwoPots = "..";
            }
            else if (i == beginPosition)
            {
                previousTwoPots = "..";
                postTwoPots = String.Concat(currentGenerationString[i + 1], currentGenerationString[i + 2]);
            }
            else if (i == beginPosition + 1)
            {
                previousTwoPots = String.Concat(".", currentGenerationString[i - 1]);
                postTwoPots = String.Concat(currentGenerationString[i + 1], currentGenerationString[i + 2]);
            }
            else
            {
                previousTwoPots = String.Concat(currentGenerationString[i - 2], currentGenerationString[i - 1]);
                postTwoPots = String.Concat(currentGenerationString[i + 1], currentGenerationString[i + 2]);
            }

            var rulePatternToFind = String.Concat(previousTwoPots, currentPot, postTwoPots);

            var rulesfound = rules.Where(x => x.RulePatten.Substring(0, 5) == rulePatternToFind);

            if (rulesfound.Count() == 1)
            {
                return rulesfound.First().RulePatten.Substring(9, 1);
            }
            else if (rulesfound.Count() > 1)
            {
                Log.ErrorFormat($"More than 1 rule match found for pattern: " + rulePatternToFind);
                return "";
            }
            else
            {
                return ".";
            }            
        }

        private static void ReturnNewGeneration(List<Plant> plants, List<Rule> rules, int i, int maxPosition,
            int beginPosition, char currentPot)
        {
            string returnNewGeneration = "";
            string previousTwoPots;
            string postTwoPots;

            if (i == maxPosition - 1)
            {
                previousTwoPots = String.Concat(plants.Select(x => x.Location == (i - 2)), plants.Select(x => x.Location == (i - 1)));
                postTwoPots = String.Concat(plants.Select(x => x.Location == (i + 1)), ".");
            }
            else if (i == maxPosition)
            {
                previousTwoPots = String.Concat(plants.Select(x => x.Location == (i - 2)), plants.Select(x => x.Location == (i - 1)));
                postTwoPots = "..";
            }
            else if (i == beginPosition)
            {
                previousTwoPots = "..";
                postTwoPots = String.Concat(plants.Select(x => x.Location == (i + 1)), plants.Select(x => x.Location == (i + 2)));
            }
            else if (i == beginPosition + 1)
            {
                previousTwoPots = String.Concat(".", plants.Select(x => x.Location == (i - 1)));
                postTwoPots = String.Concat(plants.Select(x => x.Location == (i + 1)), plants.Select(x => x.Location == (i + 2)));
            }
            else
            {
                previousTwoPots = String.Concat(plants.Select(x => x.Location == (i - 2)), plants.Select(x => x.Location == (i - 1)));
                postTwoPots = String.Concat(plants.Select(x => x.Location == (i + 1)), plants.Select(x => x.Location == (i + 2)));
            }

            var rulePatternToFind = String.Concat(previousTwoPots, currentPot, postTwoPots);

            var rulesfound = rules.Where(x => x.RulePatten.Substring(0, 5) == rulePatternToFind);

            if (rulesfound.Count() == 1)
            {
                //Log.InfoFormat($"******************");
                //Log.InfoFormat($" String position we are looking at:  " + i);
                //Log.InfoFormat($" String in input: " + rulePatternToFind);
                //Log.InfoFormat($" Rule found: " + rulesfound.First().RulePatten);
                //Log.InfoFormat($"******************");
                plants[i].PotContents = rulesfound.First().RulePatten.Substring(9, 1)[0];
            }
            else if (rulesfound.Count() > 1)
            {
                Log.ErrorFormat($"More than 1 rule match found for pattern: " + rulePatternToFind);
            }
            else
            {
                plants[i].PotContents = '.';
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
