using System.Numerics;
using System.Text;
using FluentAssertions;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using Beam = (System.Numerics.Complex pos, System.Numerics.Complex dir);

namespace AdventOfCode.Tests.Y2023.D16;

[ChallengeName("The Floor Will Be Lava")]
public class Y2023D16
{
    private readonly string _input = File.ReadAllText(@"Y2023\D16\Y2023D16-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = EnergizedCells(ParseMap(_input), (Complex.Zero, Right));

        output.Should().Be(7927);
    }

    [Fact]
    public void PartTwo()
    {
        var map = ParseMap(_input);
        var output = (from beam in StartBeams(map) select EnergizedCells(map, beam)).Max();

        output.Should().Be(8246);
    }


    static readonly Complex Up = -Complex.ImaginaryOne;
    static readonly Complex Down = Complex.ImaginaryOne;
    static readonly Complex Left = -Complex.One;
    static readonly Complex Right = Complex.One;

    // follow the beam in the map and return the energized cell count.
    int EnergizedCells(Map map, Beam beam)
    {
        // this is essentially just a flood fill algorithm.
        var q = new Queue<Beam>([beam]);
        var seen = new HashSet<Beam>();

        while (q.TryDequeue(out beam))
        {
            seen.Add(beam);
            foreach (var dir in Exits(map[beam.pos], beam.dir))
            {
                var pos = beam.pos + dir;
                if (map.ContainsKey(pos) && !seen.Contains((pos, dir)))
                {
                    q.Enqueue((pos, dir));
                }
            }
        }

        return seen.Select(beam => beam.pos).Distinct().Count();
    }

    // go around the edges (top, right, bottom, left order) of the map
    // and return the inward pointing directions
    IEnumerable<Beam> StartBeams(Map map)
    {
        var maxX = map.Keys.Max(p => p.Real);
        var maxY = map.Keys.Max(p => p.Imaginary);

        return
        [
            ..from pos in map.Keys where pos.Real == 0      select (pos, Right),
            ..from pos in map.Keys where pos.Real == maxX   select (pos, Left),
            ..from pos in map.Keys where pos.Imaginary == 0 select (pos, Down),
            ..from pos in map.Keys where pos.Imaginary == maxY select (pos, Up),
        ];
    }


    // using a dictionary helps with bounds check (simply containskey):
    Map ParseMap(string input)
    {
        var lines = input.TrimEnd().Split('\n');
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            let cell = lines[irow][icol]
            let pos = new Complex(icol, irow)
            select new KeyValuePair<Complex, char>(pos, cell)
        ).ToDictionary();
    }

    // the 'exit' direction(s) of the given cell when entered by a beam moving in 'dir'
    // we have some special cases for mirrors and spliters, the rest keeps the direction
    Complex[] Exits(char cell, Complex dir) => cell switch
    {
        '-' when dir == Up || dir == Down => [Left, Right],
        '|' when dir == Left || dir == Right => [Up, Down],
        '/' => [-new Complex(dir.Imaginary, dir.Real)],
        '\\' => [new Complex(dir.Imaginary, dir.Real)],
        _ => [dir]
    };
}