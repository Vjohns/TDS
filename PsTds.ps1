  <#
  .SYNOPSIS
  Run the TDS.exe unit-test suite and optionally
    send the test report to a text file.

  .DESCRIPTION
  Use .\psTds to run the TDS.exe unit-test suite and
      optionallly write the results to a text file.

    The value of the exit code (summarizing the
      outcome of all of the unit tests) appears following
      the report but is not written to the file.

  Usage:

    .\psTds
      With no parameters, display the test report 
      onto the Console window, followed by a line
      similar to this:

  ...
  |
  |  Exit code: 0 = Passed -- all tests passed.

    -------------------

    .\psTds [file]
      Write the test report to a new file whose
      name contains the specified [file] string, 
      prefixed with "Tds_" and followed by
      a result code and ".txt".

    For example, running ".\psTds Monday"
      and failing one or more tests
      might generate output similar to this:

  | 
  |TDS test report has been written to file Tds_Monday_F.txt .
  | 
  |  Exit code: 3 = Failed -- at least one test Failed.

    -------------------

    .\psTds x
      Write the test report to a new file whose
      name contains the coded date, time, and
      exit code, in the format
      "Tds" <year> <month> <day> "_" <hour> <minute>
        <second> "_" <exit code> ".txt"

      For example, if the test was run beginning
      December 1, 2013, at 8:05:23 am, and
      one of the tests failed, it 
      might generate output similar to this:

  |
  |TDS test report has been written to file Tds_131201_080523_F.txt .
  |
  |  Exit code: 3 = Failed -- at least one test Failed.

       File names in this format, if sorted by name,
       will also be sorted by the time their tests were run.

    -------------------

  Exit code values:
      0 = Passed -- All tests Passed.
      1 = Unmatched -- Tests to be run did not match
          the defined test methods, or no test was run.
      2 = Inconclusive -- At least one test was Inconclusive.
      3 = Failed -- At least one test Failed.
  #>

  Param ([parameter(HelpMessage =
   "Name of the report file (optional)")]
   [String]$file = "to*the*console")

  #Name of placeholder for generating automatic file name
  $AutoFileName = "x"

  #Tag written at the beginning of all report file names
  $filePrefix = "Tds_"

  $exitCodeValues = 
    ("0 = Passed -- All tests Passed."),
    ("1 = Unmatched -- Tests to be run did not match`n" +
     "                 the defined test methods, or no test was run."),
    ("2 = Inconclusive -- At least one test was Inconclusive."),
    ("3 = Failed -- At least one test Failed.")

  if ($file -eq "to*the*console")
  {
    # Report is displayed in the Command Prompt window
    cls
    .\bin\debug\TDS -nopause
    $exitCode = $LASTEXITCODE

    Write-Host
    Write-Host ("  Exit code: " + $exitCodeValues[$exitCode])
  }
  else
  {
    if ($file -ne $AutoFileName)
    {
      $outFileBase = $file
    }
    else
    {
      $outFileBase = (get-date -format "yMMdd_HHmmss")
    }
  
  $conflictingFiles = @(get-childitem (
          ".\" + $filePrefix + $outFileBase + "*.txt") -name)

    if 
      ($conflictingFiles.Count -gt 0)
    {
      Write-Host ("`nThe name `"$file`" may cause a conflict with`n" + 
          "  one of the following files; please choose another name:")
      Write-Host
      $conflictingFiles | Write-Host
    }
    else
    {

      $oldFile = $filePrefix + $outFileBase + ".txt"
    
      .\bin\debug\TDS -nopause | out-file $oldFile

      $exitCode = $LASTEXITCODE
 
      #Add a file-name suffix: _P[assed], _U[nmatched],
      #  _I[nconclusive], or _F[ailed]
      $annotatedName = $filePrefix + 
          $outFileBase + "_" +  
          ("PUIF"[$exitCode]) + ".txt"
    
      Rename-Item $oldFile -NewName $annotatedName

      Write-Host
      Write-Host ("TDS test report has been written " +
            "to file " + $annotatedName + " .")
      Write-Host
      Write-Host ("  Exit code: " + $exitCodeValues[$exitCode])
    }
  }

  Write-Host
