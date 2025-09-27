using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D19;

[ChallengeName("A Series of Tubes")]
public class Y2017D19
{
    private readonly string _input = File.ReadAllText(@"Y2017\D19\Y2017D19-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = FollowPath(_input).msg;

        output.Should().Be("0");
    }

    [Fact]
    public void PartTwo()
    {
        var output = FollowPath(_input).steps;

        output.Should().Be(0);
    }

    (string msg, int steps) FollowPath(string input)
    {
        var map = input.Split('\n');
        var (ccol, crow) = (map[0].Length, map.Length);
        var (icol, irow) = (map[0].IndexOf('|'), 0);
        var (dcol, drow) = (0, 1);

        var msg = "";
        var steps = 0;

        while (true)
        {
            irow += drow;
            icol += dcol;
            steps++;

            if (icol < 0 || icol >= ccol || irow < 0 || irow >= crow || map[irow][icol] == ' ')
            {
                break;
            }

            switch (map[irow][icol])
            {
                case '+':
                    (dcol, drow) = (
                        from q in new[] { (drow: dcol, dcol: -drow), (drow: -dcol, dcol: drow) }
                        let icolT = icol + q.dcol
                        let irowT = irow + q.drow
                        where icolT >= 0 && icolT < ccol && irowT >= 0 && irowT < crow && map[irowT][icolT] != ' '
                        select (q.dcol, q.drow)
                    ).Single();
                    break;
                case char ch when (ch >= 'A' && ch <= 'Z'):
                    msg += ch;
                    break;
            }
        }

        return (msg, steps);
    }
}