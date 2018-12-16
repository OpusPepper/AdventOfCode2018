using AdventOfCode11.Models;
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
            List<ThreeByThreeSquare> totalsList = new List<ThreeByThreeSquare>();


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
                        newPowerLevel = Convert.ToInt32(powerLevelToString.Substring((powerLevelToString.Length) - 3, 1));
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

            // Test for checking cells
            //Fuel cell at  122,79, grid serial number 57: power level -5.
            //Fuel cell at 217,196, grid serial number 39: power level  0.
            //Fuel cell at 101,153, grid serial number 71: power level  4.
            //var x2 = 101;
            //var y2 = 153;
            //Log.InfoFormat($"Cell at: [" + x2 + ", " + y2 + "] = " + grid[x2, y2] );

            // Calculate the 3x3 total power
            for (int x = 1; x < (301 - 2); x++ )
            {
                for (int y = 1; y < (301 - 2); y++)
                {
                    var totalSquareEnergy = grid[x, y] + grid[x + 1, y] + grid[x + 2, y] +
                                            grid[x, y + 1] + grid[x + 1, y + 1] + grid[x + 2, y + 1] +
                                            grid[x, y + 2] + grid[x + 1, y + 2] + grid[x + 2, y + 2];

                    var tempSquare = new ThreeByThreeSquare(x, y, x + 3, y + 3, totalSquareEnergy);
                    totalsList.Add(tempSquare);
                }
            }

            var maxEnergy = totalsList.Max(a => a.totalEnergy);
            var maxSquare = totalsList.Where(z => z.totalEnergy == maxEnergy);
            if (maxSquare.Count() == 1)
            {
                partOneAnswer = maxSquare.First().totalEnergy;
            }
            else
                partOneAnswer = -999;

            Log.InfoFormat($"*** PART I Ends ***");

            // Part II
            Log.InfoFormat($"*** PART II Begins ***");
            int[,] part2Totals = new int[301, 301];
            List<SquareTotal> squareTotals = new List<SquareTotal>();
            SquareTotal maxForThisSquare;
            for (int s = 1; s <= 300; s++)
            {
                Log.InfoFormat($"Testing size: " + s + "x" + s);
                maxForThisSquare = CalculateSquare(grid, s);

                squareTotals.Add(maxForThisSquare);

                var maxEnergy3 = squareTotals.Max(a => a.totalEnergy);
                var maxSquare3 = squareTotals.Where(z => z.totalEnergy == maxEnergy3);

                Log.InfoFormat($"** Max so far: " + maxSquare3.First().totalEnergy + " x,y " +
                    maxSquare3.First().xStart + "," + maxSquare3.First().yStart + " size: " + maxSquare3.First().SquareSize + "x" + maxSquare3.First().SquareSize);
            }

            var partTwoAnswer = 0;
            var maxEnergy2 = squareTotals.Max(a => a.totalEnergy);
            var maxSquare2 = squareTotals.Where(z => z.totalEnergy == maxEnergy2);
            if (maxSquare2.Count() == 1)
            {
                partTwoAnswer = maxSquare2.First().totalEnergy;
            }
            else
                partTwoAnswer = -999;

            Log.InfoFormat($"*** PART II Ends ***");

            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 11");
            Log.InfoFormat($"Part I: Energy: " + partOneAnswer + ", X,Y = " + maxSquare.First().xStart + ", " + maxSquare.First().yStart);
            Log.InfoFormat($"Part II: Energy: " + partTwoAnswer + ", X,Y = " + maxSquare2.First().xStart + ", " + maxSquare2.First().yStart + ", size: " + maxSquare2.First().SquareSize + "x" + maxSquare2.First().SquareSize);
            Log.InfoFormat($"******************");

            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }

        public static SquareTotal CalculateSquare(int[,] grid, int squareSize)
        {
            SquareTotal maxSquareTotal = new SquareTotal(0,0,0,0,squareSize, 0);

            for (int x = 1; x < (301 - (squareSize - 1)); x++)
            {
                for (int y = 1; y < (301 - (squareSize - 1)); y++)
                {
                    if (((x + (squareSize - 1)) > 301) || ((y + (squareSize - 1)) > 301))
                    {

                    }
                    else
                    {
                        var currentSize = CalculateSquareEnergy(grid, x, y, squareSize);
                        if (currentSize > maxSquareTotal.totalEnergy)
                        {
                            maxSquareTotal = new SquareTotal(x, y, (x + squareSize - 1), (y + squareSize - 1), squareSize, currentSize);
                        }
                    }
                    
                }
            }

            return maxSquareTotal;
        }

        public static int CalculateSquareEnergy(int[,] grid, int xStart, int yStart, int squareSize)
        {
            var totalEnergy = 0;
            for(int x = xStart; x < (xStart + squareSize); x++)
            {
                for (int y = yStart; y < (yStart + squareSize); y++)
                {

                        totalEnergy += grid[x, y];
                                      
                }
            }

            return totalEnergy;
        }
    }
}
