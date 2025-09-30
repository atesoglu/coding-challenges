using System.Text;
using AdventOfCode.Tests.Y2020.D20;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2020.D24;

[ChallengeName("Lobby Layout")]
public class Y2020D24
{
    private readonly string _input = File.ReadAllText(@"Y2020\D24\Y2020D24-input.txt", Encoding.UTF8);

    private readonly Dictionary<string, (int x, int y)> hexDirections = new Dictionary<string, (int x, int y)>
    {
        { "o", (0, 0) },
        { "ne", (1, 1) },
        { "nw", (-1, 1) },
        { "e", (2, 0) },
        { "w", (-2, 0) },
        { "se", (1, -1) },
        { "sw", (-1, -1) },
    };

    [Fact]
    public void PartOne()
    {
        var output = ParseBlackTiles(_input).Count();

        output.Should().Be(465);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Enumerable.Range(0, 100)
            .Aggregate(ParseBlackTiles(_input), (blackTiles, _) => Flip(blackTiles))
            .Count();

        output.Should().Be(4078);
    }


    private IEnumerable<Tile> Neighbourhood(Tile tile) =>
        from dir in hexDirections.Values select new Tile(tile.x + dir.x, tile.y + dir.y);

    private HashSet<Tile> Flip(HashSet<Tile> blackTiles)
    {
        var tiles = (
            from black in blackTiles
            from tile in Neighbourhood(black)
            select tile
        ).ToHashSet();

        return (
            from tile in tiles
            let blacks = Neighbourhood(tile).Count(n => blackTiles.Contains(n))
            where blacks == 2 || blacks == 3 && blackTiles.Contains(tile)
            select tile
        ).ToHashSet();
    }

    private HashSet<Tile> ParseBlackTiles(string input)
    {
        // Normalize line endings to just "\n"
        input = input.Replace("\r\n", "\n").TrimEnd();

        var tiles = new Dictionary<Tile, bool>();

        foreach (var line in input.Split("\n"))
        {
            var tile = Walk(line);
            tiles[tile] = !tiles.GetValueOrDefault(tile);
        }

        return (from kvp in tiles where kvp.Value select kvp.Key).ToHashSet();
    }

    private Tile Walk(string line)
    {
        var (x, y) = (0, 0);
        while (line != "")
        {
            foreach (var kvp in hexDirections)
            {
                if (line.StartsWith(kvp.Key))
                {
                    line = line.Substring(kvp.Key.Length);
                    (x, y) = (x + kvp.Value.x, y + kvp.Value.y);
                }
            }
        }

        return new Tile(x, y);
    }
}

internal record Tile(int x, int y);