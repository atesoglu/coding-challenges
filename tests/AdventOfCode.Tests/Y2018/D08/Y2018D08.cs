using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D08;

[ChallengeName("Memory Maneuver")]
public class Y2018D08
{
    private readonly string _input = File.ReadAllText(@"Y2018\D08\Y2018D08-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Parse(_input).fold(0, (cur, node) => cur + node.metadata.Sum());

        output.Should().Be(40848);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Parse(_input).value();

        output.Should().Be(34466);
    }

    Node Parse(string input)
    {
        var nums = input.Split(" ").Select(int.Parse).GetEnumerator();
        var next = () =>
        {
            nums.MoveNext();
            return nums.Current;
        };

        Func<Node> read = null;
        read = () =>
        {
            var node = new Node()
            {
                children = new Node[next()],
                metadata = new int[next()]
            };
            for (var i = 0; i < node.children.Length; i++)
            {
                node.children[i] = read();
            }

            for (var i = 0; i < node.metadata.Length; i++)
            {
                node.metadata[i] = next();
            }

            return node;
        };
        return read();
    }
}

class Node
{
    public Node[] children;
    public int[] metadata;

    public T fold<T>(T seed, Func<T, Node, T> aggregate)
    {
        return children.Aggregate(aggregate(seed, this), (cur, child) => child.fold(cur, aggregate));
    }

    public int value()
    {
        if (children.Length == 0)
        {
            return metadata.Sum();
        }

        var res = 0;
        foreach (var i in metadata)
        {
            if (i >= 1 && i <= children.Length)
            {
                res += children[i - 1].value();
            }
        }

        return res;
    }
}