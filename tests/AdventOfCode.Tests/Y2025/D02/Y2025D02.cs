using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D02;

[ChallengeName("Gift Shop")]
public class Y2025D02
{
    private readonly string _input = File.ReadAllText(@"Y2025\D02\Y2025D02-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, st => 2);

        output.Should().Be(12850231731);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, st => st.Length);

        output.Should().Be(24774350322);
    }

    private long Solve(string input, Func<string, int> maxRepetition)
    {
        long sum = 0;

        foreach (var id in GetIds(input))
        {
            var s = id.ToString();
            var maxRep = maxRepetition(s);

            // Check repetition counts from 2 up to maxRep
            for (var k = 2; k <= maxRep; k++)
            {
                if (IsPeriodic(s, k))
                {
                    sum += id;
                    break; // no need to check more repetitions for this id
                }
            }
        }

        return sum;
    }

    private static IEnumerable<long> GetIds(string input)
    {
        var ranges = input.Split(",");

        foreach (var range in ranges)
        {
            var parts = range.Split("-");
            var start = long.Parse(parts[0]);
            var end = long.Parse(parts[1]);

            for (var i = start; i <= end; i++)
                yield return i;
        }
    }

    /// <summary>
    /// Returns true if the string can be divided into 'count' identical repeating parts.
    /// Example: "121121121" with count=3 → true.
    /// </summary>
    private static bool IsPeriodic(string text, int count)
    {
        // Must divide evenly
        if (text.Length % count != 0)
            return false;

        var chunkSize = text.Length / count;
        var chunk = text.Substring(0, chunkSize);

        for (var i = chunkSize; i < text.Length; i += chunkSize)
        {
            if (text.Substring(i, chunkSize) != chunk)
                return false;
        }

        return true;
    }
}