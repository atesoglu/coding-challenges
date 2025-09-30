using System.Text;
using FluentAssertions;
using Cache = System.Collections.Concurrent.ConcurrentDictionary<string, long>;

namespace AdventOfCode.Tests.Y2024.D19;

[ChallengeName("Linen Layout")]
public class Y2024D19
{
    private readonly string _input = File.ReadAllText(@"Y2024\D19\Y2024D19-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = CalculateMatchCounts(_input).Count(count => count != 0);

        output.Should().Be(236);
    }

    [Fact]
    public void PartTwo()
    {
        var output = CalculateMatchCounts(_input).Sum();

        output.Should().Be(643685981770598);
    }

    private IEnumerable<long> CalculateMatchCounts(string input)
    {
        // Normalize line endings to just "\n"
        input = input.Replace("\r\n", "\n").TrimEnd();

        var blocks = input.Split("\n\n");
        var towelPieces = blocks[0].Split(", ");
        return
            from pattern in blocks[1].Split("\n")
            select CountPatternMatches(towelPieces, pattern, new Cache());
    }

    private static long CountPatternMatches(string[] towelPieces, string pattern, Cache cache) =>
        cache.GetOrAdd(pattern, (pattern) =>
            pattern switch
            {
                "" => 1,
                _ => towelPieces
                    .Where(pattern.StartsWith)
                    .Sum(towel => CountPatternMatches(towelPieces, pattern[towel.Length..], cache))
            }
        );
}