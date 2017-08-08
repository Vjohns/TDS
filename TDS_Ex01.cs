// This file may be used in addition to TDS.cs
//   to help organize suites of TDS methods, for example to
//   facilitate concurrent development by more than one developer.

#region Conditional compilation symbols
////If defined, RunOnlySelectedTestData
////  enables "#if" statements in the test methods
////  that may skip some of the test data.
////  This could be used to select a specific test case for 
////  detailed debug tracing, skipping immaterial cases.
////This symbol may also be defined in other source files
////  in this project.
//#define RunOnlySelectedTestData

////If defined, NUnit_platform
////  activates the definitions that invoke the NUnit 
////  unit-test platform.
//#define NUnit_platform

////If defined, TDS_platform
////  activates the definitions that invoke the TDS
////  unit-test platform.
#define TDS_platform

#if RunOnlySelectedTestData
#warning Only selected tests will be run, because RunOnlySelectedTestData is defined in file TDS_Ex01.cs .
#endif //RunOnlySelectedTestData

#if (NUnit_platform && TDS_platform)
#error Use TDS_platform only if no other platform is available.
#endif //(NUnit_platform && ...

#endregion Conditional compilation symbols

using System;
using System.Linq;

#if NUnit_platform
using NUnit.Framework;
#elif !TDS_platform
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif //NUnit_platform

//TODO: Usings -- Include "using" statements for the namespaces of the code
//  being developed, as in the following example:
using NewCodeNamespace;

namespace TDS
{

  public partial class Test
  {

    #region ReportSymbols
    //TODO: Conditional-compilation symbols in TDS_Ex01.cs -- For each such symbol
    //      that may be used in this TDS source file,
    //      include a similar #if() directive.
    //  This is not necessary for the symbol DEBUG,
    //      which is usually defined whenever this code runs.
    // Code snippet TdsSymbol may be used to generate this code.
    // Each TDS source file should contain a similar #region.

    /// <summary>
    /// True iff "#define RunOnlySelectedTestData"
    /// is active in this file, TDS_Ex01.cs .
    /// </summary>
    /// <remarks>
    /// <para>
    /// This condition is intended to be true
    /// iff some test cases (sets of values for
    /// initialization and comparison)
    /// within each test method in this source file
    /// may be skipped during the test run.
    /// </para><para>
    /// For example, a warning message in a calling
    /// program might utilize this.</para>
    /// </remarks>
    public const bool RunOnlySelectedTestData_TDS_Ex01 =
#if RunOnlySelectedTestData
 true;
#else
 false;
#endif //RunOnlySelectedTestData

    /// <summary>
    /// True iff "#define NUnit_platform"
    /// is active in this file, TDS_Ex01.cs .
    /// </summary>
    public const bool NUnit_platform_TDS_Ex01 =
#if NUnit_platform
 true;
#else
 false;
#endif //NUnit_platform

    /// <summary>
    /// True iff "#define TDS_platform"
    /// is active in this file, TDS_Ex01.cs .
    /// </summary>
    public const bool TDS_platform_TDS_Ex01 =
#if TDS_platform
 true;
#else
 false;
#endif //TDS_platform

    #endregion ReportSymbols

