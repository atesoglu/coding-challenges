using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D22;

[ChallengeName("Slam Shuffle")]
public class Y2019D22
{
    private readonly string _input = File.ReadAllText(@"Y2019\D22\Y2019D22-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var m = 10007;
        var iter = 1;
        var (a, b) = Parse(_input, m, iter);

        var output = Mod(a * 2019 + b, m);

        output.Should().Be(8191);
    }

    [Fact]
    public void PartTwo()
    {
        var m = 119315717514047;
        var iter = 101741582076661;
        var (a, b) = Parse(_input, m, iter);

        var output = Mod(ModInv(a, m) * (2020 - b), m);

        output.Should().Be(1644352419829);
    }

    private BigInteger Mod(BigInteger a, BigInteger m) => ((a % m) + m) % m;
    private BigInteger ModInv(BigInteger a, BigInteger m) => BigInteger.ModPow(a, m - 2, m);

    private (BigInteger a, BigInteger big) Parse(string input, long m, long n)
    {
        var a = new BigInteger(1);
        var b = new BigInteger(0);

        foreach (var line in input.Split('\n'))
        {
            if (line.Contains("into new stack"))
            {
                a = -a;
                b = m - b - 1;
            }
            else if (line.Contains("cut"))
            {
                var i = long.Parse(Regex.Match(line, @"-?\d+").Value);
                b = m + b - i;
            }
            else if (line.Contains("increment"))
            {
                var i = long.Parse(Regex.Match(line, @"-?\d+").Value);
                a *= i;
                b *= i;
            }
            else
            {
                throw new Exception();
            }
        }

        var resA = BigInteger.One;
        var resB = BigInteger.Zero;

        // resA = a^n
        resA = BigInteger.ModPow(a, n, m);
        // resB = b * (1 + a + a^2 + ... a^n) = b * (a^n - 1) / (a - 1);
        resB = b * (BigInteger.ModPow(a, n, m) - 1) * ModInv(a - 1, m) % m;

        return (resA, resB);
    }
}