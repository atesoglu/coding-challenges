using FluentAssertions;

namespace AdventOfCode.Tests.Y2023.D04;

public class Y2023D04ScratchcardsTests
{
    private readonly Card[] _cards = File.ReadAllLines(@"Y2023\D04\Y2023D04Scratchcards-input.txt")
        .Where(line => !string.IsNullOrWhiteSpace(line))
        .Select(line =>
        {
            var parts = line.Split(": ");
            var cardNumber = int.Parse(parts[0].Split(" ").Last());
            var numbers = parts[1].Split(" | ");
            var winningNumbers = numbers[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            var ourNumbers = numbers[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

            return new Card(cardNumber, winningNumbers, ourNumbers);
        })
        .ToArray();

    private readonly Card[] _sampleData =
    [
        new Card(1, [41, 48, 83, 86, 17], [83, 86, 6, 31, 17, 9, 48, 53]),
        new Card(2, [13, 32, 20, 16, 61], [61, 30, 68, 82, 17, 32, 24, 19]),
        new Card(3, [1, 21, 53, 59, 44], [69, 82, 63, 72, 16, 21, 14, 1]),
        new Card(4, [41, 92, 73, 84, 69], [59, 84, 76, 51, 58, 5, 54, 83]),
        new Card(5, [87, 83, 26, 28, 32], [88, 30, 70, 12, 93, 22, 82, 36]),
        new Card(6, [31, 18, 13, 56, 72], [74, 77, 10, 23, 35, 67, 36, 11])
    ];

    [Fact]
    public void PartOneWithSampleData()
    {
        var output = Y2023D04Scratchcards.PartOne(_sampleData);

        output.Should().Be(13);
    }

    [Fact]
    public void PartTwoWithSampleData()
    {
        var output = Y2023D04Scratchcards.PartTwo(_sampleData);

        output.Should().Be(30);
    }

    [Fact]
    public void PartOneWithRealInput()
    {
        var output = Y2023D04Scratchcards.PartOne(_cards);

        output.Should().Be(20107);
    }

    [Fact]
    public void PartTwoWithRealInput()
    {
        var output = Y2023D04Scratchcards.PartTwo(_cards);

        output.Should().Be(8172507);
    }
}