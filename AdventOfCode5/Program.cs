using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode5
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part I
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);
            int partOneAnswer = 0;
            string output = "";
            bool pairFound = false;
            string input = allLines[0];
            int pairsRemoved = 0;
            input = PerformReaction(input, ref pairsRemoved);

            partOneAnswer = input.Length;

            // Part II
            int partTwoAnswer = 0;
            int bestPolymerLength = 999999999;
            string bestPolymerPair = "";

         
                for (int j = 65; j <= 90; j++)
                {
                    Console.WriteLine("Checking character '" + ((char)j).ToString() + "', '" + ((char)(j+32)).ToString() + "'");
                    input = allLines[0];                    
                    pairsRemoved = 0;

                    do
                    {
                        for (int i = 0; i < input.Length; i++)
                        {
                            //Console.WriteLine("Checking for characters: " + ((char) j) + ", " + ((char) (j + 32)));
                            //Console.WriteLine("Input[" + i + "] = " + input[i]);
                            if (input[i] == ((char) j) || (int) input[i] == ((char) (j + 32)))
                            {
                                //Console.WriteLine("Removing:  " + input[i]);
                                input = input.Remove(i, 1);
                                break;
                            }
                        }
                    } while (input.Contains((char) j) || input.Contains((char) (j + 32)));

                    //Console.WriteLine("Input after removing characters is: " + input);
                input = PerformReaction(input, ref pairsRemoved);

                //Console.WriteLine("Pairs removed: " + pairsRemoved);
                    //Console.WriteLine("PolymerLength: " + input.Length);
                    if (input.Length < bestPolymerLength)
                    {
                        bestPolymerLength = input.Length;
                        bestPolymerPair = ((char)j).ToString();
                    }
                }

            Console.WriteLine("BestPolymer length: " + bestPolymerLength);
            Console.WriteLine("BestPolymer pair: " + bestPolymerPair);

            partTwoAnswer = bestPolymerLength;


            // Results
            Console.WriteLine("******************");
            Console.WriteLine("AdventOfCode Day 5");
            Console.WriteLine("Part I: " + partOneAnswer);
            Console.WriteLine("Part II: " + partTwoAnswer);
            Console.WriteLine("******************");
            Console.WriteLine("Pairs removed:  " + pairsRemoved);
            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }

        private static string PerformReaction(string input, ref int pairsRemoved)
        {
            bool pairFound;
            do
            {
                pairFound = false;
                for (int i = 0; i < input.Length - 1; i++)
                {
                    // Console.WriteLine("input["+i+"] = "+input[i]+" int: "+(int)input[i]);
                    // Console.WriteLine("input[" + (i + 1) + "] = " + input[(i + 1)] + " int: " + (int)input[(i + 1)]);
                    // Console.WriteLine("ABS: "+ Math.Abs((int)input[i] - (int)input[i + 1]));
                    if (Math.Abs((int) input[i] - (int) input[i + 1]) == 32)
                    {
                        //Console.WriteLine("Removing:  " + input[i] + input[i+1]);
                        pairFound = true;
                        input = input.Remove(i, 2);
                        pairsRemoved++;
                        //Console.WriteLine("Output now: " + input);                        
                    }
                }
            } while (pairFound);

            return input;
        }
    }
}
