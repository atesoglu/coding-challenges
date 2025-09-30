using System.Text;
using FluentAssertions;

namespace AdventOfCode.Tests.Y2015.D03;

[ChallengeName("Perfectly Spherical Houses in a Vacuum")]
public class Y2015D03
{
    private readonly string _input = File.ReadAllText(@"Y2015\D03\Y2015D03-input.txt", Encoding.UTF8);

    [Fact]
    public void PartOne()
    {
        var output = CountVisitedHouses(_input, numberOfDeliverers: 1);

        output.Should().Be(2565);
    }

    [Fact]
    public void PartTwo()
    {
        var output = CountVisitedHouses(_input, numberOfDeliverers: 2);

        output.Should().Be(2639);
    }

    private static int CountVisitedHouses(string directions, int numberOfDeliverers)
    {
        var visited = new HashSet<(int row, int col)>();
        var positions = Enumerable.Repeat((row: 0, col: 0), numberOfDeliverers).ToArray();

        visited.Add((0, 0));

        var currentDeliverer = 0;
        foreach (var move in directions)
        {
            positions[currentDeliverer] = Move(positions[currentDeliverer], move);
            visited.Add(positions[currentDeliverer]);

            currentDeliverer = (currentDeliverer + 1) % numberOfDeliverers;
        }

        return visited.Count;
    }

    private static (int row, int col) Move((int row, int col) position, char direction) => direction switch
    {
        '^' => (position.row - 1, position.col),
        'v' => (position.row + 1, position.col),
        '<' => (position.row, position.col - 1),
        '>' => (position.row, position.col + 1),
        _ => position
    };
}