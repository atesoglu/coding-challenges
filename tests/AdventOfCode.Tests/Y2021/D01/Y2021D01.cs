using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D01;

[ChallengeName("Sonar Sweep")]
public class Y2021D01
{
    private readonly string _input = File.ReadAllText(@"Y2021\D01\Y2021D01-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) => DepthIncrease(Numbers(input));

    private object PartTwo(string input) => DepthIncrease(ThreeMeasurements(Numbers(input)));

    int DepthIncrease(IEnumerable<int> ns) => (
        from p in Enumerable.Zip(ns, ns.Skip(1))
        where p.First < p.Second
        select 1
    ).Count();

    // the sum of elements in a sliding window of 3
    IEnumerable<int> ThreeMeasurements(IEnumerable<int> ns) =>
        from t in Enumerable.Zip(ns, ns.Skip(1), ns.Skip(2)) // â­ .Net 6 comes with three way zip
        select t.First + t.Second + t.Third;

    // parse input to array of numbers
    IEnumerable<int> Numbers(string input) =>
        from n in input.Split('\n')
        select int.Parse(n);
}