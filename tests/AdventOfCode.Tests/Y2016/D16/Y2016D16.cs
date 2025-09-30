using System.Text;
using FluentAssertions;
using System.Linq;

namespace AdventOfCode.Tests.Y2016.D16;

[ChallengeName("Dragon Checksum")]
public class Y2016D16
{
    private readonly string _input = File.ReadAllText(@"Y2016\D16\Y2016D16-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Checksum(_input, 272);

        output.Should().Be("10010010110011010");
    }

    [Fact]
    public void PartTwo()
    {
        var output = Checksum(_input, 35651584);

        output.Should().Be("01010100101011100");
    }


    private static string Checksum(string st, int length)
    {
        // 1. Fill data to required length
        while (st.Length < length)
        {
            var a = st;
            var b = string.Join("", a.Reverse().Select(ch => ch == '0' ? '1' : '0'));
            st = a + "0" + b;
        }

        st = st.Substring(0, length);

        // 2. Compute checksum until odd length
        while (st.Length % 2 == 0)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < st.Length; i += 2)
            {
                sb.Append(st[i] == st[i + 1] ? '1' : '0');
            }
            st = sb.ToString();
        }

        return st;
    }

}