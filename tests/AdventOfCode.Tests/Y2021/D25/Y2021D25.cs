using System.Text;
using FluentAssertions;
using System.Collections.Generic;

namespace AdventOfCode.Tests.Y2021.D25;

[ChallengeName("Sea Cucumber")]
public class Y2021D25
{
    private readonly string _input = File.ReadAllText(@"Y2021\D25\Y2021D25-input.txt", Encoding.UTF8);

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


    private object PartOne(string input)
    {
        var map = input.Split('\n');
        var (ccol, crow) = (map[0].Length, map.Length);

        int right(int icol) => (icol + 1) % ccol;
        int left(int icol) => (icol - 1 + ccol) % ccol;
        int up(int irow) => (irow - 1 + crow) % crow;
        int down(int irow) => (irow + 1) % crow;

        bool movesRight(int irow, int icol) =>
            map[irow][icol] == '>' && map[irow][right(icol)] == '.';

        bool movesDown(int irow, int icol) =>
            map[irow][icol] == 'v' && map[down(irow)][icol] == '.';

        for (var steps = 1;; steps++)
        {
            var anyMoves = false;

            var newMap = new List<string>();
            for (var irow = 0; irow < crow; irow++)
            {
                var st = "";
                for (var icol = 0; icol < ccol; icol++)
                {
                    anyMoves |= movesRight(irow, icol);
                    st +=
                        movesRight(irow, icol) ? '.' :
                        movesRight(irow, left(icol)) ? '>' :
                        map[irow][icol];
                }

                newMap.Add(st);
            }

            map = newMap.ToArray();
            newMap.Clear();

            for (var irow = 0; irow < crow; irow++)
            {
                var st = "";
                for (var icol = 0; icol < ccol; icol++)
                {
                    anyMoves |= movesDown(irow, icol);
                    st +=
                        movesDown(irow, icol) ? '.' :
                        movesDown(up(irow), icol) ? 'v' :
                        map[irow][icol];
                }

                newMap.Add(st);
            }

            map = newMap.ToArray();

            if (!anyMoves)
            {
                return steps;
            }
        }
    }
}