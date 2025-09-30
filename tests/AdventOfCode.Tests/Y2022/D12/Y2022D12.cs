using System.Collections.Immutable;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D12;

[ChallengeName("Hill Climbing Algorithm")]
public class Y2022D12
{
    private readonly string _input = File.ReadAllText(@"Y2022\D12\Y2022D12-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetPois(_input)
            .Single(poi => poi.symbol == startSymbol)
            .distanceFromGoal;

        output.Should().Be(380);
    }

    [Fact]
    public void PartTwo()
    {
        var output = GetPois(_input)
            .Where(poi => poi.elevation == lowestElevation)
            .Select(poi => poi.distanceFromGoal)
            .Min();

        output.Should().Be(375);
    }


    // I feel like a cartographer today
    private record struct Coord(int lat, int lon);

    // we have two 'char' like things, let's introduce wrappers to keep them well separated in code
    private record struct Symbol(char value);

    private record struct Elevation(char value);

    // locations on the map will be represented by the following structure of points-of-interests.
    private record struct Poi(Symbol symbol, Elevation elevation, int distanceFromGoal);

    private Symbol startSymbol = new Symbol('S');
    private Symbol goalSymbol = new Symbol('E');
    private Elevation lowestElevation = new Elevation('a');
    private Elevation highestElevation = new Elevation('z');


    private IEnumerable<Poi> GetPois(string input)
    {
        var map = ParseMap(input);
        var goal = map.Keys.Single(point => map[point] == goalSymbol);

        // starting from the goal symbol compute shortest paths for each point of 
        // the map using a breadth-first search.
        var poiByCoord = new Dictionary<Coord, Poi>()
        {
            { goal, new Poi(goalSymbol, GetElevation(goalSymbol), 0) }
        };

        var q = new Queue<Coord>();
        q.Enqueue(goal);
        while (q.Any())
        {
            var thisCoord = q.Dequeue();
            var thisPoi = poiByCoord[thisCoord];

            foreach (var nextCoord in Neighbours(thisCoord).Where(map.ContainsKey))
            {
                if (poiByCoord.ContainsKey(nextCoord))
                {
                    continue;
                }

                var nextSymbol = map[nextCoord];
                var nextElevation = GetElevation(nextSymbol);

                if (thisPoi.elevation.value - nextElevation.value <= 1)
                {
                    poiByCoord[nextCoord] = new Poi(
                        symbol: nextSymbol,
                        elevation: nextElevation,
                        distanceFromGoal: thisPoi.distanceFromGoal + 1
                    );
                    q.Enqueue(nextCoord);
                }
            }
        }

        return poiByCoord.Values;
    }

    private Elevation GetElevation(Symbol symbol) =>
        symbol.value switch
        {
            'S' => lowestElevation,
            'E' => highestElevation,
            _ => new Elevation(symbol.value)
        };

    // locations are parsed into a dictionary so that valid coordinates and
    // neighbours are easy to deal with
    private static ImmutableDictionary<Coord, Symbol> ParseMap(string input)
    {
        var lines = input.Split("\n");
        return (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Coord, Symbol>(
                new Coord(x, y), new Symbol(lines[y][x])
            )
        ).ToImmutableDictionary();
    }

    private static IEnumerable<Coord> Neighbours(Coord coord) =>
        new[]
        {
            coord with { lat = coord.lat + 1 },
            coord with { lat = coord.lat - 1 },
            coord with { lon = coord.lon + 1 },
            coord with { lon = coord.lon - 1 },
        };
}