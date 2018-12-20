using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode14.Models
{
    public class Elf
    {
        public int ElfId { get; set; }
        public char EnclosingBracketBegin { get; set; }
        public char EnclosingBracketEnd { get; set; }
        public int CurrentRecipeIndex { get; set; }

        public Elf(int elfId, char enclosingBracketBegin, char enclosingBracketEnd, int currentRecipeIndex)
        {
            ElfId = elfId;
            EnclosingBracketBegin = enclosingBracketBegin;
            EnclosingBracketEnd = enclosingBracketEnd;
            CurrentRecipeIndex = currentRecipeIndex;
        }
    }
}
