using System.Diagnostics;

namespace Sandbox;

internal static class Program
{
    private static readonly Random Rand = new Random();

    private static int _max = 5;
    private static bool _skipWeekend = true;
    private static int _years = 2;

    private static void Main(string[] args)
    {
        ParseCommandLine(args);

        var end = DateTime.Now;
        var start = end.AddYears(-_years);
        start = end.AddDays(-5);
        start = new DateTime(2025, 3, 7);

        for (var d = start; d < end; d = d.AddDays(1))
        {
            if (_skipWeekend && d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
            {
                continue;
            }

            var commits = Rand.Next(_max);
            Console.WriteLine($"Committing {commits} times on {d}");

            for (var i = 0; i < commits; i++)
            {
                var formattedDate = d.ToString("ddd MMM dd HH:mm:ss -0700 yyyy");
                var commitMessage = $"Commit from {d:yyyy-MM-dd}";

                ExecuteGitCommand("git", $"commit --allow-empty --date \"{formattedDate}\" -m \"{commitMessage}\"");
            }
        }
    }

    private static void ParseCommandLine(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-max":
                    _max = int.Parse(args[++i]);
                    break;
                case "-skip-weekend":
                    _skipWeekend = bool.Parse(args[++i]);
                    break;
                case "-years":
                    _years = int.Parse(args[++i]);
                    break;
            }
        }
    }

    private static void ExecuteGitCommand(string command, string arguments)
    {
        var psi = new ProcessStartInfo(command, arguments)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = psi;
        process.Start();
        process.WaitForExit();
    }
}