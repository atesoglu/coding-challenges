using System.Text;
using FluentAssertions;
using System;
using System.Linq;

namespace AdventOfCode.Tests.Y2018.D08;

[ChallengeName("Memory Maneuver")]
public class Y2018D08
{
    private readonly string _input = File.ReadAllText(@"Y2018\D08\Y2018D08-input.txt", Encoding.UTF8);

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


    private object PartOne(string input) =>
        Parse(input).fold(0, (cur, node) => cur + node.metadata.Sum());


    private object PartTwo(string input)
    {
        return Parse(input).value();
    }

    Node Parse(string input)
    {
        var nums = input.Split(" ").Select(int.Parse).GetEnumerator();
        Func<int> next = () =>
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