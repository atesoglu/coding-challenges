using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D04;

[ChallengeName("High-Entropy Passphrases")]
public class Y2017D04
{
    private readonly string _input = File.ReadAllText(@"Y2017\D04\Y2017D04-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = ValidLineCount(_input, word => word);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = ValidLineCount(_input, word => string.Concat(word.OrderBy(ch => ch)));

        output.Should().Be(0);
    }


    int ValidLineCount(string lines, Func<string, string> normalizer) =>
        lines.Split('\n').Where(line => IsValidLine(line.Split(' '), normalizer)).Count();

    bool IsValidLine(string[] words, Func<string, string> normalizer) =>
        words.Select(normalizer).Distinct().Count() == words.Count();
}