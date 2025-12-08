using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D07;

[ChallengeName("Laboratories")]
public class Y2025D07
{
    private readonly string _input = File.ReadAllText(@"Y2025\D07\Y2025D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Run(_input).splits;

        output.Should().Be(1555);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Run(_input).timelineCount;

        output.Should().Be(12895232295789);
    }


    private (int splits, long timelineCount) Run(string input)
    {
        var grid = input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.ToCharArray())
            .ToArray();

        var rows = grid.Length;
        var cols = grid[0].Length;

        var current = new long[cols];
        var totalSplits = 0;

        for (var r = 0; r < rows; r++)
        {
            var next = new long[cols];

            for (var c = 0; c < cols; c++)
            {
                var cell = grid[r][c];
                var incoming = current[c];

                switch (cell)
                {
                    case 'S':
                        // A start creates a new timeline here.
                        next[c] = 1;
                        break;

                    case '^':
                        // Only count a split if there's something to split.
                        if (incoming > 0)
                            totalSplits++;

                        // Safely send timelines left and right.
                        if (c > 0) next[c - 1] += incoming;
                        if (c + 1 < cols) next[c + 1] += incoming;
                        break;

                    default:
                        // Normal cell: just pass the timeline downward.
                        next[c] += incoming;
                        break;
                }
            }

            current = next;
        }

        return (totalSplits, current.Sum());
    }
}