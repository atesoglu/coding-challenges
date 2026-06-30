using System.Diagnostics;
using System.Reflection;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: dotnet run --project csharp/src/AdventOfCode -- --year <year> --day <day>");
            Console.WriteLine("Example: dotnet run --project csharp/src/AdventOfCode -- --year 2024 --day 1");
            return 1;
        }

        int year = 0;
        int day = 0;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--year" && i + 1 < args.Length)
                int.TryParse(args[i + 1], out year);
            else if (args[i] == "--day" && i + 1 < args.Length)
                int.TryParse(args[i + 1], out day);
        }

        if (year == 0 || day == 0)
        {
            Console.WriteLine("Error: Invalid year or day");
            return 1;
        }

        try
        {
            var inputPath = Path.Combine(AppContext.BaseDirectory, "AdventOfCode", $"Y{year}", $"D{day:D2}", "input.txt");
            
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Error: Input file not found for Y{year} D{day:D2}");
                return 1;
            }

            var input = File.ReadAllText(inputPath);
            var className = $"AdventOfCode.Solutions.Y{year}.D{day:D2}";
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
            var result = method.Invoke(instance, new object[] { input });
            stopwatch.Stop();

            if (result is ValueTuple<string, string> tuple)
            {
                Console.WriteLine($"Y{year} D{day:D2}");
                Console.WriteLine($"Part 1: {tuple.Item1} ({stopwatch.ElapsedMilliseconds}ms)");
                // For now, Part 2 is included in the stopwatch time
                Console.WriteLine($"Part 2: {tuple.Item2}");
            }
            else
            {
                Console.WriteLine($"Error: Unexpected return type from Solve method");
                return 1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}
