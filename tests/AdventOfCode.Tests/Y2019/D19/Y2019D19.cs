using System.Text;
using AdventOfCode.Tests.Y2019.D02;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D19;

[ChallengeName("Tractor Beam")]
public class Y2019D19
{
    private readonly string _input = File.ReadAllText(@"Y2019\D19\Y2019D19-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var detector = Detector(_input);

        var output = (from x in Enumerable.Range(0, 50)
            from y in Enumerable.Range(0, 50)
            where detector(x, y)
            select 1).Count();

        output.Should().Be(186);
    }

    [Fact]
    public void PartTwo()
    {
        var output = DetectPartTwo(_input);

        output.Should().Be(9231141);
    }


    private static Func<int, int, bool> Detector(string input)
    {
        var icm = new ImmutableIntCodeMachine(input);
        return (int x, int y) =>
        {
            var (_, output) = icm.Run(x, y);
            return output[0] == 1;
        };
    }

    private object DetectPartTwo(string input)
    {
        var detector = Detector(input);

        var (xStart, y) = (0, 100);
        while (true)
        {
            while (!detector(xStart, y))
            {
                xStart++;
            }

            var x = xStart;
            while (detector(x + 99, y))
            {
                if (detector(x, y + 99) && detector(x + 99, y + 99))
                {
                    return (x * 10000 + y);
                }

                x++;
            }

            y++;
        }
    }
}