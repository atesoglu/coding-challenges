using System.Text;
using FluentAssertions;
using System.Collections.Generic;

namespace AdventOfCode.Tests.Y2017.D17;

[ChallengeName("Spinlock")]
public class Y2017D17
{
    private readonly string _input = File.ReadAllText(@"Y2017\D17\Y2017D17-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var step = int.Parse(_input);
        var nums = new List<int>() { 0 };
        var pos = 0;
        for (int i = 1; i < 2018; i++)
        {
            pos = (pos + step) % nums.Count + 1;
            nums.Insert(pos, i);
        }

        var output = nums[(pos + 1) % nums.Count];

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var step = int.Parse(_input);
        var pos = 0;
        var numsCount = 1;
        var res = 0;
        for (int i = 1; i < 50000001; i++)
        {
            pos = (pos + step) % numsCount + 1;
            if (pos == 1)
            {
                res = i;
            }

            numsCount++;
        }

        var output = res;

        output.Should().Be(0);
    }
}