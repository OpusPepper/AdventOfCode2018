using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);
            List<Int64> frequenciesFound = new List<Int64>();

            Int64 frequency = 0;
            Int64 intValueCheck;
            Int64 firstFrequencyReachedTwice = 0;
            Int64 finalFrequency = 0;
            bool checkForFirstFrequency = true;
            int failSafeNumberOfTries = 1000;
            int failSafeCounter = 0;

            // Find part I
            foreach (var line in allLines)
            {
                if (Int64.TryParse(line, out intValueCheck))
                {
                    frequency += intValueCheck;
                }
            }

            finalFrequency = frequency;

            frequency = 0;
            frequenciesFound.Add(frequency);
            // Find part II
            do
            {
                foreach (var line in allLines)
                {
                    if (Int64.TryParse(line, out intValueCheck))
                    {
                        frequency += intValueCheck;
                        if (!frequenciesFound.Contains(frequency))
                            frequenciesFound.Add(frequency);
                        else
                        {
                            if (checkForFirstFrequency)
                            {
                                firstFrequencyReachedTwice = frequency;
                                checkForFirstFrequency = false;
                            }
                        }
                    }
                }
                //Console.WriteLine("Frequency = " + frequency.ToString());
                //Console.WriteLine("failSafeCounter = " + failSafeCounter.ToString());

                failSafeCounter++;
            } while (checkForFirstFrequency && failSafeCounter < failSafeNumberOfTries);

            Console.WriteLine("Frequency = " + frequency.ToString());
            Console.WriteLine("failSafeCounter = " + failSafeCounter.ToString());

            Console.WriteLine("******************");
            Console.WriteLine("AdventOfCode Day 1");
            Console.WriteLine("Part I: " + finalFrequency.ToString());
            Console.WriteLine("Part II:  " + firstFrequencyReachedTwice.ToString());
            Console.WriteLine("******************");
            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }

    }
}
