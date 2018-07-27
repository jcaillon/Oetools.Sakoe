@if not defined _echo echo off
setlocal enabledelayedexpansion

REM [works for gitlab and appveyor]
REM https://docs.gitlab.com/ee/ci/variables/
REM https://www.appveyor.com/docs/environment-variables/
if "%CI_COMMIT_SHA%"=="" set CI_COMMIT_SHA=%APPVEYOR_REPO_COMMIT%

REM @@@@@@@@@@@@@@
REM Are we on a CI build? 
set IS_CI_BUILD=false
if not "%CI_COMMIT_SHA%"=="" set IS_CI_BUILD=true

REM @@@@@@@@@@@@@@
set MSBUILD_VERBOSITY=n
if "%IS_CI_BUILD%"=="true" set MSBUILD_VERBOSITY=n

echo.=========================
echo.[%time:~0,8% INFO] RUNNING ALL DOTNET TESTS
echo.[%time:~0,8% INFO] VERBOSITY : %MSBUILD_VERBOSITY%
echo.

for /f "delims=" %%G in ('dir /s /b *Test.csproj') do (
	echo.=========================
	echo.[%time:~0,8% INFO] RUNNING %%G
	echo.
	rem dotnet test --no-build -v %MSBUILD_VERBOSITY% %%G
	dotnet test -v %MSBUILD_VERBOSITY% %%G
	if %errorlevel% neq 0 goto ENDINERROR
)

:DONE
echo.
echo.=========================
echo.[%time:~0,8% INFO] TESTS DONE

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
echo.[%time:~0,8% ERRO] TESTS ENDED IN ERROR, ERRORLEVEL = %errorlevel%

if "%IS_CI_BUILD%"=="false" (
	pause
)

exit /b 1