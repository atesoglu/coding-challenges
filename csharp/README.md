# C# Implementation

This directory contains C# implementations of coding challenges from AdventOfCode, LeetCode, and HackerRank platforms.

## Structure

```
csharp/
├── Coding-Challenges.sln         Visual Studio solution file
├── Directory.Build.props         Centralized build configuration
│                                (.NET 10.0, C# 14, ImplicitUsings, Nullable)
├── global.json                   .NET SDK version pinning
├── src/
│   ├── AdventOfCode/             AdventOfCode solver console application
│   ├── LeetCode/                 LeetCode solver console application
│   ├── HackerRank/               HackerRank solver console application
│   └── Runner/                   General-purpose utility application
├── tests/
│   ├── AdventOfCode/             Solution implementations (for reference)
│   ├── LeetCode/                 Solution implementations (for reference)
│   └── HackerRank/               Solution implementations (for reference)
└── README.md                     This file
```

## Prerequisites

- .NET SDK 10.0 or later
- Windows, macOS, or Linux

## Building

```bash
# Build all C# projects (uses centralized Directory.Build.props)
dotnet build

# Or individual projects
dotnet build src/AdventOfCode/
dotnet build src/LeetCode/
dotnet build src/HackerRank/
dotnet build src/Runner/
```

## Running Solutions

### AdventOfCode

```bash
dotnet run --project csharp/src/AdventOfCode -- --year 2024 --day 1
```

Output:
```
Y2024 D01
Part 1: <answer> (<time>ms)
Part 2: <answer>
```

### LeetCode

```bash
dotnet run --project csharp/src/LeetCode -- --problem AddTwoNumbers
```

### HackerRank

```bash
dotnet run --project csharp/src/HackerRank -- --problem ProblemName
```

## Configuration

All projects are configured via [Directory.Build.props](Directory.Build.props):

- **TargetFramework**: .NET 10.0 (latest)
- **LangVersion**: latest (C# 14)
- **ImplicitUsings**: enabled
- **Nullable**: enabled
- **Deterministic Builds**: enabled
- **NuGet Packages**: Centralized versions for test frameworks

## Adding a New Solution

### For AdventOfCode

1. Create the solution file in the appropriate test project:
   `tests/AdventOfCode/Solutions/Y{Year}/D{Day}.cs`

2. Implement the class with the `Solve` method:

```csharp
namespace AdventOfCode.Solutions.Y2024
{
    public class D01
    {
        public (string, string) Solve(string input)
        {
            // Parse input and solve
            string part1 = "";
            string part2 = "";
            return (part1, part2);
        }
    }
}
```

3. Run the solver:
```bash
dotnet run --project src/AdventOfCode -- --year 2024 --day 1
```

### For LeetCode / HackerRank

1. Create the solution file: `tests/LeetCode/Solutions/{ProblemName}.cs` or `tests/HackerRank/Solutions/{ProblemName}.cs`
2. Implement with a `Solve` method that returns a string result
3. Run the appropriate runner:
```bash
dotnet run --project src/LeetCode -- --problem AddTwoNumbers
dotnet run --project src/HackerRank -- --problem ProblemName
```

## Architecture

- **src/{Platform}/**: Console runners with CLI argument parsing and reflection-based solution discovery
- **tests/{Platform}/**: Solution implementations organized by year/day or problem name
- **Runner/**: General-purpose utility application for testing and experimentation
- **Directory.Build.props**: Centralized configuration ensuring consistency across all projects

## Notes

- Input files are automatically copied from `../resources/AdventOfCode/Y{Year}/D{Day}/input.txt`
- Runners use reflection to dynamically locate and execute solution classes
- All projects use latest C# language features (C# 14 with nullable reference types enabled)
- No external NuGet dependencies beyond test frameworks
