using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode4.Models
{
    public class InputRecord
    {
        public DateTime LogDateTime { get; set; }

        public string LogInfo { get; set; }

        public InputRecord(DateTime dateTimeIn, string logInfoIn)
        {
            LogDateTime = dateTimeIn;
            LogInfo = logInfoIn;
        }
    }
}
