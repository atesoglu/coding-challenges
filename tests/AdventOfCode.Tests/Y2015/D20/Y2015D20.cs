using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D20;

[ChallengeName("Infinite Elves and Infinite Houses")]
public class Y2015D20
{
    private readonly string _input = File.ReadAllText(@"Y2015\D20\Y2015D20-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var l = int.Parse(_input);
        var output = PresentsByHouse(1000000, 10, l);

        output.Should().Be(665280);
    }

    [Fact]
    public void PartTwo()
    {
        var l = int.Parse(_input);
        var output = PresentsByHouse(50, 11, l);

        output.Should().Be(705600);
    }

    private static int PresentsByHouse(int steps, int mul, int l)
    {
        var presents = new int[1000000];
        for (var i = 1; i < presents.Length; i++)
        {
            var j = i;
            var step = 0;
            while (j < presents.Length && step < steps)
            {
                presents[j] += mul * i;
                j += i;
                step++;
            }
        }

        for (var i = 0; i < presents.Length; i++)
        {
            if (presents[i] >= l)
            {
                return i;
            }
        }

        return -1;
    }
}