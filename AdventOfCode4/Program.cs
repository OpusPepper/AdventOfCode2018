using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using AdventOfCode4.Models;

namespace AdventOfCode4
{
    // TODO - handle when the shift starts AFTER midnight (like 02:00, instead of 00:00)
    class Program
    {
        public static List<InputRecord> ImportedRecords { get; set; } = new List<InputRecord>();

        public static List<GuardDuty> Guards { get; set; } = new List<GuardDuty>();

        static void Main(string[] args)
        {
            // Part I
            string path = Path.Combine(@"..\..\Data\input.txt");
            string[] allLines = File.ReadAllLines(path);
            int partOneAnswer = 0;
            string delimited = @"\[([^\]]+)\] (.*)";
            int errorsFound = 0;
            InputRecord inputRecord = null;
            DateTime dateTimeOut;
            int minuteDifferences = 0;
            // Part I
            // Get records, regex them into a list of objects
            foreach (var line in allLines)
            {
                var matches = Regex.Matches(line, delimited);

                if (matches.Count != 1)
                {
                    Console.WriteLine("Can't find right values in string: " + line);
                    errorsFound++;
                    continue;  // jump to next input
                }

                if (DateTime.TryParse(matches[0].Groups[1].Value, out dateTimeOut))
                {
                    inputRecord = new InputRecord(dateTimeOut, matches[0].Groups[2].Value);
                    ImportedRecords.Add(inputRecord);
                }
                else
                {
                    Console.WriteLine("DateTime not in right format: " + matches[0].Groups[1].Value);
                    errorsFound++;
                }
            }

            // sort the objects on date
            var sortedRecords = ImportedRecords.OrderBy(x => x.LogDateTime);

            // Go through the log, assign to each guard
            delimited = @"#(\d+)";
            int guardNumber = 0;
            GuardDuty guard = null;
            GuardDuty newGuard;
            DutyDay newDutyDay;
            DateTime startTime;
            DateTime endTime;
            bool isNewGuard;
            DateTime baseStartDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime newDateTime;
            foreach (var log in sortedRecords)
            {
                isNewGuard = false;
                var matches = Regex.Matches(log.LogInfo, delimited);

                if (matches.Count == 1)
                {
                    if (Guards.Count() != 0)
                    {
                        CalculateFinalMinutesOfShift(guard);
                    }

                    guardNumber = Convert.ToInt32(matches[0].Groups[1].Value);

                    guard = Guards.FirstOrDefault(x => x.GuardId == guardNumber);

                    if (guard == null)
                    {
                        // new guard
                        if (log.LogDateTime.Hour != 0)
                        {
                            log.LogDateTime.AddDays(1);
                            newDateTime = new DateTime(log.LogDateTime.Year, log.LogDateTime.Month, log.LogDateTime.Day, 0, 0, 0);
                            log.LogDateTime = newDateTime;
                        }
                        guard = new GuardDuty(guardNumber, log.LogDateTime);
                        Guards.Add(guard);
                    }

                    if (log.LogDateTime.Minute != 0)
                    {
                        newDateTime = new DateTime(log.LogDateTime.Year, log.LogDateTime.Month, log.LogDateTime.Day, 0, 0, 0);
                        log.LogDateTime = newDateTime;
                    }
                    newDutyDay = new DutyDay(log.LogDateTime);
                    guard.ListOfDuties.Add(newDutyDay);
                    
                }
                else
                {
                    CalculateAwakeAsleep(guard, log);
                }
            }

            // For last guard, there isn't a new record to close out the end of the last shift, doing that here
            CalculateFinalMinutesOfShift(guard);

            //debug, let's write out the list:
            var debugGuard = Guards.First();
            foreach (var g in Guards)
            {
                foreach (var ld in g.ListOfDuties)
                {
                    Console.WriteLine("Guard: " + g.GuardId);
                    Console.WriteLine("Date: " + ld.GuardDate.ToShortDateString());
                    Console.WriteLine("000000000011111111112222222222333333333344444444445555555555");
                    Console.WriteLine("012345678901234567890123456789012345678901234567890123456789");
                
                    foreach (var l in ld.AwakeAsleep)
                    {
                        Console.Write(l);
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                }
            }

            // Now let's calculate the guard who spends the most time asleep
            GuardWhoSleepsTheMost guardAnswer = new GuardWhoSleepsTheMost();
            guardAnswer.GuardId = 0;
            guardAnswer.TotalHoursAsleep = 0;
            int totalHoursAsleep = 0;
            foreach (var g in Guards)
            {
                totalHoursAsleep = 0;
                foreach (var d in g.ListOfDuties)
                {
                    totalHoursAsleep += d.AwakeAsleep.Count(x => x == 0);
                }

                if (totalHoursAsleep > guardAnswer.TotalHoursAsleep)
                {
                    guardAnswer.GuardId = g.GuardId;
                    guardAnswer.TotalHoursAsleep = totalHoursAsleep;
                }
            }

            guard = Guards.FirstOrDefault(x => x.GuardId == guardAnswer.GuardId);
            int[] overlappingMinute = new int[60];
            foreach (var m in guard.ListOfDuties)
            {
                for (int i = 0; i < 60; i++)
                {
                    if (m.AwakeAsleep[i] == 0)
                    {
                        overlappingMinute[i] += 1;
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Overlapping minutes: ");
            Console.WriteLine("000000000011111111112222222222333333333344444444445555555555");
            Console.WriteLine("012345678901234567890123456789012345678901234567890123456789");
            foreach (var i in overlappingMinute)
            {
                Console.Write(i.ToString() + " ");
            }

            Console.WriteLine();
            Console.WriteLine();
            //foreach (var r in sortedRecords)
            //{
            //    Console.WriteLine("LogTime:  " + r.LogDateTime + ", Message: " + r.LogInfo);
            //}

            int rightMinute = Array.IndexOf(overlappingMinute, overlappingMinute.Max());

            Console.WriteLine("GuardId: " + guardAnswer.GuardId + ", TotalHours: " + guardAnswer.TotalHoursAsleep + ", Overlapping minute: " + rightMinute);

            partOneAnswer = guardAnswer.GuardId * rightMinute;

            // Part II
            int partTwoAnswer = 0;

            guardAnswer = new GuardWhoSleepsTheMost();
            guardAnswer.GuardId = 0;
            guardAnswer.TotalHoursAsleep = 0;            
            foreach (var g in Guards)
            {
                overlappingMinute = new int[60];
                foreach (var d in g.ListOfDuties)
                {                   
                        for (int i = 0; i < 60; i++)
                        {
                            if (d.AwakeAsleep[i] == 0)
                            {
                                overlappingMinute[i] += 1;
                            }
                        }                   
                }

                if (overlappingMinute.Max() > guardAnswer.TotalHoursAsleep)
                {
                    guardAnswer.GuardId = g.GuardId;
                    guardAnswer.TotalHoursAsleep = overlappingMinute.Max();
                    guardAnswer.MinuteOverlapMostAsleep = Array.IndexOf(overlappingMinute, overlappingMinute.Max());
                }
            }

            Console.WriteLine("GuardId: " + guardAnswer.GuardId + ", Slept Most during a given minute: " + guardAnswer.TotalHoursAsleep + ", Max Minute: " + guardAnswer.MinuteOverlapMostAsleep);

            partTwoAnswer = guardAnswer.GuardId * guardAnswer.MinuteOverlapMostAsleep;

            // Results
            Console.WriteLine("******************");
            Console.WriteLine("AdventOfCode Day 4");
            Console.WriteLine("Part I: " + partOneAnswer);
            Console.WriteLine("Part II: " + partTwoAnswer.ToString());
            Console.WriteLine("******************");
            Console.WriteLine("Errors found:  " + errorsFound.ToString());
            Console.WriteLine("Press any key to end...");
            Console.ReadLine();
        }

        private static void CalculateFinalMinutesOfShift(GuardDuty guard)
        {
            DateTime startTime;
            int minuteDifferences;
// need to close out last guard
            var guardDutyRecord = guard.ListOfDuties.LastOrDefault();
            startTime = guardDutyRecord.GuardDate;
            minuteDifferences = 60 - startTime.Minute;
            // use reverse of last status
            if (guard.LastLogInfo == "falls asleep")
                SetAwakeAsleep("wakes up", minuteDifferences, guardDutyRecord, startTime);
            else
                SetAwakeAsleep("falls asleep", minuteDifferences, guardDutyRecord, startTime);
        }

        private static void CalculateAwakeAsleep(GuardDuty guard, InputRecord log)
        {
            DateTime startTime;
            DateTime endTime;
            int minuteDifferences;
            // Always get the start time from last duty record
            var guardDutyRecord = guard.ListOfDuties.LastOrDefault();
            startTime = guard.ListOfDuties.LastOrDefault().GuardDate;
            endTime = log.LogDateTime;
            minuteDifferences = endTime.Minute - startTime.Minute;
            Console.WriteLine("Start: " + startTime.ToString() + ", End: " + endTime.ToString() + ", minutes to fill: " +
                              minuteDifferences + ", " + log.LogInfo.Trim().ToLower());
            SetAwakeAsleep(log.LogInfo.Trim().ToLower(), minuteDifferences, guardDutyRecord, startTime);

            guardDutyRecord.GuardDate = log.LogDateTime;
            guard.LastLogInfo = log.LogInfo;
        }

        private static void SetAwakeAsleep(string asleepAwake, int minuteDifferences, DutyDay guardDutyRecord, DateTime startTime)
        {
            if (asleepAwake == "falls asleep")
            {
                for (int i = 0; i < minuteDifferences; i++)
                {
                    guardDutyRecord.AwakeAsleep[startTime.Minute + i] = 1;
                }
            }

            if (asleepAwake == "wakes up")
            {
                for (int i = 0; i < minuteDifferences; i++)
                {
                    guardDutyRecord.AwakeAsleep[startTime.Minute + i] = 0;
                }
            }
        }
    }
}
