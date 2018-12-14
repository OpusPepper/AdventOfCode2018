using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace AdventOfCode9
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            // Part I
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);
            long partOneAnswer = 0;
            string delimited = @"(\d+)";
            List<long> marbleOrder = new List<long>();
            long numOfPlayers = 0;
            long maxMarble = 0;
            long marble = 0;
            long currentMarble = 0;
            int playerNumber = 1;

            // Find beginning and end nodes
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                numOfPlayers = Convert.ToInt32(matches[0].Value);

                maxMarble = Convert.ToInt32(matches[1].Value);
            }

            List<long> scores = new List<long>();
            for (long i = 0; i < numOfPlayers; i++)
            {
                scores.Add(0);
            }

            //Console.WriteLine("Game Begins");
            Log.InfoFormat($"Game Begins");
            marbleOrder.Add(marble);
            currentMarble = marble;
            //Console.WriteLine("[-] " + GetMarbleOrderString(marbleOrder, currentMarble));
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
                        long marbleToRemove = GetMarble7MarblesBack(marbleOrder, currentMarble);
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
                {
                    //Console.WriteLine(DateTime.Now.ToShortTimeString() + ": On marble # " + marble);
                    Log.InfoFormat($"On marble # {marble}");
                }
            } while (!gameover);

            partOneAnswer = scores.Max(); 
            
            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 9");
            Log.InfoFormat($"Part I: " + partOneAnswer);
            Log.InfoFormat($"******************");
            Log.InfoFormat($"Game ends");
            //Console.WriteLine("Press any key to end...");
            //Console.ReadLine();

        }

        private static string GetMarbleOrderString(IList<long> marbleOrder, long currentMarble)
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

        private static long GetInsertLocation(IList<long> marbleOrder, long currentMarble)
        {
            var locationOfCurrentMarble = marbleOrder.IndexOf(currentMarble);
           
            for (long i = 0; i < 2; i++)
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

        private static long GetMarble7MarblesBack(IList<long> marbleOrder, long currentMarble)
        {
            var locationOfCurrentMarble = marbleOrder.IndexOf((currentMarble));

            for (long i = 7; i > 0; i--)
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
