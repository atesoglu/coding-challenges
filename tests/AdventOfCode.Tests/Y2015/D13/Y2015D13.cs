using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D13;

[ChallengeName("Knights of the Dinner Table")]
public class Y2015D13
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D13\Y2015D13-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Happiness(false).Max();

        output.Should().Be(664);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Happiness(true).Max();

        output.Should().Be(640);
    }

    private IEnumerable<int> Happiness(bool includeMe)
    {
        var dh = new Dictionary<(string, string), int>();
        foreach (var line in _lines)
        {
            var m = Regex.Match(line, @"(.*) would (.*) (.*) happiness units by sitting next to (.*).");
            var a = m.Groups[1].Value;
            var b = m.Groups[4].Value;
            var happiness = int.Parse(m.Groups[3].Value) * (m.Groups[2].Value == "gain" ? 1 : -1);
            if (!dh.ContainsKey((a, b)))
            {
                dh[(a, b)] = 0;
                dh[(b, a)] = 0;
            }

            dh[(a, b)] += happiness;
            dh[(b, a)] += happiness;
        }

        var people = dh.Keys.Select(k => k.Item1).Distinct().ToList();
        if (includeMe)
        {
            people.Add("me");
        }

        return Permutations(people.ToArray()).Select(order => order.Zip(order.Skip(1).Append(order[0]), (a, b) => dh.GetValueOrDefault((a, b), 0)).Sum()
        );
    }

    private static IEnumerable<T[]> Permutations<T>(T[] rgt)
    {
        IEnumerable<T[]> PermutationsRec(int i)
        {
            if (i == rgt.Length)
            {
                yield return rgt.ToArray();
            }

            for (var j = i; j < rgt.Length; j++)
            {
                (rgt[i], rgt[j]) = (rgt[j], rgt[i]);
                foreach (var perm in PermutationsRec(i + 1))
                {
                    yield return perm;
                }

                (rgt[i], rgt[j]) = (rgt[j], rgt[i]);
            }
        }

        return PermutationsRec(0);
    }
}