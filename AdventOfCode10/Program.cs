using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode10.Models;

namespace AdventOfCode10
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
            string delimited = @"(-?\d+)";
            List<LightNode> lightNodes = new List<LightNode>();
            List<PointInTime> pointsInTime = new List<PointInTime>();

            // Find beginning and end nodes
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                var lightNode = new LightNode();
                lightNode.TuplePosition = (Convert.ToInt32(matches[0].Value), Convert.ToInt32(matches[1].Value));
                lightNode.TupleVelocity = (Convert.ToInt32(matches[2].Value), Convert.ToInt32(matches[3].Value));

                lightNodes.Add(lightNode);
            }

            var seconds = 0;
            var numberOfPointsTouching = 0;
            var multiplicationFactor = 1;
            do
            {
                // Start moving the points
                foreach (var point in lightNodes)
                {
                    point.TuplePosition.x += point.TupleVelocity.x;
                    point.TuplePosition.y += point.TupleVelocity.y;
                }

                // After 1 session of moving points, calculate if the point is touching another point
                foreach (var point in lightNodes)
                {
                    point.PointsTouching = 0;
                    point.PointsTouching += lightNodes.Where(z =>
                            z.TuplePosition.x == point.TuplePosition.x + 1 &&
                            z.TuplePosition.y == point.TuplePosition.y)
                        .Count();
                    point.PointsTouching += lightNodes.Where(z =>
                            z.TuplePosition.x == point.TuplePosition.x - 1 &&
                            z.TuplePosition.y == point.TuplePosition.y)
                        .Count();
                    point.PointsTouching += lightNodes.Where(z =>
                            z.TuplePosition.x == point.TuplePosition.x &&
                            z.TuplePosition.y == point.TuplePosition.y + 1)
                        .Count();
                    point.PointsTouching += lightNodes.Where(z =>
                            z.TuplePosition.x == point.TuplePosition.x &&
                            z.TuplePosition.y == point.TuplePosition.y - 1)
                        .Count();
                    //check diaganols too
                    point.PointsTouching += lightNodes.Where(z =>
        z.TuplePosition.x == point.TuplePosition.x + 1 &&
        z.TuplePosition.y == point.TuplePosition.y + 1)
    .Count();
                    point.PointsTouching += lightNodes.Where(z =>
                            z.TuplePosition.x == point.TuplePosition.x + 1 &&
                            z.TuplePosition.y == point.TuplePosition.y -1)
                        .Count();
                    point.PointsTouching += lightNodes.Where(z =>
                            z.TuplePosition.x == point.TuplePosition.x -1 &&
                            z.TuplePosition.y == point.TuplePosition.y + 1)
                        .Count();
                    point.PointsTouching += lightNodes.Where(z =>
                            z.TuplePosition.x == point.TuplePosition.x -1  &&
                            z.TuplePosition.y == point.TuplePosition.y - 1)
                        .Count();
                }

                var pointsTouching = lightNodes.Where(z => z.PointsTouching > 0);

                numberOfPointsTouching = lightNodes.Count(z => z.PointsTouching > 0);
                var minX = lightNodes.Min(z => z.TuplePosition.x);
                var maxX = lightNodes.Max(z => z.TuplePosition.x);
                var minY = lightNodes.Min(z => z.TuplePosition.y);
                var maxY = lightNodes.Max(z => z.TuplePosition.y);
                var pointInTime = new PointInTime(seconds, numberOfPointsTouching, minX, minY, maxX, maxY);
                pointsInTime.Add(pointInTime);

                if (numberOfPointsTouching > 18)
                    Log.InfoFormat($"Second:  " + seconds.ToString() + ", Number of points touching:  " + numberOfPointsTouching);
                    seconds++;
            } while (numberOfPointsTouching != lightNodes.Count);

            var highestSecond = pointsInTime.Max(z => z.HighestTouching);
            var highestPointInTime = pointsInTime.Where(z => z.HighestTouching == highestSecond);
            Log.InfoFormat($"Number highest seconds found: " + highestSecond);
            Log.InfoFormat($"Number highest seconds found: " + highestPointInTime.Count());
            if (highestPointInTime.Count() == 1)
            {
                Log.InfoFormat($"MinX: " + highestPointInTime.First().MinX);
                Log.InfoFormat($"MaxX: " + highestPointInTime.First().MaxX);
                Log.InfoFormat($"MinY: " + highestPointInTime.First().MinY);
                Log.InfoFormat($"MaxY: " + highestPointInTime.First().MaxY);
            }

            //StringBuilder sb = new StringBuilder();
            //for (int x = highestPointInTime.First().MinX; x <= highestPointInTime.First().MaxX; x++)
            //{
            //    sb = new StringBuilder();
            //    for (int y = highestPointInTime.First().MinY; y <= highestPointInTime.First().MaxY; y++)
            //    {
            //        if (lightNodes.Any(z => z.TuplePosition.x == x && z.TuplePosition.y == y))
            //        {
            //            sb.Append("#");
            //        }
            //        else
            //        {
            //            sb.Append(".");
            //        }
            //    }
            //    sb.AppendLine();
            //    Log.InfoFormat(sb.ToString());
            //}

            using (System.IO.StreamWriter file =
    new System.IO.StreamWriter(@"C:\Temp\AdventOfCodeDay10Text.txt"))
            {
                foreach (var node in lightNodes)
                {
                    file.WriteLine(node.TuplePosition.x + ";" + node.TuplePosition.y);
                }
            }

            partOneAnswer = seconds;
            Log.InfoFormat($"Total lightNodes: " + lightNodes.Count);

            Log.InfoFormat($"** Part I End **");
            

            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 10");
            Log.InfoFormat($"Part I (second of solution): " + partOneAnswer);
            Log.InfoFormat($"******************");
            Log.InfoFormat($"Game ends");

            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }
    }
}
