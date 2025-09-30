using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D19;

[ChallengeName("Medicine for Rudolph")]
public class Y2015D19
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2015\D19\Y2015D19-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (rules, molecule) = Parse();
        var output = ReplaceAll(rules, molecule).ToHashSet().Count;
        output.Should().Be(535);
    }

    [Fact]
    public void PartTwo()
    {
        var (rules, molecule) = Parse();
        var rand = new Random();
        var steps = 0;
        var current = molecule;

        // Shuffle and try random reductions until we reach "e"
        while (current != "e")
        {
            var replacements = Replacements(rules, current, forward: false).ToArray();
            if (replacements.Length == 0)
            {
                current = molecule;
                steps = 0;
                continue;
            }

            var (fromIdx, fromLen, toStr) = replacements[rand.Next(replacements.Length)];
            current = Replace(current, fromIdx, toStr, fromLen);
            steps++;
        }

        steps.Should().Be(212);
    }

    private static IEnumerable<string> ReplaceAll((string from, string to)[] rules, string molecule)
    {
        foreach (var (idx, len, to) in Replacements(rules, molecule, forward: true))
            yield return Replace(molecule, idx, to, len);
    }

    private static string Replace(string molecule, int start, string replacement, int length) => molecule.Substring(0, start) + replacement + molecule.Substring(start + length);

    private static IEnumerable<(int idx, int length, string to)> Replacements((string from, string to)[] rules, string molecule, bool forward)
    {
        for (var pos = 0; pos < molecule.Length; pos++)
        {
            foreach (var (a, b) in rules)
            {
                var (from, to) = forward ? (a, b) : (b, a);

                if (molecule.Substring(pos).StartsWith(from))
                    yield return (pos, from.Length, to);
            }
        }
    }

    private ((string from, string to)[] rules, string molecule) Parse()
    {
        var rules = _lines.TakeWhile(line => line.Contains("=>")).Select(line =>
            {
                var parts = line.Split(" => ");
                return (parts[0], parts[1]);
            })
            .ToArray();

        var molecule = _lines.Last();
        return (rules, molecule);
    }
}