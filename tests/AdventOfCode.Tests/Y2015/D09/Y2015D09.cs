using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D09;

[ChallengeName("All in a Single Night")]
public class Y2015D09
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D09\Y2015D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = AllRouteLengths().Min();

        output.Should().Be(141);
    }

    [Fact]
    public void PartTwo()
    {
        var output = AllRouteLengths().Max();

        output.Should().Be(736);
    }

    private IEnumerable<int> AllRouteLengths()
    {
        var distances = new Dictionary<(string From, string To), int>();
        foreach (var line in _lines)
        {
            var parts = line.Split(" = ");
            var cities = parts[0].Split(" to ");
            distances[(cities[0], cities[1])] = int.Parse(parts[1]);
            distances[(cities[1], cities[0])] = int.Parse(parts[1]);
        }

        var citiesList = distances.Keys.Select(k => k.From).Distinct().ToArray();

        foreach (var route in Permutations(citiesList))
        {
            yield return route.Zip(route.Skip(1), (a, b) => distances[(a, b)]).Sum();
        }
    }

    private static IEnumerable<T[]> Permutations<T>(T[] items) => Recurse(items, 0);

    private static IEnumerable<T[]> Recurse<T>(T[] items, int start)
    {
        if (start == items.Length)
        {
            yield return items.ToArray();
            yield break;
        }

        for (int i = start; i < items.Length; i++)
        {
            (items[start], items[i]) = (items[i], items[start]);
            foreach (var p in Recurse(items, start + 1))
                yield return p;
            (items[start], items[i]) = (items[i], items[start]);
        }
    }
}