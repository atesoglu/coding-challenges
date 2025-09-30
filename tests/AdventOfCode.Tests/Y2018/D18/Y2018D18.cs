using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D18;

[ChallengeName("Settlers of The North Pole")]
public class Y2018D18
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2018\D18\Y2018D18-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Iterate(10);

        output.Should().Be(483840);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Iterate(1000000000);

        output.Should().Be(219919);
    }


    private int Iterate(int lim)
    {
        var seen = new Dictionary<string, int>();
        var mtx = _lines;

        for (var t = 0; t < lim; t++)
        {
            var hash = string.Join("", mtx);
            if (seen.ContainsKey(hash))
            {
                var loopLength = t - seen[hash];
                var remainingSteps = lim - t - 1;
                var remainingLoops = remainingSteps / loopLength;
                t += remainingLoops * loopLength;
            }
            else
            {
                seen[hash] = t;
            }

            mtx = Step(mtx);
        }

        var res = string.Join("", mtx);
        return Regex.Matches(res, @"\#").Count * Regex.Matches(res, @"\|").Count;
    }

    private static string[] Step(string[] mtx)
    {
        var res = new List<string>();
        var crow = mtx.Length;
        var ccol = mtx[0].Length;

        for (var irow = 0; irow < crow; irow++)
        {
            var line = "";
            for (var icol = 0; icol < ccol; icol++)
            {
                var (tree, lumberyard, empty) = (0, 0, 0);
                foreach (var drow in new[] { -1, 0, 1 })
                {
                    foreach (var dcol in new[] { -1, 0, 1 })
                    {
                        if (drow != 0 || dcol != 0)
                        {
                            var (icolT, irowT) = (icol + dcol, irow + drow);
                            if (icolT >= 0 && icolT < ccol && irowT >= 0 && irowT < crow)
                            {
                                switch (mtx[irowT][icolT])
                                {
                                    case '#': lumberyard++; break;
                                    case '|': tree++; break;
                                    case '.': empty++; break;
                                }
                            }
                        }
                    }
                }

                line += mtx[irow][icol] switch
                {
                    '#' when lumberyard >= 1 && tree >= 1 => '#',
                    '|' when lumberyard >= 3 => '#',
                    '.' when tree >= 3 => '|',
                    '#' => '.',
                    var c => c
                };
            }

            res.Add(line);
        }

        return res.ToArray();
    }
}