using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode14.Models
{
    public class Recipe
    {
        public int RecipeScore { get; set; }

        public bool ElfCurrentlyWorking { get; set; }

        public int IdOfElfCurrentlyWorking { get; set; }

        public Recipe(int recipeScore, int idOfElfCurrentlyWorking)
        {
            RecipeScore = recipeScore;
            IdOfElfCurrentlyWorking = idOfElfCurrentlyWorking;
        }
    }
}
