using System.Diagnostics;
using System.Reflection;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: dotnet run --project csharp/src/LeetCode -- --problem <ProblemName>");
            Console.WriteLine("Example: dotnet run --project csharp/src/LeetCode -- --problem AddTwoNumbers");
            return 1;
        }

        string? problemName = null;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--problem" && i + 1 < args.Length)
                problemName = args[i + 1];
        }

        if (string.IsNullOrEmpty(problemName))
        {
            Console.WriteLine("Error: Invalid problem name");
            return 1;
        }

        try
        {
            var className = $"LeetCode.Solutions.{problemName}";
            var type = Type.GetType(className);

            if (type == null)
            {
                Console.WriteLine($"Error: Solution class not found: {className}");
                return 1;
            }

            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("Solve", BindingFlags.Public | BindingFlags.Instance);

            if (method == null)
            {
                Console.WriteLine($"Error: Solve method not found in {className}");
                return 1;
            }

            var stopwatch = Stopwatch.StartNew();
            var result = method.Invoke(instance, Array.Empty<object>());
            stopwatch.Stop();

            Console.WriteLine($"{problemName}");
            Console.WriteLine($"Result: {result} ({stopwatch.ElapsedMilliseconds}ms)");

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}
