using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D06;

[ChallengeName("Tuning Trouble")]
public class Y2022D06
{
    private readonly string _input = File.ReadAllText(@"Y2022\D06\Y2022D06-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = StartOfBlock(_input, 4);

        output.Should().Be(1651);
    }

    [Fact]
    public void PartTwo()
    {
        var output = StartOfBlock(_input, 14);

        output.Should().Be(3837);
    }


    // Slides a window of length l over the input and finds the first position
    // where each character is different. Returns the right end of the window.
    private int StartOfBlock(string input, int l) =>
        Enumerable.Range(l, input.Length)
            .First(i => input.Substring(i - l, l).ToHashSet().Count == l);
}