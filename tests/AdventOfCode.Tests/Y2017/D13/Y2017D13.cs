using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Tests.Y2017.D13;

[ChallengeName("Packet Scanners")]
public class Y2017D13
{
    private readonly string _input = File.ReadAllText(@"Y2017\D13\Y2017D13-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Severities(Parse(_input), 0).Sum();

        output.Should().Be(2160);
    }

    [Fact]
    public void PartTwo()
    {
        var layers = Parse(_input);

        var output = Enumerable
            .Range(0, int.MaxValue)
            .First(n => !Severities(layers, n).Any());

        output.Should().Be(3907470);
    }


    private static Layers Parse(string input) =>
        new Layers(
            from line in input.Split('\n')
            let parts = Regex.Split(line, ": ").Select(int.Parse).ToArray()
            select (parts[0], parts[1])
        );

    private static IEnumerable<int> Severities(Layers layers, int t)
    {
        var packetPos = 0;
        foreach (var layer in layers)
        {
            t += layer.depth - packetPos;
            packetPos = layer.depth;
            var scannerPos = t % (2 * layer.range - 2);
            if (scannerPos == 0)
            {
                yield return layer.depth * layer.range;
            }
        }
    }

    private class Layers : List<(int depth, int range)> {
        public Layers(IEnumerable<(int depth, int range)> layers) : base(layers) {
        }
    }
}