using System.Text;
using FluentAssertions;
using System;
using System.Linq;

namespace AdventOfCode.Tests.Y2017.D05;

[ChallengeName("A Maze of Twisty Trampolines, All Alike")]
public class Y2017D05
{
    private readonly string _input = File.ReadAllText(@"Y2017\D05\Y2017D05-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetStepCount(_input, x => x + 1);

        output.Should().Be(375042);
    }

    [Fact]
    public void PartTwo()
    {
        var output = GetStepCount(_input, x => x < 3 ? x + 1 : x - 1);

        output.Should().Be(28707598);
    }


    private static int GetStepCount(string input, Func<int, int> update)
    {
        var numbers = input.Split('\n').Select(int.Parse).ToArray();
        var i = 0;
        var stepCount = 0;
        while (i < numbers.Length && i >= 0)
        {
            var jmp = numbers[i];
            numbers[i] = update(numbers[i]);
            i += jmp;
            stepCount++;
        }

        return stepCount;
    }
}