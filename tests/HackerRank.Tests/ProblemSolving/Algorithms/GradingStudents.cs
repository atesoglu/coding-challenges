namespace HackerRank.Tests.ProblemSolving.Algorithms;

public static class GradingStudents
{
    public static List<int> Solve(List<int>? grades)
    {
        if (grades == null || grades.Count == 0)
        {
            return [];
        }

        var finalGrades = new List<int>();

        grades.ForEach(grade =>
        {
            if (grade < 38)
            {
                finalGrades.Add(grade);
            }
            else
            {
                var nextMultipleOfFive = GetNextMultipleOfFive(grade);

                finalGrades.Add(nextMultipleOfFive - grade < 3 ? nextMultipleOfFive : grade);
            }
        });

        return finalGrades;
    }

    private static int GetNextMultipleOfFive(int number) => number % 5 == 0 ? number : number + (5 - number % 5);
}