using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode14.Models;
using log4net.Config;

namespace AdventOfCode14
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            // Part I
            int numberOfRecipes = 260321;            
            string partOneAnswer = "";            
            List<Recipe> recipes = new List<Recipe>()
            {
                new Recipe(3, 0),
                new Recipe(7, 1)
            };
            List<Elf> elves = new List<Elf>()
            {
                new Elf(0, '(', ')', 0),
                new Elf(1, '[', ']', 1)
            };

            do
            {
                var recipesCurrentlyBeingWorkedOn = recipes.Where(x => x.IdOfElfCurrentlyWorking >= 0).ToList();
                Recipe tempRecipe;
                if (recipesCurrentlyBeingWorkedOn.Count() == 2)
                {
                    // First lets add new recipes
                    var addToRecipes = recipesCurrentlyBeingWorkedOn[0].RecipeScore +
                                       recipesCurrentlyBeingWorkedOn[1].RecipeScore;
                    foreach (char c in addToRecipes.ToString().ToCharArray())
                    {
                        tempRecipe = new Recipe((int) Char.GetNumericValue(c), -1);
                        recipes.Add(tempRecipe);
                    }

                    // now let's assign where the elves are going to
                    foreach (var elf in elves)
                    {
                        var stepForward = recipes[elf.CurrentRecipeIndex].RecipeScore + 1;
                        recipes[elf.CurrentRecipeIndex].IdOfElfCurrentlyWorking = -1;

                        elf.CurrentRecipeIndex += stepForward;

                        if ((recipes.Count() - 1) < elf.CurrentRecipeIndex)
                        {
                            elf.CurrentRecipeIndex = elf.CurrentRecipeIndex % recipes.Count();
                        }


                        recipes[elf.CurrentRecipeIndex].IdOfElfCurrentlyWorking = elf.ElfId;
                    }

                    //DisplayRecipes(recipes, elves);
                    if (recipes.Count() % 1000 == 0)
                    {
                        Log.InfoFormat($"Recipes: " + recipes.Count().ToString());
                    }
                }
                else
                {
                    Log.InfoFormat($"**Couldn't find both elves**");
                }                

            } while (recipes.Count() < (numberOfRecipes + 11));

            for (int i = 0; i < recipes.Count(); i++)
            {
                if (i > (numberOfRecipes - 1) && (partOneAnswer.Length < 10))
                {
                    partOneAnswer = String.Concat(partOneAnswer, recipes[i].RecipeScore.ToString());
                }
            }

            // Results
            Log.InfoFormat($"******************");
            Log.InfoFormat($"AdventOfCode Day 14");
            Log.InfoFormat($"Part I: " + partOneAnswer);
            Log.InfoFormat($"Part II: " + "");
            Log.InfoFormat($"******************");

            Console.WriteLine("Press any key to end...");
            Console.ReadLine();


        }

        public static void DisplayRecipes(List<Recipe> recipes, List<Elf> elves)
        {            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < recipes.Count(); i++)
            {                
                var elfWorkingRecipe = elves.FirstOrDefault(x => x.CurrentRecipeIndex == i);

                if (elfWorkingRecipe != null)
                {
                    sb.Append((char)elfWorkingRecipe.EnclosingBracketBegin + recipes[i].RecipeScore.ToString() + (char)elfWorkingRecipe.EnclosingBracketEnd);
                }
                else
                    sb.Append(" " + recipes[i].RecipeScore.ToString() + " ");

            }
            Log.InfoFormat(sb.ToString());
        }
    }
}
