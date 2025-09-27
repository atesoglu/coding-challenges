using System.Text;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using ChildToParent = System.Collections.Generic.Dictionary<string, string>;

namespace AdventOfCode.Tests.Y2019.D06;

[ChallengeName("Universal Orbit Map")]
public class Y2019D06
{
    private readonly string _input = File.ReadAllText(@"Y2019\D06\Y2019D06-input.txt", Encoding.UTF8);

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
        var childToParent = ParseTree(input);
        return (
            from node in childToParent.Keys
            select GetAncestors(childToParent, node).Count()
        ).Sum();
    }

    private object PartTwo(string input)
    {
        var childToParent = ParseTree(input);
        var ancestors1 = new Stack<string>(GetAncestors(childToParent, "YOU"));
        var ancestors2 = new Stack<string>(GetAncestors(childToParent, "SAN"));
        while (ancestors1.Peek() == ancestors2.Peek())
        {
            ancestors1.Pop();
            ancestors2.Pop();
        }

        return ancestors1.Count + ancestors2.Count;
    }

    ChildToParent ParseTree(string input) =>
        input
            .Split("\n")
            .Select(line => line.Split(")"))
            .ToDictionary(
                parent_child => parent_child[1],
                parent_child => parent_child[0]
            );

    IEnumerable<string> GetAncestors(ChildToParent childToParent, string node)
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