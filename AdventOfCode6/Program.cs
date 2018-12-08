using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode6.Models;
using LINQPad;

namespace AdventOfCode6
{
    class Program
    {
        public static List<PointOnMap> Points { get; set; } = new List<PointOnMap>();

        static void Main(string[] args)
        {
            // Part I
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);
            int partOneAnswer = 0;

            string delimited = @"(\d+)";
            int charNumber = 47;

            //Build List of points            
            PointOnMap newPoint;
            Tuple<int, int> coord;
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                coord = new Tuple<int, int>(Convert.ToInt32(matches[0].Value), Convert.ToInt32(matches[1].Value));
                newPoint = new PointOnMap((char)charNumber, coord);

                Points.Add(newPoint);
                charNumber++;
            }

            // Find points that will be infinite
            //if the point has no points "above" it, then it's infinite
            //if the point has no points "below" it, then it's infinite
            //if the point has no points to the "left" of it, then it's infinite
            //if the point has no points to the "right" of it, then it's infinite

            var maxRowValue = Points.Max(x => x.coordinate.Item1);
            var minRowValue = Points.Min(x => x.coordinate.Item1);
            var maxColumnValue = Points.Max(x => x.coordinate.Item2);
            var minColumnValue = Points.Min(x => x.coordinate.Item2);

            // Get grid setup to appropriate size
            int maxSize;           
            if (maxRowValue >= maxColumnValue)
            {
                maxSize = maxRowValue + 1;
            }
            else
            {
                maxSize = maxColumnValue + 1;
            }

            //maxSize = 400;

            // Identifying infinite members
            char[,] grid = new char[maxSize, maxSize];

            bool hasPointsInQ1 = false;
            bool hasPointsInQ2 = false;
            bool hasPointsInQ3 = false;
            bool hasPointsInQ4 = false;

            foreach (var p in Points)
            {
                //hasPointsInQ1 = false;
                //hasPointsInQ2 = false;
                //hasPointsInQ3 = false;
                //hasPointsInQ4 = false;

                //hasPointsInQ1 = Points.Any(x =>
                //    x.Name != p.Name && x.coordinate.Item1 > p.coordinate.Item1 &&
                //    x.coordinate.Item2 < p.coordinate.Item2);
                //hasPointsInQ2 = Points.Any(x =>
                //    x.Name != p.Name && x.coordinate.Item1 > p.coordinate.Item1 &&
                //    x.coordinate.Item2 > p.coordinate.Item2);
                //hasPointsInQ3 = Points.Any(x =>
                //    x.Name != p.Name && x.coordinate.Item1 < p.coordinate.Item1 &&
                //    x.coordinate.Item2 > p.coordinate.Item2);
                //hasPointsInQ4 = Points.Any(x =>
                //    x.Name != p.Name && x.coordinate.Item1 < p.coordinate.Item1 &&
                //    x.coordinate.Item2 < p.coordinate.Item2);

                //if (hasPointsInQ1 && hasPointsInQ2 && hasPointsInQ3 && hasPointsInQ4)
                //{
                //    p.isInfinite = false;
                //}
                //else
                //    p.isInfinite = true;

                //if ((!Points.Where(x => x.Name != p.Name).Any(x => x.coordinate.Item1 > p.coordinate.Item1)) ||
                //    (!Points.Where(x => x.Name != p.Name).Any(x => x.coordinate.Item1 < p.coordinate.Item1)) ||
                //    (!Points.Where(x => x.Name != p.Name).Any(x => x.coordinate.Item2 > p.coordinate.Item2)) ||
                //    (!Points.Where(x => x.Name != p.Name).Any(x => x.coordinate.Item2 < p.coordinate.Item2)))
                //{
                //    p.isInfinite = true;
                //}

                //if (((p.coordinate.Item1 == maxRowValue) || (p.coordinate.Item2 == minRowValue)) ||
                //    ((p.coordinate.Item2 == maxColumnValue) || (p.coordinate.Item2 == minColumnValue)))
                //{
                //    p.isInfinite = true;
                //}
            }

            // Print out list of points and values
            //foreach (var p in Points)
            //{
            //    Console.WriteLine("Name: "+ p.Name + ", Coordinate:" + p.coordinate.Item1 + "," + p.coordinate.Item2 + " IsInfinite: " + p.isInfinite.ToString());
            //}

            //Console.WriteLine("Press any key to continue");
            //Console.ReadLine();

