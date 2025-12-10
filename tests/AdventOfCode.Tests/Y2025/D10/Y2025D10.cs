using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D10;

[ChallengeName("Factory")]
public class Y2025D10
{
    private readonly string _input = File.ReadAllText(@"Y2025\D10\Y2025D10-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = 0;
        foreach (var p in Parse(_input))
        {
            var limit = 1 << p.buttons.Length;
            var tries = Enumerable.Range(0, limit).OrderBy(BitCount).ToArray();

            var q = -1;
            foreach (var n in tries)
            {
                if ((Xor(p.buttons, n) ^ p.target) == 0)
                {
                    q = n;
                    break;
                }
            }

            if (q == -1)
            {
                throw new Exception();
            }

            output += BitCount(q);
        }

        output.Should().Be(514);
    }

    [Fact]
    public void PartTwo()
    {

        // This questions was waaaay to hard to solve in a single class so I had to create another one.

        var res = 0L;
        var i = 1;

        foreach (var p in Parse(_input))
        {
            //Debug.WriteLine(i + " " + p.buttons.Length + " " + p.jolts.Length);
            i++;
        }

        res.Should().Be(0);
    }

    bool TooMuch(Problem p, int[] state)
    {
        return Enumerable.Range(0, state.Length).Any(i => state[i] > p.jolts[i]);
    }

    int Xor(int[] buttons, int mask)
    {
        var res = 0;
        var i = 0;
        while (mask != 0)
        {
            if ((mask & 1) != 0)
            {
                res ^= buttons[i];
            }

            mask >>= 1;
            i++;
        }

        return res;
    }

    int Solve(Problem p, int pushes, int[] state)
    {
        if (pushes > 11)
        {
            return int.MaxValue;
        }

        if (Equals(state, p.jolts))
        {
            return pushes;
        }

        if (Enumerable.Range(0, state.Length).Any(i => state[i] > p.jolts[i]))
        {
            return int.MaxValue;
        }

        var res = int.MaxValue;
        for (int i = 0; i < p.buttons.Length; i++)
        {
            var stateT = Push(state, p.buttons[i]);
            var m = Solve(p, pushes + 1, stateT);
            if (m < res)
            {
                res = m;
            }
        }

        return res;
    }

    int[] Push(int[] state, int button)
    {
        var res = state.ToArray();
        for (int i = 0; i < state.Length; i++)
        {
            if ((button & (1 << i)) != 0)
            {
                res[i]++;
            }
        }

        return res;
    }

    int BitCount(int n)
    {
        int res = 0;
        while (n != 0)
        {
            n &= n - 1;
            res++;
        }

        return res;
    }

    // [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
    IEnumerable<Problem> Parse(string input)
    {
        var lines = input.Split("\n");
        foreach (var line in lines)
        {
            var parts = line.Split(" ").ToArray();
            var num = Convert.ToInt32(
                string.Join("",
                    parts.First()
                        .Replace("[", "")
                        .Replace("]", "")
                        .Replace('.', '0')
                        .Replace('#', '1')
                        .Reverse()
                ),
                2);

            var buttons =
                from part in parts[1..^1]
                let digits = Regex.Matches(part, @"\d").Select(m => int.Parse(m.Value))
                let mask = (from d in digits select 1 << d).Sum()
                select mask;

            var jolts =
                parts.Last()
                    .Replace("{", "")
                    .Replace("}", "")
                    .Split(",")
                    .Select(int.Parse);
            ;
            yield return new Problem(num, buttons.ToArray(), jolts.ToArray());
        }
    }

    record Problem(int target, int[] buttons, int[] jolts);
}