@if not defined _echo echo off
setlocal enabledelayedexpansion

REM https://github.com/Microsoft/msbuild/wiki/MSBuild-Tips-&-Tricks
REM msbuild.cmd Oetools.Runner.Cli /p:targetFrameworks="net461" /p:VersionPrefix=1.0.1 /p:VersionSuffix= /p:FileVersion=1.0.1.789 /p:ProductVersion=1.0.1 /p:Configuration=Release /p:Platform="Any CPU" /p:IncludeSymbols=true /verbosity:minimal /t:Restore,Rebuild /m

dotnet publish Oetools.Runner.Cli\Oetools.Runner.Cli.csproj --framework netcoreapp2.0 --configuration Release /p:Version="1.0.1" /p:Platform="Any CPU" /p:IncludeSymbols=true /verbosity:minimal /m /bl:dotnet.binlog

msbuild.cmd Oetools.Runner.Cli\Oetools.Runner.Cli.csproj /p:targetFrameworks="net461" /p:Version="1.0.1" /p:Configuration=Release /p:Platform="Any CPU" /p:IncludeSymbols=true /verbosity:minimal /t:Restore,Rebuild /m
