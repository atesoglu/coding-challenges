using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D18;

[ChallengeName("RAM Run")]
public class Y2024D18
{
    private readonly string _input = File.ReadAllText(@"Y2024\D18\Y2024D18-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Distance(GetBlocks(_input).Take(1024));

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        // find the first block position that will cut off the goal position
        // we can use a binary search for this

        var blocks = GetBlocks(_input);
        var (lo, hi) = (0, blocks.Length);
        while (hi - lo > 1)
        {
            var m = (lo + hi) / 2;
            if (Distance(blocks.Take(m)) == null)
            {
                hi = m;
            }
            else
            {
                lo = m;
            }
        }

        var output = $"{blocks[lo].Real},{blocks[lo].Imaginary}";

        output.Should().Be("0");
    }

    int? Distance(IEnumerable<Complex> blocks)
    {
        // our standard priority queue based path finding

        var size = 70;
        var (start, goal) = (0, size + size * Complex.ImaginaryOne);
        var blocked = blocks.Concat(start).ToHashSet();

        var q = new PriorityQueue<Complex, int>();
        q.Enqueue(start, 0);
        while (q.TryDequeue(out var pos, out var dist))
        {
            if (pos == goal)
            {
                return dist;
            }

            foreach (var dir in new[] { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne })
            {
                var posT = pos + dir;
                if (!blocked.Contains(posT) &&
                    0 <= posT.Imaginary && posT.Imaginary <= size &&
                    0 <= posT.Real && posT.Real <= size
                   )
                {
                    q.Enqueue(posT, dist + 1);
                    blocked.Add(posT);
                }
            }
        }

        return null;
    }

    Complex[] GetBlocks(string input) => (
        from line in input.Split("\n")
        let nums = Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToArray()
        select nums[0] + nums[1] * Complex.ImaginaryOne
    ).ToArray();
}