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
        var output = PartOne(_input);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = PartTwo(_input);

        output.Should().Be(0);
    }


    private object PartOne(string input) => NumberAt(input, 2020);
    private object PartTwo(string input) => NumberAt(input, 30000000);

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