using System.Numerics;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D23;

[ChallengeName("Unstable Diffusion")]
public class Y2022D23
{
    private readonly string _input = File.ReadAllText(@"Y2022\D23\Y2022D23-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        // I used complex numbers for a change. The map is represented with a hashset of positions.
        var output = Simulate(Parse(_input)).Select(Area).ElementAt(9);

        output.Should().Be(3757);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Simulate(Parse(_input)).Count();

        output.Should().Be(918);
    }


    private IEnumerable<HashSet<Complex>> Simulate(HashSet<Complex> elves)
    {
        var lookAround = new Queue<Complex>(new[] { N, S, W, E });

        for (var fixpoint = false; !fixpoint; lookAround.Enqueue(lookAround.Dequeue()))
        {
            // 1) collect proposals; for each position (key) compute the list of the elves 
            //    who want to step there
            var proposals = new Dictionary<Complex, List<Complex>>();

            foreach (var elf in elves)
            {
                var lonely = Directions.All(dir => !elves.Contains(elf + dir));
                if (lonely)
                {
                    continue;
                }

                foreach (var dir in lookAround)
                {
                    // elf proposes a postion if nobody stands in that direction
                    var proposes = ExtendDir(dir).All(d => !elves.Contains(elf + d));
                    if (proposes)
                    {
                        var pos = elf + dir;
                        if (!proposals.ContainsKey(pos))
                        {
                            proposals[pos] = new List<Complex>();
                        }

                        proposals[pos].Add(elf);
                        break;
                    }
                }
            }

            // 2) move elves, compute fixpoint flag
            fixpoint = true;
            foreach (var p in proposals)
            {
                var (to, from) = p;
                if (from.Count == 1)
                {
                    elves.Remove(from.Single());
                    elves.Add(to);
                    fixpoint = false;
                }
            }

            yield return elves;
        }
    }

    private double Area(HashSet<Complex> elves)
    {
        // smallest enclosing rectangle
        var width = elves.Select(p => p.Real).Max() -
            elves.Select(p => p.Real).Min() + 1;

        var height = elves.Select(p => p.Imaginary).Max() -
            elves.Select(p => p.Imaginary).Min() + 1;

        return width * height - elves.Count;
    }

    private static HashSet<Complex> Parse(string input)
    {
        var lines = input.Split("\n");
        return (
            from irow in Enumerable.Range(0, lines.Length)
            from icol in Enumerable.Range(0, lines[0].Length)
            where lines[irow][icol] == '#'
            select new Complex(icol, irow)
        ).ToHashSet();
    }

    ///  -------
    private static Complex N = new Complex(0, -1);

    private static Complex E = new Complex(1, 0);
    private static Complex S = new Complex(0, 1);
    private static Complex W = new Complex(-1, 0);
    private static Complex NW = N + W;
    private static Complex NE = N + E;
    private static Complex SE = S + E;
    private static Complex SW = S + W;

    private static Complex[] Directions = new[] { NW, N, NE, E, SE, S, SW, W };

    // Extends an ordinal position with its intercardinal neighbours
    private static Complex[] ExtendDir(Complex dir) =>
        dir == N ? new[] { NW, N, NE } :
        dir == E ? new[] { NE, E, SE } :
        dir == S ? new[] { SW, S, SE } :
        dir == W ? new[] { NW, W, SW } :
        throw new Exception();
}