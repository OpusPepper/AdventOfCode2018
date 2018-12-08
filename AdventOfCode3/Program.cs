using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode3
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part I
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);

            // fabric is a two dimensional array
            // fabric[x,y] = x = columns, y = rows
            int maxSize = 1001;
            int[,] fabric = new int[maxSize, maxSize];

            int claimId = 0;
            int inchesFromLeftEdge = 0;
            int inchesFromTopEdge = 0;
            int inchesWide = 0;
            int inchesTall = 0;
            int errorsFound = 0;
            string delimited = @"(\d+)";


            // Part I
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                if (matches.Count != 5)
                {
                    Console.WriteLine("Can't find right values in string: " + line);
                    errorsFound++;
                    continue;  // jump to next input
                }
                claimId = Convert.ToInt32(matches[0].Groups[0].Value);
                inchesFromLeftEdge = Convert.ToInt32(matches[1].Groups[0].Value);
                inchesFromTopEdge = Convert.ToInt32(matches[2].Groups[0].Value);
                inchesWide = Convert.ToInt32(matches[3].Groups[0].Value);
                inchesTall = Convert.ToInt32(matches[4].Groups[0].Value);

                for (int w = 0; w < inchesWide; w++)
                {
                    //fabric[inchesFromLeftEdge + w, inchesFromTopEdge] =
                    //    fabric[inchesFromLeftEdge + w, inchesFromTopEdge] + 1;
                    for (int t = 0; t < inchesTall; t++)
                    {
                        fabric[inchesFromLeftEdge + w, inchesFromTopEdge + t] =
                            fabric[inchesFromLeftEdge + w, inchesFromTopEdge + t] + 1;
                    }
                }              
            }

            int totalSquareInchesOverlapping = 0;
            for (int x = 0; x < maxSize; x++)
            {
                for (int y = 0; y < maxSize; y++)
                {
                    //Console.WriteLine("x: " + x.ToString() + ", y: " + y.ToString() + ", value: " + fabric[x,y].ToString());
                    if (fabric[x, y] > 1)
                    {
                        totalSquareInchesOverlapping++;
                        //Console.WriteLine("x: " + x.ToString() + ", y: " + y.ToString() + ", value: " + fabric[x, y].ToString());
                    }
                }
            }

            var partOneAnswer = totalSquareInchesOverlapping;

            // Part II
            // Using the fabric matrix, go back through the list and see if all locations for a claim only have 1s
            bool isThisClaimAllByItself = false;
            int totalAllByItselfAreas = 0;
            int partTwoAnswer = 0;

            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);
                

                if (matches.Count != 5)
                {
                    Console.WriteLine("Can't find right values in string: " + line);
                    errorsFound++;
                    continue;  // jump to next input
                }
                claimId = Convert.ToInt32(matches[0].Groups[0].Value);
                inchesFromLeftEdge = Convert.ToInt32(matches[1].Groups[0].Value);
                inchesFromTopEdge = Convert.ToInt32(matches[2].Groups[0].Value);
                inchesWide = Convert.ToInt32(matches[3].Groups[0].Value);
                inchesTall = Convert.ToInt32(matches[4].Groups[0].Value);

                isThisClaimAllByItself = true;
                for (int w = 0; w < inchesWide; w++)
                {
                    //fabric[inchesFromLeftEdge + w, inchesFromTopEdge] =
                    //    fabric[inchesFromLeftEdge + w, inchesFromTopEdge] + 1;
                    for (int t = 0; t < inchesTall; t++)
                    {
                        isThisClaimAllByItself = isThisClaimAllByItself &&
                                                 (fabric[inchesFromLeftEdge + w, inchesFromTopEdge + t] == 1);
                    }

                    if (!isThisClaimAllByItself)
                        break;  // if even one square is overlapping, let's jump to the next
                }
                if (isThisClaimAllByItself)
                {
                    totalAllByItselfAreas++;
                    partTwoAnswer = claimId;
                }
            }

            // Results

            Console.WriteLine("******************");
            Console.WriteLine("AdventOfCode Day 3");
            Console.WriteLine("Part I: " + partOneAnswer.ToString());
            Console.WriteLine("Part II: " + partTwoAnswer.ToString());
            Console.WriteLine("******************");
            Console.WriteLine("Errors found:  " + errorsFound.ToString());
            Console.WriteLine("totalAllByItselfAreas found:  " + totalAllByItselfAreas.ToString());
            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }
    }
}
