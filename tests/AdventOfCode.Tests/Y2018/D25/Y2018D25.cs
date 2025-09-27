using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2018.D25;

[ChallengeName("Four-Dimensional Adventure")]
public class Y2018D25
{
    private readonly string _input = File.ReadAllText(@"Y2018\D25\Y2018D25-input.txt", Encoding.UTF8);

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
        var sets = new List<HashSet<int[]>>();

        foreach (var line in input.Split("\n"))
        {
            var set = new HashSet<int[]>();
            set.Add(line.Split(",").Select(int.Parse).ToArray());
            sets.Add(set);
        }

        foreach (var set in sets.ToList())
        {
            var pt = set.Single();
            var closeSets = new List<HashSet<int[]>>();
            foreach (var setB in sets)
            {
                foreach (var ptB in setB)
                {
                    if (Dist(pt, ptB) <= 3)
                    {
                        closeSets.Add(setB);
                    }
                }
            }

            var mergedSet = new HashSet<int[]>();
            foreach (var setB in closeSets)
            {
                foreach (var ptB in setB)
                {
                    mergedSet.Add(ptB);
                }

                sets.Remove(setB);
            }

            sets.Add(mergedSet);
        }

        return sets.Count;
    }

    int Dist(int[] a, int[] b) => Enumerable.Range(0, a.Length).Select(i => Math.Abs(a[i] - b[i])).Sum();
}