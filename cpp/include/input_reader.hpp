#pragma once

#include <string>
#include <fstream>
#include <sstream>
#include <filesystem>
#include <stdexcept>

namespace fs = std::filesystem;

class InputReader
{
public:
    static std::string load(const std::string& platform, int year, int day)
    {
        // Construct path like: resources/AdventOfCode/Y2024/D01/input.txt
        std::string path = (fs::path("resources") / platform / 
                           ("Y" + std::to_string(year)) / 
                           ("D" + (day < 10 ? "0" : "") + std::to_string(day)) / 
                           "input.txt").string();
        
        std::ifstream file(path);
        if (!file.is_open())
        {
            throw std::runtime_error("Could not open input file: " + path);
        }
        
        std::stringstream buffer;
        buffer << file.rdbuf();
        return buffer.str();
    }

    static std::string loadProblem(const std::string& platform, const std::string& problemName)
    {
        // For future use: resources/LeetCode/Problems/{ProblemName}/input.txt
        std::string path = (fs::path("resources") / platform / 
                           "Problems" / problemName / "input.txt").string();
        
        std::ifstream file(path);
        if (!file.is_open())
        {
            throw std::runtime_error("Could not open input file: " + path);
        }
        
        std::stringstream buffer;
        buffer << file.rdbuf();
        return buffer.str();
    }
};
