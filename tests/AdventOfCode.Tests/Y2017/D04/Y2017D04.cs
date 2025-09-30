using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D04;

[ChallengeName("High-Entropy Passphrases")]
public class Y2017D04
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2017\D04\Y2017D04-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = ValidLineCount(word => word);

        output.Should().Be(451);
    }

    [Fact]
    public void PartTwo()
    {
        var output = ValidLineCount(word => string.Concat(word.OrderBy(ch => ch)));

        output.Should().Be(223);
    }


    private int ValidLineCount(Func<string, string> normalizer) => _lines.Count(line => IsValidLine(line.Split(' '), normalizer));

    private static bool IsValidLine(string[] words, Func<string, string> normalizer) => words.Select(normalizer).Distinct().Count() == words.Length;
}