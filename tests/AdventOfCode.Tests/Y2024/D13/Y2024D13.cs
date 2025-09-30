using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D13;

using Machine = (Vec2 a, Vec2 b, Vec2 p);

internal record struct Vec2(long x, long y);

[ChallengeName("Claw Contraption")]
public class Y2024D13
{
    private readonly string _input = File.ReadAllText(@"Y2024\D13\Y2024D13-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = ParseMachines(_input).Sum(CalculatePrize);

        output.Should().Be(36758);
    }

    [Fact]
    public void PartTwo()
    {
        var output = ParseMachines(_input, shift: 10000000000000).Sum(CalculatePrize);

        output.Should().Be(76358113886726);
    }

    private long CalculatePrize(Machine machine)
    {
        var (vectorA, vectorB, targetPoint) = machine;

        var coefficientI = CalculateDeterminant(targetPoint, vectorB) / CalculateDeterminant(vectorA, vectorB);
        var coefficientJ = CalculateDeterminant(vectorA, targetPoint) / CalculateDeterminant(vectorA, vectorB);

        if (coefficientI >= 0 && coefficientJ >= 0 && 
            vectorA.x * coefficientI + vectorB.x * coefficientJ == targetPoint.x && 
            vectorA.y * coefficientI + vectorB.y * coefficientJ == targetPoint.y)
        {
            return 3 * coefficientI + coefficientJ;
        }
        else
        {
            return 0;
        }
    }

    private static long CalculateDeterminant(Vec2 vectorA, Vec2 vectorB) => vectorA.x * vectorB.y - vectorA.y * vectorB.x;

    private static IEnumerable<Machine> ParseMachines(string input, long shift = 0)
    {
        // Normalize line endings to just "\n"
        input = input.Replace("\r\n", "\n").TrimEnd();

        var blocks = input.Split("\n\n");
        foreach (var block in blocks)
        {
            var numbers =
                Regex.Matches(block, @"\d+", RegexOptions.Multiline)
                    .Select(match => int.Parse(match.Value))
                    .Chunk(2).Select(pair => new Vec2(pair[0], pair[1]))
                    .ToArray();

            numbers[2] = new Vec2(numbers[2].x + shift, numbers[2].y + shift);
            yield return (numbers[0], numbers[1], numbers[2]);
        }
    }
}