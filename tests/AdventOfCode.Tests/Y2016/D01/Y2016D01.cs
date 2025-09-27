using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D01;

[ChallengeName("No Time for a Taxicab")]
public class Y2016D01
{
    private readonly string _input = File.ReadAllText(@"Y2016\D01\Y2016D01-input.txt");

    [Fact]
    public void PartOne()
    {
        var (x, y) = FollowInstructions(_input).Last();
        var distance = Math.Abs(x) + Math.Abs(y);

        distance.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var visitedLocations = new HashSet<(int, int)>();
        var distance = 0;

        foreach (var location in FollowInstructions(_input))
        {
            if (!visitedLocations.Add(location))
            {
                distance = Math.Abs(location.Item1) + Math.Abs(location.Item2);
                break;
            }
        }

        distance.Should().Be(0);
    }

    private IEnumerable<(int x, int y)> FollowInstructions(string input)
    {
        var (x, y) = (0, 0);
        var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) }; // N, E, S, W
        var facing = 0; // start facing north
        yield return (x, y);

        foreach (var instruction in input.Split(", "))
        {
            facing = (facing + (instruction[0] == 'R' ? 1 : 3)) % 4;
            var distance = int.Parse(instruction[1..]);

            for (var i = 0; i < distance; i++)
            {
                x += directions[facing].Item1;
                y += directions[facing].Item2;
                yield return (x, y);
            }
        }
    }
}