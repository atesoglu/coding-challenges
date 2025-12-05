using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D05;

[ChallengeName("Cafeteria")]
public class Y2025D05
{
    private readonly string _input = File.ReadAllText(@"Y2025\D05\Y2025D05-input.txt", Encoding.UTF8);
    private readonly List<Range> _ranges;
    private readonly long[] _numbers;

    public Y2025D05()
    {
        (_ranges, _numbers) = Parse(_input);
    }

    [Fact]
    public void PartOne()
    {
        var output = _numbers.Count(n => _ranges.Any(r => r.Contains(n)));

        output.Should().Be(529);
    }

    [Fact]
    public void PartTwo()
    {
        // normalize & merge all ranges
        var merged = MergeRanges(_ranges);

        // sum the lengths
        var output = merged.Sum(r => r.Length);

        output.Should().Be(344260049617193);
    }

    // -----------------------------
    // Parsing
    // -----------------------------

    private static (List<Range> ranges, long[] numbers) Parse(string input)
    {
        var blocks = input.Split("\n\n");

        var ranges = blocks[0]
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                var parts = line.Split('-');
                return new Range(long.Parse(parts[0]), long.Parse(parts[1]));
            })
            .ToList();

        var numbers = blocks[1]
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        return (ranges, numbers);
    }

    // -----------------------------
    // Merge Logic (Part Two)
    // -----------------------------

    private static List<Range> MergeRanges(IEnumerable<Range> ranges)
    {
        var sorted = ranges.OrderBy(r => r.Start).ToList();
        var result = new List<Range>();

        foreach (var range in sorted)
        {
            if (result.Count == 0)
            {
                result.Add(range);
                continue;
            }

            var last = result[^1];

            if (range.Start <= last.End)
            {
                // overlap -> extend last
                result[^1] = new Range(last.Start, Math.Max(last.End, range.End));
            }
            else
            {
                // gap -> add new range
                result.Add(range);
            }
        }

        return result;
    }

    private record Range(long Start, long End)
    {
        public bool Contains(long value) => Start <= value && value <= End;

        public long Length => End - Start + 1;
    }
}