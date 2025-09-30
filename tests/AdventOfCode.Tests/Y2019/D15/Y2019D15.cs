using System.Collections.Immutable;
using System.Text;
using AdventOfCode.Tests.Y2019.D02;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2019.D15;

[ChallengeName("Oxygen System")]
public class Y2019D15
{
    private readonly string _input = File.ReadAllText(@"Y2019\D15\Y2019D15-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var iicm = new ImmutableIntCodeMachine(_input);
        var output = Bfs(iicm).First(s => s.tile == Tile.O2).path.Count;

        output.Should().Be(246);
    }

    [Fact]
    public void PartTwo()
    {
        var iicm = Bfs(new ImmutableIntCodeMachine(_input)).First(s => s.tile == Tile.O2).iicm;
        var output = Bfs(iicm).Last().path.Count;
        output.Should().Be(376);
    }


    private enum Tile
    {
        Wall = 0,
        Empty = 1,
        O2 = 2,
    }

    private static IEnumerable<(ImmutableIntCodeMachine iicm, ImmutableList<int> path, Tile tile)> Bfs(ImmutableIntCodeMachine startIicm)
    {
        (int dx, int dy)[] dirs = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

        var seen = new HashSet<(int x, int y)> { (0, 0) };
        var q = new Queue<(ImmutableIntCodeMachine iicm, ImmutableList<int> path, int x, int y)>();
        q.Enqueue((startIicm, ImmutableList<int>.Empty, 0, 0));
        while (q.Any())
        {
            var current = q.Dequeue();

            for (var i = 0; i < dirs.Length; i++)
            {
                var (nextX, nextY) = (current.x + dirs[i].dx, current.y + dirs[i].dy);

                if (!seen.Contains((nextX, nextY)))
                {
                    seen.Add((nextX, nextY));
                    var nextPath = current.path.Add(i + 1);
                    var (nextIicm, output) = current.iicm.Run(i + 1);

                    var tile = (Tile)output.Single();
                    if (tile != Tile.Wall)
                    {
                        yield return (nextIicm, nextPath, tile);
                        q.Enqueue((nextIicm, nextPath, nextX, nextY));
                    }
                }
            }
        }
    }
}