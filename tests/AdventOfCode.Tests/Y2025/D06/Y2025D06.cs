using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D06;

[ChallengeName("Trash Compactor")]
public class Y2025D06
{
    private readonly string _input = File.ReadAllText(@"Y2025/D06/Y2025D06-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var problems = ParseBlocks(_input).Select(block => new Problem(block[^1][0], block[..^1].Select(long.Parse)));

        Solve(problems).Should().Be(4412382293768);
    }

    [Fact]
    public void PartTwo()
    {
        var problems = ParseBlocks(_input).Select(Transpose).Select(cols => new Problem(cols[0][^1], cols.Select(c => long.Parse(c[..^1]))));

        Solve(problems).Should().Be(7858808482092);
    }

    private static long Solve(IEnumerable<Problem> problems) => problems.Sum(p => p.Op == '+' ? p.Numbers.Sum() : p.Numbers.Aggregate(1L, (a, b) => a * b));

    private static IEnumerable<string[]> ParseBlocks(string input)
    {
        var lines = NormalizeLines(input);
        var width = lines[0].Length;

        var from = 0;

        for (var x = 0; x < width; x++)
        {
            if (IsBlankColumn(lines, x))
            {
                yield return Slice(lines, from, x);
                from = x + 1;
            }
        }

        yield return Slice(lines, from, width);
    }

    private static string[] NormalizeLines(string input)
    {
        var lines = input.Split('\n').Select(l => l.TrimEnd('\r')).ToArray();

        var max = lines.Max(l => l.Length);

        for (var i = 0; i < lines.Length; i++)
            lines[i] = lines[i].PadRight(max);

        return lines;
    }

    private static bool IsBlankColumn(string[] lines, int x) => lines.All(line => line[x] == ' ');

    private static string[] Slice(string[] lines, int from, int to)
    {
        var block = new string[lines.Length];
        var width = to - from;

        for (var i = 0; i < lines.Length; i++)
            block[i] = lines[i].Substring(from, width);

        return block;
    }

    private static string[] Transpose(string[] block)
    {
        var height = block.Length;
        var width = block[0].Length;

        var result = new string[width];
        var sb = new StringBuilder(height);

        for (var x = 0; x < width; x++)
        {
            sb.Clear();
            for (var y = 0; y < height; y++)
                sb.Append(block[y][x]);
            result[x] = sb.ToString();
        }

        return result;
    }

    private record Problem(char Op, IEnumerable<long> Numbers);
}