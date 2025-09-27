using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D13;

[ChallengeName("A Maze of Twisty Little Cubicles")]
public class Y2016D13
{
    private readonly string _input = File.ReadAllText(@"Y2016\D13\Y2016D13-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Steps(int.Parse(_input))
            .First(s => s.icol == 31 && s.irow == 39)
            .steps;

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Steps(int.Parse(_input))
            .TakeWhile(s => s.steps <= 50)
            .Count();

        output.Should().Be(0);
    }

    IEnumerable<(int steps, int irow, int icol)> Steps(int input)
    {
        var q = new Queue<(int steps, int irow, int icol)>();
        q.Enqueue((0, 1, 1));
        var seen = new HashSet<(int, int)>();
        seen.Add((1, 1));
        var n = (
            from drow in new[] { -1, 0, 1 }
            from dcol in new[] { -1, 0, 1 }
            where Math.Abs(drow) + Math.Abs(dcol) == 1
            select (drow, dcol)
        ).ToArray();

        while (q.Any())
        {
            var (steps, irow, icol) = q.Dequeue();

            yield return (steps, irow, icol);

            foreach (var (drow, dcol) in n)
            {
                var (irowT, icolT) = (irow + drow, icol + dcol);
                if (irowT >= 0 && icolT >= 0)
                {
                    var w = icolT * icolT + 3 * icolT + 2 * icolT * irowT + irowT + irowT * irowT + input;
                    if (Convert.ToString(w, 2).Count(ch => ch == '1') % 2 == 0)
                    {
                        if (!seen.Contains((irowT, icolT)))
                        {
                            q.Enqueue((steps + 1, irowT, icolT));
                            seen.Add((irowT, icolT));
                        }
                    }
                }
            }
        }
    }
}