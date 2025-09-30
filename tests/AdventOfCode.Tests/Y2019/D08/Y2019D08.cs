using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D08;

[ChallengeName("Space Image Format")]
public class Y2019D08
{
    private readonly string _input = File.ReadAllText(@"Y2019\D08\Y2019D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var zeroMin = int.MaxValue;
        var checksum = 0;
        foreach (var layer in Layers(_input))
        {
            var zero = layer.Count(item => item == 0);
            var ones = layer.Count(item => item == 1);
            var twos = layer.Count(item => item == 2);

            if (zeroMin > zero)
            {
                zeroMin = zero;
                checksum = ones * twos;
            }
        }

        var output = checksum;

        output.Should().Be(2904);
    }

    [Fact]
    public void PartTwo()
    {
        var img = new char[6 * 25];
        foreach (var layer in Layers(_input).Reverse())
        {
            for (var i = 0; i < img.Length; i++)
            {
                img[i] = layer[i] switch
                {
                    0 => ' ',
                    1 => '#',
                    _ => img[i]
                };
            }
        }

        var output = string.Join("", img.Chunk(25).Select(line => string.Join("", line) + "\n")).ToScreenText().ToString();

        output.Should().Be("HGBCF");
    }

    private int[][] Layers(string input) => input.Select(ch => ch - '0').Chunk(6 * 25).ToArray();
}