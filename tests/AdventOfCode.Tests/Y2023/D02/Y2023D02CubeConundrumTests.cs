using System.Text;
using FluentAssertions;
using static AdventOfCode.Tests.Y2023.D02.CubeColors;

namespace AdventOfCode.Tests.Y2023.D02;

public class Y2023D02CubeConundrumTests
{
    private readonly Game[] _games = File.ReadAllLines(@"Y2023\D02\Y2023D02CubeConundrum-input.txt", Encoding.UTF8).Select(line =>
        {
            var parts = line.Split(":");
            var gameId = int.Parse(parts[0].Split(" ")[1]);

            var sets = parts[1]
                .Split(";", StringSplitOptions.RemoveEmptyEntries)
                .Select(set => Set(
                    set.Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(cube =>
                        {
                            var cubeInfo = cube.Trim().Split(" ");
                            var count = int.Parse(cubeInfo[0]);
                            Enum.TryParse(cubeInfo[1], true, out CubeColors color);
                            return (color, count);
                        })
                        .ToArray()
                ))
                .ToArray();

            return G(gameId, sets);
        })
        .ToArray();

    private readonly Game[] _sampleData =
    [
        G(1, Set((Blue, 3), (Red, 4)), Set((Red, 1), (Green, 2)), Set((Blue, 6), (Green, 2))),
        G(2, Set((Blue, 1), (Green, 2)), Set((Green, 3), (Blue, 4), (Red, 1)), Set((Green, 1), (Blue, 1))),
        G(3, Set((Green, 8), (Blue, 6), (Red, 20)), Set((Blue, 5), (Red, 4), (Green, 13)), Set((Green, 5), (Red, 1))),
        G(4, Set((Green, 1), (Red, 3), (Blue, 6)), Set((Green, 3), (Red, 6)), Set((Green, 3), (Blue, 15), (Red, 14))),
        G(5, Set((Red, 6), (Blue, 1), (Green, 3)), Set((Blue, 2), (Red, 1), (Green, 2)))
    ];

    [Fact]
    public void PartOneWithSampleData()
    {
        var output = Y2023D02CubeConundrum.PartOne(_sampleData, 12, 13, 14);

        output.Should().Be(8);
    }

    [Fact]
    public void PartTwoWithSampleData()
    {
        var output = Y2023D02CubeConundrum.PartTwo(_sampleData);

        output.Should().Be(2286);
    }

    [Fact]
    public void PartOneWithRealInput()
    {
        var output = Y2023D02CubeConundrum.PartOne(_games, 12, 13, 14);

        output.Should().Be(2149);
    }

    [Fact]
    public void PartTwoWithRealInput()
    {
        var output = Y2023D02CubeConundrum.PartTwo(_games);

        output.Should().Be(71274);
    }

    private static Game G(int id, params GameSet[] sets) => new(id, sets.ToList());
    private static GameSet Set(params (CubeColors color, int count)[] cubes) => new() { Cubes = cubes.Select(c => new CubeInfo(c.color, c.count)).ToList() };
}