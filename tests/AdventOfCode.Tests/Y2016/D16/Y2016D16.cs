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

        output.Should().Be("0");
    }

    [Fact]
    public void PartTwo()
    {
        var output = Checksum(_input, 35651584);

        output.Should().Be("0");
    }


    string Checksum(string st, int length)
    {
        while (st.Length < length)
        {
            var a = st;
            var b = string.Join("", from ch in a.Reverse() select ch == '0' ? '1' : '0');
            st = a + "0" + b;
        }

        st = st.Substring(0, length);
        var sb = new StringBuilder();

        while (sb.Length % 2 == 0)
        {
            sb.Clear();
            for (int i = 0; i < st.Length; i += 2)
            {
                sb.Append(st[i] == st[i + 1] ? "1" : "0");
            }

            st = sb.ToString();
        }

        return st;
    }
}