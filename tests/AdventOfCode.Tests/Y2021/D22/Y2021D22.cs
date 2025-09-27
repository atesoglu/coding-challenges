using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Tests.Y2021.D22;

[ChallengeName("Reactor Reboot")]
public class Y2021D22
{
    private readonly string _input = File.ReadAllText(@"Y2021\D22\Y2021D22-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => ActiveCubesInRange(input, 50);
    private object PartTwo(string input) => ActiveCubesInRange(input, int.MaxValue);

    private long ActiveCubesInRange(string input, int range)
    {
        var cmds = Parse(input);

        // Recursive approach

        // If we can determine the number of active cubes in subregions
        // we can compute the effect of the i-th cmd as well:
        long activeCubesAfterIcmd(int icmd, Region region)
        {
            if (region.IsEmpty || icmd < 0)
            {
                return 0; // empty is empty
            }
            else
            {
                var intersection = region.Intersect(cmds[icmd].region);
                var activeInRegion = activeCubesAfterIcmd(icmd - 1, region);
                var activeInIntersection = activeCubesAfterIcmd(icmd - 1, intersection);
                var activeOutsideIntersection = activeInRegion - activeInIntersection;

                // outside the intersection is unaffected, the rest is either on or off:
                return cmds[icmd].turnOff ? activeOutsideIntersection : activeOutsideIntersection + intersection.Volume;
            }
        }

        return activeCubesAfterIcmd(
            cmds.Length - 1,
            new Region(
                new Segment(-range, range),
                new Segment(-range, range),
                new Segment(-range, range)));
    }

    Cmd[] Parse(string input)
    {
        var res = new List<Cmd>();
        foreach (var line in input.Split("\n"))
        {
            var turnOff = line.StartsWith("off");
            // get all the numbers with a regexp:
            var m = Regex.Matches(line, "-?[0-9]+").Select(m => int.Parse(m.Value)).ToArray();
            res.Add(new Cmd(turnOff, new Region(new Segment(m[0], m[1]), new Segment(m[2], m[3]), new Segment(m[4], m[5]))));
        }

        return res.ToArray();
    }
}