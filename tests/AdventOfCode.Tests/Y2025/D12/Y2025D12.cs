using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2025.D12;

[ChallengeName("Christmas Tree Farm")]
public class Y2025D12
{
    private readonly string _input = File.ReadAllText(@"Y2025\D12\Y2025D12-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var blocks = _input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

        var treeBlocks = blocks[..^1];
        var todoBlock = blocks[^1];

        var areas = treeBlocks.Select(b => b.Count(c => c == '#')).ToArray();

        var todos = ParseTodos(todoBlock);

        var output = 0;

        foreach (var todo in todos)
        {
            var requiredArea = 0;

            for (var i = 0; i < todo.Counts.Length; i++)
            {
                requiredArea += todo.Counts[i] * areas[i];
            }

            if (requiredArea <= todo.Width * todo.Height)
            {
                output++;
            }
        }

        output.Should().Be(521);
    }

    private static Todo[] ParseTodos(string block)
    {
        var lines = block.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var todos = new List<Todo>();

        foreach (var line in lines)
        {
            var nums = Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToArray();

            var w = nums[0];
            var h = nums[1];
            var counts = nums.Skip(2).ToArray();

            todos.Add(new Todo(w, h, counts));
        }

        return todos.ToArray();
    }

    private record Todo(int Width, int Height, int[] Counts);
}