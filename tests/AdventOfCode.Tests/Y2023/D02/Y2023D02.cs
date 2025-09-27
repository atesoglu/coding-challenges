using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Tests.Y2023.D02;

[ChallengeName("Cube Conundrum")]
public class Y2023D02
{
    private readonly string _input = File.ReadAllText(@"Y2023\D02\Y2023D02-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = (
            from line in _input.Split("\n")
            let game = ParseGame(line)
            where game.red <= 12 && game.green <= 13 && game.blue <= 14
            select game.id
        ).Sum();

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = (
            from line in _input.Split("\n")
            let game = ParseGame(line)
            select game.red * game.green * game.blue
        ).Sum();

        output.Should().Be(0);
    }


    // no need to keep track of the individual rounds in a game, just return
    // the maximum of the red, green, blue boxes
    Game ParseGame(string line) =>
        new Game(
            ParseInts(line, @"Game (\d+)").First(),
            ParseInts(line, @"(\d+) red").Max(),
            ParseInts(line, @"(\d+) green").Max(),
            ParseInts(line, @"(\d+) blue").Max()
        );

    // extracts integers from a string identified by a single regex group.
    IEnumerable<int> ParseInts(string st, string rx) =>
        from m in Regex.Matches(st, rx)
        select int.Parse(m.Groups[1].Value);
}
record Game(int id, int red, int green, int blue);