using System.Text;
using FluentAssertions;

namespace advent_of_code.tests._2023.day_03;

/// <summary>
/// --- Day 3: Gear Ratios ---
/// You and the Elf eventually reach a gondola lift station; he says the gondola lift will take you up to the water source, but this is as far as he can bring you. You go inside.
///
/// It doesn't take long to find the gondolas, but there seems to be a problem: they're not moving.
///
/// "Aaah!"
///
/// You turn around to see a slightly-greasy Elf with a wrench and a look of surprise. "Sorry, I wasn't expecting anyone!
/// The gondola lift isn't working right now; it'll still be a while before I can fix it." You offer to help.
///
/// The engineer explains that an engine part seems to be missing from the engine, but nobody can figure out which one.
/// If you can add up all the part numbers in the engine schematic, it should be easy to work out which part is missing.
///
/// The engine schematic (your puzzle input) consists of a visual representation of the engine.
/// There are lots of numbers and symbols you don't really understand, but apparently any number adjacent to a symbol, even diagonally, is a "part number" and should be included in your sum.
/// (Periods (.) do not count as a symbol.)
/// </summary>
public class AoC202303
{
    private readonly string[] _lines = File.ReadAllLines(@"2023\day-03\input.txt", Encoding.UTF8).Select(x => $"{x}.").ToArray();
    private readonly Dictionary<(int x, int y), List<int>> _gears;

    public AoC202303()
    {
        _gears = new Dictionary<(int x, int y), List<int>>();
        var neighbors = new Dictionary<(int x, int y), (int x, int y)>();
        for (var y = 0; y < _lines.Length; y++)
        {
            for (var x = 0; x < _lines[y].Length; x++)
            {
                if (_lines[y][x] == '.' || char.IsDigit(_lines[y][x]))
                {
                    continue;
                }

                _gears[(x, y)] = new List<int>();

                neighbors[(x - 1, y - 1)] = (x, y);
                neighbors[(x, y - 1)] = (x, y);
                neighbors[(x + 1, y - 1)] = (x, y);

                neighbors[(x - 1, y)] = (x, y);
                neighbors[(x + 1, y)] = (x, y);

                neighbors[(x - 1, y + 1)] = (x, y);
                neighbors[(x, y + 1)] = (x, y);
                neighbors[(x + 1, y + 1)] = (x, y);
            }
        }

        for (var y = 0; y < _lines.Length; y++)
        {
            var buf = "";
            var adjacentGears = new HashSet<(int x, int y)>();
            for (var x = 0; x < _lines[y].Length; x++)
            {
                if (char.IsDigit(_lines[y][x]))
                {
                    buf += _lines[y][x];
                    if (neighbors.ContainsKey((x, y)))
                    {
                        adjacentGears.Add(neighbors[(x, y)]);
                    }
                }
                else
                {
                    if (buf.Length > 0 && adjacentGears.Count > 0)
                    {
                        var num = int.Parse(buf);
                        foreach (var adjacentGear in adjacentGears)
                        {
                            _gears[adjacentGear].Add(num);
                        }
                    }

                    adjacentGears.Clear();
                    buf = "";
                }
            }
        }
    }

    /// <summary>
    /// Here is an example engine schematic:
    ///
    /// 467..114..
    /// ...*......
    /// ..35..633.
    /// ......#...
    /// 617*......
    /// .....+.58.
    /// ..592.....
    /// ......755.
    /// ...$.*....
    /// .664.598..
    /// In this schematic, two numbers are not part numbers because they are not adjacent to a symbol: 114 (top right) and 58 (middle right).
    /// Every other number is adjacent to a symbol and so is a part number; their sum is 4361.
    ///
    /// Of course, the actual engine schematic is much larger. What is the sum of all of the part numbers in the engine schematic?
    /// </summary>
    [Fact]
    public void PartOne()
    {
        var sum = _gears.Sum(x => x.Value.Sum());
        sum.Should().Be(531561);
    }

    /// <summary>
    /// --- Part Two ---
    /// The engineer finds the missing part and installs it in the engine! As the engine springs to life, you jump in the closest gondola, finally ready to ascend to the water source.
    ///
    /// You don't seem to be going very fast, though. Maybe something is still wrong? Fortunately, the gondola has a phone labeled "help", so you pick it up and the engineer answers.
    ///
    /// Before you can explain the situation, she suggests that you look out the window. There stands the engineer, holding a phone in one hand and waving with the other. You're going so slowly that you haven't even left the station. You exit the gondola.
    ///
    /// The missing part wasn't the only issue - one of the gears in the engine is wrong. A gear is any * symbol that is adjacent to exactly two part numbers. Its gear ratio is the result of multiplying those two numbers together.
    ///
    /// This time, you need to find the gear ratio of every gear and add them all up so that the engineer can figure out which gear needs to be replaced.
    ///
    /// Consider the same engine schematic again:
    ///
    /// 467..114..
    /// ...*......
    /// ..35..633.
    /// ......#...
    /// 617*......
    /// .....+.58.
    /// ..592.....
    /// ......755.
    /// ...$.*....
    /// .664.598..
    /// In this schematic, there are two gears. The first is in the top left; it has part numbers 467 and 35, so its gear ratio is 16345. The second gear is in the lower right; its gear ratio is 451490. (The * adjacent to 617 is not a gear because it is only adjacent to one part number.) Adding up all of the gear ratios produces 467835.
    ///
    /// What is the sum of all of the gear ratios in your engine schematic?
    /// </summary>
    [Fact]
    public void PartTwo()
    {
        var sum = _gears.Where(x => x.Value.Count == 2).Sum(x => x.Value[0] * x.Value[1]);
        sum.Should().Be(83279367);
    }
}