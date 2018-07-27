@if not defined _echo echo off
setlocal enabledelayedexpansion

REM on CI, the version built will be replaced by the tag name instead of taking the version from csproj
REM if PROJECT_PATH is empty, we use the solution
if "%PROJECT_PATH%"=="" set "PROJECT_PATH=Oetools.Runner.Cli\Oetools.Runner.Cli.csproj"
if "%CUSTOM_BUILD_PARAMS%"=="" set "CUSTOM_BUILD_PARAMS=/p:ZipCopiedOutput=true"
REM set below to false if you don't want to change the target framework on build
if "%CHANGE_DEFAULT_TARGETFRAMEWORK%"=="" set "CHANGE_DEFAULT_TARGETFRAMEWORK=true"
if "%TARGETED_FRAMEWORKS%"=="" set TARGETED_FRAMEWORKS=(netcoreapp2.0 net461)
REM if you are packing a lib, CHANGE_DEFAULT_TARGETFRAMEWORK should be false and MSBUILD_DEFAULT_TARGET = Pack
REM otherwise, CHANGE_DEFAULT_TARGETFRAMEWORK should be true with correct TARGETED_FRAMEWORKS and MSBUILD_DEFAULT_TARGET = Publish
if "%MSBUILD_DEFAULT_TARGET%"=="" set "MSBUILD_DEFAULT_TARGET=Publish"

REM https://github.com/Microsoft/msbuild/wiki/MSBuild-Tips-&-Tricks

REM [works for gitlab and appveyor]
REM https://docs.gitlab.com/ee/ci/variables/
REM https://www.appveyor.com/docs/environment-variables/
rem if "%CI_BUILD_ID%"=="" set CI_BUILD_ID=%APPVEYOR_BUILD_ID%
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
if not "%VERSION_TO_BUILD%"=="" (
	if "%VERSION_TO_BUILD:~0,1%"=="v" (
		set "VERSION_TO_BUILD=%VERSION_TO_BUILD:~1%"
	)
)

REM @@@@@@@@@@@@@@
REM Verbosity
set MSBUILD_VERBOSITY=m
if "%IS_CI_BUILD%"=="true" set MSBUILD_VERBOSITY=n

REM @@@@@@@@@@@@@@
REM Solution name
FOR /F "tokens=* USEBACKQ" %%F IN (`dir /b *.sln`) DO (
	set SOLUTION_NAME=%%F
)

if "%PROJECT_PATH%"=="" (
	set "PROJECT_PATH=%SOLUTION_NAME%"
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

REM @@@@@@@@@@@@@@
REM Actual Build

echo.
echo.=========================
echo.[%time:~0,8% INFO] Restoring packages


set ADD_RESTORE=false
where nuget 1>nul 2>nul
if "%ERRORLEVEL%"=="0" (
	echo.[%time:~0,8% INFO] Nuget found
	nuget restore %SOLUTION_NAME% -Recursive -NonInteractive
	if not "!ERRORLEVEL!"=="0" (
		set ADD_RESTORE=true
	)
) else (
	echo.[%time:~0,8% INFO] Nuget not found in PATH, will add Restore to msbuild target
	set ADD_RESTORE=true
)



if "%CHANGE_DEFAULT_TARGETFRAMEWORK%"=="true" (
	for %%i in %TARGETED_FRAMEWORKS% do (
		Call :BUILD_ONE "%%i" "%ADD_RESTORE%"
		if not "!ERRORLEVEL!"=="0" (
			GOTO ENDINERROR
		)
	)
) else (
	Call :BUILD_ONE "" "%ADD_RESTORE%"
	if not "!ERRORLEVEL!"=="0" (
		GOTO ENDINERROR
	)
)


:DONE
echo.=========================
echo.[%time:~0,8% INFO] BUILD DONE

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
echo.[%time:~0,8% ERRO] BUILD ENDED IN ERROR, ERRORLEVEL = %errorlevel%

if "%IS_CI_BUILD%"=="false" (
	pause
)

exit /b 1


REM - -------------------------------------
REM Build one target
REM - -------------------------------------
REM - %1: target framework
REM - %2: need restore
REM - -------------------------------------
:BUILD_ONE

set "INPUT_FRAMEWORK=%~1"
IF "%INPUT_FRAMEWORK%"=="" (
	set "INPUT_FRAMEWORK=default"	
)

echo.
echo.=========================
echo.[%time:~0,8% INFO] BUILDING %~1

if "%~2"=="true" (
	set "MSBUILD_TARGET=Restore,%MSBUILD_DEFAULT_TARGET%"
) else (
	set "MSBUILD_TARGET=%MSBUILD_DEFAULT_TARGET%"
)

set "BUILD_PARAMS="
if not "%VERSION_TO_BUILD%"=="" (
	set "BUILD_PARAMS=%BUILD_PARAMS% /p:Version="%VERSION_TO_BUILD%""
)

if "%CHANGE_DEFAULT_TARGETFRAMEWORK%"=="true" (
	set "BUILD_PARAMS=%BUILD_PARAMS% /p:TargetFramework="%INPUT_FRAMEWORK%""
)

echo.[%time:~0,8% INFO] Starting msbuild
echo.[%time:~0,8% INFO] MSBUILD_TARGET = %MSBUILD_TARGET%
echo.[%time:~0,8% INFO] msbuild binlog viewer : http://msbuildlog.com/
echo.

call :MS_BUILD %PROJECT_PATH% /verbosity:%MSBUILD_VERBOSITY% /t:%MSBUILD_TARGET% /bl:%INPUT_FRAMEWORK%.binlog %BUILD_PARAMS% %CUSTOM_BUILD_PARAMS%
if not "%ERRORLEVEL%"=="0" (
	exit /b 1
)

echo.

exit /b 0


REM - -------------------------------------
REM MS_BUILD
REM - -------------------------------------
:MS_BUILD

@REM Determine if MSBuild is already in the PATH
for /f "usebackq delims=" %%I in (`where msbuild.exe 2^>nul`) do (
    "%%I" %*
    exit /b !ERRORLEVEL!
)

@REM Find the latest MSBuild that supports our projects
for /f "usebackq delims=" %%I in ('"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -version "[15.0,)" -latest -prerelease -products * -requires Microsoft.Component.MSBuild Microsoft.VisualStudio.Component.Roslyn.Compiler Microsoft.VisualStudio.Component.VC.140 -property InstallationPath') do (
    for /f "usebackq delims=" %%J in (`where /r "%%I\MSBuild" msbuild.exe 2^>nul ^| sort /r`) do (
        "%%J" %*
        exit /b !ERRORLEVEL!
    )
)

echo.=========================
echo.[%time:~0,8% ERRO] Could not find msbuild.exe 1>&2
exit /b 2