    /// <summary>
    /// Template for tests of methods that do not utilize the Console.  
    /// <para>The method actually called in this template is
    /// <see cref="TestableNoConsoleMethod"/>(), 
    /// but its only purpose is as a demonstration.</para>
    /// </summary>
    /// <remarks>
    /// <para>This test method writes tracing information to the Console,
    /// for example via <see cref="InitializeTestMethod"/>(),
    /// to record in the test report that it has been called.</para>
    /// </remarks>
    [TestMethod]
    public void TestableNoConsoleMethodTest()
    {
      #region testValues[]

#if RunOnlySelectedTestData
      //Space-separated list of "Id" tags of test cases, to be run
      //  in the order in which they are defined in testValues[]
      const string testSelectionList = "01";
#endif //RunOnlySelectedTestData

      var testValues = new[] {
        new {
          Id = "01 Out-of-bounds exception", // Test case identifier
          Arg = -3,  // Input value
          ExceptionExp = "Whoop",  // Expected exception
          ValueExp = 4,  // Expected returned value
        },

        new {
          Id = "02 Sample test",
          Arg = 3,
          ExceptionExp = DefaultExceptionMessage,
          ValueExp = 4,
        },
      };
      #endregion testValues[]

      try
      {
        if (IsUsingStandAloneTds)
          InitializeTestMethod();

        foreach (var tCase in
#if !RunOnlySelectedTestData
  testValues
#else //RunOnlySelectedTestData
  from testCase in testValues
  where IsInSelectionList(testCase.Id, testSelectionList)
  select testCase
#endif //RunOnlySelectedTestData
)
        {

          #region Invoke testable function members
          //Message value of an exception that the
          //  function member to be called might throw
          var exceptionThrown = DefaultExceptionMessage;

          //Expected exception message for this test case
          var exceptionMsgExp = (tCase.ExceptionExp == "")
                ? DefaultExceptionMessage
                : tCase.ExceptionExp;

          //Local variable to receive a value to be returned
          //  by the function member
          var actual = 0;

          try
          {
            //The following statements are two versions of the same code,
            //    since they call an extension method:
            actual = StaticCode.TestableNoConsoleMethod(tCase.Arg);
            actual = tCase.Arg.TestableNoConsoleMethod();

            //Before any tests are added (in the "#region Apply tests"
            //    region), the purpose of the above statement
            //    is only to invoke the function member (in this
            //    case, TestableNoConsoleMethod )
            //    while it is under development, to make it easy
            //    to observe the values of variables during tracing,
            //    so any returned value is unimportant here.
            //  After the new function member's code is complete enough
            //    to begin returning values to the TDS test method,
            //    these returned values (and the results of any side effects,
            //    such as changes to the values of public fields)
            //    are available for use in Assert statements,
            //    such as those appearing here
            //    and in the "#region Apply tests when ..." region.

          }
          catch (Exception e)
          {
            exceptionThrown = e.Message;

            Assert.IsTrue(
                  exceptionThrown.StartsWith
                    (exceptionMsgExp),
                  MsgForUnexpectedException(
                    "TestableNoConsoleMethod", tCase.Id,
                    exceptionMsgExp, exceptionThrown
                  ));

            //Skip the remaining tests for this test case;
            //  no useful values are returned after an Exception.
            continue;
          }
          #endregion Invoke testable function members

          #region Apply tests when no exception is raised

          //Test that if no exception occurred, none was expected.
          Assert.IsTrue(
                exceptionMsgExp == DefaultExceptionMessage,
                MsgForMissingException(
                  "TestableNoConsoleMethod", tCase.Id,
                  exceptionMsgExp
                ));

          Assert.AreEqual(
                tCase.ValueExp,
                actual,
                string.Format(@"
TestableNoConsoleMethodTest(), test case ""{0}"",
  Argument: {1}"
                    , tCase.Id  //{0}
                    , tCase.Arg  //{1}
                  )
                );
          #endregion Apply tests when no exception is raised

        }  // end:foreach (var tCase...

      }
      finally
      {
        if (IsUsingStandAloneTds)
          CleanupTestMethod();
      }
      ////TODO: TestableNoConsoleMethodTest() -- Remove the Assert.Inconclusive()
      ////  statement after this [TestMethod] is working:
      Assert.Inconclusive(
@"Verify the correctness of TestableNoConsoleMethodTest().");

    }  // end:TestableNoConsoleMethodTest()

