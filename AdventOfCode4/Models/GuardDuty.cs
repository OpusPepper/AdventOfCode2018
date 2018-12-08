using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode4.Models
{
    public class GuardDuty
    {
        public int GuardId { get; set; }

        public DateTime DutyDateTime { get; set; }

        public List<DutyDay> ListOfDuties { get; set; } = new List<DutyDay>();

        public string LastLogInfo { get; set; }

        public GuardDuty(int id, DateTime dutyDate)
        {
            GuardId = id;
            DutyDateTime = dutyDate;
            ListOfDuties = new List<DutyDay>();
        }
    }
}
