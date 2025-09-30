using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2021.D01;

[ChallengeName("Sonar Sweep")]
public class Y2021D01
{
    private readonly string _input = File.ReadAllText(@"Y2021\D01\Y2021D01-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = DepthIncrease(Numbers(_input));

        output.Should().Be(1655);
    }

    [Fact]
    public void PartTwo()
    {
        var output = DepthIncrease(ThreeMeasurements(Numbers(_input)));

        output.Should().Be(1683);
    }


    private int DepthIncrease(IEnumerable<int> ns) => (
        from p in Enumerable.Zip(ns, ns.Skip(1))
        where p.First < p.Second
        select 1
    ).Count();

    // the sum of elements in a sliding window of 3
    private IEnumerable<int> ThreeMeasurements(IEnumerable<int> ns) =>
        from t in Enumerable.Zip(ns, ns.Skip(1), ns.Skip(2)) // â­ .Net 6 comes with three way zip
        select t.First + t.Second + t.Third;

    // parse input to array of numbers
    private IEnumerable<int> Numbers(string input) =>
        from n in input.Split('\n')
        select int.Parse(n);
}