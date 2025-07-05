namespace advent_of_code.tests._2023.day_02;

public class GameSet
{
    public ICollection<CubeInfo> Cubes { get; } = new List<CubeInfo>();

    public override string ToString() => string.Join(", ", Cubes);
}