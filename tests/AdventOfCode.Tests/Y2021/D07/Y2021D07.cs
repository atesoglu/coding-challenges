using System.Text;
using FluentAssertions;
using System;
using System.Linq;

namespace AdventOfCode.Tests.Y2021.D07;

[ChallengeName("The Treachery of Whales")]
public class Y2021D07
{
    private readonly string _input = File.ReadAllText(@"Y2021\D07\Y2021D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = FuelMin(_input, fuelConsumption: distance => distance);

        output.Should().Be(342534);
    }

    [Fact]
    public void PartTwo()
    {
        var output = FuelMin(_input, fuelConsumption: distance => (1 + distance) * distance / 2);

        output.Should().Be(94004208);
    }

    private int FuelMin(string input, Func<int, int> fuelConsumption)
    {
        var positions = input.Split(",").Select(int.Parse).ToArray();

        var totalFuelToReachTarget = (int target) =>
            positions.Select(position => fuelConsumption(Math.Abs(target - position))).Sum();

        // Minimize the total fuel consumption checking each possible target position.
        // We have just about 1000 of these, so an O(n^2) algorithm will suffice.
        var minPosition = positions.Min();
        var maxPosition = positions.Max();
        return Enumerable.Range(minPosition, maxPosition - minPosition + 1).Select(totalFuelToReachTarget).Min();
    }
}