using System.Text;
using FluentAssertions;
using System.Linq;

namespace AdventOfCode.Tests.Y2020.D25;

[ChallengeName("Combo Breaker")]
public class Y2020D25
{
    private readonly string _input = File.ReadAllText(@"Y2020\D25\Y2020D25-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = PartOne(_input);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = PartTwo(_input);

        output.Should().Be(0);
    }


    private object PartOne(string input)
    {
        // https://en.wikipedia.org/wiki/Diffie%E2%80%93Hellman_key_exchange
        var numbers = input.Split("\n").Select(int.Parse).ToArray();
        var mod = 20201227;
        var pow = 0;
        var subj = 7L;
        var num = subj;
        while (num != numbers[0] && num != numbers[1])
        {
            num = (num * subj) % mod;
            pow++;
        }

        subj = num == numbers[0] ? numbers[1] : numbers[0];
        num = subj;
        while (pow > 0)
        {
            num = (num * subj) % mod;
            pow--;
        }

        return num;
    }
}