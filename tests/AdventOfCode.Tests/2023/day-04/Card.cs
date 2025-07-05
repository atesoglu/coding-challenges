namespace advent_of_code.tests._2023.day_04;

public class Card(int number, IEnumerable<int> winningNumbers, IEnumerable<int> ourNumbers)
{
    public int Number { get; } = number;
    public IEnumerable<int> WinningNumbers { get; } = winningNumbers;
    public IEnumerable<int> OurNumbers { get; } = ourNumbers;
}