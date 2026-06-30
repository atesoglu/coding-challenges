#pragma once

#include <string>
#include <utility>
#include <map>
#include <functional>

namespace AdventOfCode
{
    using SolveFunc = std::function<std::pair<std::string, std::string>(const std::string&)>;
    
    class Runner
    {
    public:
        static void registerSolution(int year, int day, SolveFunc func)
        {
            std::string key = std::to_string(year) + "-" + (day < 10 ? "0" : "") + std::to_string(day);
            solutions[key] = func;
        }

        static std::pair<std::string, std::string> solve(int year, int day, const std::string& input)
        {
            std::string key = std::to_string(year) + "-" + (day < 10 ? "0" : "") + std::to_string(day);
            auto it = solutions.find(key);
            if (it == solutions.end())
            {
                throw std::runtime_error("Solution not found for Y" + std::to_string(year) + " D" + std::to_string(day));
            }
            return it->second(input);
        }

    private:
        static std::map<std::string, SolveFunc> solutions;
    };

    std::map<std::string, SolveFunc> Runner::solutions;
}
