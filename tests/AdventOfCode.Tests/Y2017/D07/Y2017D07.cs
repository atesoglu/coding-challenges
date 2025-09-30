using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2017.D07;

[ChallengeName("Recursive Circus")]
public partial class Y2017D07
{
    private readonly string[] _lines = File.ReadAllLines(@"Y2017\D07\Y2017D07-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Root(Parse()).Id;

        output.Should().Be("cyrupz");
    }

    [Fact]
    public void PartTwo()
    {
        var tree = Parse();
        var root = Root(tree);
        ComputeTreeWeights(root, tree);
        var bogusChild = BogusChild(root, tree);
        var desiredWeight = tree[root.Children.First(childId => childId != bogusChild.Id)].TreeWeight;

        var output = Fix(bogusChild, desiredWeight, tree);

        output.Should().Be(193);
    }

    private Tree Parse()
    {
        var tree = new Tree();
        foreach (var line in _lines)
        {
            var parts = NodeParserRegex().Match(line);

            tree.Add(parts.Groups["id"].Value, new Node
            {
                Id = parts.Groups["id"].Value,
                Weight = int.Parse(parts.Groups["weight"].Value),
                Children = string.IsNullOrEmpty(parts.Groups["children"].Value) ? [] : NodeSplitterRegex().Split(parts.Groups["children"].Value),
            });
        }

        return tree;
    }

    private static Node Root(Tree tree) => tree.Values.First(node => !tree.Values.Any(nodeParent => nodeParent.Children.Contains(node.Id)));

    private static int ComputeTreeWeights(Node node, Tree tree)
    {
        node.TreeWeight = node.Weight + node.Children.Select(childId => ComputeTreeWeights(tree[childId], tree)).Sum();
        return node.TreeWeight;
    }

    private static Node BogusChild(Node node, Tree tree)
    {
        var w = (from childId in node.Children
            let child = tree[childId]
            group child by child.TreeWeight
            into childrenByTreeWeight
            orderby childrenByTreeWeight.Count()
            select childrenByTreeWeight).ToArray();

        return w.Length == 1 ? null : w[0].Single();
    }

    private static int Fix(Node node, int desiredWeight, Tree tree)
    {
        while (true)
        {
            if (node.Children.Length < 2)
            {
                throw new NotImplementedException();
            }

            var bogusChild = BogusChild(node, tree);

            if (bogusChild == null)
            {
                return desiredWeight - node.TreeWeight + node.Weight;
            }
            else
            {
                desiredWeight = desiredWeight - node.TreeWeight + bogusChild.TreeWeight;
                node = bogusChild;
            }
        }
    }

    private class Node
    {
        public string Id;
        public string[] Children;
        public int Weight;
        public int TreeWeight = -1;
    }

    private class Tree : Dictionary<string, Node>
    {
    }

    [GeneratedRegex(@"(?<id>[a-z]+) \((?<weight>[0-9]+)\)( -> (?<children>.*))?")]
    private static partial Regex NodeParserRegex();

    [GeneratedRegex(", ")]
    private static partial Regex NodeSplitterRegex();
}