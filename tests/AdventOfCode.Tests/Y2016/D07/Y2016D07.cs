using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2016.D07;

[ChallengeName("Internet Protocol Version 7")]
public class Y2016D07
{
    private readonly string _input = File.ReadAllText(@"Y2016\D07\Y2016D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = _input.Split('\n').Count(TLS);

        output.Should().Be(110);
    }

    [Fact]
    public void PartTwo()
    {
        var output = _input.Split('\n').Count(SSL);

        output.Should().Be(242);
    }


    private bool TLS(string st) =>
        Classify(st).Any(c => !c.f && Abba(c.st).Any()) &&
        Classify(st).All(c => !c.f || !Abba(c.st).Any());

    private bool SSL(string st) => (
        from c1 in Classify(st)
        from c2 in Classify(st)
        where !c1.f && c2.f
        from aba in Aba(c1.st)
        let bab = $"{aba[1]}{aba[0]}{aba[1]}"
        where c2.st.Contains(bab)
        select true
    ).Any();

    private IEnumerable<(string st, bool f)> Classify(string st)
    {
        var part = "";
        for (var i = 0; i < st.Length; i++)
        {
            var ch = st[i];
            if (ch == '[')
            {
                yield return (part, false);
                part = "";
            }
            else if (ch == ']')
            {
                yield return (part, true);
                part = "";
            }
            else
            {
                part += ch;
            }
        }

        if (part != "")
            yield return (part, false);
    }

    private IEnumerable<string> Abba(string st)
    {
        for (var i = 0; i < st.Length - 3; i++)
        {
            if (st[i + 2] == st[i + 1] && st[i] == st[i + 3] && st[i] != st[i + 2])
                yield return st.Substring(i, 4);
        }
    }

    private IEnumerable<string> Aba(string st)
    {
        for (var i = 0; i < st.Length - 2; i++)
        {
            if (st[i] == st[i + 2] && st[i] != st[i + 1])
                yield return st.Substring(i, 3);
        }
    }
}