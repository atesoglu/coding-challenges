#include <iostream>
#include <chrono>
#include "AdventOfCode_runner.hpp"
#include "input_reader.hpp"

// Forward declaration - solutions will be registered here
void registerAdventOfCodeSolutions();

int main(int argc, char* argv[])
{
    if (argc < 4)
    {
        std::cerr << "Usage: ./AdventOfCode --year <year> --day <day>" << std::endl;
        std::cerr << "Example: ./AdventOfCode --year 2024 --day 1" << std::endl;
        return 1;
    }

    int year = 0;
    int day = 0;

    for (int i = 1; i < argc; i++)
    {
        std::string arg = argv[i];
        if (arg == "--year" && i + 1 < argc)
            year = std::stoi(argv[i + 1]);
        else if (arg == "--day" && i + 1 < argc)
            day = std::stoi(argv[i + 1]);
    }

    if (year == 0 || day == 0)
    {
        std::cerr << "Error: Invalid year or day" << std::endl;
        return 1;
    }

    try
    {
        registerAdventOfCodeSolutions();
        
        auto input = InputReader::load("AdventOfCode", year, day);
        
        auto start = std::chrono::high_resolution_clock::now();
        auto [part1, part2] = AdventOfCode::Runner::solve(year, day, input);
        auto end = std::chrono::high_resolution_clock::now();
        
        auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(end - start);
        
        std::cout << "Y" << year << " D" << (day < 10 ? "0" : "") << day << std::endl;
        std::cout << "Part 1: " << part1 << " (" << duration.count() << "ms)" << std::endl;
        std::cout << "Part 2: " << part2 << std::endl;
        
        return 0;
    }
    catch (const std::exception& ex)
    {
        std::cerr << "Error: " << ex.what() << std::endl;
        return 1;
    }
}

void registerAdventOfCodeSolutions()
{
    // Solutions will be registered here
    // Example:
    // AdventOfCode::Runner::registerSolution(2024, 1, 
    //     [](const std::string& input) { return Y2024::D01::solve(input); });
}
