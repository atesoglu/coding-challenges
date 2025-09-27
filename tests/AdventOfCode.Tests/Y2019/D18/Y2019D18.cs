using System.Text;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Tests.Y2019.D18;

[ChallengeName("Many-Worlds Interpretation")]
public class Y2019D18
{
    private readonly string _input = File.ReadAllText(@"Y2019\D18\Y2019D18-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = PartOne(_input);

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var output = PartTwo(_input);

        output.Should().Be(0);
    }


    private object PartOne(string input)
    {
        var maze = new Maze(input);


        var dependencies = GenerateDependencies(maze);
        return Solve(maze);
    }


    private object PartTwo(string input)
    {
        var d = 0;
        foreach (var subMaze in GenerateSubMazes(input))
        {
            var maze = new Maze(subMaze);

            var dependencies = GenerateDependencies(maze);
            d += Solve(maze);
        }

        return d;
    }

    IEnumerable<string> GenerateSubMazes(string input)
    {
        var mx = input.Split("\n").Select(x => x.ToCharArray()).ToArray();
        var crow = mx.Length;
        var ccol = mx[0].Length;
        var hrow = crow / 2;
        var hcol = ccol / 2;
        var pattern = "@#@\n###\n@#@".Split();
        foreach (var drow in new[] { -1, 0, 1 })
        {
            foreach (var dcol in new[] { -1, 0, 1 })
            {
                mx[hrow + drow][hcol + dcol] = pattern[1 + drow][1 + dcol];
            }
        }

        foreach (var (drow, dcol) in new[] { (0, 0), (0, hcol + 1), (hrow + 1, 0), (hrow + 1, hcol + 1) })
        {
            var res = "";
            for (var irow = 0; irow < hrow; irow++)
            {
                res += string.Join("", mx[irow + drow].Skip(dcol).Take(hcol)) + "\n";
            }

            for (var ch = 'A'; ch <= 'Z'; ch++)
            {
                if (!res.Contains(char.ToLower(ch)))
                {
                    res = res.Replace(ch, '.');
                }
            }

            res = res.Substring(0, res.Length - 1);
            yield return res;
        }
    }


    int Solve(Maze maze)
    {
        var dependencies = GenerateDependencies(maze);
        var cache = new Dictionary<string, int>();

        int SolveRecursive(char currentItem, ImmutableHashSet<char> keys
        )
        {
            if (keys.Count == 0)
            {
                return 0;
            }

            var cacheKey = currentItem + string.Join("", keys);

            if (!cache.ContainsKey(cacheKey))
            {
                var result = int.MaxValue;
                foreach (var key in keys)
                {
                    if (dependencies[key].Intersect(keys).Count == 0)
                    {
                        var d = maze.Distance(currentItem, key) + SolveRecursive(key, keys.Remove(key));
                        result = Math.Min(d, result);
                    }
                }

                cache[cacheKey] = result;
            }

            return cache[cacheKey];
        }

        return SolveRecursive('@', dependencies.Keys.ToImmutableHashSet());
    }

    Dictionary<char, ImmutableHashSet<char>> GenerateDependencies(Maze maze)
    {
        var q = new Queue<((int irow, int icol) pos, string dependsOn)>();
        var pos = maze.Find('@');
        var dependsOn = "";
        q.Enqueue((pos, dependsOn));

        var res = new Dictionary<char, ImmutableHashSet<char>>();
        var seen = new HashSet<(int irow, int icol)>();
        seen.Add(pos);
        while (q.Any())
        {
            (pos, dependsOn) = q.Dequeue();

            foreach (var (drow, dcol) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
            {
                var posT = (pos.irow + drow, pos.icol + dcol);
                var ch = maze.Look(posT);

                if (seen.Contains(posT) || ch == '#')
                {
                    continue;
                }

                seen.Add(posT);
                var dependsOnT = dependsOn;

                if (char.IsLower(ch))
                {
                    res[ch] = ImmutableHashSet.CreateRange(dependsOn);
                }

                if (char.IsLetter(ch))
                {
                    dependsOnT += char.ToLower(ch);
                }

                q.Enqueue((posT, dependsOnT));
            }
        }

        return res;
    }
}