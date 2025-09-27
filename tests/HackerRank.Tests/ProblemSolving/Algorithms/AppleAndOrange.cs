namespace HackerRank.Tests.ProblemSolving.Algorithms;

public static class AppleAndOrange
{
    /// <summary>
    /// Prints the number of apples and oranges that land on Sam's house, each on a separate line.
    /// </summary>
    /// <param name="s">s: integer, starting point of Sam's house location.</param>
    /// <param name="t">t: integer, ending location of Sam's house location.</param>
    /// <param name="a">a: integer, location of the Apple tree.</param>
    /// <param name="b">b: integer, location of the Orange tree.</param>
    /// <param name="apples">apples: integer array, distances at which each apple falls from the tree.</param>
    /// <param name="oranges">oranges: integer array, distances at which each orange falls from the tree.</param>
    /// <returns></returns>
    public static List<int> Solve(int s, int t, int a, int b, List<int> apples, List<int> oranges)
    {
        var applePositions = new List<int>();
        var orangePositions = new List<int>();

        apples.ForEach(d => applePositions.Add(a + d));
        oranges.ForEach(d => orangePositions.Add(b + d));

        return
        [
            applePositions.Count(w => w >= s && w <= t),
            orangePositions.Count(w => w >= s && w <= t)
        ];
    }
}