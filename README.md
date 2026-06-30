# Coding Challenges - Multi-Language Repository

This repository contains solutions to coding challenges from multiple platforms (**AdventOfCode**, **LeetCode**, **HackerRank**) implemented in multiple programming languages (**C#**, **C++**, **Rust**, **Go**).

## Purpose

This repo aims to:

- Document solutions to algorithmic challenges across multiple platforms
- Practice problem solving in multiple programming languages
- Provide a reference for algorithms, patterns, and techniques
- Enable solving the same problem in different languages using shared input files
- Maintain consistent project structure and naming across all languages

## Repository Structure

```
coding-challenges/
├── csharp/                      C# implementations (.NET 10.0)
│   ├── Coding-Challenges.sln
│   ├── Directory.Build.props   (Centralized build configuration)
│   ├── src/
│   │   ├── AdventOfCode/       AdventOfCode solver console app
│   │   ├── LeetCode/           LeetCode solver console app
│   │   ├── HackerRank/         HackerRank solver console app
│   │   └── Runner/             General-purpose utility & testing
│   ├── tests/
│   │   ├── AdventOfCode/       Solution implementations (for reference)
│   │   ├── LeetCode/           Solution implementations (for reference)
│   │   └── HackerRank/         Solution implementations (for reference)
│   └── README.md
│
├── cpp/                         C++ implementations (C++17, CMake)
│   ├── CMakeLists.txt
│   ├── include/                Header files & runner definitions
│   ├── src/
│   │   ├── AdventOfCode_runner.cpp
│   │   ├── LeetCode_runner.cpp
│   │   ├── HackerRank_runner.cpp
│   │   ├── AdventOfCode/       Solution implementations
│   │   ├── LeetCode/           Solution implementations
│   │   └── HackerRank/         Solution implementations
│   ├── build/                  Build output (generated)
│   └── README.md
│
├── rust/                        Rust implementations (in planning)
├── go/                          Go implementations (in planning)
│
├── resources/                   Shared input files & resources
│   ├── AdventOfCode/           Y{Year}/D{Day}/input.txt (512 files)
│   ├── LeetCode/               Problem inputs (ready for data)
│   └── HackerRank/             Problem inputs (ready for data)
│
└── README.md                    This file
```

## Quick Start

### C# - AdventOfCode
```bash
dotnet run --project csharp/src/AdventOfCode -- --year 2024 --day 1
```

### C# - LeetCode
```bash
dotnet run --project csharp/src/LeetCode -- --problem AddTwoNumbers
```

### C# - HackerRank
```bash
dotnet run --project csharp/src/HackerRank -- --problem ProblemName
```

### C++ - AdventOfCode
```bash
cd cpp
mkdir build && cd build
cmake .. && make
./bin/AdventOfCode --year 2024 --day 1
```

### C++ - LeetCode
```bash
cd cpp/build
./bin/LeetCode --problem AddTwoNumbers
```

### C++ - HackerRank
```bash
cd cpp/build
./bin/HackerRank --problem ProblemName
```

## Languages

- **C#**: .NET 10.0 (latest) with PascalCase projects; console runners using reflection-based solution discovery; centralized configuration via Directory.Build.props
- **C++**: C++17 with CMake 3.15+; header-only runner system with function pointer registration
- **Rust**: Planned implementation (stub with structure in place)
- **Go**: Planned implementation (stub with structure in place)

## Key Features

- **Multi-Language Consistency**: Same project structure (AdventOfCode, LeetCode, HackerRank, Runner) across all languages
- **Shared Resources**: All input files stored once in `resources/` (512 AdventOfCode inputs pre-loaded) and accessed by all implementations
- **Independent Runners**: Each platform has its own executable/console app per language with CLI argument parsing
- **No Test Frameworks**: Solutions output results directly to console for simplicity and consistency
- **Centralized Configuration (C#)**: Single Directory.Build.props file manages all project settings (.NET 10.0, C# 14, ImplicitUsings, Nullable)
- **Language Best Practices**: Each language follows its community conventions (PascalCase for C#, snake_case for Rust, etc.)

## Solution Development

### C# Solutions

Create file: `csharp/tests/AdventOfCode/Solutions/Y{Year}/D{Day}.cs`

```csharp
namespace AdventOfCode.Solutions.Y2024
{
    public class D01
    {
        public (string, string) Solve(string input)
        {
            // Your solution implementation
            return (part1, part2);
        }
    }
}
```

Run: `dotnet run --project csharp/src/AdventOfCode -- --year 2024 --day 1`

### C++ Solutions

Create file: `cpp/src/AdventOfCode/Y2024/D01.cpp` with solution function, then register in `cpp/src/AdventOfCode_runner.cpp`:

```cpp
#include <string>
#include <utility>

namespace AdventOfCode::Y2024::D01 {
    std::pair<std::string, std::string> solve(const std::string& input) {
        // Your solution implementation
        return {part1, part2};
    }
}
```

Register in runner header:
```cpp
#include "../AdventOfCode_runner.hpp"
#include "Y2024/D01.cpp"

// In main function:
AdventOfCode::Runner::registerSolution(2024, 1,
    [](const std::string& input) { return AdventOfCode::Y2024::D01::solve(input); });
```

Build and run:
```bash
cd cpp/build && cmake .. && make
./bin/AdventOfCode --year 2024 --day 1
```

### Rust Solutions (When Implemented)

Structure similar to C# and C++ with module organization by platform and year/day.

### Go Solutions (When Implemented)

Structure similar to C# and C++ with package organization by platform and year/day.

## License

See `LICENSE` for details.

## Disclaimer

These solutions are intended for learning. Do not submit code verbatim to competitive platforms where it violates terms. Use this repository to understand approaches and verify your own solutions.

---

Happy Coding! 🚀
