using System.Text;
using FluentAssertions;
using System;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Tests.Y2020.D12;

[ChallengeName("Rain Risk")]
public class Y2020D12
{
    private readonly string _input = File.ReadAllText(@"Y2020\D12\Y2020D12-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = MoveShip(_input, true);

        output.Should().Be(2879);
    }

    [Fact]
    public void PartTwo()
    {
        var output = MoveShip(_input, false);

        output.Should().Be(178986);
    }

    double MoveShip(string input, bool part1) =>
        input
            .Split("\n")
            .Select(line => (line[0], int.Parse(line.Substring(1))))
            .Aggregate(
                new State(pos: Complex.Zero, dir: part1 ? Complex.One : new Complex(10, 1)),
                (state, line) =>
                    line switch
                    {
                        ('N', var arg) when part1 => state with { pos = state.pos + arg * Complex.ImaginaryOne },
                        ('N', var arg) => state with { dir = state.dir + arg * Complex.ImaginaryOne },
                        ('S', var arg) when part1 => state with { pos = state.pos - arg * Complex.ImaginaryOne },
                        ('S', var arg) => state with { dir = state.dir - arg * Complex.ImaginaryOne },
                        ('E', var arg) when part1 => state with { pos = state.pos + arg },
                        ('E', var arg) => state with { dir = state.dir + arg },
                        ('W', var arg) when part1 => state with { pos = state.pos - arg },
                        ('W', var arg) => state with { dir = state.dir - arg },
                        ('F', var arg) => state with { pos = state.pos + arg * state.dir },
                        ('L', 90) or ('R', 270) => state with { dir = state.dir * Complex.ImaginaryOne },
                        ('L', 270) or ('R', 90) => state with { dir = -state.dir * Complex.ImaginaryOne },
                        ('L', 180) or ('R', 180) => state with { dir = -state.dir },
                        _ => throw new Exception()
                    },
                state => Math.Abs(state.pos.Imaginary) + Math.Abs(state.pos.Real));
}
record State(Complex pos, Complex dir);