using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D02;

[ChallengeName("Bathroom Security")]
public class Y2016D02
{
    private readonly string _input = File.ReadAllText(@"Y2016\D02\Y2016D02-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, "123\n456\n789");

        output.Should().Be("33444");
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, "  1  \n 234 \n56789\n ABC \n  D  ");

        output.Should().Be("446A6");
    }

    string Solve(string input, string keypad)
    {
        var res = "";
        var lines = keypad.Split('\n');
        var (crow, ccol) = (lines.Length, lines[0].Length);
        var (irow, icol) = (crow / 2, ccol / 2);
        foreach (var line in input.Split('\n'))
        {
            foreach (var ch in line)
            {
                var (drow, dcol) = ch switch
                {
                    'U' => (-1, 0),
                    'D' => (1, 0),
                    'L' => (0, -1),
                    'R' => (0, 1),
                    _ => throw new ArgumentException()
                };

                var (irowT, icolT) = (irow + drow, icol + dcol);
                if (irowT >= 0 && irowT < crow && icolT >= 0 && icolT < ccol && lines[irowT][icolT] != ' ')
                {
                    (irow, icol) = (irowT, icolT);
                }
            }

            res += lines[irow][icol];
        }

        return res;
    }
}