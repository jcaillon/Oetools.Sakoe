@if not defined _echo echo off
setlocal enabledelayedexpansion

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" -t:XmlOeProject "Oetools.Sakoe\bin\Any Cpu\Release\net461\sakoe.exe"
if not "!ERRORLEVEL!"=="0" (
	GOTO ENDINERROR
)

move /y "schema0.xsd" "docs\Project.xsd"
if not "!ERRORLEVEL!"=="0" (
	GOTO ENDINERROR
)

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" -t:XmlOeBuildConfiguration "Oetools.Sakoe\bin\Any Cpu\Release\net461\sakoe.exe"
if not "!ERRORLEVEL!"=="0" (
	GOTO ENDINERROR
)

move /y "schema0.xsd" "docs\BuildConfiguration.xsd"
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
