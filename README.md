# Coding Challenges

Solutions and tests for coding problems from platforms like **LeetCode**, **HackerRank**, and seasonal events like **Advent of Code**. This repository is organized as a .NET solution with projects per source/platform and is designed to be built and validated via automated tests.

## Purpose

This repo aims to:

- Document solutions to algorithmic challenges over time.
- Practice clean, test-driven problem solving in C#/.NET.
- Provide a browsable reference of techniques, patterns, and trade-offs.
- Encourage learning and collaboration through readable code and tests.

## Projects in this Solution

The Visual Studio solution (`Coding-Challenges.sln`) currently includes the following projects:

- `tests/LeetCode.Tests` — Unit tests (and often the implementations) for LeetCode problems.
- `tests/HackerRank.Tests` — Unit tests (and often the implementations) for HackerRank challenges.
- `tests/AdventOfCode.Tests` — Unit tests and inputs for Advent of Code (multiple years/days).

Note: Implementations for problems may live alongside their tests within these projects. For Advent of Code, you will also find input files and per-day structures under `tests/AdventOfCode.Tests`.

## Repository Layout

- `Coding-Challenges.sln` — Solution file referencing all projects and solution items.
- `tests/` — All test projects, each grouping problems by platform or event.
  - `LeetCode.Tests/`
  - `HackerRank.Tests/`
  - `AdventOfCode.Tests/` (contains many problems, inputs, and helpers)
- `README.md`, `LICENSE`, `.gitignore`
- `global.json` — Pins the .NET SDK (see below)

## Prerequisites

- .NET SDK 9.0 (see `global.json` for the pinned version).
  - Install from the official .NET download page.

## Getting Started

1. Clone the repository
   ```bash
   git clone <this-repo-url>
   cd coding-challenges
   ```
2. Build the solution
   ```bash
   dotnet build
   ```
3. Run all tests
   ```bash
   dotnet test
   ```

### Run tests for a single project

```bash
dotnet test tests/LeetCode.Tests/LeetCode.Tests.csproj
dotnet test tests/HackerRank.Tests/HackerRank.Tests.csproj
dotnet test tests/AdventOfCode.Tests/AdventOfCode.Tests.csproj
```

### Filter tests by name or trait

```bash
dotnet test --filter FullyQualifiedName~TwoSum
dotnet test tests/AdventOfCode.Tests/AdventOfCode.Tests.csproj --filter TestCategory=Day05
```

## How to Navigate

- Browse the `tests` projects to find problems by platform/event.
- For Advent of Code, look for folders/files named by year and day; inputs are usually included under the same project tree.
- Many problems include both the solution and its tests side by side to keep each task self-contained.

## Contributing / Adding New Solutions

- Prefer adding new problems within the corresponding `tests/*` project to keep implementations co-located with tests.
- Follow consistent naming (problem name or AoC year/day) and include a clear, minimal set of tests for correctness and edge cases.
- If you introduce shared helpers, keep them small and well-named; avoid over-abstracting.
- Open a PR with a brief explanation of the approach and any complexity notes.

## License

See `LICENSE` for details.

## Disclaimer

These solutions are intended for learning. Do not submit code verbatim to competitive platforms where it violates terms. Use this repository to understand approaches and verify your own solutions.

---

Happy Coding! 🚀
