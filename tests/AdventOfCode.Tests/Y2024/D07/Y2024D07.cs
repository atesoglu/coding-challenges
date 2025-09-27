using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Tests.Y2024.D07;

[ChallengeName("Bridge Repair")]
public class Y2024D07
{
    private readonly string _input = File.ReadAllText(@"Y2024\D07\Y2024D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Filter(_input, Check1).Sum();

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Filter(_input, Check2).Sum();

        output.Should().Be(0);
    }


    // returns those calibrations that are valid according to the checker
    private IEnumerable<long> Filter(string input, Func<long, long, List<long>, bool> check) =>
        from line in input.Split("\n")
        let parts = Regex.Matches(line, @"\d+").Select(m => long.Parse(m.Value))
        let target = parts.First()
        let nums = parts.Skip(1).ToList()
        where check(target, nums[0], nums[1..])
        select target;

    // separate checkers provided for the two parts, these recursive functions go
    // over the numbers and use all allowed operators to update the accumulated result.
    // at the end of the recursion we simply check if we reached the target
    private bool Check1(long target, long acc, List<long> nums) =>
        nums switch
        {
            [] => target == acc,
            _ => Check1(target, acc * nums[0], nums[1..]) ||
                 Check1(target, acc + nums[0], nums[1..])
        };

    private bool Check2(long target, long acc, List<long> nums) =>
        nums switch
        {
            _ when acc > target => false, // optimization: early exit from deadend
            [] => target == acc,
            _ => Check2(target, long.Parse($"{acc}{nums[0]}"), nums[1..]) ||
                 Check2(target, acc * nums[0], nums[1..]) ||
                 Check2(target, acc + nums[0], nums[1..])
        };
}