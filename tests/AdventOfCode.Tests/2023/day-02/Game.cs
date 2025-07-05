namespace advent_of_code.tests._2023.day_02;

public class Game(int number, ICollection<GameSet> sets)
{
    public int Number { get; } = number;
    public ICollection<GameSet> Sets { get; } = sets;

    public override string ToString() => $"Game {Number}: {string.Join("; ", Sets)}";
}