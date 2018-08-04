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

echo.=========================
echo.[%time:~0,8% INFO] BUILDING SERIALIZATION PROJECT

set "PROJECT_PATH=Oetools.Serialization\Oetools.Serialization.csproj"
set "CHANGE_DEFAULT_TARGETFRAMEWORK=true"
set TARGETED_FRAMEWORKS=(net461)
set "MSBUILD_DEFAULT_TARGET=Build"
set CI_COMMIT_SHA="no_commit_just_for_no_pause"

call build.bat

echo.=========================
echo.[%time:~0,8% INFO] GENERATING XSD

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" -t:XmlOeProject "Oetools.Serialization\bin\Any Cpu\Release\net461\Oetools.Serialization.dll"
if not "!ERRORLEVEL!"=="0" (
	GOTO ENDINERROR
)

move /y "schema0.xsd" "Oetools.Serialization\Resources\Xsd\Project.xsd"
if not "!ERRORLEVEL!"=="0" (
	GOTO ENDINERROR
)

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" -t:XmlOeBuildConfiguration "Oetools.Serialization\bin\Any Cpu\Release\net461\Oetools.Serialization.dll"
if not "!ERRORLEVEL!"=="0" (
	GOTO ENDINERROR
)

move /y "schema0.xsd" "Oetools.Serialization\Resources\Xsd\BuildConfiguration.xsd"
if not "!ERRORLEVEL!"=="0" (
	GOTO ENDINERROR
)

:DONE
echo.=========================
echo.[%time:~0,8% INFO] DONE

pause


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
