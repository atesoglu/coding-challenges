using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D08;

[ChallengeName("Two-Factor Authentication")]
public class Y2016D08
{
    private readonly string _input = File.ReadAllText(@"Y2016\D08\Y2016D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var mtx = Execute(_input);
        var output = (
            from irow in Enumerable.Range(0, mtx.GetLength(0))
            from icol in Enumerable.Range(0, mtx.GetLength(1))
            where mtx[irow, icol]
            select 1
        ).Count();

        output.Should().Be(110);
    }

    [Fact]
    public void PartTwo()
    {
        var mtx = Execute(_input);
        var res = "";
        foreach (var irow in Enumerable.Range(0, mtx.GetLength(0)))
        {
            foreach (var icol in Enumerable.Range(0, mtx.GetLength(1)))
            {
                res += mtx[irow, icol] ? "#" : " ";
            }

            res += "\n";
        }

        var output = res.ToScreenText().ToString();

        output.Should().Be("ZJHRKCPLYJ");
    }

    private bool[,] Execute(string input)
    {
        var (crow, ccol) = (6, 50);
        var mtx = new bool[crow, ccol];
        foreach (var line in input.Split('\n'))
        {
            if (Match(line, @"rect (\d+)x(\d+)", out var m))
            {
                var (ccolT, crowT) = (int.Parse(m[0]), int.Parse(m[1]));
                for (var irow = 0; irow < crowT; irow++)
                {
                    for (var icol = 0; icol < ccolT; icol++)
                    {
                        mtx[irow, icol] = true;
                    }
                }
            }
            else if (Match(line, @"rotate row y=(\d+) by (\d+)", out m))
            {
                var (irow, d) = (int.Parse(m[0]), int.Parse(m[1]));
                for (var i = 0; i < d; i++)
                {
                    var t = mtx[irow, ccol - 1];
                    for (var icol = ccol - 1; icol >= 1; icol--)
                    {
                        mtx[irow, icol] = mtx[irow, icol - 1];
                    }

                    mtx[irow, 0] = t;
                }
            }
            else if (Match(line, @"rotate column x=(\d+) by (\d+)", out m))
            {
                var (icol, d) = (int.Parse(m[0]), int.Parse(m[1]));
                for (var i = 0; i < d; i++)
                {
                    var t = mtx[crow - 1, icol];
                    for (var irow = crow - 1; irow >= 1; irow--)
                    {
                        mtx[irow, icol] = mtx[irow - 1, icol];
                    }

                    mtx[0, icol] = t;
                }
            }
        }

        return mtx;
    }

    private static bool Match(string stm, string pattern, out string[] m)
    {
        var match = Regex.Match(stm, pattern);
        m = null;
        if (match.Success)
        {
            m = match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray();
            return true;
        }
        else
        {
            return false;
        }
    }
}