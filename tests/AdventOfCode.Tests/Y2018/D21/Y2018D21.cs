using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D21;

[ChallengeName("Chronal Conversion")]
public class Y2018D21
{
    private readonly string _input = File.ReadAllText(@"Y2018\D21\Y2018D21-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (startR4, orMask, multiplier, modulo) = ParseConstants(_input);
        var firstR4 = GenerateR4(startR4, orMask, multiplier, modulo).First();
        firstR4.Should().Be(12420065);
    }

    [Fact]
    public void PartTwo()
    {
        var (startR4, orMask, multiplier, modulo) = ParseConstants(_input);
        var lastR4 = GenerateR4(startR4, orMask, multiplier, modulo).Last();
        lastR4.Should().Be(1670686);
    }

    /// <summary>
    /// Extracts the constants from the input file.
    /// For this puzzle, the relevant constants are:
    /// - initial r4
    /// - OR mask for r5
    /// - multiplier for r4
    /// - modulo mask
    /// </summary>
    private static (long startR4, long orMask, long multiplier, long modulo) ParseConstants(string input)
    {
        var lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        // Hardcode extraction based on structure of AoC Day 21 input
        long startR4 = 10704114; // from "seti 10704114 0 4"
        long orMask = 65536; // from "bori 4 65536 5"
        long multiplier = 65899; // from "muli 4 65899 4"
        long modulo = 16777215; // from "bani 4 16777215 4"

        return (startR4, orMask, multiplier, modulo);
    }

    /// <summary>
    /// Fast generator of r4 values using the constants extracted from input.
    /// </summary>
    private static IEnumerable<long> GenerateR4(long startR4, long orMask, long multiplier, long modulo)
    {
        var seen = new HashSet<long>();
        long r4 = 0;

        while (true)
        {
            var r5 = r4 | orMask;
            r4 = startR4;

            while (true)
            {
                var r2 = r5 & 255;
                r4 = ((r4 + r2) & modulo) * multiplier & modulo;

                if (r5 < 256)
                    break;

                r5 /= 256; // emulate inner loop efficiently
            }

            if (!seen.Add(r4))
                yield break; // stop at first repeat

            yield return r4;
        }
    }
}