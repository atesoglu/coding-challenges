namespace AdventOfCode.Tests.Y2023.D03;

public static class Y2023D03GearRatios
{
    public static int PartOne(Dictionary<(int x, int y), List<int>> gears)
    {
        return gears.Sum(x => x.Value.Sum());
    }

    public static int PartTwo(Dictionary<(int x, int y), List<int>> gears)
    {
        return gears.Where(x => x.Value.Count == 2).Sum(x => x.Value[0] * x.Value[1]);
    }
}