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
        var output = MatchCounts(_input).Count(c => c != 0);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = MatchCounts(_input).Sum();

        output.Should().Be(0);
    }


    IEnumerable<long> MatchCounts(string input)
    {
        var blocks = input.Split("\n\n");
        var towels = blocks[0].Split(", ");
        return
            from pattern in blocks[1].Split("\n")
            select MatchCount(towels, pattern, new Cache());
    }

    // computes the number of ways the pattern can be build up from the towels. 
    // works recursively by matching the prefix of the pattern with each towel.
    // a full match is found when the pattern becomes empty. the cache is applied 
    // to _dramatically_ speed-up execution
    long MatchCount(string[] towels, string pattern, Cache cache) =>
        cache.GetOrAdd(pattern, (pattern) =>
            pattern switch
            {
                "" => 1,
                _ => towels
                    .Where(pattern.StartsWith)
                    .Sum(towel => MatchCount(towels, pattern[towel.Length ..], cache))
            }
        );
}