using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D02;

[ChallengeName("Dive!")]
public class Y2021D02
{
    private readonly string _input = File.ReadAllText(@"Y2021\D02\Y2021D02-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Parse(_input)
            .Aggregate(
                new State1(0, 0),
                (state, step) => step.dir switch
                {
                    'f' => state with { x = state.x + step.amount },
                    'u' => state with { y = state.y - step.amount },
                    'd' => state with { y = state.y + step.amount },
                    _ => throw new Exception(),
                },
                res => res.x * res.y
            );

        output.Should().Be(2102357);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Parse(_input)
            .Aggregate(
                new State2(0, 0, 0),
                (state, step) => step.dir switch
                {
                    'f' => state with
                    {
                        x = state.x + step.amount,
                        y = state.y + step.amount * state.aim
                    },
                    'u' => state with { aim = state.aim - step.amount },
                    'd' => state with { aim = state.aim + step.amount },
                    _ => throw new Exception(),
                },
                res => res.x * res.y
            );

        output.Should().Be(2101031224);
    }

    private IEnumerable<Input> Parse(string st) =>
        from
            line in st.Split('\n')
        let parts = line.Split()
        select
            new Input(parts[0][0], int.Parse(parts[1]));
}

internal record Input(char dir, int amount);

internal record State1(int x, int y);

internal record State2(int x, int y, int aim);