using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2019.D01;

[ChallengeName("The Tyranny of the Rocket Equation")]
public class Y2019D01
{
    private readonly string _input = File.ReadAllText(@"Y2019\D01\Y2019D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = PartOne(_input);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = PartTwo(_input);

        output.Should().Be(0);
    }


    private object PartOne(string input) => Solve(input, false);
    private object PartTwo(string input) => Solve(input, true);

    int Solve(string input, bool recursive)
    {
        var weights = new Queue<int>(input.Split("\n").Select(x => int.Parse(x)));
        var res = 0;
        while (weights.Any())
        {
            var weight = weights.Dequeue();
            var fuel = (int)(Math.Floor(weight / 3.0) - 2);
            if (fuel > 0)
            {
                if (recursive)
                {
                    weights.Enqueue(fuel);
                }

                res += fuel;
            }
        }

        return res;
    }
}