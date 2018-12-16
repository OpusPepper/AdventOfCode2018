using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode12.Models
{
    public class Rule
    {
        public int RuleNumber { get; set; }
        public string RulePatten { get; set; }

        public Rule(int inRuleNumber, string inRulePattern)
        {
            RuleNumber = inRuleNumber;
            RulePatten = inRulePattern;
        }
    }
}
