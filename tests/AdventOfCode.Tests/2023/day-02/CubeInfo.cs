namespace advent_of_code.tests._2023.day_02;

public class CubeInfo(CubeColors color, int count)
{
    public CubeColors Color { get; } = color;
    public int Count { get; } = count;

    public override string ToString() => $"{Count} {Color}";
}