using System.Text;
using FluentAssertions;
using System.Linq;

namespace AdventOfCode.Tests.Y2016.D18;

[ChallengeName("Like a Rogue")]
public class Y2016D18
{
    private readonly string _input = File.ReadAllText(@"Y2016\D18\Y2016D18-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = SafeCount(_input, 40);

        output.Should().Be(1982);
    }

    [Fact]
    public void PartTwo()
    {
        var output = SafeCount(_input, 400000);

        output.Should().Be(20005203);
    }


    private int SafeCount(string input, int lines)
    {
        var rowPrev = input;
        var safeCount = rowPrev.Count(ch => ch == '.');
        for (var i = 0; i < lines - 1; i++)
        {
            var sb = new StringBuilder();
            for (var j = 0; j < rowPrev.Length; j++)
            {
                var leftTrap = j != 0 && rowPrev[j - 1] == '^';
                var centerTrap = rowPrev[j] == '^';
                var rightTrap = j != rowPrev.Length - 1 && rowPrev[j + 1] == '^';

                var trap =
                        (leftTrap && centerTrap && !rightTrap) ||
                        (!leftTrap && centerTrap && rightTrap) ||
                        (leftTrap && !centerTrap && !rightTrap) ||
                        (!leftTrap && !centerTrap && rightTrap)
                    ;
                sb.Append(trap ? '^' : '.');
            }

            rowPrev = sb.ToString();
            safeCount += rowPrev.Count(ch => ch == '.');
        }

        return safeCount;
    }
}