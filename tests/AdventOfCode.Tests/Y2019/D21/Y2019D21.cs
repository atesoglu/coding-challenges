using System.Text;
using FluentAssertions;
using System.Linq;

namespace AdventOfCode.Tests.Y2019.D21;

[ChallengeName("Springdroid Adventure")]
public class Y2019D21
{
    private readonly string _input = File.ReadAllText(@"Y2019\D21\Y2019D21-input.txt", Encoding.UTF8);

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
        var icm = new IntCodeMachine(input);

        // J = (Â¬A âˆ¨ Â¬B âˆ¨ Â¬C) âˆ§ D  
        // jump if no road ahead, but we can continue from D
        return new IntCodeMachine(input).Run(
            "OR A T",
            "AND B T",
            "AND C T",
            "NOT T J",
            "AND D J",
            "WALK"
        ).Last();
    }

    private object PartTwo(string input)
    {
        // J = (Â¬A âˆ¨ Â¬B âˆ¨ Â¬C) âˆ§ D âˆ§ (H âˆ¨ E) 
        // same as part 1, but also check that D is not a dead end
        return new IntCodeMachine(input).Run(
            "OR A T",
            "AND B T",
            "AND C T",
            "NOT T J",
            "AND D J",
            "OR H T",
            "OR E T",
            "AND T J",
            "RUN"
        ).Last();
    }
}