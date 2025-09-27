using System.Text;
using FluentAssertions;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D06;

[ChallengeName("Lanternfish")]
public class Y2021D06
{
    private readonly string _input = File.ReadAllText(@"Y2021\D06\Y2021D06-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => FishCountAfterNDays(input, 80);
    private object PartTwo(string input) => FishCountAfterNDays(input, 256);

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