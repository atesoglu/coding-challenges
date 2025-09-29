using System.Collections.Immutable;
using System.Numerics;
using System.Text;
using FluentAssertions;
using Map = System.Collections.Immutable.IImmutableDictionary<System.Numerics.Complex, char>;

namespace AdventOfCode.Tests.Y2024.D15;

[ChallengeName("Warehouse Woes")]
public class Y2024D15
{
    private readonly string _input = File.ReadAllText(@"Y2024\D15\Y2024D15-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = CalculateScore(_input);

        output.Should().Be(1511865);
    }

    [Fact]
    public void PartTwo()
    {
        var output = CalculateScore(ScaleUpMap(_input));

        output.Should().Be(1519991);
    }


    static Complex Up = -Complex.ImaginaryOne;
    static Complex Down = Complex.ImaginaryOne;
    static Complex Left = -1;
    static Complex Right = 1;

    double CalculateScore(string input)
    {
        var (map, steps) = ParseMapAndSteps(input);

        var robotPosition = map.Keys.Single(key => map[key] == '@');
        foreach (var direction in steps)
        {
            if (TryMoveRobot(ref map, robotPosition, direction))
            {
                robotPosition += direction;
            }
        }

        return map.Keys
            .Where(key => map[key] == '[' || map[key] == 'O')
            .Sum(box => box.Real + 100 * box.Imaginary);
    }

    bool TryMoveRobot(ref Map map, Complex position, Complex direction)
    {
        var originalMap = map;

        if (map[position] == '.')
        {
            return true;
        }
        else if (map[position] == 'O' || map[position] == '@')
        {
            if (TryMoveRobot(ref map, position + direction, direction))
            {
                map = map
                    .SetItem(position + direction, map[position])
                    .SetItem(position, '.');
                return true;
            }
        }
        else if (map[position] == ']')
        {
            return TryMoveRobot(ref map, position + Left, direction);
        }
        else if (map[position] == '[')
        {
            if (direction == Left)
            {
                if (TryMoveRobot(ref map, position + Left, direction))
                {
                    map = map
                        .SetItem(position + Left, '[')
                        .SetItem(position, ']')
                        .SetItem(position + Right, '.');
                    return true;
                }
            }
            else if (direction == Right)
            {
                if (TryMoveRobot(ref map, position + 2 * Right, direction))
                {
                    map = map
                        .SetItem(position, '.')
                        .SetItem(position + Right, '[')
                        .SetItem(position + 2 * Right, ']');
                    return true;
                }
            }
            else
            {
                if (TryMoveRobot(ref map, position + direction, direction) && TryMoveRobot(ref map, position + Right + direction, direction))
                {
                    map = map
                        .SetItem(position, '.')
                        .SetItem(position + Right, '.')
                        .SetItem(position + direction, '[')
                        .SetItem(position + direction + Right, ']');
                    return true;
                }
            }
        }

        map = originalMap;
        return false;
    }

    string ScaleUpMap(string input) =>
        input.Replace("#", "##").Replace(".", "..").Replace("O", "[]").Replace("@", "@.");

    (Map, Complex[]) ParseMapAndSteps(string input)
    {
        var blocks = input.Split("\n\n");
        var lines = blocks[0].Split("\n");
        var map = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, lines[0].Length)
            select new KeyValuePair<Complex, char>(x + y * Down, lines[y][x])
        ).ToImmutableDictionary();

        var steps = blocks[1].ReplaceLineEndings("").Select(character =>
            character switch
            {
                '^' => Up,
                '<' => Left,
                '>' => Right,
                'v' => Down,
                _ => throw new Exception()
            });

        return (map, steps.ToArray());
    }
}