using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode11
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
            int[,] grid = new int[301, 301];
 

            string delimited = @"(-?\d+)";
            int gridSerialNumber = 0;

            // Find beginning and end nodes
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                gridSerialNumber = Convert.ToInt32(matches[0].Value);
                
            }
            Log.InfoFormat($"Grid serial number: " + gridSerialNumber);

            // Load fuel cell grid
            for (int x = 1; x < 301; x++)
            {
                for (int y = 1; y < 301;y++)
                {
                    var rackId = x + 10;
                    var powerLevel = ((rackId * y) + gridSerialNumber) * rackId;
                    var powerLevelToString = powerLevel.ToString();
                    var newPowerLevel = 0;
                    if (powerLevelToString.Length >= 3)
                    {
                        newPowerLevel = Convert.ToInt32(powerLevelToString.Substring((powerLevelToString.Length - 1) - 3, 1));
                    }
                    else
                        newPowerLevel = 0;

                    newPowerLevel -= 5;

                    grid[x, y] = newPowerLevel;
                }
            }
            //  Todo:  test examples with the specific locations given
            //  todo:  maybe add z dimension to track total powerlevel for 3x3 square?
            //  todo; make sure we are ignoring row 0 and column 0

            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 11");
            Log.InfoFormat($"Part I (second of solution): " + partOneAnswer);
            Log.InfoFormat($"******************");

            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }
    }
}
