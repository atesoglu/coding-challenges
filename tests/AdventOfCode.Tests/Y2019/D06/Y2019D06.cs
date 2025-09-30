using System.Text;
using FluentAssertions;
using ChildToParent = System.Collections.Generic.Dictionary<string, string>;

namespace AdventOfCode.Tests.Y2019.D06;

[ChallengeName("Universal Orbit Map")]
public class Y2019D06
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2019\D06\Y2019D06-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var childToParent = ParseTree();
        var output = (
            from node in childToParent.Keys
            select GetAncestors(childToParent, node).Count()
        ).Sum();

        output.Should().Be(117672);
    }

    [Fact]
    public void PartTwo()
    {
        var childToParent = ParseTree();
        var ancestors1 = new Stack<string>(GetAncestors(childToParent, "YOU"));
        var ancestors2 = new Stack<string>(GetAncestors(childToParent, "SAN"));
        while (ancestors1.Peek() == ancestors2.Peek())
        {
            ancestors1.Pop();
            ancestors2.Pop();
        }

        var output = ancestors1.Count + ancestors2.Count;

        output.Should().Be(277);
    }

    private ChildToParent ParseTree() => _lines
        .Select(line => line.Split(")"))
        .ToDictionary(
            parent_child => parent_child[1],
            parent_child => parent_child[0]
        );

    private static IEnumerable<string> GetAncestors(ChildToParent childToParent, string node)
    {
        for (
            var parent = childToParent[node];
            parent != null;
            parent = childToParent.GetValueOrDefault(parent, null)
        )
        {
            yield return parent;
        }
    }
}