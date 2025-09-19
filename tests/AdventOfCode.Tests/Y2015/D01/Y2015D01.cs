namespace AdventOfCode.Tests.Y2015.D01;

[ChallengeName("Not Quite Lisp")]
public class Y2015D01
{
    public int PartOne(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return 0;
        }

        return input.Sum(c => c == '(' ? 1 : -1);
    }

    public int PartTwo(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return 0;
        }

        var step = EnumerateFloors(input).FirstOrDefault(p => p.Floor == -1);
        return step?.Position ?? 0;
    }

    private static IEnumerable<FloorStep> EnumerateFloors(string input)
    {
        var floor = 0;
        for (var i = 0; i < input.Length; i++)
        {
            floor += input[i] == '(' ? 1 : -1;
            yield return new FloorStep(i + 1, floor);
        }
    }

    private record FloorStep(int Position, int Floor);
}