@ECHO OFF 
IF NOT %1x == x GOTO :RunTds
CLS
ECHO Run the TDS.exe unit-test suite and optionally
ECHO   send the test report to a text file.
ECHO. 
ECHO   The value of the exit code (summarizing the
ECHO   outcome of all of the unit tests) appears following
ECHO   the report but is not written to the file.
ECHO. 
ECHO Usage:
ECHO. 
ECHO   cmdTds
ECHO     With no parameters, display this help information.
ECHO.
ECHO   cmdTds Console
ECHO     Display the test report onto the Console window,
ECHO         followed by a line similar to this:
ECHO.
ECHO       Exit code: 0 = Passed -- all tests passed.
ECHO.
ECHO   cmdTds [file]
ECHO      Write the test report to a new file whose
ECHO        name contains the specified [file] string, 
ECHO        prefixed with "Tds_" and followed by 
ECHO        a result code and ".txt".
ECHO      For example, running "cmdTds Monday" 
ECHO        and failing one or more tests 
ECHO        might generate output similar to this:
ECHO.
ECHO       TDS test report has been written to file Tds_Monday_F.txt .
ECHO.
ECHO       Exit code: 3 = Failed -- at least one test Failed.
ECHO.
ECHO.
ECHO  Exit code values:
ECHO      0 = Passed; all tests passed.
ECHO      1 = Unmatched; tess to be run did not match
ECHO          the defined test methods, or no test was run.
ECHO      2 = Inconclusive; at least one test was Inconclusive.
ECHO      3 = Failed; at least one test Failed.
ECHO.
ECHO    The generated file name has a suffix identifying the result code:
ECHO      _P = 0, _U = 1, _I = 2, _F = 3
ECHO.

GOTO :Done

:RunTds
IF /I NOT %1 == Console GOTO :WriteFile

REM Report is displayed in the Command Prompt window
cls
.\bin\debug\TDS.exe -nopause
GOTO :Done

:WriteFile
REM Report is written to the specified file
IF EXIST Tds_%1*.txt GOTO :DuplicateOutput
.\bin\debug\TDS.exe -nopause > Tds_%1.txt
GOTO :TdsResult%ERRORLEVEL%

:TdsResult0
IF %1x == x GOTO :TdsResult0Summary
rename Tds_%1.txt Tds_%1_P.txt
ECHO.
ECHO TDS test report has been written to file Tds_%1_P.txt .
:TdsResult0Summary
ECHO.
ECHO Exit code: 0 = Passed -- all tests passed.
GOTO Done:

:TdsResult1
IF %1x == x GOTO :TdsResult1Summary
rename Tds_%1.txt Tds_%1_U.txt
ECHO.
ECHO TDS test report has been written to file Tds_%1_U.txt .
:TdsResult1Summary
ECHO.
ECHO   Exit code: 1 = Unmatched -- tests run did not match the defined
ECHO     test methods, or no test was run.
GOTO Done:

:TdsResult2
IF %1x == x GOTO :TdsResult2Summary
rename Tds_%1.txt Tds_%1_I.txt
ECHO.
ECHO TDS test report has been written to file Tds_%1_I.txt .
:TdsResult2Summary
ECHO.
ECHO   Exit code: 2 = Inconclusive -- at least one test was Inconclusive.
GOTO Done:

:TdsResult3
IF %1x == x GOTO :TdsResult3Summary
rename Tds_%1.txt Tds_%1_F.txt
ECHO.
ECHO TDS test report has been written to file Tds_%1_F.txt .
:TdsResult3Summary
ECHO.
ECHO Exit code: 3 = Failed -- at least one test Failed.
GOTO Done:

:DuplicateOutput
ECHO.
ECHO The name "%1" may cause a conflict with
ECHO   one of the following files; please choose another name:
ECHO.
dir /on /b Tds_%1*.txt
ECHO.

:Done
PAUSE
