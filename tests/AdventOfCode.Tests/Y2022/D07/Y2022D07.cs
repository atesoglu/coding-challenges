using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2022.D07;

[ChallengeName("No Space Left On Device")]
public class Y2022D07
{
    private readonly string _input = File.ReadAllText(@"Y2022\D07\Y2022D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = GetDirectorySizes(_input).Where(size => size < 100000).Sum();

        output.Should().Be(0);
    }

    [Fact]
    public void PartTwo()
    {
        var directorySizes = GetDirectorySizes(_input);
        var freeSpace = 70000000 - directorySizes.Max();

        var output = directorySizes.Where(size => size + freeSpace >= 30000000).Min();

        output.Should().Be(0);
    }

    private List<int> GetDirectorySizes(string input)
    {
        var path = new Stack<string>();
        var sizes = new Dictionary<string, int>();
        foreach (var line in input.Split("\n"))
        {
            if (line == "$ cd ..")
            {
                path.Pop();
            }
            else if (line.StartsWith("$ cd"))
            {
                path.Push(string.Join("", path) + line.Split(" ")[2]);
            }
            else if (Regex.Match(line, @"\d+").Success)
            {
                var size = int.Parse(line.Split(" ")[0]);
                foreach (var dir in path)
                {
                    sizes[dir] = sizes.GetValueOrDefault(dir) + size;
                }
            }
        }

        return sizes.Values.ToList();
    }
}