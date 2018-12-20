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
            int rowCount = 151;
            int columnCount = 151;
            char[,] grid = new char[rowCount, columnCount];
            char[] possibleEngines = new char[4] {'<', '>', '^', 'v'};

            Rail myTrack = new Rail(grid, rowCount, columnCount);
            Engine tempEngine;
            List<Engine> engines = new List<Engine>();

            //string delimited = @"(-?\d+)";
            int gridSerialNumber = 0;

            // Find beginning and end nodes
            int r = 0;
            int c = 0;
            char changeEngineTo;
            foreach (var line in allLines)
            {
                //var matches = Regex.Matches(line, delimited);

                //gridSerialNumber = Convert.ToInt32(matches[0].Value);
                //Log.InfoFormat($"Line length: " + line.Length);
                c = 0;
                foreach (var ch in line.ToCharArray())
                {
                    if (possibleEngines.Contains(ch))
                        {
                            tempEngine = new Engine(ch, r, c);
                            engines.Add(tempEngine);

                            switch (ch)
                            {
                                case '>':
                                case '<':
                                    changeEngineTo = '-';
                                    break;
                                case '^':
                                case 'v':
                                    changeEngineTo = '|';
                                    break;
                                default:
                                    changeEngineTo = '?';
                                    break;
                            }

                            myTrack.Track[r, c] = changeEngineTo;
                        }
                        else
                        {
                            myTrack.Track[r, c] = ch;
                        }                        
                        c++;
                    
                }
                    r++;
            }

            //DisplayTrack(myTrack, engines);

            // let's iterate the trains
            List<Engine> crashes = new List<Engine>();
            int iteration = 1;
            char whatsInGridNextLocation;
            do
            {
                foreach (var eng in engines.OrderBy(x => x.CurrentRow).ThenBy(x => x.CurrentColumn))
                {
                    switch (eng.EnginePic)
                    {
                        case '>':
                            whatsInGridNextLocation = grid[eng.CurrentRow, eng.CurrentColumn + 1];
                            if (engines.Any(x => x.CurrentRow == eng.CurrentRow && x.CurrentColumn == eng.CurrentColumn + 1))
                            {
                                eng.CurrentColumn++;
                                eng.Crashed = true;
                                eng.EnginePic = 'X';
                            }
                            else if (whatsInGridNextLocation == '-')
                            {
                                eng.CurrentColumn++;
                            }
                            else if (whatsInGridNextLocation == '/')
                            {
                                eng.EnginePic = '^';
                                eng.CurrentColumn++;
                            }
                            else if (whatsInGridNextLocation == '\\')
                            {
                                eng.EnginePic = 'v';
                                eng.CurrentColumn++;
                            }
                            else if (whatsInGridNextLocation == '+')
                            {
                                var directionToMove = eng.Directions[eng.NextDirection];
                                switch (directionToMove)
                                {
                                    case "Left":
                                        eng.EnginePic = '^';
                                        break;
                                    case "Straight":
                                        break;
                                    case "Right":
                                        eng.EnginePic = 'v';
                                        break;
                                    default:
                                        Log.InfoFormat($"Error: Unknown direction: " + directionToMove);
                                        break;
                                }

                                eng.CurrentColumn++;

                                // setup next direction for train
                                if (eng.NextDirection == 2)
                                    eng.NextDirection = 0;
                                else
                                    eng.NextDirection++;
                            }
                            break;
                        case '<':
                            whatsInGridNextLocation = grid[eng.CurrentRow, eng.CurrentColumn - 1];
                            if (engines.Any(x => x.CurrentRow == eng.CurrentRow && x.CurrentColumn == eng.CurrentColumn - 1))
                            {
                                eng.CurrentColumn--;
                                eng.Crashed = true;
                                eng.EnginePic = 'X';
                            }
                            else if (whatsInGridNextLocation == '-')
                            {
                                eng.CurrentColumn--;
                            }
                            else if (whatsInGridNextLocation == '/')
                            {
                                eng.EnginePic = 'v';
                                eng.CurrentColumn--;
                            }
                            else if (whatsInGridNextLocation == '\\')
                            {
                                eng.EnginePic = '^';
                                eng.CurrentColumn--;
                            }
                            else if (whatsInGridNextLocation == '+')
                            {
                                var directionToMove = eng.Directions[eng.NextDirection];
                                switch (directionToMove)
                                {
                                    case "Left":
                                        eng.EnginePic = 'v';
                                        break;
                                    case "Straight":
                                        break;
                                    case "Right":
                                        eng.EnginePic = '^';
                                        break;
                                    default:
                                        Log.InfoFormat($"Error: Unknown direction: " + directionToMove);
                                        break;
                                }

                                eng.CurrentColumn--;

                                // setup next direction for train
                                if (eng.NextDirection == 2)
                                    eng.NextDirection = 0;
                                else
                                    eng.NextDirection++;
                            }
                            break;
                        case '^':
                            whatsInGridNextLocation = grid[eng.CurrentRow - 1, eng.CurrentColumn];
                            if (engines.Any(x => x.CurrentRow == eng.CurrentRow - 1 && x.CurrentColumn == eng.CurrentColumn))
                            {
                                eng.CurrentRow--;
                                eng.Crashed = true;
                                eng.EnginePic = 'X';
                            }
                            else if (whatsInGridNextLocation == '|')
                            {
                                eng.CurrentRow--;
                            }
                            else if (whatsInGridNextLocation == '/')
                            {
                                eng.EnginePic = '>';
                                eng.CurrentRow--;
                            }
                            else if (whatsInGridNextLocation == '\\')
                            {
                                eng.EnginePic = '<';
                                eng.CurrentRow--;
                            }
                            else if (whatsInGridNextLocation == '+')
                            {
                                var directionToMove = eng.Directions[eng.NextDirection];
                                switch (directionToMove)
                                {
                                    case "Left":
                                        eng.EnginePic = '<';
                                        break;
                                    case "Straight":
                                        break;
                                    case "Right":
                                        eng.EnginePic = '>';
                                        break;
                                    default:
                                        Log.InfoFormat($"Error: Unknown direction: " + directionToMove);
                                        break;
                                }

                                eng.CurrentRow--;

                                // setup next direction for train
                                if (eng.NextDirection == 2)
                                    eng.NextDirection = 0;
                                else
                                    eng.NextDirection++;
                            }
                            break;
                        case 'v':
                            whatsInGridNextLocation = grid[eng.CurrentRow + 1, eng.CurrentColumn];
                            if (engines.Any(x => x.CurrentRow == eng.CurrentRow + 1 && x.CurrentColumn == eng.CurrentColumn))
                            {
                                eng.CurrentRow++;
                                eng.Crashed = true;
                                eng.EnginePic = 'X';
                            }
                            else if (whatsInGridNextLocation == '|')
                            {
                                eng.CurrentRow++;
                            }
                            else if (whatsInGridNextLocation == '/')
                            {
                                eng.EnginePic = '<';
                                eng.CurrentRow++;
                            }
                            else if (whatsInGridNextLocation == '\\')
                            {
                                eng.EnginePic = '>';
                                eng.CurrentRow++;
                            }
                            else if (whatsInGridNextLocation == '+')
                            {
                                var directionToMove = eng.Directions[eng.NextDirection];
                                switch (directionToMove)
                                {
                                    case "Left":
                                        eng.EnginePic = '>';
                                        break;
                                    case "Straight":
                                        break;
                                    case "Right":
                                        eng.EnginePic = '<';
                                        break;
                                    default:
                                        Log.InfoFormat($"Error: Unknown direction: " + directionToMove);
                                        break;
                                }

                                eng.CurrentRow++;

                                // setup next direction for train
                                if (eng.NextDirection == 2)
                                    eng.NextDirection = 0;
                                else
                                    eng.NextDirection++;
                            }
                            break;
                    }
                }

                // Colision detector
                //foreach (var e in engines)
                //{
                //    //var sharesSameSpace = engines.Where(x =>
                //    //    x.CurrentRow == e.CurrentRow && x.CurrentColumn == e.CurrentColumn).ToList();
                //    var sharesSameSpace = engines.Where(x => x.EnginePic == 'X').OrderBy(x => x.CurrentRow).ThenBy(x => x.CurrentColumn);
                //    if (sharesSameSpace.Count() > 0)
                //    {
                //        e.EnginePic = 'X';
                //        e.Crashed = true;
                //        foreach (var e2 in sharesSameSpace)
                //        {
                //            e2.EnginePic = 'X';
                //            e2.Crashed = true;
                //        }
                //    }
                //}
                crashes = engines.Where(x => x.Crashed).ToList();
                if (crashes.Any())
                    Log.InfoFormat($"Crash detected: " + crashes[0].CurrentColumn + ", " + crashes[0].CurrentRow);


                //DisplayTrack(myTrack, engines);
                Log.InfoFormat($"Iteration: " + iteration);
                iteration++;
            } while (!crashes.Any());
            //} while (iteration < 1000);

            DisplayTrack(myTrack, engines);

            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 13");
            Log.InfoFormat($"Part I: " + crashes[0].CurrentColumn + ", " + crashes[0].CurrentRow);
            Log.InfoFormat($"******************");

            Console.WriteLine("Press any key to end...");
            Console.ReadLine();

        }

        public static void DisplayTrack(Rail myTrack, List<Engine> engines)
        {
            Log.InfoFormat($"******************");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            for (int r = 0; r < myTrack.NumRows; r++)
            {
                for (int c = 0; c < myTrack.NumColumns; c++)
                {
                    var isEngineHere = engines.FirstOrDefault(x => x.CurrentRow == r && x.CurrentColumn == c);
                    if (isEngineHere != null)
                    {
                        sb.Append(isEngineHere.EnginePic);
                    }
                    else
                        sb.Append(myTrack.Track[r, c]);
                }

                sb.AppendLine();
            }
            Log.InfoFormat(sb.ToString());
            Log.InfoFormat($"******************");
        }
    }
}
