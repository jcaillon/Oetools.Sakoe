@if not defined _echo echo off
setlocal enabledelayedexpansion

echo ========================================
echo ========= STARTING DOTNET TEST =========
echo ========================================

dotnet test -v q Oetools.Packager\Oetools.Packager.Test\Oetools.Packager.Test.csproj
if %errorlevel% neq 0 exit %errorlevel%

dotnet test -v q Oetools.Runner.Cli.Test\Oetools.Runner.Cli.Test.csproj
if %errorlevel% neq 0 exit %errorlevel%

dotnet test -v q Oetools.Packager\Oetools.Utilities\Oetools.Utilities.Test\Oetools.Utilities.Test.csproj 
if %errorlevel% neq 0 exit %errorlevel%
