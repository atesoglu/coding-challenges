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
            .First(s => s.col == 31 && s.row == 39)
            .steps;

        output.Should().Be(86);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Steps(int.Parse(_input))
            .Where(s => s.steps <= 50)
            .Count();

        output.Should().Be(127);
    }

    IEnumerable<(int steps, int row, int col)> Steps(int input)
    {
        var q = new Queue<(int steps, int row, int col)>();
        var seen = new HashSet<(int, int)>();

        q.Enqueue((0, 1, 1));
        seen.Add((1, 1));

        var directions = new[] { (-1,0), (1,0), (0,-1), (0,1) };

        while (q.Count > 0)
        {
            var (steps, row, col) = q.Dequeue();
            yield return (steps, row, col);

            foreach (var (dr, dc) in directions)
            {
                int r2 = row + dr, c2 = col + dc;
                if (r2 < 0 || c2 < 0) continue;

                var w = (long)c2 * c2 + 3L * c2 + 2L * c2 * r2 + r2 + (long)r2 * r2 + input;
                var isOpen = CountBits(w) % 2 == 0;

                if (isOpen && seen.Add((r2, c2)))
                {
                    q.Enqueue((steps + 1, r2, c2));
                }
            }
        }
    }

    int CountBits(long n)
    {
        var count = 0;
        while (n != 0)
        {
            count += (int)(n & 1);
            n >>= 1;
        }
        return count;
    }
}