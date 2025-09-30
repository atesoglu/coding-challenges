using System.Text;
using FluentAssertions;
using AsteroidsByDir = System.Collections.Generic.Dictionary<(int drow, int dcol), System.Collections.Generic.List<(int irow, int icol)>>;

namespace AdventOfCode.Tests.Y2019.D10;

[ChallengeName("Monitoring Station")]
public class Y2019D10
{
    private readonly string _input = File.ReadAllText(@"Y2019\D10\Y2019D10-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = SelectStationPosition(_input).asteroidsByDir.Count;

        output.Should().Be(256);
    }

    [Fact]
    public void PartTwo()
    {
        var asteroid = Destroy(_input).ElementAt(199);

        var output = (asteroid.icol * 100 + asteroid.irow);

        output.Should().Be(1707);
    }


    private IEnumerable<(int irow, int icol)> Destroy(string input)
    {
        var (station, asteroidsByDir) = SelectStationPosition(input);

        foreach (var dir in asteroidsByDir.Keys.ToArray())
        {
            asteroidsByDir[dir] = asteroidsByDir[dir]
                .OrderBy(a => Math.Abs(a.irow - station.irow) + Math.Abs(a.icol - station.icol))
                .ToList();
        }

        foreach (var dir in Rotate(asteroidsByDir.Keys))
        {
            if (asteroidsByDir.ContainsKey(dir))
            {
                var asteroid = asteroidsByDir[dir].First();
                asteroidsByDir[dir].RemoveAt(0);

                yield return asteroid;

                if (!asteroidsByDir[dir].Any())
                {
                    asteroidsByDir.Remove(dir);
                }
            }
        }
    }

    private static IEnumerable<(int drow, int dcol)> Rotate(IEnumerable<(int drow, int dcol)> dirs)
    {
        var ordered = dirs.OrderBy(dir => -Math.Atan2(dir.dcol, dir.drow)).ToList();
        for (var i = 0;; i++)
        {
            yield return ordered[i % ordered.Count];
        }
    }

    private ((int irow, int icol) station, AsteroidsByDir asteroidsByDir) SelectStationPosition(string input)
    {
        var res = ((0, 0), asteroidsByDir: new AsteroidsByDir());
        var asteroids = Asteroids(input);

        foreach (var station in asteroids)
        {
            var asteroidsByDir = new AsteroidsByDir();
            foreach (var asteroid in asteroids)
            {
                if (station != asteroid)
                {
                    var (rowDir, colDir) = (asteroid.irow - station.irow, asteroid.icol - station.icol);
                    var gcd = Math.Abs(Gcd(rowDir, colDir));
                    var dir = (rowDir / gcd, colDir / gcd);

                    if (!asteroidsByDir.ContainsKey(dir))
                    {
                        asteroidsByDir[dir] = new List<(int irow, int icol)>();
                    }

                    asteroidsByDir[dir].Add(asteroid);
                }
            }

            if (asteroidsByDir.Count > res.asteroidsByDir.Count)
            {
                res = (station, asteroidsByDir);
            }
        }

        return res;
    }

    private static List<(int irow, int icol)> Asteroids(string input)
    {
        var map = input.Split("\n");
        var (crow, ccol) = (map.Length, map[0].Length);

        return (
            from irow in Enumerable.Range(0, crow)
            from icol in Enumerable.Range(0, ccol)
            where map[irow][icol] == '#'
            select (irow, icol)
        ).ToList();
    }

    private static int Gcd(int a, int b) => b == 0 ? a : Gcd(b, a % b);
}