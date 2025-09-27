using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D16;

[ChallengeName("Aunt Sue")]
public partial class Y2015D16
{
    private readonly List<Dictionary<string, int>> _sues;

    private static readonly Dictionary<string, int> Target = new()
    {
        ["children"] = 3,
        ["cats"] = 7,
        ["samoyeds"] = 2,
        ["pomeranians"] = 3,
        ["akitas"] = 0,
        ["vizslas"] = 0,
        ["goldfish"] = 5,
        ["trees"] = 3,
        ["cars"] = 2,
        ["perfumes"] = 1,
    };

    public Y2015D16()
    {
        var lines = File.ReadAllLines(@"Y2015\D16\Y2015D16-input.txt", Encoding.UTF8);
        _sues = lines.Select(l => PropertyValueRegex().Matches(l).ToDictionary(m => m.Groups[1].Value, m => int.Parse(m.Groups[2].Value))).ToList();
    }

    [Fact]
    public void PartOne()
    {
        var output = _sues.FindIndex(p => p.All(kv => kv.Value == Target[kv.Key])) + 1;
        output.Should().Be(213);
    }

    [Fact]
    public void PartTwo()
    {
        var output = _sues.FindIndex(p => p.All(MatchesRule)) + 1;
        output.Should().Be(323);
    }

    private static bool MatchesRule(KeyValuePair<string, int> kv) => kv.Key switch
    {
        "cats" => kv.Value > Target[kv.Key],
        "trees" => kv.Value > Target[kv.Key],
        "pomeranians" => kv.Value < Target[kv.Key],
        "goldfish" => kv.Value < Target[kv.Key],
        _ => kv.Value == Target[kv.Key]
    };

    [GeneratedRegex(@"(\w+): (\d+)")]
    private static partial Regex PropertyValueRegex();
}