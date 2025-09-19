using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2023.D03;

public class Y2023D03GearRatiosTests
{
    private readonly Dictionary<(int x, int y), List<int>> _gears = new Dictionary<(int x, int y), List<int>>();
    private readonly Dictionary<(int x, int y), List<int>> _sampleData = new Dictionary<(int x, int y), List<int>>();

    public Y2023D03GearRatiosTests()
    {
        var lines = File.ReadAllLines(@"Y2023\D03\Y2023D03GearRatios-input.txt", Encoding.UTF8).Select(x => $"{x}.").ToArray();

        PrepareData(lines, _gears);

        lines =
        [
            "467..114..",
            "...*......",
            "..35..633.",
            "......#...",
            "617*......",
            ".....+.58.",
            "..592.....",
            "......755.",
            "...$.*....",
            ".664.598.."
        ];

        PrepareData(lines, _sampleData);
    }

    private static void PrepareData(string[] lines, Dictionary<(int x, int y), List<int>> gears)
    {
        var neighbors = new Dictionary<(int x, int y), (int x, int y)>();

        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '.' || char.IsDigit(lines[y][x]))
                {
                    continue;
                }

                gears[(x, y)] = new List<int>();

                neighbors[(x - 1, y - 1)] = (x, y);
                neighbors[(x, y - 1)] = (x, y);
                neighbors[(x + 1, y - 1)] = (x, y);

                neighbors[(x - 1, y)] = (x, y);
                neighbors[(x + 1, y)] = (x, y);

                neighbors[(x - 1, y + 1)] = (x, y);
                neighbors[(x, y + 1)] = (x, y);
                neighbors[(x + 1, y + 1)] = (x, y);
            }
        }

        for (var y = 0; y < lines.Length; y++)
        {
            var buf = "";
            var adjacentGears = new HashSet<(int x, int y)>();
            for (var x = 0; x < lines[y].Length; x++)
            {
                if (char.IsDigit(lines[y][x]))
                {
                    buf += lines[y][x];
                    if (neighbors.ContainsKey((x, y)))
                    {
                        adjacentGears.Add(neighbors[(x, y)]);
                    }
                }
                else
                {
                    if (buf.Length > 0 && adjacentGears.Count > 0)
                    {
                        var num = int.Parse(buf);
                        foreach (var adjacentGear in adjacentGears)
                        {
                            gears[adjacentGear].Add(num);
                        }
                    }

                    adjacentGears.Clear();
                    buf = "";
                }
            }
        }
    }

    [Fact]
    public void PartOneWithSampleData()
    {
        var output = Y2023D03GearRatios.PartOne(_sampleData);

        output.Should().Be(4361);
    }

    [Fact]
    public void PartTwoWithSampleData()
    {
        var output = Y2023D03GearRatios.PartTwo(_sampleData);

        output.Should().Be(467835);
    }

    [Fact]
    public void PartOneWithRealInput()
    {
        var sum = Y2023D03GearRatios.PartOne(_gears);

        sum.Should().Be(531561);
    }

    [Fact]
    public void PartTwoWithRealInput()
    {
        var sum = Y2023D03GearRatios.PartTwo(_gears);

        sum.Should().Be(83279367);
    }
}