using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D14;

[ChallengeName("Restroom Redoubt")]
public class Y2024D14
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D14\Y2024D14-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = SimulateRobots(_lines)
            .ElementAt(100)
            .CountBy(GetQuadrant)
            .Where(group => group.Key.x != 0 && group.Key.y != 0)
            .Aggregate(1, (acc, group) => acc * group.Value);

        output.Should().Be(229421808);
    }

    [Fact]
    public void PartTwo()
    {
        var output = SimulateRobots(_lines)
            .TakeWhile(robots => !PlotRobots(robots).Contains("#################"))
            .Count();

        output.Should().Be(6577);
    }


    private const int width = 101;
    private const int height = 103;


    private IEnumerable<Robot[]> SimulateRobots(IEnumerable<string> lines)
    {
        var robots = ParseRobots(lines).ToArray();
        while (true)
        {
            yield return robots;
            robots = robots.Select(AdvanceRobot).ToArray();
        }
    }

    private Robot AdvanceRobot(Robot robot) => robot with { pos = AddWithWrapAround(robot.pos, robot.vel) };

    private Vec2 GetQuadrant(Robot robot) =>
        new Vec2(Math.Sign(robot.pos.x - width / 2), Math.Sign(robot.pos.y - height / 2));

    private static Vec2 AddWithWrapAround(Vec2 position, Vec2 velocity) =>
        new Vec2((position.x + velocity.x + width) % width, (position.y + velocity.y + height) % height);

    private static string PlotRobots(IEnumerable<Robot> robots)
    {
        var grid = new char[height, width];
        foreach (var robot in robots)
        {
            grid[robot.pos.y, robot.pos.x] = '#';
        }

        var result = new StringBuilder();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                result.Append(grid[y, x] == '#' ? "#" : " ");
            }

            result.AppendLine();
        }

        return result.ToString();
    }

    private static IEnumerable<Robot> ParseRobots(IEnumerable<string> lines) =>
        from line in lines
        let numbers = Regex.Matches(line, @"-?\d+").Select(match => int.Parse(match.Value)).ToArray()
        select new Robot(new Vec2(numbers[0], numbers[1]), new Vec2(numbers[2], numbers[3]));
}

internal record struct Vec2(int x, int y);

internal record struct Robot(Vec2 pos, Vec2 vel);