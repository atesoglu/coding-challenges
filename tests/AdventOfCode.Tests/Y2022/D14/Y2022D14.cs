using System.Numerics;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D14;

[ChallengeName("Regolith Reservoir")]
public class Y2022D14
{
    private readonly string _input = File.ReadAllText(@"Y2022\D14\Y2022D14-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = new Cave(_input, hasFloor: false).FillWithSand(new Complex(500, 0));

        output.Should().Be(961);
    }

    [Fact]
    public void PartTwo()
    {
        var output = new Cave(_input, hasFloor: true).FillWithSand(new Complex(500, 0));

        output.Should().Be(26375);
    }
}

internal class Cave
{
    private bool hasFloor;

    private Dictionary<Complex, char> map;
    private int maxImaginary;

    public Cave(string input, bool hasFloor)
    {
        this.hasFloor = hasFloor;
        map = new Dictionary<Complex, char>();

        foreach (var line in input.Split("\n"))
        {
            var steps = (
                from step in line.Split(" -> ")
                let parts = step.Split(",")
                select new Complex(int.Parse(parts[0]), int.Parse(parts[1]))
            ).ToArray();

            for (var i = 1; i < steps.Length; i++)
            {
                FillWithRocks(steps[i - 1], steps[i]);
            }
        }

        maxImaginary = (int)map.Keys.Select(pos => pos.Imaginary).Max();
    }

    // Adds a line of rocks to the cave
    public int FillWithRocks(Complex from, Complex to)
    {
        var dir = new Complex(
            Math.Sign(to.Real - from.Real),
            Math.Sign(to.Imaginary - from.Imaginary)
        );

        var steps = 0;
        for (var pos = from; pos != to + dir; pos += dir)
        {
            map[pos] = '#';
            steps++;
        }

        return steps;
    }

    // Sand flows into the cave from the source location, returns the amount of sand added.
    public int FillWithSand(Complex sandSource)
    {
        while (true)
        {
            var location = SimulateFallingSand(sandSource);

            // already has sand there
            if (map.ContainsKey(location))
            {
                break;
            }

            // flows out into the void
            if (!hasFloor && location.Imaginary == maxImaginary + 1)
            {
                break;
            }

            map[location] = 'o';
        }

        return map.Values.Count(x => x == 'o');
    }

    // Returns the final location of a falling unit of sand following the rules of cave physics
    private Complex SimulateFallingSand(Complex sand)
    {
        var down = new Complex(0, 1);
        var left = new Complex(-1, 1);
        var right = new Complex(1, 1);

        while (sand.Imaginary < maxImaginary + 1)
        {
            if (!map.ContainsKey(sand + down))
            {
                sand += down;
            }
            else if (!map.ContainsKey(sand + left))
            {
                sand += left;
            }
            else if (!map.ContainsKey(sand + right))
            {
                sand += right;
            }
            else
            {
                break;
            }
        }

        return sand;
    }
}