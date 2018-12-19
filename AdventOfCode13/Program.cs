using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode13.Models;
using log4net;
using log4net.Config;

namespace AdventOfCode13
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
            int rowCount = 6;
            int columnCount = 14;
            char[,] grid = new char[rowCount, columnCount];

            Rail myTrack = new Rail(grid, rowCount, columnCount);

            //string delimited = @"(-?\d+)";
            int gridSerialNumber = 0;

            // Find beginning and end nodes
            int r = 0;
            int c = 0;
            foreach (var line in allLines)
            {
                //var matches = Regex.Matches(line, delimited);

                //gridSerialNumber = Convert.ToInt32(matches[0].Value);
                Log.InfoFormat($"Line length: " + line.Length);
                c = 0;
                foreach (var ch in line.ToCharArray())
                {
                    if (c < columnCount)
                    {
                        myTrack.Track[r, c] = ch;
                        c++;
                    }
                }

                if (r < rowCount)
                {
                    r++;
                }
            }

            DisplayTrack(myTrack);

            Log.InfoFormat($"Grid serial number: " + gridSerialNumber);


            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 13");
            Log.InfoFormat($"Part I: Energy: ");
            Log.InfoFormat($"Part II: Energy: ");
            Log.InfoFormat($"******************");

            Console.WriteLine("Press any key to end...");
            Console.ReadLine();

        }

        public static void DisplayTrack(Rail myTrack)
        {
            Log.InfoFormat($"******************");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            for (int r = 0; r < myTrack.NumRows; r++)
            {
                for (int c = 0; c < myTrack.NumColumns; c++)
                {
                    sb.Append(myTrack.Track[r, c]);
                }

                sb.AppendLine();
            }
            Log.InfoFormat(sb.ToString());
            Log.InfoFormat($"******************");
        }
    }
}
