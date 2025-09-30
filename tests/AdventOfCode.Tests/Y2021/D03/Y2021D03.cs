using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2021.D03;

[ChallengeName("Binary Diagnostic")]
public class Y2021D03
{
    private readonly string _input = File.ReadAllText(@"Y2021\D03\Y2021D03-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var diagnosticReport = _input.Split("\n");
        var output = GammaRate(diagnosticReport) * EpsilonRate(diagnosticReport);

        output.Should().Be(1997414);
    }

    [Fact]
    public void PartTwo()
    {
        var diagnosticReport = _input.Split("\n");
        var output = OxygenGeneratorRating(diagnosticReport) * Co2ScruberRating(diagnosticReport);

        output.Should().Be(1032597);
    }

    private int GammaRate(string[] diagnosticReport) => Extract1(diagnosticReport, MostCommonBitAt);
    private int EpsilonRate(string[] diagnosticReport) => Extract1(diagnosticReport, LeastCommonBitAt);
    private int OxygenGeneratorRating(string[] diagnosticReport) => Extract2(diagnosticReport, MostCommonBitAt);
    private int Co2ScruberRating(string[] diagnosticReport) => Extract2(diagnosticReport, LeastCommonBitAt);

    private char MostCommonBitAt(string[] lines, int ibit) =>
        2 * lines.Count(line => line[ibit] == '1') >= lines.Length ? '1' : '0';

    private char LeastCommonBitAt(string[] lines, int ibit) =>
        MostCommonBitAt(lines, ibit) == '1' ? '0' : '1';

    private int Extract1(string[] lines, Func<string[], int, char> selectBitAt)
    {
        var cbit = lines[0].Length;

        var bits = "";
        for (var ibit = 0; ibit < cbit; ibit++)
        {
            bits += selectBitAt(lines, ibit);
        }

        return Convert.ToInt32(bits, 2);
    }

    private int Extract2(string[] lines, Func<string[], int, char> selectBitAt)
    {
        var cbit = lines[0].Length;

        for (var ibit = 0; lines.Length > 1 && ibit < cbit; ibit++)
        {
            var bit = selectBitAt(lines, ibit);
            lines = lines.Where(line => line[ibit] == bit).ToArray();
        }

        return Convert.ToInt32(lines[0], 2);
    }
}