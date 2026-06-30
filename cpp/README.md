# C++ Implementation

This directory contains C++ implementations of coding challenges from AdventOfCode, LeetCode, and HackerRank platforms, using a header-only runner system with function pointer registration.

## Structure

```
cpp/
├── CMakeLists.txt               Build configuration (creates 3 targets)
├── include/                     Header files
│   ├── AdventOfCode_runner.hpp  AdventOfCode runner with solution registry
│   ├── LeetCode_runner.hpp      LeetCode runner with solution registry
│   ├── HackerRank_runner.hpp    HackerRank runner with solution registry
│   └── input_reader.hpp         Shared input file loader
├── src/                         Source files
│   ├── AdventOfCode_runner.cpp  Main executable (CLI parsing, reflection)
│   ├── LeetCode_runner.cpp      Main executable (CLI parsing, reflection)
│   ├── HackerRank_runner.cpp    Main executable (CLI parsing, reflection)
│   ├── AdventOfCode/            Solution implementations (Y{Year}/D{Day}.cpp)
│   ├── LeetCode/                Solution implementations ({ProblemName}.cpp)
│   └── HackerRank/              Solution implementations ({ProblemName}.cpp)
├── build/                       Build output (generated)
└── README.md                    This file
```

## Prerequisites

- CMake 3.15 or later
- C++17 compatible compiler
- Linux, macOS, or Windows (with MSVC or MinGW)

**Tested on**: Fedora 45, GCC/Clang with C++17 support

## Building

```bash
cd cpp

# Create and configure build directory
mkdir build
cd build
cmake ..

# Build all targets
make

# Or use CMake directly
cmake --build .
```

**Output executables:**
- `bin/AdventOfCode`
- `bin/LeetCode`
- `bin/HackerRank`

## Running Solutions

### AdventOfCode

```bash
./bin/AdventOfCode --year 2024 --day 1
```

Output:
```
Y2024 D01
Part 1: <answer> (<time>ms)
Part 2: <answer>
```

### LeetCode

```bash
./bin/LeetCode --problem AddTwoNumbers
```

### HackerRank

```bash
./bin/HackerRank --problem ProblemName
```

## Adding a New Solution

### For AdventOfCode

1. Create solution file: `src/AdventOfCode/Y{Year}/D{Day}.cpp`
2. Implement solution function in appropriate namespace:

```cpp
#include <string>
#include <utility>

namespace AdventOfCode::Y2024::D01 {
    std::pair<std::string, std::string> solve(const std::string& input) {
        // Parse input and solve
        std::string part1 = "";
        std::string part2 = "";
        return {part1, part2};
    }
}
```

3. Register in `src/AdventOfCode_runner.cpp`:

```cpp
AdventOfCode::Runner::registerSolution(2024, 1, 
    [](const std::string& input) { return AdventOfCode::Y2024::D01::solve(input); });
```

4. Update `CMakeLists.txt` if needed to include new source files
5. Rebuild: `cd build && cmake .. && make`
6. Run: `./bin/AdventOfCode --year 2024 --day 1`

### For LeetCode / HackerRank

1. Create solution file: `src/LeetCode/{ProblemName}.cpp` or `src/HackerRank/{ProblemName}.cpp`
2. Implement solution function
3. Register in the appropriate runner
4. Rebuild and run

## Features

- **Header-only runner system**: Solution registration via lambdas and function pointers
- **InputReader utility**: Loads input files from `resources/` directory
- **Shared resources**: Uses same input files as other language implementations
- **Portable**: CMake-based build system works across platforms

## Notes

- Input files are expected in `resources/AdventOfCode/Y{Year}/D{Day}/input.txt` relative to build directory
- CMakeLists.txt automatically copies resources to build directory
- Solutions return `std::pair<std::string, std::string>` for AdventOfCode
- Solutions return `std::string` for LeetCode and HackerRank
- Timing is measured in milliseconds
