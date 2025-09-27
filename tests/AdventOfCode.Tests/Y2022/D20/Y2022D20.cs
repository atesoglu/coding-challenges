using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D20;

[ChallengeName("Grove Positioning System")]
public class Y2022D20
{
    private readonly string _input = File.ReadAllText(@"Y2022\D20\Y2022D20-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetGrooveCoordinates(Mix(Parse(_input, 1)));

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var data = Parse(_input, 811589153L);
        for (var i = 0; i < 10; i++)
        {
            data = Mix(data);
        }

        var output = GetGrooveCoordinates(data);

        output.Should().Be(0);
    }


    private record Data(int idx, long num);

    List<Data> Parse(string input, long m) =>
        input
            .Split("\n")
            .Select((line, idx) => new Data(idx, long.Parse(line) * m))
            .ToList();

    List<Data> Mix(List<Data> numsWithIdx)
    {
        var mod = numsWithIdx.Count - 1;
        for (var idx = 0; idx < numsWithIdx.Count; idx++)
        {
            var srcIdx = numsWithIdx.FindIndex(x => x.idx == idx);
            var num = numsWithIdx[srcIdx];

            var dstIdx = (srcIdx + num.num) % mod;
            if (dstIdx < 0)
            {
                dstIdx += mod;
            }

            numsWithIdx.RemoveAt(srcIdx);
            numsWithIdx.Insert((int)dstIdx, num);
        }

        return numsWithIdx;
    }

    long GetGrooveCoordinates(List<Data> numsWithIdx)
    {
        var idx = numsWithIdx.FindIndex(x => x.num == 0);
        return (
            numsWithIdx[(idx + 1000) % numsWithIdx.Count].num +
            numsWithIdx[(idx + 2000) % numsWithIdx.Count].num +
            numsWithIdx[(idx + 3000) % numsWithIdx.Count].num
        );
    }
}