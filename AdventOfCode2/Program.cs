using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part I
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);
            string[] allLines2 = File.ReadAllLines(path);
            int exactlyTwoOfAnyLetter = 0;
            int exactlyThreeOfAnyLetter = 0;

            foreach (var line in allLines)
            {
                if (isContainsExactlyTwo(line))
                    exactlyTwoOfAnyLetter++;
                if (isContainsExactlyThree(line))
                    exactlyThreeOfAnyLetter++;
            }

            var checkSum = exactlyTwoOfAnyLetter * exactlyThreeOfAnyLetter;

            // Part II
            string box1 = "";
            string box2 = "";
            foreach (var line in allLines)
            {
                foreach (var line2 in allLines2)
                {
                    if (stringsDifferByExactlyOneCharacterPositionSpecific(line, line2))
                    {
                        box1 = line;
                        box2 = line2;
                        Console.WriteLine("Found a match! Box1: " + box1 + ", Box2: " + box2);
                    }
                }
            }

            // Results

            Console.WriteLine("******************");
            Console.WriteLine("AdventOfCode Day 2");
            Console.WriteLine("Part I: " + checkSum.ToString());
            Console.WriteLine("Part II: " + removeDifferentCharaters(box1, box2));
            Console.WriteLine("******************");
            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }

        private static bool isContainsExactlyTwo(string line)
        {
            foreach (char c in line.ToCharArray())
            {
                if (line.Count(x => x == c) == 2)
                {
                    // first time we find two characters in a word, return true
                    return true;
                }
            }
            return false;
        }

        private static bool isContainsExactlyThree(string line)
        {
            foreach (char c in line.ToCharArray())
            {
                if (line.Count(x => x == c) == 3)
                {
                    // first time we find three characters in a word, return true
                    return true;
                }
            }
            return false;
        }

        private static bool stringsDifferByExactlyOneCharacterPositionSpecific(string string1, string string2)
        {
            if (string1.Length != string2.Length)
                return false;

            int numberOfCharacterDifferences = 0;

            for (int i = 0; i < string1.Length; i++)
            {
                if (string1[i] != string2[i])
                {
                    numberOfCharacterDifferences++;
                }
            }

            if (numberOfCharacterDifferences == 1)
                return true;
            else
                return false;
        }

        private static string removeDifferentCharaters(string string1, string string2)
        {
            string outputString = "";

            for (int i = 0; i < string1.Length; i++)
            {
                if (string1[i] == string2[i])
                {
                    outputString += string1[i];
                }
            }

            return outputString;
        }

    }
}
