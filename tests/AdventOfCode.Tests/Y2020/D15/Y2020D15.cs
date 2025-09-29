using System.Text;
using FluentAssertions;
using System.Linq;

namespace AdventOfCode.Tests.Y2020.D15;

[ChallengeName("Rambunctious Recitation")]
public class Y2020D15
{
    private readonly string _input = File.ReadAllText(@"Y2020\D15\Y2020D15-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = NumberAt(_input, 2020);

        output.Should().Be(234);
    }

    [Fact]
    public void PartTwo()
    {
        var output = NumberAt(_input, 30000000);

        output.Should().Be(8984);
    }

    private int NumberAt(string input, int count)
    {
        var numbers = input.Split(",").Select(int.Parse).ToArray();
        var (lastSeen, number) = (new int[count], numbers[0]);
        for (var round = 0; round < count; round++)
        {
            (lastSeen[number], number) = (round,
                round < numbers.Length ? numbers[round] :
                lastSeen[number] == 0 ? 0 :
                /* otherwise */ round - lastSeen[number]);
        }

        return number;
    }
}