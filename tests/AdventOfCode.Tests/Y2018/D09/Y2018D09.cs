using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2018.D09;

[ChallengeName("Marble Mania")]
public class Y2018D09
{
    private readonly string _input = File.ReadAllText(@"Y2018\D09\Y2018D09-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = Solve(_input, 1);

        output.Should().Be(439341);
    }

    [Fact]
    public void PartTwo()
    {
        var output = Solve(_input, 100);

        output.Should().Be(3566801385);
    }


    long Solve(string input, int mul)
    {
        var match = Regex.Match(input, @"(?<players>\d+) players; last marble is worth (?<points>\d+) points");
        var players = new long[int.Parse(match.Groups["players"].Value)];
        var targetPoints = int.Parse(match.Groups["points"].Value) * mul;

        var current = new Node { value = 0 };
        current.left = current;
        current.right = current;

        var points = 1;
        var iplayer = 1;
        while (points <= targetPoints)
        {
            if (points % 23 == 0)
            {
                for (var i = 0; i < 7; i++)
                {
                    current = current.left;
                }

                players[iplayer] += points + current.value;

                var left = current.left;
                var right = current.right;
                right.left = left;
                left.right = right;
                current = right;
            }
            else
            {
                var left = current.right;
                var right = current.right.right;
                current = new Node { value = points, left = left, right = right };
                left.right = current;
                right.left = current;
            }

            points++;
            iplayer = (iplayer + 1) % players.Length;
        }

        return players.Max();
    }
}

class Node
{
    public int value;
    public Node left;
    public Node right;
}