using FluentAssertions;
using Solutions.HackerRank.ProblemSolving.Algorithms;

namespace Tests.HackerRank.ProblemSolving;

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
}