<#
.SYNOPSIS
    Advent of Code Input Downloader (single year).

.PARAMETER Year
    The year of Advent of Code to download (e.g., 2026).

.PARAMETER SessionCookie
    Your Advent of Code session cookie (required for authentication).

.PARAMETER Overwrite
    Switch to overwrite existing input files.

.EXAMPLE
    .\Get-AoCInputs.ps1 -Year 2026 -SessionCookie "YOUR_COOKIE_HERE" -Overwrite
#>

param(
    [Parameter(Mandatory = $true)]
    [int]$Year,

    [Parameter(Mandatory = $true)]
    [string]$SessionCookie,

    [switch]$Overwrite
)

# ---- Base folder (script location) ----
$BaseFolder = $PSScriptRoot

# ---- Setup web session ----
$session = New-Object Microsoft.PowerShell.Commands.WebRequestSession
$session.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)"
$session.Cookies.Add((New-Object System.Net.Cookie("session", $SessionCookie, "/", ".adventofcode.com")))

# ---- Main loop: download day inputs ----
For ($day = 1; $day -le 25; $day++) {
    $dayPadded = "{0:D2}" -f $day
    $folderPath = Join-Path $BaseFolder ("Y$Year\D$dayPadded")
    $outputPath = Join-Path $folderPath ("Y{0}D{1}-input.txt" -f $Year, $dayPadded)

    # Ensure folder exists
    if (-not (Test-Path $folderPath)) {
        New-Item -ItemType Directory -Path $folderPath -Force | Out-Null
    }

    # Skip existing files unless overwrite is specified
    if ((Test-Path $outputPath) -and -not $Overwrite) {
        Write-Host "Skipping $outputPath (already exists)"
        continue
    }

    try {
        $uri = "https://adventofcode.com/$Year/day/$day/input"
        Write-Host "Downloading $uri ..."

        $response = Invoke-WebRequest -Uri $uri -WebSession $session -UseBasicParsing

        # Write content exactly as-is, no extra newline
        [System.IO.File]::WriteAllText($outputPath, $response.Content.TrimEnd(), [System.Text.Encoding]::UTF8)

        Write-Host "Saved to $outputPath"
    }
    catch {
        $msg = $_.Exception.Message
        Write-Warning "Failed to download day $day $msg"
    }
}

Write-Host "All downloads completed."
