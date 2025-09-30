using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D25;

[ChallengeName("Full of Hot Air")]
public class Y2022D25
{
    private readonly string _input = File.ReadAllText(@"Y2022\D25\Y2022D25-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = LongToSnafu(_input.Split("\n").Select(SnafuToLong).Sum());

        output.Should().Be("2=01-0-2-0=-0==-1=01");
    }


    // This is just string to number conversion in base 5
    // with the two special digits that's worth -2 and -1.
    private long SnafuToLong(string snafu)
    {
        var res = 0L;
        foreach (var digit in snafu)
        {
            res = res * 5;
            switch (digit)
            {
                case '=': res += -2; break;
                case '-': res += -1; break;
                case '0': res += 0; break;
                case '1': res += 1; break;
                case '2': res += 2; break;
            }
        }

        return res;
    }

    // Snafu numbers have digits -2, -1, 0, 1 and 2, so this is almost 
    // standard base 5 conversion, but when dealing with digits 3 and 4 we 
    // need to increment the higher decimal place so that we have
    // something to subtract 2 and 1 from.
    private string LongToSnafu(long d)
    {
        var res = "";
        while (d > 0)
        {
            switch (d % 5)
            {
                case 0: res = '0' + res; break;
                case 1: res = '1' + res; break;
                case 2: res = '2' + res; break;
                // add 5 and emit -2 because 3 = 5 -2
                case 3:
                    d += 5;
                    res = '=' + res;
                    break;
                // add 5 and emit -1 because 4 = 5 -1
                case 4:
                    d += 5;
                    res = '-' + res;
                    break;
            }

            d /= 5;
        }

        return res;
    }
}