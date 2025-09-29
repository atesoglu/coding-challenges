using System.Text;
using AdventOfCode.Tests.Y2019.D02;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D21;

[ChallengeName("Springdroid Adventure")]
public class Y2019D21
{
    private readonly string _input = File.ReadAllText(@"Y2019\D21\Y2019D21-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var icm = new IntCodeMachine(_input);

        // J = (Â¬A âˆ¨ Â¬B âˆ¨ Â¬C) âˆ§ D
        // jump if no road ahead, but we can continue from D
        var output = new IntCodeMachine(_input).Run(
            "OR A T",
            "AND B T",
            "AND C T",
            "NOT T J",
            "AND D J",
            "WALK"
        ).Last();

        output.Should().Be(19356081);
    }

    [Fact]
    public void PartTwo()
    {
        // J = (Â¬A âˆ¨ Â¬B âˆ¨ Â¬C) âˆ§ D âˆ§ (H âˆ¨ E)
        // same as part 1, but also check that D is not a dead end
        var output = new IntCodeMachine(_input).Run(
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

        output.Should().Be(1141901823);
    }
}