using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D01;

[ChallengeName("Not Quite Lisp")]
public class Y2015D01
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D01\Y2015D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = _lines.Where(line => !string.IsNullOrEmpty(line)).Sum(line => line.Sum(c => c == '(' ? 1 : -1));

        output.Should().Be(74);
    }

    [Fact]
    public void PartTwo()
    {
        var output = _lines.Sum(line =>
        {
            if (string.IsNullOrEmpty(line))
            {
                return 0;
            }

            var step = EnumerateFloors(line).FirstOrDefault(p => p.Floor == -1);
            return step?.Position ?? 0;
        });

        output.Should().Be(1795);
    }

    private static IEnumerable<FloorStep> EnumerateFloors(string input)
    {
        var floor = 0;
        for (var i = 0; i < input.Length; i++)
        {
            floor += input[i] == '(' ? 1 : -1;
            yield return new FloorStep(i + 1, floor);
        }
    }

    private record FloorStep(int Position, int Floor);
}