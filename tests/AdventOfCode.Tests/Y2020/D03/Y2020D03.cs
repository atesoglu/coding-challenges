using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2020.D03;

[ChallengeName("Toboggan Trajectory")]
public class Y2020D03
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2020\D03\Y2020D03-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = TreeCount((1, 3));

        output.Should().Be(209);
    }

    [Fact]
    public void PartTwo()
    {
        var output = TreeCount((1, 1), (1, 3), (1, 5), (1, 7), (2, 1));

        output.Should().Be(1574890240);
    }


    private long TreeCount(params (int drow, int dcol)[] slopes)
    {
        var (rowCount, columnCount) = (_lines.Length, _lines[0].Length);
        var mul = 1L;

        foreach (var (drow, dcol) in slopes)
        {
            var (irow, icol) = (drow, dcol);
            var trees = 0;
            while (irow < rowCount)
            {
                if (_lines[irow][icol % columnCount] == '#')
                {
                    trees++;
                }

                (irow, icol) = (irow + drow, icol + dcol);
            }

            mul *= trees;
        }

        return mul;
    }
}