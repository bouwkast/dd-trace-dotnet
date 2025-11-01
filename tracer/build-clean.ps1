<#
.SYNOPSIS
    Clean build helper - ensures a complete rebuild from scratch

.DESCRIPTION
    This script runs a full clean build with all incremental optimizations disabled.
    It will:
    - Clean all build artifacts
    - Re-download dependencies (libdatadog, etc.)
    - Run a full NuGet restore
    - Regenerate solution files
    - Rebuild all projects

    Use this when you need to ensure everything is built correctly, such as:
    - After switching branches
    - After updating dependencies
    - When troubleshooting build issues
    - Before creating a pull request

.PARAMETER BuildArguments
    Additional arguments to pass to the build system (e.g., target names, parameters)

.EXAMPLE
    .\build-clean.ps1
    Runs a clean build with the default target

.EXAMPLE
    .\build-clean.ps1 -BuildConfiguration Release
    Runs a clean Release build

.EXAMPLE
    .\build-clean.ps1 BuildTracerHome
    Runs a clean build of the BuildTracerHome target

.NOTES
    For quick incremental builds during development, use build-quick.ps1 instead.
#>

[CmdletBinding()]
Param(
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$BuildArguments
)

Write-Host "Running clean build..." -ForegroundColor Yellow
Write-Host "This will clean all artifacts and rebuild everything from scratch." -ForegroundColor Yellow
Write-Host ""

# First run Clean target, then run the build with incremental optimizations disabled
$allArgs = @('Clean', '--incremental-build', 'false') + $BuildArguments

& "$PSScriptRoot\build.ps1" @allArgs
