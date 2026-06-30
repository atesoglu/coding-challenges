#pragma once

#include <string>
#include <utility>
#include <map>
#include <functional>

namespace HackerRank
{
    using SolveFunc = std::function<std::string()>;
    
    class Runner
    {
    public:
        static void registerSolution(const std::string& name, SolveFunc func)
        {
            solutions[name] = func;
        }

        static std::string solve(const std::string& problemName)
        {
            auto it = solutions.find(problemName);
            if (it == solutions.end())
            {
                throw std::runtime_error("Solution not found: " + problemName);
            }
            return it->second();
        }

    private:
        static std::map<std::string, SolveFunc> solutions;
    };

    std::map<std::string, SolveFunc> Runner::solutions;
}
