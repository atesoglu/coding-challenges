using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D01;

[ChallengeName("Inverse Captcha")]
public class Y2017D01
{
    private readonly string _input = File.ReadAllText(@"Y2017\D01\Y2017D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = InverseCaptcha(_input, 1);

        output.Should().Be(1216);
    }

    [Fact]
    public void PartTwo()
    {
        var output = InverseCaptcha(_input, _input.Length / 2);

        output.Should().Be(1072);
    }


    private int InverseCaptcha(string input, int skip)
    {
        return (
            from i in Enumerable.Range(0, input.Length)
            where input[i] == input[(i + skip) % input.Length]
            select int.Parse(input[i].ToString())
        ).Sum();
    }
}