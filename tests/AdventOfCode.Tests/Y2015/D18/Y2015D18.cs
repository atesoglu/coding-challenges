using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D18;

[ChallengeName("Like a GIF For Your Yard")]
public class Y2015D18
{
    private readonly IEnumerable<string> _lines = File.ReadAllLines(@"Y2015\D18\Y2015D18-input.txt", Encoding.UTF8);
    private readonly int[][] _input;

    public Y2015D18()
    {
        _input = (from line in _lines select (from ch in line select ch == '#' ? 1 : 0).ToArray()).ToArray();
    }

    [Fact]
    public void PartOne()
    {
        var output = Enumerable.Range(0, 100).Aggregate(_input, (acc, _) => NextGridState(acc, false)).Select(row => row.Sum()).Sum();

        output.Should().Be(821);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Enumerable.Range(0, 100).Aggregate(_input, (acc, _) => NextGridState(acc, true)).Select(row => row.Sum()).Sum();

        output.Should().Be(886);
    }

    private static int[][] NextGridState(int[][] input, bool stuck)
    {
        var nextGrid = new List<int[]>();
        var (rowCount, colCount) = (input.Length, input[0].Length);

        if (stuck)
        {
            input[0][0] = 1;
            input[rowCount - 1][0] = 1;
            input[0][colCount - 1] = 1;
            input[rowCount - 1][colCount - 1] = 1;
        }

        for (var row = 0; row < rowCount; row++)
        {
            var nextRow = new List<int>();
            for (var col = 0; col < colCount; col++)
            {
                if (stuck && ((col == 0 && row == 0) || (col == colCount - 1 && row == 0) || (col == 0 && row == rowCount - 1) || (col == colCount - 1 && row == rowCount - 1)))
                {
                    nextRow.Add(1);
                }
                else
                {
                    var neighbours = (
                        from d in new (int row, int col)[] { (-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1) }
                        let irowT = row + d.row
                        let icolT = col + d.col
                        where irowT >= 0 && irowT < rowCount && icolT >= 0 && icolT < colCount && input[irowT][icolT] == 1
                        select 1).Sum();

                    nextRow.Add(input[row][col] == 1 ? new[] { 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 }[neighbours] : new[] { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 }[neighbours]);
                }
            }

            nextGrid.Add(nextRow.ToArray());
        }

        return nextGrid.ToArray();
    }
}