param (
    [Parameter(Mandatory=$true)]
    [int]$Year
)

# Define the base directory as the folder where this script is located
$baseDir = $PSScriptRoot

# Number of days
$days = 25

# Create the year folder
$yearFolder = Join-Path $baseDir "Y$Year"
if (-not (Test-Path $yearFolder)) {
    New-Item -ItemType Directory -Path $yearFolder | Out-Null
}

# Loop through each day
for ($day = 1; $day -le $days; $day++) {
    $dayPadded = "{0:D2}" -f $day
    $dayFolder = Join-Path $yearFolder "D$dayPadded"

    # Create the day folder if it doesn't exist
    if (-not (Test-Path $dayFolder)) {
        New-Item -ItemType Directory -Path $dayFolder | Out-Null
    }

    # Create placeholder files
    $codeFile = Join-Path $dayFolder "Y${Year}D${dayPadded}.cs"
    $inputFile = Join-Path $dayFolder "Y${Year}D${dayPadded}-input.txt"

    # Create empty files if they don't exist
    foreach ($file in @($codeFile, $inputFile)) {
        if (-not (Test-Path $file)) {
            New-Item -ItemType File -Path $file | Out-Null
        }
    }
}

Write-Host "Advent of Code folder structure created for year $Year in $baseDir."
