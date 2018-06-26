@if not defined _echo echo off
setlocal enabledelayedexpansion

set VERSION_TO_BUILD=%APPVEYOR_REPO_TAG_NAME%

if "%VERSION_TO_BUILD%"=="" (
    set VERSION_TO_BUILD=0.1.0
)

echo =========================================
echo == BUILDING v%VERSION_TO_BUILD% ==
echo =========================================

REM https://github.com/Microsoft/msbuild/wiki/MSBuild-Tips-&-Tricks
REM dotnet publish Oetools.Runner.Cli\Oetools.Runner.Cli.csproj --framework netcoreapp2.0 --configuration Release /p:Version="1.0.1" /p:Platform="Any CPU" /p:IncludeSymbols=true /verbosity:minimal /m /bl:dotnet.binlog

set "COMMON_PARAM=Oetools.Runner.Cli\Oetools.Runner.Cli.csproj /p:Version="%VERSION_TO_BUILD%" /p:Configuration=Release /p:Platform="Any CPU" /p:IncludeSymbols=true /verbosity:minimal /m /nologo"

call msbuild.cmd %COMMON_PARAM% /p:targetFramework="netcoreapp2.0" /t:Restore,Publish /m /bl:netcoreapp.binlog /p:CopyBuildOutputToPublishDirectory=true /p:CopyOutputSymbolsToPublishDirectory=true
if %errorlevel% neq 0 exit %errorlevel%

call msbuild.cmd %COMMON_PARAM% /p:targetFramework="net461" /t:Restore,Rebuild /bl:net461.binlog
if %errorlevel% neq 0 exit %errorlevel%
