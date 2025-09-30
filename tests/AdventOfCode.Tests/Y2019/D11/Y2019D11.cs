using System.Text;
using AdventOfCode.Tests.Y2019.D02;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D11;

[ChallengeName("Space Police")]
public class Y2019D11
{
    private readonly string _input = File.ReadAllText(@"Y2019\D11\Y2019D11-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Run(_input, 0).Count;

        output.Should().Be(2511);
    }

    [Fact]
    public void PartTwo()
    {
        var dict = Run(_input, 1);
        var irowMin = dict.Keys.Select(pos => pos.irow).Min();
        var icolMin = dict.Keys.Select(pos => pos.icol).Min();
        var irowMax = dict.Keys.Select(pos => pos.irow).Max();
        var icolMax = dict.Keys.Select(pos => pos.icol).Max();
        var crow = irowMax - irowMin + 1;
        var ccol = icolMax - icolMin + 1;
        var st = "";
        for (var irow = 0; irow < crow; irow++)
        {
            for (var icol = 0; icol < ccol; icol++)
            {
                st += " #"[dict.GetValueOrDefault((irowMin + irow, icolMin + icol), 0)];
            }

            st += "\n";
        }

        var output = st.ToScreenText().ToString();

        output.Should().Be("HJKJKGPH");
    }

    private Dictionary<(int irow, int icol), int> Run(string input, int startColor)
    {
        var mtx = new Dictionary<(int irow, int icol), int>();
        (int irow, int icol) pos = (0, 0);
        (int drow, int dcol) dir = (-1, 0);
        mtx[(0, 0)] = startColor;
        var icm = new IntCodeMachine(input);
        while (true)
        {
            var output = icm.Run(mtx.GetValueOrDefault(pos, 0));
            if (icm.Halted())
            {
                return mtx;
            }

            mtx[pos] = (int)output[0];
            dir = output[1] switch
            {
                0 => (-dir.dcol, dir.drow),
                1 => (dir.dcol, -dir.drow),
                _ => throw new ArgumentException()
            };
            pos = (pos.irow + dir.drow, pos.icol + dir.dcol);
        }
    }
}