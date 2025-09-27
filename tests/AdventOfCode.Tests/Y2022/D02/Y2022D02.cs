using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D02;

[ChallengeName("Rock Paper Scissors")]
public class Y2022D02
{
    private readonly string _input = File.ReadAllText(@"Y2022\D02\Y2022D02-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Total(_input, Elf, Human1);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Total(_input, Elf, Human2);

        output.Should().Be(0);
    }


    // There are many obscure ways of solving this challenge. You can use 
    // mod 3 arithmetic or play with ASCII encoding. This approach is more 
    // explicit. I think it is as simple as it gets.

    // We parse the input lines into a pair of Rock/Paper/Scissors signs 
    // represented by 1,2,3 (the values from the problem description), 
    // calculate the score for each pair and sum it up.

    // Part one and two differs only in the decoding of the X, Y and Z signs.

    private enum Sign
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3,
    }

    Sign Elf(string line) =>
        line[0] == 'A' ? Sign.Rock :
        line[0] == 'B' ? Sign.Paper :
        line[0] == 'C' ? Sign.Scissors :
        throw new ArgumentException(line);

    Sign Human1(string line) =>
        line[2] == 'X' ? Sign.Rock :
        line[2] == 'Y' ? Sign.Paper :
        line[2] == 'Z' ? Sign.Scissors :
        throw new ArgumentException(line);

    Sign Human2(string line) =>
        line[2] == 'X' ? Next(Next(Elf(line))) : // elf wins
        line[2] == 'Y' ? Elf(line) : // draw
        line[2] == 'Z' ? Next(Elf(line)) : // you win
        throw new ArgumentException(line);

    int Total(string input, Func<string, Sign> elf, Func<string, Sign> human) =>
        input
            .Split("\n")
            .Select(line => Score(elf(line), human(line)))
            .Sum();

    int Score(Sign elfSign, Sign humanSign) =>
        humanSign == Next(elfSign) ? 6 + (int)humanSign : // human wins
        humanSign == elfSign ? 3 + (int)humanSign : // draw
        humanSign == Next(Next(elfSign)) ? 0 + (int)humanSign : // elf wins
        throw new ArgumentException();

    Sign Next(Sign sign) =>
        sign == Sign.Rock ? Sign.Paper :
        sign == Sign.Paper ? Sign.Scissors :
        sign == Sign.Scissors ? Sign.Rock :
        throw new ArgumentException();
}