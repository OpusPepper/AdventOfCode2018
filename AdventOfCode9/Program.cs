using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode9
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
            List<int> marbleOrder = new List<int>();
            int counter = 0;
            int numOfPlayers = 0;
            int maxMarble = 0;
            int marble = 0;
            int currentMarble = 0;
            int playerNumber = 1;

            // Find beginning and end nodes
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                numOfPlayers = Convert.ToInt32(matches[0].Value);

                maxMarble = Convert.ToInt32(matches[1].Value);
            }

            List<int> scores = new List<int>();
            for (int i = 0; i < numOfPlayers; i++)
            {
                scores.Add(0);
            }

            Console.WriteLine("Game Begins");
            marbleOrder.Add(marble);
            currentMarble = marble;
            Console.WriteLine("[-] " + GetMarbleOrderString(marbleOrder, currentMarble));
            marble++;

            bool gameover = false;
            do
            {
                // Insert new marble between 1st and 2nd marble clockwise of the current marble
                if (marble == 1)
                {
                    marbleOrder.Add(marble);
                    currentMarble = marble;
                }
                else
                {
                    if (marble % 23 == 0)
                    {
                        // special logic for "23"
                        // player x keeps the marble
                        var marbleToRemove = GetMarble7MarblesBack(marbleOrder, currentMarble);
                        scores[playerNumber - 1] += marbleOrder[marbleToRemove] + marble;
                        // add this marble to players score
                        marbleOrder.RemoveAt(marbleToRemove);
                        currentMarble = marbleOrder[marbleToRemove];
                    }
                    else
                    {
                        var insertLocation = GetInsertLocation(marbleOrder, currentMarble);
                        if (insertLocation > 0)
                            marbleOrder.Insert(insertLocation, marble);
                        else
                            marbleOrder.Add(marble);
                        currentMarble = marble;
                    }
                }
                

                //Console.WriteLine("[" + playerNumber + "] " + GetMarbleOrderString(marbleOrder, currentMarble));

                marble++;

                playerNumber++;
                if (playerNumber > numOfPlayers)
                    playerNumber = 1;

                if (marble == maxMarble + 1)
                    gameover = true;

                if (marble % 10000 == 0)
                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": On marble # " + marble);
            } while (!gameover);

            partOneAnswer = scores.Max(); 

            // Part II
            int partTwoAnswer = 0;

            // Results
            Console.WriteLine("******************");
            Console.WriteLine("AdventOfCode Day 9");
            Console.WriteLine("Part I: " + partOneAnswer);
            Console.WriteLine("Part II: " + partTwoAnswer);
            Console.WriteLine("******************");
            Console.WriteLine("Press any key to end...");
            Console.ReadLine();

        }

        private static string GetMarbleOrderString(List<int> marbleOrder, int currentMarble)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var m in marbleOrder)
            {
                if (m == currentMarble)
                {
                    sb.Append(" (" + m + ") ");
                }
                else
                {
                    sb.Append(" " + m);
                }
            }

            return sb.ToString();
        }

        private static int GetInsertLocation(List<int> marbleOrder, int currentMarble)
        {
            var locationOfCurrentMarble = marbleOrder.IndexOf(currentMarble);
           
            for (int i = 0; i < 2; i++)
            {
                if (locationOfCurrentMarble == marbleOrder.Count - 1)
                {
                    locationOfCurrentMarble = 0;
                }
                else
                    locationOfCurrentMarble++;
            }

            if (locationOfCurrentMarble == 0)
                locationOfCurrentMarble = -999;
            return locationOfCurrentMarble;
        }

        private static int GetMarble7MarblesBack(List<int> marbleOrder, int currentMarble)
        {
            var locationOfCurrentMarble = marbleOrder.IndexOf((currentMarble));

            for (int i = 7; i > 0; i--)
            {
                if (locationOfCurrentMarble == 0)
                {
                    locationOfCurrentMarble = marbleOrder.Count - 1;
                }
                else
                    locationOfCurrentMarble--;
            }
            
            return locationOfCurrentMarble;
        }
    }
}
