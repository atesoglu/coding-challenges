cmake_minimum_required(VERSION 3.15)
project(CodingChallenges)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Main executable for AdventOfCode
add_executable(AdventOfCode
    src/AdventOfCode_runner.cpp
)
target_include_directories(AdventOfCode PRIVATE include)

# Main executable for LeetCode
add_executable(LeetCode
    src/LeetCode_runner.cpp
)
target_include_directories(LeetCode PRIVATE include)

# Main executable for HackerRank
add_executable(HackerRank
    src/HackerRank_runner.cpp
)
target_include_directories(HackerRank PRIVATE include)

# Set output directory for binaries
set_target_properties(AdventOfCode LeetCode HackerRank
    PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_BINARY_DIR}/bin"
)

# Copy resources directory to build
add_custom_target(copy_resources ALL
    COMMAND ${CMAKE_COMMAND} -E copy_directory
    ${CMAKE_SOURCE_DIR}/../resources ${CMAKE_BINARY_DIR}/resources
)
