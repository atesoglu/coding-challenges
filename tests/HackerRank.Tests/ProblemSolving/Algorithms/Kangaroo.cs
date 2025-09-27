namespace HackerRank.Tests.ProblemSolving.Algorithms;

public abstract class Kangaroo
{
    public static string Solve(int xPosition, int xRate, int yPosition, int yRate)
    {
        var distance = yPosition - xPosition;
        var rateDifference = xRate - yRate;

        // If rateDifference is 0, they will only meet if the positions are already the same
        if (rateDifference == 0)
        {
            return distance == 0 ? "YES" : "NO";
        }

        // Check if distance is divisible by rateDifference and they converge
        if (distance % rateDifference == 0 && distance / rateDifference >= 0)
        {
            return "YES";
        }

        return "NO";
    }
}