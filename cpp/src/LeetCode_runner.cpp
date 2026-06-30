#include <iostream>
#include <chrono>
#include "LeetCode_runner.hpp"

// Forward declaration - solutions will be registered here
void registerLeetCodeSolutions();

int main(int argc, char* argv[])
{
    if (argc < 3)
    {
        std::cerr << "Usage: ./LeetCode --problem <ProblemName>" << std::endl;
        std::cerr << "Example: ./LeetCode --problem AddTwoNumbers" << std::endl;
        return 1;
    }

    std::string problemName;

    for (int i = 1; i < argc; i++)
    {
        std::string arg = argv[i];
        if (arg == "--problem" && i + 1 < argc)
            problemName = argv[i + 1];
    }

    if (problemName.empty())
    {
        std::cerr << "Error: Invalid problem name" << std::endl;
        return 1;
    }

    try
    {
        registerLeetCodeSolutions();
        
        auto start = std::chrono::high_resolution_clock::now();
        auto result = LeetCode::Runner::solve(problemName);
        auto end = std::chrono::high_resolution_clock::now();
        
        auto duration = std::chrono::duration_cast<std::chrono::milliseconds>(end - start);
        
        std::cout << problemName << std::endl;
        std::cout << "Result: " << result << " (" << duration.count() << "ms)" << std::endl;
        
        return 0;
    }
    catch (const std::exception& ex)
    {
        std::cerr << "Error: " << ex.what() << std::endl;
        return 1;
    }
}

void registerLeetCodeSolutions()
{
    // Solutions will be registered here
    // Example:
    // LeetCode::Runner::registerSolution("AddTwoNumbers", []() { return Solutions::AddTwoNumbers::solve(); });
}
