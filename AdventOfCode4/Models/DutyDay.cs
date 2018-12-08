using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode4.Models
{
    public class DutyDay
    {
        public DateTime GuardDate { get; set; }

        public int[] AwakeAsleep = new int[60];  // represents 60 minutes of midnight until 1 am, 1 = awake, 0 = asleep

        public DutyDay(DateTime dateIn)
        {
            GuardDate = dateIn;
        }
    }
}
