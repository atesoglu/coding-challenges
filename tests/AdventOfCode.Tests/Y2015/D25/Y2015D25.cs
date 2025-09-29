using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D25;

[ChallengeName("Let It Snow")]
public partial class Y2015D25
{
    private readonly string _input = File.ReadAllText(@"Y2015\D25\Y2015D25-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (row, col) = ParseTargetCell(_input);
        var position = GetGridPositionNumber(row, col);
        var codeForCell = ComputeCodeForPosition(position);

        codeForCell.Should().Be(19980801);
    }

    private static (int row, int col) ParseTargetCell(string input)
    {
        var m = CodeRegex().Match(input);
        return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
    }

    private static long ComputeCodeForPosition(long position)
    {
        const long start = 20151125;
        const long factor = 252533;
        const long mod = 33554393;

        var code = start;
        for (long i = 1; i < position; i++)
        {
            code = (code * factor) % mod;
        }

        return code;
    }

    private static long GetGridPositionNumber(int row, int col)
    {
        // The grid is filled diagonally: top-right to bottom-left.
        long diagonalNumber = row + col - 1;

        // Each diagonal starts at a triangular number + 1.
        var firstPositionOnDiagonal = diagonalNumber * (diagonalNumber - 1) / 2 + 1;

        // Offset within that diagonal is (col - 1).
        return firstPositionOnDiagonal + (col - 1);
    }

    [GeneratedRegex(@"To continue, please consult the code grid in the manual.  Enter the code at row (\d+), column (\d+).")]
    private static partial Regex CodeRegex();
}