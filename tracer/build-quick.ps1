<#
.SYNOPSIS
    Quick incremental build helper - optimized for development workflow

.DESCRIPTION
    This script runs an incremental build with optimizations enabled.
    It skips unnecessary operations like:
    - Downloading libdatadog if already present
    - Running NuGet restore if packages haven't changed
    - Regenerating solution files if they're up-to-date

    Use this for typical development iterations after making code changes.

.PARAMETER BuildArguments
    Additional arguments to pass to the build system (e.g., target names, parameters)

.EXAMPLE
    .\build-quick.ps1
    Runs the default build with incremental optimizations

.EXAMPLE
    .\build-quick.ps1 -BuildConfiguration Debug
    Runs a Debug build with incremental optimizations

.EXAMPLE
    .\build-quick.ps1 BuildTracerHome
    Runs the BuildTracerHome target with incremental optimizations

.NOTES
    For a full clean build, use build-clean.ps1 instead.
    To force a full rebuild without cleaning, use: .\build.ps1 --incremental-build false
#>

[CmdletBinding()]
Param(
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$BuildArguments
)

Write-Host "Running quick incremental build..." -ForegroundColor Cyan
Write-Host "This build will skip unnecessary downloads and restores for faster iteration." -ForegroundColor Cyan
Write-Host ""

# Incremental build is enabled by default for local builds, but we explicitly set it to be clear
$allArgs = @('--incremental-build', 'true') + $BuildArguments

& "$PSScriptRoot\build.ps1" @allArgs