    /// <summary>
    /// TDS Test of TimeRounded .
    /// </summary>
    [TestMethod]
    public void TimeRoundedTest()
    {
      #region testValues[]

#if RunOnlySelectedTestData
      //Space-separated list of "Id" tags of test cases, to be run
      //  in the order in which they are defined in testValues[]
      const string testSelectionList = "01";
#endif //RunOnlySelectedTestData

      var testValues = new[] {
        new {
          Id = "01 Int minutes invalid", // Test case identifier
          OverloadSig = "(int)",  //Overload being tested
          ParamFloat = 0F,  //(float) parameter
          ParamString = "",  //(string) parameter
          ParamInt = new[] {-2, 0, 0},  //(int) parameters
          ValueExp = new {Hour = 6, Minute = 0},  // Expected returned value
          ExceptionExp = "TimeRounded",  // Expected exception
        },

        new {
          Id = "02 Float days, 12:03",
          OverloadSig = "(float)",
          ParamFloat = 0.5F + 3F/24F/60F, //12:03 pm
          ParamString = "",
          ParamInt = new[] {0, 0, 0},
          ValueExp = new {Hour = 12, Minute = 5},  //12:05 pm
          ExceptionExp = DefaultExceptionMessage,
        },

        new {
          Id = "03 Int minutes 6:00",
          OverloadSig = "(int)",
          ParamFloat = 0F,
          ParamString = "",
          ParamInt = new[] {6*60 - 3, 0, 0},  // 5:57 am
          ValueExp = new{Hour = 5, Minute = 55},  // 5:55 am
          ExceptionExp = DefaultExceptionMessage,
        },

      };
      #endregion testValues[]

      try
      {
        if (IsUsingStandAloneTds)
          InitializeTestMethod();

        foreach (var tCase in
#if !RunOnlySelectedTestData
  testValues
#else //RunOnlySelectedTestData
  from testCase in testValues
  where IsInSelectionList(testCase.Id, testSelectionList)
  select testCase
#endif //RunOnlySelectedTestData
)
        {

          #region Invoke testable function members
          //Message value of an exception that the
          //  function member to be called might throw
          var exceptionThrown = DefaultExceptionMessage;

          //Expected exception message for this test case
          var exceptionMsgExp = (tCase.ExceptionExp == "")
                ? DefaultExceptionMessage
                : tCase.ExceptionExp;

          //Local variable to receive a value to be returned
          //  by the function member
          var actual = new TimeRounded();

          try
          {
            switch (tCase.OverloadSig)
            {
              case "(float)":
                actual = new TimeRounded
                    (tCase.ParamFloat);
                break;

              case "(int)":
                actual = new TimeRounded
                    (tCase.ParamInt[0]);
                break;

              default:
                throw (new ApplicationException
                      (string.Format(
@"Unknown signature type, OverloadSig = ""{0}"",
      is specified in testValues."
                        , tCase.OverloadSig  //{0}
                      )));
            }
          }
          catch (Exception e)
          {
            exceptionThrown = e.Message;

            Assert.IsTrue(
                  exceptionThrown.StartsWith
                    (exceptionMsgExp),
                  MsgForUnexpectedException(
                    "TimeRounded", tCase.Id,
                    exceptionMsgExp, exceptionThrown
                  ));

            //Skip the remaining tests for this test case;
            //  no useful values are returned after an Exception.
            continue;
          }
          #endregion Invoke testable function members

          #region Apply tests when no exception is raised

          //Test that if no exception occurred, none was expected.
          Assert.IsTrue(
                exceptionMsgExp == DefaultExceptionMessage,
                MsgForMissingException(
                  "TimeRounded", tCase.Id,
                  exceptionMsgExp
                ));

          Assert.AreEqual(
                tCase.ValueExp,
                new { actual.Hour, actual.Minute },
                string.Format(@"
TimeRoundedTest(), test case {0}: OverloadSig = {1},
  parameters = (float){2}, (string)""{3}"", (integer){4},{5},{6}"
                  , tCase.Id  //{0}
                  , tCase.OverloadSig  //{1}
                  , tCase.ParamFloat  //{2}
                  , tCase.ParamString  //{3}
                  , tCase.ParamInt[0]  //{4}
                  , tCase.ParamInt[1]  //{5}
                  , tCase.ParamInt[2]  //{6}
                  )
                );
          #endregion Apply tests when no exception is raised

        }  // end:foreach (var tCase...

      }
      finally
      {
        if (IsUsingStandAloneTds)
          CleanupTestMethod();
      }

    }  // end:TimeRoundedTest()

    //TODO: New TDS methods may be placed here:



  }  // end:Test{}

}  // end: Namespace TDS