using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2020.D21;

[ChallengeName("Allergen Assessment")]
public class Y2020D21
{
    private readonly string _input = File.ReadAllText(@"Y2020\D21\Y2020D21-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = PartOne(_input);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = PartTwo(_input);

        output.Should().Be(0);
    }


    private object PartOne(string input)
    {
        var problem = Parse(input);
        var suspiciousIngredients = GetIngredientsByAllergene(problem).SelectMany(kvp => kvp.Value).ToHashSet();
        return problem.mapping
            .Select(entry => entry.ingredients.Count(ingredient => !suspiciousIngredients.Contains(ingredient)))
            .Sum();
    }

    private object PartTwo(string input)
    {
        var problem = Parse(input);
        var ingredientsByAllergene = GetIngredientsByAllergene(problem);

        // The problem is set up in a way that we can identify the allergene - ingredient pairs one by one. 
        while (ingredientsByAllergene.Values.Any(ingredients => ingredients.Count > 1))
        {
            foreach (var allergen in problem.allergens)
            {
                var candidates = ingredientsByAllergene[allergen];
                if (candidates.Count == 1)
                {
                    foreach (var allergenT in problem.allergens)
                    {
                        if (allergen != allergenT)
                        {
                            ingredientsByAllergene[allergenT].Remove(candidates.Single());
                        }
                    }
                }
            }
        }

        return string.Join(",", problem.allergens.OrderBy(a => a).Select(a => ingredientsByAllergene[a].Single()));
    }

    private Problem Parse(string input)
    {
        var mapping = (
            from line in input.Split("\n")
            let parts = line.Trim(')').Split(" (contains ")
            let ingredients = parts[0].Split(" ").ToHashSet()
            let allergens = parts[1].Split(", ").ToHashSet()
            select (ingredients, allergens)
        ).ToArray();

        return new Problem(
            mapping.SelectMany(entry => entry.allergens).ToHashSet(),
            mapping.SelectMany(entry => entry.ingredients).ToHashSet(),
            mapping
        );
    }

    private Dictionary<string, HashSet<string>> GetIngredientsByAllergene(Problem problem) =>
        problem.allergens.ToDictionary(
            allergene => allergene,
            allergene => problem.mapping
                .Where(entry => entry.allergens.Contains(allergene))
                .Aggregate(
                    problem.ingredients as IEnumerable<string>,
                    (res, entry) => res.Intersect(entry.ingredients))
                .ToHashSet());
}