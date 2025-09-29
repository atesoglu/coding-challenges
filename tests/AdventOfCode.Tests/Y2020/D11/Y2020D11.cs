using System.Text;
using FluentAssertions;
using System;
using System.Linq;

namespace AdventOfCode.Tests.Y2020.D11;

[ChallengeName("Seating System")]
public class Y2020D11
{
    private readonly string _input = File.ReadAllText(@"Y2020\D11\Y2020D11-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, 4, _ => true);

        output.Should().Be(2412);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, 5, place => place != '.');

        output.Should().Be(2176);
    }

    int Solve(string input, int occupiedLimit, Func<char, bool> placeToCheck)
    {
        var (crow, ccol) = (input.Split("\n").Length, input.IndexOf('\n'));

        char PlaceInDirection(char[] st, int idx, int drow, int dcol)
        {
            var (irow, icol) = (idx / ccol, idx % ccol);
            while (true)
            {
                (irow, icol) = (irow + drow, icol + dcol);
                var place =
                    irow < 0 || irow >= crow ? 'L' :
                    icol < 0 || icol >= ccol ? 'L' :
                    st[irow * ccol + icol];
                if (placeToCheck(place))
                {
                    return place;
                }
            }
        }

        int OccupiedPlacesAround(char[] st, int idx)
        {
            var directions = new[] { (0, -1), (0, 1), (-1, 0), (1, 0), (-1, -1), (-1, 1), (1, -1), (1, 1) };
            var occupied = 0;
            foreach (var (drow, dcol) in directions)
            {
                if (PlaceInDirection(st, idx, drow, dcol) == '#')
                {
                    occupied++;
                }
            }

            return occupied;
        }

        var prevState = new char[0];
        var state = input.Replace("\n", "").Replace("L", "#").ToArray();
        while (!prevState.SequenceEqual(state))
        {
            prevState = state;
            state = state.Select((place, i) =>
                    place == '#' && OccupiedPlacesAround(state, i) >= occupiedLimit ? 'L' :
                    place == 'L' && OccupiedPlacesAround(state, i) == 0 ? '#' :
                    place /*otherwise*/
            ).ToArray();
        }

        return state.Count(place => place == '#');
    }
}