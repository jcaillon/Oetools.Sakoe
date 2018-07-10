@if not defined _echo echo off
setlocal enabledelayedexpansion

set PROJECT_PATH=Oetools.Runner.Cli\Oetools.Runner.Cli.csproj
set DEFAULT_VERSION=0.0.1

REM https://github.com/Microsoft/msbuild/wiki/MSBuild-Tips-&-Tricks

REM [works for gitlab and appveyor]
REM https://docs.gitlab.com/ee/ci/variables/
REM https://www.appveyor.com/docs/environment-variables/
if "%CI_BUILD_ID%"=="" set CI_BUILD_ID=%APPVEYOR_BUILD_ID%
if "%CI_COMMIT_TAG%"=="" set CI_COMMIT_TAG=%APPVEYOR_REPO_TAG_NAME%
if "%CI_COMMIT_SHA%"=="" set CI_COMMIT_SHA=%APPVEYOR_REPO_COMMIT%

REM @@@@@@@@@@@@@@
REM Are we on a CI build? 
set IS_CI_BUILD=false
if not "%CI_COMMIT_SHA%"=="" set IS_CI_BUILD=true

REM @@@@@@@@@@@@@@
REM Are we on a TAG build? 
set IS_TAG_BUILD=false
if "%VERSION_TO_BUILD%"=="" set VERSION_TO_BUILD=%CI_COMMIT_TAG%
if not "%VERSION_TO_BUILD%"=="" set IS_TAG_BUILD=true

REM @@@@@@@@@@@@@@
REM What is the version we are building?
set VERSION_TO_BUILD=%CI_COMMIT_TAG%
if "%VERSION_TO_BUILD%"=="" set VERSION_TO_BUILD=%DEFAULT_VERSION%
IF "%VERSION_TO_BUILD:~0,1%"=="v" set "VERSION_TO_BUILD=%VERSION_TO_BUILD:~1%"

REM @@@@@@@@@@@@@@
REM Verbosity
set MSBUILD_VERBOSITY=m
if "%IS_CI_BUILD%"=="true" set MSBUILD_VERBOSITY=n

REM @@@@@@@@@@@@@@
REM Solution name
FOR /F "tokens=* USEBACKQ" %%F IN (`dir /b *.sln`) DO (
	set SOLUTION_NAME=%%F
)

echo.=========================
echo.[%time:~0,8% INFO] BUILDING VERSION %VERSION_TO_BUILD%
echo.[%time:~0,8% INFO] CI BUILD : %IS_CI_BUILD%
echo.[%time:~0,8% INFO] TAG BUILD : %IS_TAG_BUILD%
echo.[%time:~0,8% INFO] COMMIT SHA : %CI_COMMIT_SHA%
echo.[%time:~0,8% INFO] COMMIT TAG : %CI_COMMIT_TAG%
echo.[%time:~0,8% INFO] VERBOSITY : %MSBUILD_VERBOSITY%
echo.[%time:~0,8% INFO] SOLUTION : %SOLUTION_NAME%
echo.[%time:~0,8% INFO] PROJECT PATH : %PROJECT_PATH%
echo.


REM @@@@@@@@@@@@@@
REM Actual Build

set "COMMON_PARAM=%PROJECT_PATH% /p:Version="%VERSION_TO_BUILD%" /p:Configuration=Release /p:Platform="Any CPU" /p:IncludeSymbols=true /verbosity:%MSBUILD_VERBOSITY% /m /nologo /p:ZipRelease=true"

echo.=========================
echo.[%time:~0,8% INFO] BUILDING FRAMEWORK net461
echo.
call msbuild.cmd %COMMON_PARAM% /p:targetFramework="net461" /t:Restore,Rebuild /bl:net461.binlog
if %errorlevel% neq 0 goto ENDINERROR
echo.

echo.=========================
echo.[%time:~0,8% INFO] PUBLISHING FRAMEWORK netcoreapp2.0
echo.
REM dotnet publish %COMMON_PARAM% --framework netcoreapp2.0 --configuration Release
call msbuild.cmd %COMMON_PARAM% /p:targetFramework="netcoreapp2.0" /t:Restore,Publish /bl:netcoreapp2.0.binlog
if %errorlevel% neq 0 goto ENDINERROR
echo.

:DONE
echo.=========================
echo.[%time:~0,8% INFO] DONE

if "%IS_CI_BUILD%"=="false" (
	pause
)


REM @@@@@@@@@@@@@@
REM End of script
exit /b 0


REM =================================================================================
REM 								SUBROUTINES - LABELS
REM =================================================================================


REM - -------------------------------------
REM Ending in error
REM - -------------------------------------
:ENDINERROR

echo.=========================
echo.[%time:~0,8% ERRO] ERROR!!! ERRORLEVEL = %errorlevel%

if "%IS_CI_BUILD%"=="false" (
	pause
)

exit /b 1