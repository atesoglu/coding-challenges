using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D25;

[ChallengeName("Code Chronicle")]
public class Y2024D25
{
    private readonly string _input = File.ReadAllText(@"Y2024\D25\Y2024D25-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        int[] parsePattern(string[] lines) =>
            Enumerable.Range(0, lines[0].Length).Select(x =>
                Enumerable.Range(0, lines.Length).Count(y => lines[y][x] == '#')
            ).ToArray();

        bool match(int[] k, int[] l) =>
            Enumerable.Range(0, k.Length).All(i => k[i] + l[i] <= 7);

        var patterns = _input.Split("\n\n").Select(b => b.Split("\n"));
        var keys = patterns.Where(p => p[0][0] == '.').Select(parsePattern).ToList();
        var locks = patterns.Where(p => p[0][0] == '#').Select(parsePattern).ToList();


        var output = keys.Sum(k => locks.Count(l => match(l, k)));

        output.Should().Be(0);
    }
}