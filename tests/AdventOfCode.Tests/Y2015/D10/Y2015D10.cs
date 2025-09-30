using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D10;

[ChallengeName("Elves Look, Elves Say")]
public class Y2015D10
{
    private readonly string _input = File.ReadAllText(@"Y2015\D10\Y2015D10-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = LookAndSay(_input).Skip(39).First().Length;

        output.Should().Be(360154);
    }

    [Fact]
    public void PartTwo()
    {
        var output = LookAndSay(_input).Skip(49).First().Length;

        output.Should().Be(5103798);
    }

    private static IEnumerable<string> LookAndSay(string input)
    {
        while (true)
        {
            var sb = new StringBuilder();
            var ich = 0;
            while (ich < input.Length)
            {
                if (ich < input.Length - 2 && input[ich] == input[ich + 1] && input[ich] == input[ich + 2])
                {
                    sb.Append('3');
                    sb.Append(input[ich]);
                    ich += 3;
                }
                else if (ich < input.Length - 1 && input[ich] == input[ich + 1])
                {
                    sb.Append('2');
                    sb.Append(input[ich]);
                    ich += 2;
                }
                else
                {
                    sb.Append('1');
                    sb.Append(input[ich]);
                    ich += 1;
                }
            }

            input = sb.ToString();
            yield return input;
        }
    }
}