using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D03;

[ChallengeName("Squares With Three Sides")]
public class Y2016D03
{
    private readonly string _input = File.ReadAllText(@"Y2016\D03\Y2016D03-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = ValidTriangles(Parse(_input));

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var tripplets = new List<IEnumerable<int>>();

        foreach (var lineT in Transpose(Parse(_input)))
        {
            IEnumerable<int> line = lineT;
            while (line.Any())
            {
                tripplets.Add(line.Take(3));
                line = line.Skip(3);
            }
        }

        var output = ValidTriangles(tripplets);

        output.Should().Be(0);
    }

    int[][] Parse(string input) => (
        from line in input.Split('\n')
        select Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToArray()
    ).ToArray();

    int ValidTriangles(IEnumerable<IEnumerable<int>> tripplets) =>
        tripplets.Count(tripplet =>
        {
            var nums = tripplet.OrderBy(x => x).ToArray();
            return nums[0] + nums[1] > nums[2];
        });

    int[][] Transpose(int[][] src)
    {
        var crowDst = src[0].Length;
        var ccolDst = src.Length;
        int[][] dst = new int[crowDst][];
        for (int irowDst = 0; irowDst < crowDst; irowDst++)
        {
            dst[irowDst] = new int[ccolDst];
            for (int icolDst = 0; icolDst < ccolDst; icolDst++)
            {
                dst[irowDst][icolDst] = src[icolDst][irowDst];
            }
        }

        return dst;
    }
}