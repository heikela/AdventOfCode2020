using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Common;

namespace Day21
{
    public record Recipe(List<string> Ingredients, List<string> Allergens);

    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "input.txt";
            //fileName = "sample.txt";
            Dictionary<string, List<List<string>>> recipesByAllergen = new Dictionary<string, List<List<string>>>();
            List<Recipe> recipes = File.ReadLines("../../../" + fileName)
                .Select(line =>
                {
                    Match recipeMatch = Regex.Match(line, @"((\w+)\s+)+\(contains ((, )?(\w+))+\)");
                    if (!recipeMatch.Success)
                    {
                        throw new Exception("can't match line: " + line);
                    }
                    return new Recipe(recipeMatch.Groups[2].Captures.Select(c => c.Value).ToList(), recipeMatch.Groups[5].Captures.Select(c => c.Value).ToList());
                })
                .ToList();
            foreach (var recipe in recipes)
            {
                foreach (string allergen in recipe.Allergens)
                {
                    var recipeList = recipesByAllergen.GetOrElse(allergen, new List<List<string>>());
                    recipeList.Add(recipe.Ingredients);
                    recipesByAllergen[allergen] = recipeList;
                }
            }
            Dictionary<string, HashSet<string>> possibleCulpritsByAllergen = recipesByAllergen
                .Select(kv => new KeyValuePair<string, HashSet<string>>(kv.Key,
                    kv.Value.Aggregate<IEnumerable<string>>((a, b) => a.Intersect(b)).ToHashSet()))
                .ToDictionary();

            IEnumerable<string> ingredients = recipes.SelectMany(r => r.Ingredients);
            HashSet<string> innocent = ingredients.Where(ingredient => !possibleCulpritsByAllergen.Any(suspects => suspects.Value.Contains(ingredient))).ToHashSet();

            Console.WriteLine(ingredients.Count(ingredient => innocent.Contains(ingredient)));

            while (possibleCulpritsByAllergen.Any(possibles => possibles.Value.Count() > 1))
            {
                foreach (var decided in possibleCulpritsByAllergen.Where(kv => kv.Value.Count() == 1))
                {
                    string decidedAllergen = decided.Key;
                    string decidedIngredient = decided.Value.First();
                    string ingredientWithKnownAllergen = possibleCulpritsByAllergen[decidedAllergen].First();
                    foreach (string allergen in possibleCulpritsByAllergen.Keys)
                    {
                        if (allergen == decidedAllergen)
                        {
                            continue;
                        }
                        possibleCulpritsByAllergen[allergen].Remove(decidedIngredient);
                    }
                }
            }

            Console.WriteLine(string.Join(",", possibleCulpritsByAllergen.OrderBy(kv => kv.Key).Select(kv => kv.Value.First())));
        }
    }
}
