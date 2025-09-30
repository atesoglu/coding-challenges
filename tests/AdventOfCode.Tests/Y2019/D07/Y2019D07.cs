using System.Text;
using AdventOfCode.Tests.Y2019.D02;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D07;

[ChallengeName("Amplification Circuit")]
public class Y2019D07
{
    private readonly string _input = File.ReadAllText(@"Y2019\D07\Y2019D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, false, new[] { 0, 1, 2, 3, 4 });

        output.Should().Be(21760);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, true, new[] { 5, 6, 7, 8, 9 });

        output.Should().Be(69816958);
    }

    private long Solve(string prg, bool loop, int[] prgids)
    {
        var amps = Enumerable.Range(0, 5).Select(x => new IntCodeMachine(prg)).ToArray();
        var max = 0L;

        foreach (var perm in Permutations(prgids))
        {
            max = Math.Max(max, ExecAmps(amps, perm, loop));
        }

        return max;
    }

    private long ExecAmps(IntCodeMachine[] amps, int[] prgid, bool loop)
    {
        for (var i = 0; i < amps.Length; i++)
        {
            amps[i].Reset();
            amps[i].input.Enqueue(prgid[i]);
        }

        var data = new[] { 0L };

        while (true)
        {
            for (var i = 0; i < amps.Length; i++)
            {
                data = amps[i].Run(data).ToArray();
            }

            if (amps.All(amp => amp.Halted()))
            {
                return data.Last();
            }

            if (!loop)
            {
                data = new long[0];
            }
        }
    }

    private IEnumerable<T[]> Permutations<T>(T[] rgt)
    {
        IEnumerable<T[]> PermutationsRec(int i)
        {
            if (i == rgt.Length)
            {
                yield return rgt.ToArray();
            }

            for (var j = i; j < rgt.Length; j++)
            {
                (rgt[i], rgt[j]) = (rgt[j], rgt[i]);
                foreach (var perm in PermutationsRec(i + 1))
                {
                    yield return perm;
                }

                (rgt[i], rgt[j]) = (rgt[j], rgt[i]);
            }
        }

        return PermutationsRec(0);
    }
}