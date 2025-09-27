using System.Numerics;
using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D20;

[ChallengeName("Race Condition")]
public class Y2024D20
{
    private readonly string _input = File.ReadAllText(@"Y2024\D20\Y2024D20-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, 2);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, 20);

        output.Should().Be(0);
    }


    int Solve(string input, int cheat)
    {
        var path = GetPath(input);
        var indices = Enumerable.Range(0, path.Length).ToArray();

        // sum up the worthy cheats for each index along the path
        var cheatsFromI = (int i) => (
            from j in indices[0..i]
            let dist = Manhattan(path[i], path[j])
            let saving = i - (j + dist)
            where dist <= cheat && saving >= 100
            select 1
        ).Sum();

        // parallel is gold today, it gives us an 3-4x boost
        return indices.AsParallel().Select(cheatsFromI).Sum();
    }

    int Manhattan(Complex a, Complex b) =>
        (int)(Math.Abs(a.Imaginary - b.Imaginary) + Math.Abs(a.Real - b.Real));

    // Follow the path from finish to start, supposed that there is a single track in the input.
    // The index of a position in the returned array equals to its distance from the finish
    Complex[] GetPath(string input)
    {
        var lines = input.Split("\n");
        var map = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Complex.ImaginaryOne, lines[y][x])
        ).ToDictionary();

        Complex[] dirs = [-1, 1, Complex.ImaginaryOne, -Complex.ImaginaryOne];

        var start = map.Keys.Single(k => map[k] == 'S');
        var goal = map.Keys.Single(k => map[k] == 'E');

        var (prev, cur) = ((Complex?)null, goal);
        var res = new List<Complex> { cur };

        while (cur != start)
        {
            var dir = dirs.Single(dir => map[cur + dir] != '#' && cur + dir != prev);
            (prev, cur) = (cur, cur + dir);
            res.Add(cur);
        }

        return res.ToArray();
    }
}