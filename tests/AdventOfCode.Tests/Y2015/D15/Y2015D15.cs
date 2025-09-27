using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D15;

[ChallengeName("Science for Hungry People")]
public partial class Y2015D15
{
    private readonly int[][] _ingredients;

    // Enum for cookie properties
    private enum Property
    {
        Capacity = 0,
        Durability,
        Flavor,
        Texture,
        Calories
    }

    private const int propertyCount = 5;

    public Y2015D15()
    {
        _ingredients = File.ReadAllLines(@"Y2015\D15\Y2015D15-input.txt", Encoding.UTF8)
            .Select(line =>
            {
                var match = IngredientRegex().Match(line);
                return match.Groups.Cast<Group>().Skip(1).Select(g => int.Parse(g.Value)).ToArray();
            })
            .ToArray();
    }

    [Fact]
    public void PartOne()
    {
        var output = FindOptimalCookieScore(null);
        output.Should().Be(222870);
    }

    [Fact]
    public void PartTwo()
    {
        var output = FindOptimalCookieScore(500);
        output.Should().Be(117936);
    }

    private long FindOptimalCookieScore(int? calories)
    {
        long maxScore = 0;

        foreach (var amounts in Partition(100, _ingredients.Length))
        {
            var totals = new int[propertyCount];
            for (var i = 0; i < _ingredients.Length; i++)
            for (var j = 0; j < propertyCount; j++)
                totals[j] += _ingredients[i][j] * amounts[i];

            if (!calories.HasValue || totals[(int)Property.Calories] == calories)
            {
                var score = totals.Take(propertyCount - 1).Aggregate(1L, (acc, val) => acc * Math.Max(0, val));
                maxScore = Math.Max(maxScore, score);
            }
        }

        return maxScore;
    }

    private static IEnumerable<int[]> Partition(int total, int count)
    {
        if (count == 1)
        {
            yield return [total];
        }
        else
        {
            for (var i = 0; i <= total; i++)
                foreach (var rest in Partition(total - i, count - 1))
                    yield return new[] { i }.Concat(rest).ToArray();
        }
    }

    [GeneratedRegex(@"\w+: capacity (-?\d+), durability (-?\d+), flavor (-?\d+), texture (-?\d+), calories (-?\d+)")]
    private static partial Regex IngredientRegex();
}