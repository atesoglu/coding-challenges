namespace AdventOfCode.Tests._2015;

public static class Year2015Day01NotQuiteLisp
{
    public static object PartOne(string input) => Levels(input).Last().level;

    public static object PartTwo(string input) => Levels(input).First(p => p.level == -1).idx;

    static IEnumerable<(int idx, int level)> Levels(string input)
    {
        var level = 0;
        for (var i = 0; i < input.Length; i++)
        {
            level += input[i] == '(' ? 1 : -1;
            yield return (i + 1, level);
        }
    }
}