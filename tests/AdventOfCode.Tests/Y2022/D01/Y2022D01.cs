using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D01;

[ChallengeName("Calorie Counting      ")]
public class Y2022D01
{
    private readonly string _input = File.ReadAllText(@"Y2022\D01\Y2022D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetCaloriesPerElf(_input).First();

        output.Should().Be(71023);
    }

    [Fact]
    public void PartTwo()
    {
        var output = GetCaloriesPerElf(_input).Take(3).Sum();

        output.Should().Be(206289);
    }


    // Returns the calories carried by the elves in descending order.
    private static IEnumerable<int> GetCaloriesPerElf(string input)
    {
        // Normalize line endings to just "\n"
        input = input.Replace("\r\n", "\n").TrimEnd();

        return from elf in input.Split("\n\n")
            let calories = elf.Split('\n').Select(int.Parse).Sum()
            orderby calories descending
            select calories;
    }
}