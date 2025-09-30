using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D22;

[ChallengeName("Sporifica Virus")]
public class Y2017D22
{
    private readonly string _input = File.ReadAllText(@"Y2017\D22\Y2017D22-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Iterate(_input, 10000,
            (state, drow, dcol) =>
                state switch
                {
                    State.Clean => (State.Infected, -dcol, drow),
                    State.Infected => (State.Clean, dcol, -drow),
                    _ => throw new ArgumentException()
                }
        );

        output.Should().Be(5339);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Iterate(_input, 10000000,
            (state, drow, dcol) =>
                state switch
                {
                    State.Clean => (State.Weakened, -dcol, drow),
                    State.Weakened => (State.Infected, drow, dcol),
                    State.Infected => (State.Flagged, dcol, -drow),
                    State.Flagged => (State.Clean, -drow, -dcol),
                    _ => throw new ArgumentException()
                }
        );

        output.Should().Be(2512380);
    }

    private int Iterate(string input, int iterations, Func<State, int, int, (State State, int irow, int icol)> update)
    {
        var lines = input.Split('\n');
        var crow = lines.Length;
        var ccol = lines[0].Length;
        var cells = new Dictionary<(int irow, int icol), State>();
        for (var irowT = 0; irowT < crow; irowT++)
        {
            for (var icolT = 0; icolT < ccol; icolT++)
            {
                if (lines[irowT][icolT] == '#')
                {
                    cells.Add((irowT, icolT), State.Infected);
                }
            }
        }

        var (irow, icol) = (crow / 2, ccol / 2);
        var (drow, dcol) = (-1, 0);
        var infections = 0;
        for (var i = 0; i < iterations; i++)
        {
            var state = cells.TryGetValue((irow, icol), out var s) ? s : State.Clean;

            (state, drow, dcol) = update(state, drow, dcol);

            if (state == State.Infected)
            {
                infections++;
            }

            if (state == State.Clean)
            {
                cells.Remove((irow, icol));
            }
            else
            {
                cells[(irow, icol)] = state;
            }

            (irow, icol) = (irow + drow, icol + dcol);
        }

        return infections;
    }

    private enum State
    {
        Clean,
        Weakened,
        Infected,
        Flagged
    }
}