using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2019.D08;

[ChallengeName("Space Image Format")]
public class Y2019D08
{
    private readonly string _input = File.ReadAllText(@"Y2019\D08\Y2019D08-input.txt", Encoding.UTF8);

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
        var zeroMin = int.MaxValue;
        var checksum = 0;
        foreach (var layer in Layers(input))
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

        return checksum;
    }

    private object PartTwo(string input)
    {
        var img = new char[6 * 25];
        foreach (var layer in Layers(input).Reverse())
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

        return string.Join("",
            img.Chunk(25).Select(line => string.Join("", line) + "\n")
        ).Ocr();
    }

    int[][] Layers(string input) =>
        input.Select(ch => ch - '0').Chunk(6 * 25).ToArray();
}