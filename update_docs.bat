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

set "CI_COMMIT_SHA=no_commit_just_for_no_pause"

echo.=========================
echo.[%time:~0,8% INFO] UPDATE XSD

pushd Oetools.Builder
call update_xsd.bat
if not "!ERRORLEVEL!"=="0" (
	GOTO ENDINERROR
)
popd

echo.=========================
echo.[%time:~0,8% INFO] BUILDING RELEASE SAKOE

set "CHANGE_DEFAULT_TARGETFRAMEWORK=true"
set TARGETED_FRAMEWORKS=(net461)
set "CUSTOM_BUILD_PARAMS=/p:ZipCopiedOutput=false"
call build.bat
if not "!ERRORLEVEL!"=="0" (
	GOTO ENDINERROR
)

echo.=========================
echo.[%time:~0,8% INFO] GENERATING MARKDOWN FILE

"Oetools.Sakoe\bin\sakoe\sakoe.exe" manual markdown-file docs\index.md
if not "!ERRORLEVEL!"=="0" (
	GOTO ENDINERROR
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
echo.[%time:~0,8% ERRO] ENDED IN ERROR, ERRORLEVEL = %errorlevel%

pause

exit /b 1
