using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D01;

[ChallengeName("Chronal Calibration")]
public class Y2018D01
{
    private readonly string _input = File.ReadAllText(@"Y2018\D01\Y2018D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Frequencies(_input).ElementAt(_input.Split("\n").Count() - 1);

        output.Should().Be(459);
    }

    [Fact]
    public void PartTwo()
    {
        var output = 0;

        var seen = new HashSet<int>();
        foreach (var f in Frequencies(_input))
        {
            if (seen.Contains(f))
            {
                output = f;
                break;
            }

            seen.Add(f);
        }


        output.Should().Be(65474);
    }

    private static IEnumerable<int> Frequencies(string input)
    {
        var f = 0;
        while (true)
        {
            foreach (var d in input.Split("\n").Select(int.Parse))
            {
                f += d;
                yield return f;
            }
        }
    }
}