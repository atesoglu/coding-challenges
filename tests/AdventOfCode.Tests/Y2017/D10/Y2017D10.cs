using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D10;

[ChallengeName("Knot Hash")]
public class Y2017D10
{
    private readonly string _input = File.ReadAllText(@"Y2017\D10\Y2017D10-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var chars = _input.Split(',').Select(int.Parse);
        var hash = KnotHash(chars, 1);

        var output = hash[0] * hash[1];

        output.Should().Be(52070);
    }

    [Fact]
    public void PartTwo()
    {
        var suffix = new[] { 17, 31, 73, 47, 23 };
        var chars = _input.ToCharArray().Select(b => (int)b).Concat(suffix);

        var hash = KnotHash(chars, 64);

        var output = string.Join("",
            from blockIdx in Enumerable.Range(0, 16)
            let block = hash.Skip(16 * blockIdx).Take(16)
            select block.Aggregate(0, (acc, ch) => acc ^ ch).ToString("x2"));

        output.Should().Be("7f94112db4e32e19cf6502073c66f9bb");
    }

    private static int[] KnotHash(IEnumerable<int> input, int rounds)
    {
        var output = Enumerable.Range(0, 256).ToArray();

        var current = 0;
        var skip = 0;
        for (var round = 0; round < rounds; round++)
        {
            foreach (var len in input)
            {
                for (var i = 0; i < len / 2; i++)
                {
                    var from = (current + i) % output.Length;
                    var to = (current + len - 1 - i) % output.Length;
                    (output[from], output[to]) = (output[to], output[from]);
                }

                current += len + skip;
                skip++;
            }
        }

        return output;
    }
}