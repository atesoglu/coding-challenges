using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2020.D03;

[ChallengeName("Toboggan Trajectory")]
public class Y2020D03
{
    private readonly string _input = File.ReadAllText(@"Y2020\D03\Y2020D03-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => TreeCount(input, (1, 3));
    private object PartTwo(string input) => TreeCount(input, (1, 1), (1, 3), (1, 5), (1, 7), (2, 1));

    long TreeCount(string input, params (int drow, int dcol)[] slopes)
    {
        var lines = input.Split("\n");
        var (crow, ccol) = (lines.Length, lines[0].Length);
        var mul = 1L;

        foreach (var (drow, dcol) in slopes)
        {
            var (irow, icol) = (drow, dcol);
            var trees = 0;
            while (irow < crow)
            {
                if (lines[irow][icol % ccol] == '#')
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