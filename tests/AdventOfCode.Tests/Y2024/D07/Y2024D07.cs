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
    private readonly string[] _lines = File.ReadAllLines(@"Y2024\D07\Y2024D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Filter(_lines, CheckWithMultiplyAndAdd).Sum();

        output.Should().Be(4555081946288);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Filter(_lines, CheckWithConcatMultiplyAdd).Sum();

        output.Should().Be(227921760109726);
    }


    private static IEnumerable<long> Filter(IEnumerable<string> lines, Func<long, long, List<long>, bool> check) =>
        from line in lines
        let parts = Regex.Matches(line, @"\d+").Select(m => long.Parse(m.Value))
        let target = parts.First()
        let numbers = parts.Skip(1).ToList()
        where check(target, numbers[0], numbers[1..])
        select target;

    private bool CheckWithMultiplyAndAdd(long target, long acc, List<long> nums) =>
        nums switch
        {
            [] => target == acc,
            _ => CheckWithMultiplyAndAdd(target, acc * nums[0], nums[1..]) ||
                 CheckWithMultiplyAndAdd(target, acc + nums[0], nums[1..])
        };

    private bool CheckWithConcatMultiplyAdd(long target, long acc, List<long> nums) =>
        nums switch
        {
            _ when acc > target => false,
            [] => target == acc,
            _ => CheckWithConcatMultiplyAdd(target, long.Parse($"{acc}{nums[0]}"), nums[1..]) ||
                 CheckWithConcatMultiplyAdd(target, acc * nums[0], nums[1..]) ||
                 CheckWithConcatMultiplyAdd(target, acc + nums[0], nums[1..])
        };
}