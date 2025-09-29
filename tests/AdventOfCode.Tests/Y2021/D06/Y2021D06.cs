using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2021.D06;

[ChallengeName("Lanternfish")]
public class Y2021D06
{
    private readonly string _input = File.ReadAllText(@"Y2021\D06\Y2021D06-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = FishCountAfterNDays(_input, 80);

        output.Should().Be(375482);
    }

    [Fact]
    public void PartTwo()
    {
        var output = FishCountAfterNDays(_input, 256);

        output.Should().Be(1689540415957);
    }


    long FishCountAfterNDays(string input, int days)
    {
        // group the fish by their timer, no need to deal with them one by one:
        var fishCountByInternalTimer = new long[9];
        foreach (var ch in input.Split(','))
        {
            fishCountByInternalTimer[int.Parse(ch)]++;
        }

        // we will model a circular shift register, with an additional feedback:
        //       0123456           78 
        //   â”Œâ”€â”€[.......]â”€<â”€(+)â”€â”€â”€[..]â”€â”€â”
        //   â””â”€â”€â”€â”€â”€â”€>â”€â”€â”€â”€â”€â”€â”€â”€â”´â”€â”€â”€â”€â”€>â”€â”€â”€â”€â”˜
        //     reproduction     newborn

        for (var t = 0; t < days; t++)
        {
            fishCountByInternalTimer[(t + 7) % 9] += fishCountByInternalTimer[t % 9];
        }

        return fishCountByInternalTimer.Sum();
    }
}