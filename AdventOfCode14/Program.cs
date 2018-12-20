using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode14
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            // Part I
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);
            string partOneAnswer = "";
            string delimited = @"(\w+)";


            // Find beginning and end nodes
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                haveSecondChar.Add(matches[7].Value[0]);
                haveFirstChar.Add(matches[1].Value[0]);
            }

            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 14");
            Log.InfoFormat($"Part I: " + partOneAnswer);
            Log.InfoFormat($"Part II: " + partTwoAnswer);
            Log.InfoFormat($"******************");

            Console.WriteLine("Press any key to end...");
            Console.ReadLine();


        }
    }
}
