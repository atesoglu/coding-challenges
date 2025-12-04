using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D04;

[ChallengeName("Y2025D04ChallengeName")]
public class Y2025D04
{
    private readonly string _input = File.ReadAllText(@"Y2025\D04\Y2025D04-input.txt", Encoding.UTF8);
    private readonly HashSet<(int Row, int Col)> _rolls;

    public Y2025D04()
    {
        _rolls = Parse(_input);
    }

    [Fact]
    public void PartOne()
    {
        var output = Cleanup1(_rolls);

        output.Should().Be(1464);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Cleanup2(_rolls);

        output.Should().Be(8409);
    }

    // --------------------------------------------------------------------
    // Parse the grid and collect all '@' cells as (row, col) tuples
    // --------------------------------------------------------------------
    private HashSet<(int Row, int Col)> Parse(string input)
    {
        var result = new HashSet<(int, int)>();
        var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        for (var r = 0; r < lines.Length; r++)
        {
            for (var c = 0; c < lines[r].Length; c++)
            {
                if (lines[r][c] == '@')
                    result.Add((r, c));
            }
        }

        return result;
    }

    // --------------------------------------------------------------------
    // Cleanup rule #1: remove all rolls that have fewer than 4 neighbours
    // in the surrounding 8 cells. Returns # removed.
    // --------------------------------------------------------------------
    private int Cleanup1(HashSet<(int Row, int Col)> rolls)
    {
        var toRemove = new List<(int, int)>();

        foreach (var roll in rolls)
        {
            if (IsWeak(rolls, roll))
                toRemove.Add(roll);
        }

        foreach (var r in toRemove)
            rolls.Remove(r);

        return toRemove.Count;
    }

    // --------------------------------------------------------------------
    // Cleanup rule #2: repeat Cleanup1 until stable.
    // Returns total # removed.
    // --------------------------------------------------------------------
    private int Cleanup2(HashSet<(int Row, int Col)> rolls)
    {
        var initial = rolls.Count;

        while (true)
        {
            var removed = Cleanup1(rolls);
            if (removed == 0)
                break;
        }

        return initial - rolls.Count;
    }

    // --------------------------------------------------------------------
    // A roll is "weak" (i.e. eligible for removal) if it has < 4 neighbours.
    // --------------------------------------------------------------------
    private static bool IsWeak(HashSet<(int Row, int Col)> rolls, (int Row, int Col) roll)
    {
        var neighbours = 0;

        for (var dr = -1; dr <= 1; dr++)
        {
            for (var dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0)
                    continue;

                var neighbour = (roll.Row + dr, roll.Col + dc);

                if (rolls.Contains(neighbour))
                    neighbours++;
            }
        }

        return neighbours < 4;
    }
}