            Console.WriteLine("Initial Grid of points");
            for (int r = 0; r < maxSize; r++)
            {
                for (int c = 0; c < maxSize; c++)
                {
                    var pointToGrid = Points.FirstOrDefault(x => x.coordinate.Item1 == r && x.coordinate.Item2 == c);
                    if (pointToGrid != null)
                    {
                        grid[r, c] = pointToGrid.Name;
                        //Console.WriteLine(grid[r,c]);
                    }
                    else
                    {
                        //Console.Write(grid[r, c]);
                    }                    
                }
                //Console.WriteLine();
            }

            //Console.WriteLine("Press any key to continue");
           // Console.ReadLine();

            // Let's start with 0,0
            Tuple<int, int> pointToCheck = new Tuple<int, int>(0, 0);
            int distance = 0;
            int p1, p2;
            for (int q1 = 0; q1 < maxSize; q1++)
            {
                for (int q2 = 0; q2 < maxSize; q2++)
                {
                    pointToCheck = new Tuple<int, int>(q1, q2);

                    if (!Points.Any(x => x.coordinate.Item1 == q1 && x.coordinate.Item2 == q2))
                    {
                        foreach (var p in Points)
                        {
                            p1 = p.coordinate.Item1;
                            p2 = p.coordinate.Item2;
                            distance = Math.Abs(p1 - q1) + Math.Abs(p2 - q2);

                            p.Distance = distance;
                        }

                        // go through each point and see which is the closest, assign lower case
                        var minDistancePoint = Points.OrderBy(x => x.Distance).First();

                        if (Points.Count(x => x.Distance == minDistancePoint.Distance) == 1)
                        {
                            grid[q1, q2] = minDistancePoint.Name;
                        }
                        else
                        {
                            //Console.WriteLine("Found overlapping distance");
                            grid[q1, q2] = '.';
                        }
                    }
                    else
                    {
                        Console.WriteLine("Found Main Point: " + pointToCheck.Item1 + ", " + pointToCheck.Item2);
                    }
                }
            }

            for (int r = 0; r < maxRowValue; r++)
            {
                var point = Points.FirstOrDefault(x => x.Name == grid[r, 0]);
                if (point != null)
                    point.isInfinite = true;

                point = Points.FirstOrDefault(x => x.Name == grid[r, maxColumnValue]);
                if (point != null)
                    point.isInfinite = true;
            }
            for (int c = 0; c < maxRowValue; c++)
            {
                var point = Points.FirstOrDefault(x => x.Name == grid[0, c]);
                if (point != null)
                    point.isInfinite = true;

                point = Points.FirstOrDefault(x => x.Name == grid[maxRowValue, c]);
                if (point != null)
                    point.isInfinite = true;
            }

            // Find the max area; first we need to count the area for each point
            foreach (var p in Points)
            {
                if (!p.isInfinite)
                {
                    for (int q1 = 0; q1 < maxSize; q1++)
                    {
                        for (int q2 = 0; q2 < maxSize; q2++)
                        {
                            if (p.Name == (grid[q1, q2]))
                            {
                                p.area++;
                            }
                        }
                    }
                }
            }

            // Print out list of points and values
            foreach (var p in Points.OrderByDescending(x => x.area))
            {
                Console.WriteLine("Name: " + p.Name + ", Coordinate:" + p.coordinate.Item1 + "," + p.coordinate.Item2 + " IsInfinite: " + p.isInfinite.ToString() + ", area: " + p.area);
            }

            var pointWithMaxArea = Points.OrderByDescending(x => x.area).First();
            Console.WriteLine("Largest area: " + pointWithMaxArea.Name + ", Area: " + pointWithMaxArea.area);

            partOneAnswer = pointWithMaxArea.area;

            // Part II
            int partTwoAnswer = 0;

            int totalDistance = 0;
            int totalInRegion = 0;
            //go through the grid, find each "maxarea" point and calculate the distance to ever other point
            for (int r = 0; r < maxSize; r++)
            {
                for (int c = 0; c < maxSize; c++)
                {
                        totalDistance = 0;
                            foreach (var p in Points)
                            {
                                totalDistance += Math.Abs(r - p.coordinate.Item1) + Math.Abs(c - p.coordinate.Item2);
                                if (totalDistance >= 10000)
                                    break;
                            }

                            if (totalDistance < 10000)
                            {
                                totalInRegion++;
                            }
                }
            }

            partTwoAnswer = totalInRegion;

            // Results
            Console.WriteLine("******************");
            Console.WriteLine("AdventOfCode Day 6");
            Console.WriteLine("Part I: " + partOneAnswer);
            Console.WriteLine("Part II: " + partTwoAnswer);
            Console.WriteLine("******************");
            //Console.WriteLine("Pairs removed:  " + pairsRemoved);
            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }

    }
}
