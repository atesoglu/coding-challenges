using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2024.D05;

[ChallengeName("Print Queue")]
public class Y2024D05
{
    private readonly string _input = File.ReadAllText(@"Y2024\D05\Y2024D05-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var (updates, comparer) = Parse(_input);

        var output = updates
            .Where(pages => Sorted(pages, comparer))
            .Sum(GetMiddlePage);

        output.Should().Be(6034);
    }

    [Fact]
    public void PartTwo()
    {
        var (updates, comparer) = Parse(_input);

        var output = updates
            .Where(pages => !Sorted(pages, comparer))
            .Select(pages => pages.OrderBy(p => p, comparer).ToArray())
            .Sum(GetMiddlePage);

        output.Should().Be(6305);
    }

    (string[][] updates, Comparer<string>) Parse(string input)
    {
        var parts = input.Split("\n\n");

        var ordering = new HashSet<string>(parts[0].Split("\n"));
        var comparer =
            Comparer<string>.Create((p1, p2) => ordering.Contains(p1 + "|" + p2) ? -1 : 1);

        var updates = parts[1].Split("\n").Select(line => line.Split(",")).ToArray();
        return (updates, comparer);
    }

    int GetMiddlePage(string[] nums) => int.Parse(nums[nums.Length / 2]);

    bool Sorted(string[] pages, Comparer<string> comparer) =>
        Enumerable.SequenceEqual(pages, pages.OrderBy(x => x, comparer));
}