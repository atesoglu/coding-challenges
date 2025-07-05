using FluentAssertions;
using HackerRank.Tests.ProblemSolving.Algorithms;

namespace HackerRank.Tests.ProblemSolving;

public class AlgorithmTests
{
    [Fact]
    public void GradingStudentsTest()
    {
        var input = new List<int> { 4, 73, 67, 38, 33 };
        var expected = new List<int> { 4, 75, 67, 40, 33 };

        var output = GradingStudents.Solve(input);

        output.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void AppleAndOrangeTest()
    {
        var houseX0 = 7;
        var houseX1 = 10;
        var appleTreeX = 4;
        var orangeTreeX = 12;
        var apples = new List<int> { 2, 3, -4 };
        var oranges = new List<int> { 3, -2, -4 };

        var expected = new List<int> { 1, 2 };

        var output = AppleAndOrange.Solve(houseX0, houseX1, appleTreeX, orangeTreeX, apples, oranges);

        output.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(2, 1, 1, 2, "YES")]
    public void KangarooTest(int xPosition, int xRate, int yPosition, int yRate, string expected)
    {
        var output = Kangaroo.Solve(xPosition, xRate, yPosition, yRate);

        output.Should().BeEquivalentTo(expected);
    }
}