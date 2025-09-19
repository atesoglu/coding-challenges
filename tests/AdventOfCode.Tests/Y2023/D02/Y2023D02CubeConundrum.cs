namespace AdventOfCode.Tests.Y2023.D02;

public static class Y2023D02CubeConundrum
{
    public static int PartOne(Game[] games, int red, int green, int blue)
    {
        return games.Sum(game =>
            game.Sets.Any(a => a.Cubes.FirstOrDefault(w => w.Color == CubeColors.Red)?.Count > red
                               || a.Cubes.FirstOrDefault(w => w.Color == CubeColors.Green)?.Count > green
                               || a.Cubes.FirstOrDefault(w => w.Color == CubeColors.Blue)?.Count > blue)
                ? 0
                : game.Number);
    }

    public static int PartTwo(Game[] games)
    {
        return games.Sum(game => Math.Max(game.Sets.SelectMany(set => set.Cubes.Where(cube => cube.Color == CubeColors.Red)).Select(c => c.Count).Max(), 1)
                                 * Math.Max(game.Sets.SelectMany(set => set.Cubes.Where(cube => cube.Color == CubeColors.Green)).Select(c => c.Count).Max(), 1)
                                 * Math.Max(game.Sets.SelectMany(set => set.Cubes.Where(cube => cube.Color == CubeColors.Blue)).Select(c => c.Count).Max(), 1)
        );
    }
}

public enum CubeColors
{
    Red,
    Blue,
    Green
}

public record CubeInfo(CubeColors Color, int Count);

public record Game(int Number, ICollection<GameSet> Sets);

public record GameSet(ICollection<CubeInfo> Cubes);