////The code in this file was written by Vincent R. Johns, who has
////  placed it into the public domain but makes no guarantee that it
////  will perform as expected or desired.
////Anyone may copy or modify any part of it without charge or obligation.

////This file, TDS.cs, contains working code for
////  the Test-Driven Scaffolding (TDS) system.
////It is intended to be used with Microsoft .NET
////  and Microsoft Visual Studio in a solution's "TDS" Project
////  to generate an application (TDS.exe file) that may be called
////    - from a command line or Windows Explorer
////      during debugging or unit testing, or
////    - by a unit-test system such as NUnit
////      or Microsoft Visual Studio Test.

#region Conditional compilation symbols
////The conditional compilation symbols in these directives
////  affect compilation.  Any of these symbols may also
////  be specified in command-line compiler options.
////In order to allow their state to be reported in  
////  TDS test reports, also include them in the
////    #region ReportSymbols
////  region in each TDS source file.
////Note:  The presence or absence of "#define NUnit_platform"
////  and of "#define TDS_platform" directives must be the
////  same in all of the TDS*.cs files in this project.
////  Weird error messages (that don't mention these
////  directives) may result otherwise.

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
////  unit-test platform.  This should be defined 
////  either in all TDS*.cs files in this Solution, or 
////  else in none of them.  Check the messages beginning
////  "***** The following conditional ..." at the 
////  beginning of the test report to verify this.
//#define NUnit_platform

////If defined, TDS_platform
////  activates the definitions that invoke the TDS
////  unit-test platform, an abbreviated substitute 
////  for a full test platform such as
////  Microsoft.VisualStudio.TestTools.UnitTesting .  
////  This should be defined either in all TDS*.cs files
////  in this Solution, or else in none of them.  
////  Check the messages beginning
////  "***** The following conditional ..." at the 
////  beginning of the test report to verify this.
#define TDS_platform

////If defined, UseNamedObjectTypeInTestableConsoleMethodTest 
////  causes the elements of testValues[] in the example
////  TestableConsoleMethodTest() method to be defined via
////  the nested Test.TestableConsoleMethodTestCase{} class
////  instead of via an anonymous object type.
////The example code should function identically in either case.
////This symbol is defined only in this source file.
//#define UseNamedObjectTypeInTestableConsoleMethodTest

#if RunOnlySelectedTestData
#warning Only selected tests will be run, because RunOnlySelectedTestData is defined in file TDS.cs .
#endif //RunOnlySelectedTestData

#if NUnit_platform && TDS_platform
#error Use TDS_platform only if no other platform is selected.
#endif //NUnit_platform && ...

#endregion Conditional compilation symbols

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

#if NUnit_platform
using NUnit.Framework;
#elif !TDS_platform
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif //NUnit_platform

////TODO: Usings -- Include "using" statements for the namespaces of the code
////  being developed, as in the following example:
using NewCodeNamespace;

namespace TDS
{

#if (NUnit_platform || TDS_platform)
  #region NUnit or TDS attribute definitions
  //The conditional compilation symbol "NUnit_platform"
  //  enables the definitions that allow NUnit to run
  //  unit tests, instead of the Microsoft unit-test framework.
  //The standard output from the example test methods is
  //  similar in either case.

  #region Attributes
  //The following statements allow the attribute names
  //    belonging to Visual Studio tests to be used with NUnit tests.
  //    Either version should function similarly, but the
  //    Microsoft.VisualStudio.TestTools.UnitTesting attribute names,
  //    such as [ClassInitialize], are the versions used in this file.
  //To employ NUnit in this project (using #define NUnit_platform),
  //    set references to the following DLLs after installing NUnit:
  //        nunit.core.dll
  //        nunit.core.interfaces.dll
  //        nunit.framework.dll
  //    and include this "using" statement:
  //        using NUnit.Framework;
  //To run tests using the minimal TDS test platform, use
  //    #define TDS_platform instead.

  /// <summary>
  /// <para><c>[ClassCleanup]</c> in
  /// Microsoft.VisualStudio.TestTools.UnitTesting
  /// identifies a method that contains code to be used
  /// after all the tests in the test class have run
  /// and to free resources obtained by the test class.</para>
  /// <para>This is redirected to <c>[TestFixtureTearDown]</c> in NUnit.
  /// <c>[TestFixtureTearDown]</c> is used inside a <c>[TestFixture]</c>
  /// to provide a single set of functions that are performed once
  /// after all tests are completed. </para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class ClassCleanupAttribute :
#if NUnit_platform
 TestFixtureTearDownAttribute
#else
 Attribute
#endif
  { }

  /// <summary>
  /// <para><c>[ClassInitialize]</c> in 
  /// Microsoft.VisualStudio.TestTools.UnitTesting
  /// identifies a method that contains code that must be used
  /// before any of the tests in the test class have run
  /// and to allocate resources to be used by the test class.
  /// </para><para>
  /// This is redirected to <c>[TestFixtureSetUp]</c> in NUnit;
  /// <c>[TestFixtureSetUp]</c> is used inside a <c>[TestFixture]</c>
  /// to provide a single set of functions that are performed once 
  /// prior to executing any of the tests in the fixture. 
  /// </para></summary>
  /// <remarks>
  /// The signature of a <c>[TestFixtureSetUp]</c> method in NUnit
  /// differs from that of a 
  /// Microsoft.VisualStudio.TestTools.UnitTesting
  /// version; it does not have the <c>TestContext</c>
  /// parameter required by a <c>[ClassInitialize]</c> method.
  /// Therefore, the <c>[ClassInitialize]</c> attribute is applied
  /// in TDS only to the compatible version
  /// of <see cref="InitializeClasses()"/> via the use
  /// of <c>#if NUnit_platform</c> directives.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Method)]
  public class ClassInitializeAttribute :
#if NUnit_platform
 TestFixtureSetUpAttribute
#else
 Attribute
#endif
  { }

  /// <summary>
  /// <para><c>[TestClass]</c> in
  /// Microsoft.VisualStudio.TestTools.UnitTesting
  /// is used to identify classes that contain test methods.</para>
  /// <para>This is redirected to <c>[TestFixture]</c> in NUnit; 
  /// <c>[TestFixture]</c> marks a class that contains tests and,
  /// optionally, setup or teardown methods. 
  /// </para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public class TestClassAttribute :
#if NUnit_platform
 TestFixtureAttribute
#else
 Attribute
#endif
  { }

  /// <summary>
  /// <para><c>[TestCleanup]</c> in
  /// Microsoft.VisualStudio.TestTools.UnitTesting
  /// identifies a method that contains code that must be
  /// used after the test has run and to free resources
  /// obtained by all the tests in the test class.</para>
  /// <para>This is redirected to <c>[TearDown]</c> in NUnit; 
  /// this attribute is used inside a <c>[TestFixture]</c>
  /// to provide a common set of functions
  /// that are performed after each test method is run.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class TestCleanupAttribute :
#if NUnit_platform
 TearDownAttribute
#else
 Attribute
#endif
  { }

  /// <summary>
  /// <para><c>[TestInitialize]</c> in
  /// Microsoft.VisualStudio.TestTools.UnitTesting
  /// identifies the method to run before the test
  /// to allocate and configure resources needed
  /// by all tests in the test class.</para>
  /// <para>This is redirected to <c>[SetUp]</c> in NUnit; 
  /// this attribute is used inside a <c>[TestFixture]</c>
  /// to provide a common set of functions that are performed
  /// just before each test method is called.</para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class TestInitializeAttribute :
#if NUnit_platform
 SetUpAttribute
#else
 Attribute
#endif
  { }

  /// <summary>
  /// <para><c>[TestMethod]</c> in
  /// Microsoft.VisualStudio.TestTools.UnitTesting
  /// is used to identify test methods.</para>
  /// <para>This is redirected to <c>[Test]</c> in NUnit; 
  /// <c>[Test]</c> marks a method inside
  /// a <c>[TestFixture]</c> class as a test.
  /// </para>
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class TestMethodAttribute :
#if NUnit_platform
 TestAttribute
#else
 Attribute
#endif
  { }

  #region Same-named attributes
  //The following attributes have the same name in both systems,
  // though their behavior may not be identical:

  ///// <summary>
  ///// <para><c>[ExpectedException]</c> in
  ///// Microsoft.VisualStudio.TestTools.UnitTesting
  ///// indicates that an exception is expected during
  ///// test method execution.</para>
  ///// <para><c>[ExpectedException]</c> in NUnit 
  ///// is the way to specify that the execution of a test
  ///// will throw an exception.</para>
  ///// </summary>
  //public class ExpectedExceptionAttribute : ExpectedExceptionAttribute { }

  ///// <summary>
  ///// <para><c>[Ignore]</c> in Microsoft.VisualStudio.TestTools.UnitTesting
  ///// indicates that a specific test should not be run.</para>
  ///// <para><c>[Ignore]</c> in NUnit 
  ///// is an attribute to not run a test or test fixture
  ///// for a period of time.</para>
  ///// </summary>
  //public class IgnoreAttribute : IgnoreAttribute { }

  ///// <summary>
  ///// <para><c>[Timeout]</c> in Microsoft.VisualStudio.TestTools.UnitTesting
  ///// is used to specify the time-out period of a unit test.</para>
  ///// <para><c>[Timeout]</c> in NUnit 
  ///// is used to specify a timeout value in milliseconds
  ///// for a test case.</para>
  ///// </summary>
  //public class TimeoutAttribute : TimeoutAttribute { }
  #endregion Same-named attributes
  #endregion Attributes

  #region TDS "Assert{}" class
  //The code in this #region mimics some of the
  //    Microsoft.VisualStudio.TestTools.UnitTesting
  //    or NUnit platform features, allowing TDS
  //    tests to be run independently, for example
  //    via a command-line build.
  //However, since only a small subset of members
  //    of the Assert{} class of those platforms
  //    is defined here, you may wish to define
  //    additional members similar to these.

#if ( NUnit_platform || TDS_platform)
  /// <summary>
  /// <para>The <c>AssertInconclusiveException</c> in
  /// <c>Microsoft.VisualStudio.TestTools.UnitTesting</c>
  /// is thrown whenever a test should produce a result of Inconclusive.</para>
  /// <para>Typically, you add an <c>Assert.Inconclusive</c> statement
  /// to a test that you are still working on
  /// to indicate it is not yet ready to be run.</para>
  /// </summary>
  /// <remarks>
  /// In <c>Microsoft.VisualStudio.TestTools.UnitTesting</c>,
  /// the displayed message is prefixed with the string
  ///    "Assert.Inconclusive failed. "
  /// </remarks>
  [Serializable]
  public class AssertInconclusiveException : Exception
  {
    public AssertInconclusiveException(string message) :
      base(message)
    { }  // end: AssertInconclusiveException()
  }  // end: AssertInconclusiveException{}

  /// <summary>
  /// Used to indicate failure for a test.
  /// </summary>
  [Serializable]
  public class AssertFailedException :
#if TDS_platform
 Exception
#else  //#if NUnit_platform
 AssertionException
#endif
  {
    public AssertFailedException(string message) :
      base(message)
    { }  // end: AssertFailedException()
  }  // end: AssertFailedException{}

  /// <summary>
  /// Class providing methods that support unit tests
  /// </summary>
  public class Assert
  {
    /// <summary>
    /// Verifies that the specified condition is <c>true</c>.
    /// The assertion fails if the condition is <c>false</c>.
    /// Displays a message if the assertion fails.
    /// </summary>
    /// <param name="condition">
    /// The condition to verify is <c>true</c>.</param>
    /// <param name="message">
    /// A message to display if the assertion fails.
    /// This message can be seen in the unit test results.</param>
    /// <exception cref="AssertFailedException">
    /// This is thrown if the <paramref name="condition"/>
    /// is <c>false</c>.
    /// </exception>
    public static void IsTrue(bool condition, string message)
    {
      if (!condition)
      {
        throw new AssertFailedException(
              @"Assert.IsTrue failed. " + message);
      }
    }  // end:IsTrue()

    /// <summary>
    /// Indicates that an assertion cannot
    /// be proven <c>true</c> or <c>false</c>.
    /// Also used to indicate an assertion
    /// that has not yet been implemented.
    /// </summary>
    /// <param name="message">
    /// A message to display.
    /// This message can be seen in the unit test results.
    /// </param>
    /// <exception cref="InconclusiveException">
    /// This is always thrown.
    /// </exception>
    public static void Inconclusive(string message)
    {
      throw new
#if TDS_platform
 AssertInconclusiveException
#else  //#if NUnit_platform
 InconclusiveException
#endif
(
          @"Assert.Inconclusive was thrown. " + message);
    }  // end:Inconclusive()

    /// <summary>
    /// Verifies that two specified objects 
    /// have equal <c>ToString()</c> representations
    /// (*NOT* that the objects themselves are equal).
    /// The assertion fails if the objects differ.
    /// Displays a message if the assertion fails.
    /// </summary>
    /// <param name="expected"> The first object to compare.
    /// This is the object the unit test expects.</param>
    /// <param name="actual">The second object to compare.
    /// This is the object the unit test produced.</param>
    /// <param name="message">
    /// A message to display if the assertion fails.
    /// This message can be seen in the unit test results.
    /// </param>
    /// <exception cref="AssertFailedException">
    /// This is thrown if the <c>ToString()</c>
    /// representations of the two objects do not match.
    /// </exception>
    /// <remarks>
    /// Unlike the
    /// <c>Microsoft.VisualStudio.TestTools.UnitTesting</c>
    /// version, this checks only <c>ToString()</c> values,
    /// allowing it to be used for simple checks instead of
    /// requiring multiple overloads to be defined.
    /// </remarks>
    public static void AreEqual(object expected, object actual, string message)
    {
      var act = actual.ToString();
      var exp = expected.ToString();
      if (!act.Equals(exp))
      {
        throw new AssertFailedException(
              string.Format(
@"Assert.AreEqual failed. Expected:
<{0}>. Actual:
<{1}>.  {2}"
                  , exp //{0}
                  , act //{1}
                  , message //{2}
                ));
      }
    }  // end:AreEqual()

  }  // end: Assert{}
#endif //(TDS_platform)
  #endregion TDS "Assert{}" class

  #endregion NUnit or TDS attribute definitions
#endif //(NUnit_platform || TDS_platform)

  /// <summary>
  /// Object providing items usable in mismatched-tests messages
  /// </summary>
  public class RunTdsTestsReport
  {

    /// <summary>
    ///Create a list of methods in (this) set,
    /// which are the ones we're reporting on,
    /// that are not members of (otherSet).
    /// Write the list to the Console 
    /// and return True iff it's not empty.
    /// </summary>
    /// <remarks><para>
    /// The sets are
    /// </para><para>(a) methods listed in
    /// <see cref="TestMethodsToBeRun"/> and
    /// </para><para>(b) methods with [TestMethod] attributes,
    /// </para><para>which usually should match.
    /// See comments on <see cref="TestMethodsToBeRun"/>
    /// for an example of the report output.
    /// </para></remarks>
    /// <returns>True iff at least one mismatching TDS method is found
    /// </returns>
    /// <param name="otherSet">Matching (we hope) methods from the other list
    /// </param>
    internal bool ExceptionReport(RunTdsTestsReport otherSet)
    {
      //In the (thisSet} list but not in the {otherSet} list
      var inThisButNotOther = TestMethods.Except(otherSet.TestMethods);
      var isMismatchFound = inThisButNotOther.Count() > 0;

      if (isMismatchFound)
      {
        var inThisButNotOtherList = inThisButNotOther
              .Aggregate((list, next1) => list + @"
      " + next1);
        var isPlural = inThisButNotOther.Count() > 1;
        Console.WriteLine(@"
The following TDS method{0} {1}
    but {2}:
      {3}"
              , isPlural ? "s" : ""  //{0} = "s" for multiple methods
              , isPlural ? MsgSeveralAreIn
                : MsgOneIsIn  //{1} = messsage that items
                              //      are in this set
              , isPlural ? otherSet.MsgSeveralAreAbsent
                : otherSet.MsgOneIsAbsent  //{2} = messsage that items
                                           //  are not in the other set
              , inThisButNotOtherList  //{3} = list of methods in this set
              );
      }
      else
        Console.WriteLine(@"
All TDS methods that {0}
    {1}."
                , MsgSeveralAreIn  //{0} = messsage that items
                                   //  are in this set
                , otherSet.MsgSeveralAreIn  //{1} = messsage that items
                                            //  are also in the other set
              );

      return isMismatchFound;
    }  // end:ExceptionReport()

    /// <summary>
    /// Constructor, specifying a set of method names
    /// and the relevant message strings.
    /// </summary>
    /// <param name="methodList">List of included names</param>
    /// <param name="setName">Description of this set,
    /// either a TDS test or a [TestMethod] method.
    /// <para>Expected values are "[TestMethod] attribute"
    /// or "in the TestMethodsToBeRun[] list".</para></param>
    /// <param name="oneIsIn">Message that it's in the list</param>
    /// <param name="severalAreIn">Message that multiple items
    /// are in the list</param>
    /// <param name="oneIsAbsent">Message that it's missing</param>
    /// <param name="severalAreAbsent">Message that multiple items
    /// are missing</param>
    internal RunTdsTestsReport(
      IEnumerable<string> methodList,
      string setName,
      string oneIsIn,
      string severalAreIn,
      string oneIsAbsent,
      string severalAreAbsent)
    {

      #region string msg(format)
      //Return setName placed into the specified format.
      //  For example, if setName = "[TestMethod] attribute",
      //    then msg("has a {0}") = "has a [TestMethod] attribute"
      Func<string, string> msg =
            format => string.Format(format
              , setName  //{0}
            );
      #endregion string msg(format)

      MsgOneIsIn = msg(oneIsIn);
      MsgSeveralAreIn = msg(severalAreIn);
      MsgOneIsAbsent = msg(oneIsAbsent);
      MsgSeveralAreAbsent = msg(severalAreAbsent);
      TestMethods =
          from name in methodList
          orderby name
          select
            string.Format("TDS.Test.{0}()"
                  , name  //{0}
            );
    }  // end:RunTdsTestsReport()

    /// <summary>
    /// Message that a single item is in this set,
    /// for example "has a [TestMethod] attribute"
    /// </summary>
    string MsgOneIsIn { get; set; }

    /// <summary>
    /// Message that multiple items are in this set,
    /// for example "have [TestMethod] attributes"
    /// </summary>
    string MsgSeveralAreIn { get; set; }

    /// <summary>
    /// Message that the one named item is not in this set,
    /// for example "does not (yet) have a [TestMethod] attribute"
    /// </summary>
    string MsgOneIsAbsent { get; set; }

    /// <summary>
    /// Message that multiple items are not in this set,
    /// for example "do not (yet) have [TestMethod] attributes"
    /// </summary>
    string MsgSeveralAreAbsent { get; set; }

    /// <summary>
    /// Names of all methods in this set
    /// (either in the <see cref="TestMethodsToBeRun[]"/> list
    /// or having a <c>[TestMethod]</c> attribute)
    /// </summary>
    IEnumerable<string> TestMethods { get; set; }

  }  // end:RunTdsTestsReport{}

  /// <summary>
  /// Summary of the results of running a TDS method
  /// </summary>
  public class TdsMethodResults
  {
    /// <summary>
    /// Status (e.g., "Failed") of test results
    /// </summary>
    internal Test.RunStatus TestStatus { get; private set; }

    /// <summary>
    /// TDS method name
    /// </summary>
    internal string MethodName { get; private set; }

    /// <summary>
    /// Exception, if any, thrown by the TDS method
    /// </summary>
    internal Exception AssertEx { get; private set; }

    /// <summary>
    /// Summary of the results of running a Tds method
    /// </summary>
    /// <param name="testStatus">Status (e.g., "Failed") of test results</param>
    /// <param name="methodName">TDS method name</param>
    /// <param name="assertEx">Exception, if any, thrown by the TDS method</param>
    internal TdsMethodResults(Test.RunStatus testStatus, string methodName, Exception assertEx)
    {
      TestStatus = testStatus;
      MethodName = methodName;
      AssertEx = assertEx;
    }  // end:TdsMethodResults()

  }  // end:TdsMethodResults{}

  /// <summary>
  /// This class is intended to contain
  /// "Test-Driven Scaffolding" ("TDS") drivers
  /// used in developing methods for another class.
  /// </summary>
  /// <remarks>
  /// The source file initially contains the following templates
  /// (example drivers for new methods):
  /// <para>
  /// <see cref="TestableConsoleMethodTest()"/> is an example test method
  /// for <see cref="TestableConsoleMethod()"/>,
  /// a method that uses the Console.</para>
  /// <para>
  /// <see cref="TestableNoConsoleMethodTest()"/> is an example test method
  /// for <see cref="TestableNoConsoleMethod()"/>,
  /// a method that does not use the Console.</para>
  /// <para>These may be used as a basis for developing your own
  /// Test Driven Scaffolding (TDS) drivers.</para>
  /// <para>In their present form, these templates may be used
  /// as unit tests for the example methods that accompany them,
  /// to demonstrate that the unit test system is operating properly,
  /// but their real purpose is to be copied
  /// and modified to use as drivers for new
  /// function members under development.</para>
  /// <para>After the definition of a new
  /// function member is sufficiently complete,
  /// its TDS method can be easily modified
  /// to become a unit-test method usable by NUnit
  /// or by Visual Studio's
  /// VisualStudio.TestTools.UnitTesting facilities.</para>
  /// </remarks>
  [TestClass]
  public partial class Test
  {

    /// <summary>
    /// This string contains a list of all <c>TDS*.cs</c>
    /// or similar source files in the project,
    /// including the <c>".cs"</c> suffix,
    /// and with each name on a separate line.
    /// <para>Example: "TDS.cs"
    /// </para>
    /// </summary>
    /// <remarks><para>
    /// This list is used, for example, to identify active 
    /// <c>"#define RunOnlySelectedTestData"</c> directives.
    /// </para><para>
    /// Usable file names from this list are copied
    /// to <see cref="TdsFileNamesList"/>.
    /// </para><para>
    /// "//" at the beginning of a line comments out that line.
    /// See <see cref="TdsFileNames_Pattern"/> for
    /// details on parsing these names.
    /// </para>
    /// </remarks>
    static string TestMethodsSourceFiles =
          //// TODO: TestMethodsSourceFiles -- List all files that contain TDS test methods.
          @"

            TDS.cs
            TDS_Ex01.cs

";

    /// <summary>
    /// <para>
    /// This string contains a list of names of test methods
    /// in the <c>TDS.Test{}</c> class to be run by TDS.exe . 
    /// Tests will be run in the order in which
    /// they are listed in this string;
    /// if a test is listed twice, it will be run twice.
    /// </para><para>
    /// Within this string, only the first test
    /// method named in each line will be run.
    /// The name may optionally be followed by
    /// a comma or parentheses.
    /// The prefix "TDS.Test." may precede the name.
    /// A name in this string may be commented out
    /// by inserting "//" before the name,
    /// so that the named test will not be run;
    /// it can be re-activated by deleting the "//".
    /// (The results will be similar to selecting or clearing
    /// check boxes on the names of tests in the NUnit GUI,
    /// or to specifying a list of tests in a Micosoft
    /// Visual Studio Test Explorer "Playlist" file.)
    /// </para><para>
    /// Any text following the name on each line is ignored,
    /// including any (optional) trailing parentheses.
    /// </para><para>
    /// <see cref="NameOfAllTestsAreToBeRunTest"/>
    /// contains the name of the TDS method that checks for
    /// active filtering of test cases; this name
    /// is automatically added to the end of this list
    /// and the method is therefore always run.
    /// </para>
    /// </summary>
    /// <remarks><para>
    /// The TDS test methods themselves must be
    /// <c>public</c> instance methods that return <c>void</c>
    /// and take no parameters, but they can test any accessible code
    /// that implements a function member of a class or struct.
    /// (The working code being developed might need to have
    /// its accessibility temporarily changed
    /// to <c>public</c> or <c>internal</c>,
    /// to allow it to be invoked by the TDS method.)
    /// </para><para>
    /// Each TDS method listed in the
    /// <see cref="TestMethodsToBeRun"/> list should have
    /// a <c>[TestMethod]</c> attribute, and each
    /// <c>[TestMethod]</c> method should be listed here;
    /// any mismatches are identified in the TDS report.
    /// </para><para>
    /// Usable names from this list are copied
    /// to <see cref="TestMethodsToBeRunList"/>.
    /// </para><para>
    /// See <see cref="TestMethodsToBeRun_Pattern"/> for
    /// details on parsing these names.
    /// </para><para>Example test output under normal conditions,
    /// if all tests pass and where every test method with
    /// a <c>[TestMethod]</c> attribute is listed
    /// (not commented out) in <see cref="TestMethodsToBeRun"/>:
    /// </para><para>| 
    /// </para><para>| All TDS methods that have [TestMethod] attributes
    /// </para><para>|     are in the <see cref="TestMethodsToBeRun"/> list.
    /// </para><para>| 
    /// </para><para>| All TDS methods that are in
    /// the <see cref="TestMethodsToBeRun"/> list
    /// </para><para>|     have [TestMethod] attributes.
    /// </para><para>| 
    /// </para><para>...
    /// </para><para>|   All listed TDS test methods passed.
    /// </para><para>
    /// </para><para>
    /// Example output if the reference to "NonexistentTest"
    /// in <see cref="TestMethodsToBeRun"/>
    /// (representing a misspelled name, perhaps)
    /// is de-commented, and the <c>[TestMethod]</c> attribute on
    /// <see cref="AllTestsAreToBeRunTest()"/>
    /// (whose name is always implicitly included at
    /// the end of the <see cref="TestMethodsToBeRun"/>
    /// list) is commented out:
    /// </para><para>| The following TDS method has a [TestMethod] attribute
    /// </para><para>|     but is not in the TestMethodsToBeRun list:
    /// </para><para>|       TDS.Test.TimeRoundedTest()
    /// </para><para>| 
    /// </para><para>| The following TDS methods are in the TestMethodsToBeRun list
    /// </para><para>|     but do not (yet) have [TestMethod] attributes:
    /// </para><para>|       TDS.Test.AllTestsAreToBeRunTest()
    /// </para><para>|       TDS.Test.NonexistentTest()      
    /// </para><para>| 
    /// </para><para>...
    /// </para><para>|   The TestMethodsToBeRun list does not match the [TestMethod] methods.
    /// </para></remarks>
    static string TestMethodsToBeRun =
                ////TODO: TestMethodsToBeRun -- List all TDS test methods to be run.
                @"

              TestableConsoleMethodTest
//            NonexistentTest  //Reported as a name that should be corrected
              TDS.Test.TestableNoConsoleMethodTest()  //Includes qualified-name prefix
//            TimeRoundedTest()  //Example not initially exercised in the tutorial
//            TestableNoConsoleMethodTest  //Duplicate, would be run a 2nd time

";

    /// <summary>
    /// <para>Each element of this list
    /// (after <see cref="InitializeClasses()"/>
    /// is run to populate it)
    /// contains a <see cref="SymbolInfo"/> value
    /// containing the name of an active conditional-compilation symbol
    /// and the name of the file containing it,
    /// assuming that the "#region ReportSymbols" code in each file
    /// is set up to reflect the states of those symbols.
    /// </para><para>
    /// For example, if file TDS.cs contains the directive
    /// "#define RunOnlySelectedTestData",
    /// then one element in this List should contain
    /// <c>new SymbolInfo("RunOnlySelectedTestData", "TDS", True)</c> ,
    /// </para><para>
    /// In each list element, <c>Symbol</c> = a potentiallly active
    /// conditional-symbol name,
    /// though its "#define" directive may be commented out,
    /// </para><para>
    /// <c>File</c> = the name of a file containing the symbol,
    /// but the name does not contain a trailing ".cs" extension,
    /// </para><para>
    /// <c>FileWithExt</c> = the name of a file, including the ".cs",
    /// that contains the symbol, and
    /// </para><para>
    /// <c>IsDefined</c> = <c>True</c> iff the symbol
    /// is defined in the file . 
    /// </para>
    /// </summary>
    static List<SymbolInfo> ConditionalSymbolsList;

    /// <summary>
    /// Message used to indicate that no exception 
    /// was thrown by a called method
    /// </summary>
    internal const string DefaultExceptionMessage
          = " No exception was thrown";

    /// <summary>
    /// True iff the TDS methods are being run via a call
    /// to <see cref="RunTdsTests"/>(),
    /// false if they are being run using a unit-test system
    /// (such as NUnit or the Visual Studio test facility).
    /// <para>This specifies if the TDS methods are running 
    /// independently of a unit-test system, such as NUnit.
    /// This is used within TDS methods
    /// to govern explicit calls to methods marked
    /// with attributes such as [TestInitialize] and [TestCleanup],
    /// that the unit-test facility would run automatically.</para>
    /// </summary>
    /// <remarks><para>This allows the explicit calls to methods
    /// such as <see cref="InitializeTestMethod"/>()
    /// to be bypassed by the unit-test system,
    /// which utilizes atttributes such as
    /// <c>[TestInitialize]</c> instead.</para>
    /// </remarks>
    public static bool IsUsingStandAloneTds = false;

    /// <summary>
    /// Message about missing (or misspelled)
    /// OnlySelectedTestDataAreUsed_... field.
    /// <para>{0} = symbol name</para>
    /// <para>{1} = file name</para>
    /// <para>Example, with {0} = "OnlySelectedTestDataAreUsed_TDS_Ex02"
    /// and {1} = "TDS_Ex02.cs":
    /// </para><para>| 
    /// </para><para>| Symbol "OnlySelectedTestDataAreUsed_TDS_Ex02" is
    /// apparently not defined;
    /// </para><para>|   it should be defined in file TDS_Ex02.cs .
    /// </para><para>| 
    /// </para><para>|   File TDS_Ex02.cs may be missing or be
    /// malformed in some way;
    /// </para><para>|     for example, TDS_Ex02.cs might be present
    /// in the project
    /// </para><para>|     but not contain the required
    /// </para><para>|     "public static partial class Test" declaration,
    /// </para><para>|     or its "OnlySelectedTestDataAreUsed_TDS_Ex02"
    /// field name
    /// </para><para>|     might be misspelled.
    /// </para><para>|   If file TDS_Ex02.cs should not be in the TDS system,
    /// </para><para>|     remove its name from TestMethodsSourceFiles[].
    /// </para>
    /// </summary>
    const string MissingSymbolMsg = @"
      Symbol ""{0}"" is apparently not defined;
        it should be defined in file {1} .

        File {1} may be missing or be malformed in some way;
          for example, {1} might be present in the project
          but not contain the required
          ""public static partial class Test"" declaration,
          or its ""{0}"" field name
          might be misspelled.
        If file {1} should not be in your TDS project,
          remove its name from TestMethodsSourceFiles[].";

    /// <summary>
    /// Name of the TDS test method that checks for
    /// the presence of a
    /// "#define RunOnlySelectedTestData"
    /// directive in any of the TDS source files
    /// listed in
    /// <see cref="TestMethodsSourceFiles"/>.
    /// <para>It fails whenever
    /// such a directive is active.
    /// </para><para>
    /// Value = "AllTestsAreToBeRunTest",
    /// a reference to the 
    /// <see cref="AllTestsAreToBeRunTest"/>()
    /// TDS test method.</para>
    /// </summary>
    const string NameOfAllTestsAreToBeRunTest
          = "AllTestsAreToBeRunTest";

    #region ReportSymbols
    //TODO: Conditional-compilation symbols in TDS.cs -- For each such symbol
    //      that may be used in this TDS source file,
    //      include a similar #if() directive.
    //  This is not necessary for the symbol DEBUG,
    //      which is usually defined whenever this code runs.
    // Code snippet TdsSymbol may be used to generate this code.
    // Each TDS source file should contain a similar #region.

    /// <summary>
    /// True iff "#define RunOnlySelectedTestData"
    /// is active in this file, TDS.cs .
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is intended to be true
    /// iff some test cases (sets of values for
    /// initialization and comparison)
    /// within each test method in this source file
    /// may be skipped during the test run.
    /// </para><para>
    /// For example, a calling program might utilize this
    /// to decide to issue a warning message
    /// that not all cases are being tested.
    /// </para><para>
    /// Each source file in the TDS namespace
    /// should define a field similar to this,
    /// to be used by the method specified by
    /// <see cref="NameOfAllTestsAreToBeRunTest"/> .
    /// </para>
    /// </remarks>
    public const bool RunOnlySelectedTestData_TDS =
#if RunOnlySelectedTestData
 true;
#else
 false;
#endif //(RunOnlySelectedTestData)

    /// <summary>
    /// True iff "#define NUnit_platform"
    /// is active in this file, TDS.cs .
    /// </summary>
    public const bool NUnit_platform_TDS =
#if NUnit_platform
 true;
#else
 false;
#endif //NUnit_platform

    /// <summary>
    /// True iff "#define TDS_platform"
    /// is active in this file, TDS.cs .
    /// </summary>
    public const bool TDS_platform_TDS =
#if TDS_platform
 true;
#else
 false;
#endif //TDS_platform

    /// <summary>
    /// True iff "#define UseNamedObjectTypeInTestableConsoleMethodTest"
    /// is active in this file, TDS.cs .
    /// </summary>
    public const bool UseNamedObjectTypeInTestableConsoleMethodTest_TDS =
#if UseNamedObjectTypeInTestableConsoleMethodTest
 true;
#else
 false;
#endif //UseNamedObjectTypeInTestableConsoleMethodTest
    #endregion ReportSymbols

    /// <summary>
    /// Status resulting from running a test.
    /// <para>Reported results follow the order implied 
    /// by the enum order, with
    /// the list of <see cref="Passed"/> tests appearing first.</para>
    /// </summary>
    /// <remarks>The integer values of these members
    /// will be reflected in the exit code
    /// returned by the program.</remarks>
    internal enum RunStatus : int
    {
      /// <summary>
      /// Test method did not raise an exception
      /// indicating that an <c>Assert{}</c> method failed
      /// </summary>
      Passed = 0,
      /// <summary>
      /// Test method raised an exception other than Inconclusive
      /// </summary>
      Failed,
      /// <summary>
      /// Test method raised an InconclusiveException exception
      /// </summary>
      Inconclusive,
      /// <summary>
      /// Highest value in the list
      /// </summary>
      Max = Inconclusive
    }  // end:RunStatus enum

    /// <summary>
    ///Main part of the name of each file's field containing the
    ///  state of the file's "RunOnlySelectedTestData" symbol.
    /// <para>For example, in file TDS_Ex01.cs ,
    /// the name of the field is OnlySelectedTestDataAreUsed_TDS_Ex01,
    /// formed by appending the file name (without the ".cs" suffix)
    /// to the value of <see cref="SelectedDataSymbol"/>.
    /// </para><para>
    /// Its constant value is "RunOnlySelectedTestData".</para>
    /// </summary>
    const string SelectedDataSymbol = "RunOnlySelectedTestData";

    /// <summary>
    /// Message warning about possible omitted test cases.
    /// <para>{0} = comma-separated list of file names</para>
    /// <para>Example, with {0} = "TDS.cs":
    ///</para><para>| *** Some test cases may have been skipped! ***
    ///</para><para>|
    ///</para><para>|   To run all test cases in the test methods
    ///that are called,
    ///</para><para>|   comment out or otherwise disable any
    ///</para><para>|   "#define RunOnlySelectedTestData" directives
    ///</para><para>|   in the following file(s):
    ///</para><para>|   TDS.cs</para>
    /// </summary>
    const string SkippedTestsMsg = @"
      *** Some test cases may have been skipped! ***

        To run all test cases in the test methods that are called,
        comment out or otherwise disable any
        ""#define RunOnlySelectedTestData"" directives
        in the following file(s):
        {0}
";

    /// <summary>
    /// Original Console input stream (the keyboard).
    /// <para>If we warp the Console during testing, we can
    /// use <c>Console.SetIn(<see cref="StdIn"/>)</c>
    /// and <c>Console.SetOut(<see cref="StdOut"/>)</c> to restore it.</para>
    /// </summary>
    public static StreamReader StdIn
    {
      get
      {
        return new StreamReader(Console.OpenStandardInput());
      }  // end:get
    }  // end:StdIn

    /// <summary>
    /// Original Console output stream (the command window).
    /// <para>If we warp the Console during testing, we can
    /// use <c>Console.SetIn(<see cref="StdIn"/>)</c>
    /// and <c>Console.SetOut(<see cref="StdOut"/>)</c>
    /// to restore it.</para>
    /// </summary>
    public static StreamWriter StdOut
    {
      get
      {
        var stdOut1 = new StreamWriter(Console.OpenStandardOutput());
        stdOut1.AutoFlush = true;
        return stdOut1;
      }  // end:get
    }  // end:StdOut

    /// <summary>
    /// Each item in this <c>List</c> matches one of the files
    /// in <see cref="TestMethodsSourceFiles"/>.
    /// <para>Item1 = file name, without extension
    /// (example: "TDS")</para>
    /// <para>Item2 = extension
    /// (example: ".cs")</para>
    /// </summary>
    static List<Tuple<string, string>> TdsFileNamesList
          = new List<Tuple<string, string>>();

    /// <summary>
    /// RegEx pattern identifying valid TDS file names in
    /// the list provided in <c>TestMethodsSourceFiles</c>.
    /// </summary>
    /// <remarks>
    /// Lines beginning (ignoring white space) with
    /// "//" are excluded from consideration.
    /// <para>
    /// Text following the file name is ignored, so the line
    /// "            TDS_Ex01.cs.txt"
    /// is interpreted as FileName = "TDS_Ex01"
    /// and Ext = ".cs" .
    /// </para>
    /// </remarks>
    static string TdsFileNames_Pattern =
          @"^\s*(?<FileName>[\p{L}]"
          + @"[\p{L}\p{Nd}_]*)(?<Ext>.([\p{L}\d])+)";

    /// <summary>
    /// This allows us to identify TDS [TestMethod] methods by name.
    /// </summary>
    /// <remarks>
    /// Example: The method
    /// <see cref="AllTestsAreToBeRunTest"/>
    /// may be run using
    /// <see cref="RunTestMethod"/>, where <c>MethodInfo</c> =
    /// <see cref="TestMethodDict"/><c>["AllTestsAreToBeRunTest"]</c>).
    /// </remarks>
    static Dictionary<string, MethodInfo> TestMethodDict
      = new Dictionary<string, MethodInfo>();

    /// <summary>
    /// Each item in this <c>List</c> matches
    /// one of the TDS test methods listed
    /// in <see cref="TestMethodsToBeRun"/> .
    /// <para>The method named in 
    /// <see cref="NameOfAllTestsAreToBeRunTest"/>
    /// is automatically added to
    /// the end of this list, as if it were
    /// explicitly included at the end of the
    /// <see cref="TestMethodsToBeRun"/> string.
    /// </para><para>
    /// Example: "AllTestsAreToBeRunTest"</para>
    /// </summary>
    static List<string> TestMethodsToBeRunList
          = new List<string>();

    /// <summary>
    /// <para>
    /// RegEx pattern identifying valid C# identifiers
    /// that may be used as names of TDS test methods in
    /// the list provided to <see cref="RunTdsTests()"/> .
    /// </para><para>
    /// It allows "//" at the beginning of a line
    /// to be used to ignore that line, filtering out
    /// the named method from the list of TDS methods to be run.
    /// </para><para>
    /// Names that are bypassed, for example due to
    /// misspelling or being preceded by "//",
    /// will be listed near the end of the TDS test report.
    /// </para><para>
    /// The prefix "TDS.Test." (case sensitive)
    /// may precede the TDS method name.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is intended to match the pattern for
    /// C# identifiers, weeding out lines with malformed 
    /// names (or those beginning with "//").
    /// The name may begin with '@' or '_' or
    /// a letter, and that may be followed by
    /// any of a greater variety of characters.
    /// </para><para>
    /// Note that the underline ('_') character is included
    /// in the "\p{Pc}" (= "ConnectorPunctuation")
    /// Unicode general category.
    /// </para><para>
    /// This pattern does not account for names containing
    /// Unicode escape sequences;
    /// modify the pattern if your test method names need to 
    /// include any such escape sequences.</para>
    /// </remarks>
    static string TestMethodsToBeRun_Pattern =
          @"^\s*(TDS\.Test\.)?"
          + @"(?<MethodName>@?[\p{L}\p{Nl}_]"
          + @"[\p{L}\p{Nl}\p{Nd}\p{Pc}\p{Mn}\p{Mc}\p{Cf}]*)";

    /// <summary>
    /// String included at the beginning of each trace message,
    /// currently "***** " .
    /// </summary>
    /// <remarks><para>This can help identify messages
    /// in the Console output that indicate
    /// that the constructors (for example) have run.</para>
    /// <para>Since each such message contains this string,
    /// all of these messages can be easily located
    /// if you wish to remove them.</para>
    /// <para>One exception occurs in the #if Code_to_be_copied region,
    /// in the example static constructor for the <c>Program{}</c> class,
    /// where the literal value "***** " is used
    /// instead of the name <c>TraceFlag</c>,
    /// to avoid making an unwanted reference to TDS code.</para>
    /// </remarks>
    public const string TraceFlag = "***** ";

    /// <summary>
    /// <para>
    /// Test to verify that conditional compilation symbol
    /// <c>"RunOnlySelectedTestData"</c>
    /// is undefined in all TDS source files.
    /// </para><para>
    /// The compiler directive
    /// <c>"#define RunOnlySelectedTestData"</c> may be
    /// active (de-commented) in some TDS source files,
    /// to allow test methods to run only selected test cases,
    /// for example to facilitate debugging the test methods or
    /// the function members that are being tested.
    /// However, it is likely to mislead the test system
    /// (NUnit or Visual Studio) into indicating that some tests
    /// are succeeding when they would fail if they were
    /// to be run against the full set of test cases.
    /// </para><para>
    /// This method is designed to fail if any of the
    /// other [TestMethod]s in the TDS files are only partially run,
    /// as a reminder to disable (comment out) each
    /// <c>"#define RunOnlySelectedTestData"</c> directive
    /// when it is no longer needed.
    /// </para>
    /// </summary>
    /// <remarks><para>
    /// The purpose is to help identify TDS source
    /// files in which some test cases may have been skipped,
    /// by listing any remaining TDS files that have active
    /// <c>"#define RunOnlySelectedTestData"</c> directives
    /// and including this list of file names in the test report.
    /// </para><para>
    /// For this method to operate as intended,
    /// all the TDS source files should be listed in
    /// <see cref="TestMethodsSourceFiles"/>,
    /// and each TDS file should contain a
    /// <c>"#region ReportSymbols"</c> region similar to the
    /// one defined in TDS.cs, defining public constants
    /// similar to <see cref="RunOnlySelectedTestData_TDS"/>
    /// that identifies if a <c>#define RunOnlySelectedTestData</c>
    /// directive is active in that file.
    /// </para></remarks>
    [TestMethod]
    public void AllTestsAreToBeRunTest()
    {
      try
      {
        if (IsUsingStandAloneTds)
          InitializeTestMethod();

        //Produce a list of files
        //  containing "#define RunOnlySelectedTestData"
        //  similar to "TDS.cs, TDS_Ex01.cs".
        //If none, value = "".
        var fileList = (
              from s in ConditionalSymbolsList
              where s.Symbol == SelectedDataSymbol
                && s.IsDefined
              select s.FileWithExt)
            .Aggregate("",
              (list, file) => list + ", " + file,
              list => list == "" ? "" : list.Substring(2));

        Assert.IsTrue(fileList == "",
              string.Format(SkippedTestsMsg
                  , fileList //{0}
              ));
      }
      finally
      {
        if (IsUsingStandAloneTds)
          CleanupTestMethod();
      }
    }  // end:AllTestsAreToBeRunTest()

    /// <summary>
    ///Run each listed test method and collect a list of the results,
    ///    to include status and the text of exception messages.
    ///<para>Tests are allowed to be listed,
    /// and therefore run, more than once.
    ///</para>
    /// </summary>
    /// <remarks>
    ///Messages written to the Console are intended to mimic those
    ///   displayed in NUnit's "Text Output" window.
    /// </remarks>
    /// <param name="testStatusList">
    /// Output from tests having a given RunStatus.
    /// </param>
    private static void CallTdsMethods(
          List<TdsMethodResults> testStatusList)
    {
      InitializeClasses();

      var t = new Test();
      foreach (var tdsMethodName in TestMethodsToBeRunList)
        if (TestMethodDict.Keys.Contains(tdsMethodName))
          testStatusList.Add(
                RunTestMethod(t, TestMethodDict[tdsMethodName]));

      CleanupTestSession();
    }  // end:CallTdsMethods()

    /// <summary>
    /// <c>CheckForInconsistentPlatforms()</c>
    ///  throws an exception if
    ///  test platform values
    ///  (Microsoft, NUnit, or neither)
    ///  are inconsistent.
    /// </summary>
    /// <remarks>
    ///  Often, the program won't even compile if
    ///  the platforms are inconsistent, but
    ///  if it does, this can be used to 
    ///  report the error.
    /// </remarks>
    /// <exception cref="ApplicationException">
    /// This is thrown if
    /// the conditional-compilation directives
    /// (<c>"#define NUnit_platform"</c>
    /// or <c>"#define TDS_platform"</c>)
    /// specifying the test platform to be used
    /// in each TDS source file are inconsistent.
    /// The choice should be the same in each file.
    /// </exception>
    static void CheckForInconsistentPlatforms()
    {
      //Unit-test platform to be used if neither 
      //  "#define NUnit_platform" nor
      //  "#define TDS_platform" is specified.
      var Ms_Platform = @"Microsoft platform";

      //This Dictionary counts the number
      //  of TDS source files that use
      //  each of the 3 test platforms.
      //Each item reflects which TDS source
      //  files use the test platform
      //  (Microsoft, NUnit, or neither)
      //  that the key specifies.
      //For example, if the directive
      //  "#define NUnit_platform"
      //  is used in file TDS.cs, then the constant
      //    NUnit_platform_TDS
      //  has a value of True,
      //  and the value of
      //    platDict["NUnit_platform"]
      //  has "TDS" appended to it to reflect the
      //  directive's presence in the file.
      var platDict = new Dictionary<string, string>();
      platDict.Add(@"NUnit_platform", "");
      //  "TDS_platform" specifies that neither NUnit
      //  nor Microsoft test platform is to be used.
      platDict.Add(@"TDS_platform", "");
      platDict.Add(Ms_Platform, "");

      var definedPlatforms = (
            from tdsFile in
              (from fn in TdsFileNamesList
               select fn.Item1 + fn.Item2)
            from s in ConditionalSymbolsList
            where s.IsDefined
              && s.FileWithExt == tdsFile
              && platDict.Keys.Contains(s.Symbol)
            select new { FileName = s.FileWithExt, Platform = s.Symbol }
          //Example: new { FileName = "TDS_Ex01.cs", Platform = "TDS_platform" }
          );

      foreach (var f in
            from fn in TdsFileNamesList
            select fn.Item1 + fn.Item2)
      {
        var whichP = (
                from p in definedPlatforms
                where p.FileName == f
                select p.Platform)
              .DefaultIfEmpty(Ms_Platform)  //Microsoft platform
              .First();
        platDict[whichP] += f + ", ";
      }

      var specifiedPlatforms = (
            from v
            in platDict
            where v.Value != ""
            select v);

      if (specifiedPlatforms.Count() <= 1) return;

      //Here we know that the platforms
      //  are inconsistent.
      //Generate a message identifying them,
      //  and throw an exception.
      var platformsUsed = specifiedPlatforms
            .Aggregate(@"",
              (result, platform) =>
                string.Format(
                @"{0}
#define {1}  (used in {2})"
                , result  //{0}
                , platform.Key  //{1}
                , platform.Value.Substring(0,
                  platform.Value.Length - 2)  //{2}
                )
              );

      throw new ApplicationException(
            string.Format(
@"Only one of the following platforms should be used: {0}"
        , platformsUsed  //{0} 
        ));

    }  // end:CheckForInconsistentPlatforms()

    /// <summary>
    /// <para>This is intended to be run after, or at the end of,
    /// each <c>[TestMethod]</c> method,
    /// to finalize the code being tested.</para>
    /// <para>It writes a message to the Console
    /// (visible on the test report),
    /// but, for ease of maintenance, any other code needed at the
    /// end of each test should also be located here.</para>
    /// </summary>
    public static void CleanupTestMethod()
    {

      //TODO: CleanupTestMethod() -- Add other test-cleanup code here

      Console.WriteLine(
@"{0}CleanupTestMethod() is complete.
{0}(End of test)
"
              , TraceFlag  //{0}
            );
    }  // end:CleanupTestMethod()

    /// <summary>
    /// Instance-method version of 
    /// <see cref="CleanupTestMethod()"/>,
    /// for use with 
    /// <see cref="TestCleanupAttribute"/>.
    /// </summary>
    [TestCleanup]
    public void CleanupTestMethod_()
    {
      CleanupTestMethod();
    }  // end:CleanupTestMethod_()

    /// <summary>
    /// <para>This is intended to be run after
    /// the last <c>[TestMethod]</c> method is complete,
    /// to finalize the code being tested.</para>
    /// <para>It writes a message to the Console (visible on the test report),
    /// but, for ease of maintenance, 
    /// any other code needed to be run after the final test
    /// should also be located here.</para>
    /// </summary>
    [ClassCleanup]
    public static void CleanupTestSession()
    {

      //TODO: CleanupTestSession() -- Add other end-of-session code here


      Console.WriteLine(@"
{0}The final test was completed at {1} .
{0}CleanupTestSession() is complete.
"
            , TraceFlag  //{0} 
            , DateTime.Now.ToString("o")  //{1}, high-precision time
            );

    }  // end:CleanupTestSession()

    /// <summary>
    /// Do housekeeping relating to user-maintained lists
    /// and <c>"#define"</c> directives.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Collect names from <see cref="TestMethodsSourceFiles"/>
    /// and <see cref="TestMethodsToBeRun"/>;
    /// populate <see cref="ConditionalSymbolsList"/>;
    /// check <see cref="SelectedDataSymbol"/>
    /// and <see cref="RunOnlySelectedTestData"/> for consistency;
    /// report on files with active #define statements;
    /// compare specified and available [TestMethod]s.
    /// </para></remarks>
    static void CollectSymbols()
    {
      #region Collect names from TestMethodsSourceFiles and TestMethodsToBeRun
      //Collect each name that qualifies as a TDS source-code file
      //  at the beginning of a line in TestMethodsSourceFiles.
      foreach (Match m in Regex.Matches(TestMethodsSourceFiles,
            TdsFileNames_Pattern,
            RegexOptions.Multiline | RegexOptions.Compiled))
        TdsFileNamesList.Add(
          new Tuple<string, string>(
              m.Groups["FileName"].Value.Trim(),
              m.Groups["Ext"].Value.Trim()));

      //Collect each name that qualifies as a C# identifier
      //  at the beginning of a line in TestMethodsToBeRun.
      //Keep them in the order listed in TestMethodsToBeRun.
      foreach (Match m in Regex.Matches(TestMethodsToBeRun,
            TestMethodsToBeRun_Pattern,
            RegexOptions.Multiline | RegexOptions.Compiled))
        TestMethodsToBeRunList.Add(
              m.Groups["MethodName"].Value.Trim());

      //Always include AllTestsAreToBeRunTest() at the end.
      TestMethodsToBeRunList.Add(NameOfAllTestsAreToBeRunTest);
      #endregion Collect names from TestMethodsSourceFiles and TestMethodsToBeRun

      ConditionalSymbolsList = GetConditionalSymbols();

      #region Check SelectedDataSymbol/RunOnlySelectedTestData
      //Check file names.  All TDS files should contain some
      //  properly named "#region ReportSymbols" symbols,
      //  specifically including one with a name similar to 
      //    "RunOnlySelectedTestData_TDS",
      //  the name in file TDS.cs that corresponds to a
      //    "#define RunOnlySelectedTestData" directive in the file. 
      //  (Any such symbols that do not end with a valid TDS file name,
      //  namely, a file listed in TestMethodsSourceFiles,
      //  will not be recognized here.)
      var filesWithNoSyms = (
            from fName in TdsFileNamesList
            select fName.Item1 + fName.Item2)
            .Except((
                from sym in ConditionalSymbolsList
                where sym.Symbol == SelectedDataSymbol
                select sym.FileWithExt)
              .Distinct());

      var numFilesWithNoSyms = filesWithNoSyms.Count();
      if (numFilesWithNoSyms > 0)
        Console.WriteLine(@"
{0}Error -- The following TDS file{1} no matching
      ""#region ReportSymbols"" symbols defined: {2} 
      (and/or the ""#region ReportSymbols"" region{3}
      not up to date) *****
"
              , TraceFlag  //{0}
              , numFilesWithNoSyms > 1 ? "s have" : " has"  //{1}
              , filesWithNoSyms.Aggregate("",
                (string list, string fName) => list + ", " + fName,
                (string list) => list.Substring(2)) //{2}
              , numFilesWithNoSyms > 1 ? "s are" : " is"  //{3}
               );
      #endregion Check SelectedDataSymbol/RunOnlySelectedTestData

      #region Report on files with active #define statements
      foreach (var tdsFile in (
              from s in ConditionalSymbolsList
              where s.IsDefined
              let file = s.FileWithExt
              orderby file
              select file)
            .Distinct()
        )
      {
        var thisFilesSymbols =
              from s in ConditionalSymbolsList
              let symbol = s.Symbol
              where s.IsDefined
                && s.FileWithExt == tdsFile
              orderby symbol
              select symbol;

        var numSyms = thisFilesSymbols.Count();

        Console.WriteLine(
@"{0}The following conditional compilation directive{1}
      included in TDS source-code file {2}:"
          , TraceFlag  //{0} 
          , numSyms <= 1 ? @" is" : @"s are"  //{1} 
          , tdsFile  //{2} 
          );

        foreach (var utName in thisFilesSymbols)
          Console.WriteLine(
@"          #define {0}"
                , utName  //{0} 
                );

        Console.WriteLine();

      }  // end:foreach (var tdsFile ...
      #endregion Report on files with active #define statements

      CheckForInconsistentPlatforms();

      #region Identify specified and available [TestMethod]s
      //Collect all suitable methods having [TestMethod] attributes,
      //  in preparation for running them.
      //Static unit-test methods are excluded from this list because the
      //  Microsoft.VisualStudio.TestTools.UnitTesting tools
      //  appear to ignore them.
      foreach (var ut in
            from unitTest in typeof(Test)
                  .GetMethods(
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly)
            from myAtt in unitTest.GetCustomAttributesData()
            where myAtt.ToString().Contains("TestMethodAttribute()]")
            select new { unitTest.Name, unitTest })
        TestMethodDict.Add(ut.Name, ut.unitTest);

      #endregion Identify specified and available [TestMethod]s

    }  // end:CollectSymbols()

    /// <summary>
    /// Run the listed TDS methods and 
    /// send their test results to the Console
    /// </summary>
    /// <param name="testStatusList">List of returns from the TDS methods.</param>
    /// <param name="totals">List of number of TDS methods
    /// returning each type of <see cref="RunStatus"/>.</param>
    /// <returns>Error level of the collection of tests,
    /// = highest error level returned.</returns>
    private static int DisplayTdsTestResults(
            List<TdsMethodResults> testStatusList,
            Dictionary<RunStatus, int> totals
          )
    {
      int errorLevel;
      CallTdsMethods(testStatusList);

      #region void writeSeparator() definition
      //Write a separator line (a "rule") to the Console
      //  to visually separate the sections of the status report
      Action writeSeparator = () =>
            Console.WriteLine(@"
________________"
            );
      #endregion void writeSeparator() definition

      //The following messages contain information about the results of tests.
      //Some of the included information is similar to messages that might
      //    appear in NUnit's "Errors and Failures" window, but the format
      //    is different.
      #region Display the lists of tests and results
      writeSeparator();

      Console.WriteLine(@"
{0} This was a test run.  The following results were generated. {0}"
              , TraceFlag  //{0}
            );

      for (RunStatus status = 0;
            status <= RunStatus.Max; status++)
      {
        //List of names of test methods returning this status
        //  ListPage[].Name = method name
        //  ListPage[].ExMsg = "Assert" exception message text
        var listPage =
              from s in testStatusList
              where s.TestStatus == status
              select new
              {
                //Method name
                Name = s.MethodName,
                //"Assert" exception message text
                ExMsg = s.AssertEx == null
                  ? ""
                  : s.AssertEx.InnerException.Message
              };

        var listCount = listPage.Count();

        totals.Add(status, listCount);

        if (listCount == 0)
        {
          writeSeparator();

          //Example:
          //No called test method returned a status of Failed.
          Console.WriteLine(@"
No called test method returned a status of {0}."
                , status  //{0}
                );
        }
        else
        {
          writeSeparator();

          //Example:
          //Failed tests
          //  The following 3 test methods returned a status of Failed:
          Console.WriteLine(@"
{0} tests
  The following {1}test method{2} returned a status of {0}:"
                , status  //{0}
                , listCount <= 1 ? "" : listCount + " "  //{1}
                , listCount <= 1 ? "" : "s"  //{2}
                );

          foreach (var reportItem in
                 listPage)
          {
            if (status != RunStatus.Passed)
              //There are no comments on Passed tests,
              //  so there's no need to separate their reports
              Console.WriteLine();

            Console.WriteLine("  - {0}"
                    , reportItem.Name  //{0}
                  );
            if (reportItem.ExMsg != "")
              Console.WriteLine(
@"      Exception message:
{0}"
                      , reportItem.ExMsg  //{0}
                    );
          }
        }
      }  // end:for (var status...
      #endregion Display the lists of tests and results

      //The set of methods in the list of those to be run
      //  may not be the same as those with [TestMethod] attributes.
      //  Here we list any differences.
      #region List mismatched methods
      writeSeparator();

      var unitTestSet = new RunTdsTestsReport(
            TestMethodDict.Keys,
            "[TestMethod] attribute",
            "has a {0}",
            "have {0}s",
            "does not (yet) have a {0}",
            "do not (yet) have {0}s"
            );

      var tdsMethodSet = new RunTdsTestsReport(
            TestMethodsToBeRunList,
            "in the TestMethodsToBeRun list",
            "is {0}",
            "are {0}",
            "is not {0}",
            "are not {0}"
            );

      //Display mismatched method names, if any
      var isMismatched = unitTestSet.ExceptionReport(tdsMethodSet)
            | tdsMethodSet.ExceptionReport(unitTestSet);

      #endregion List mismatched methods

      #region Write test summary -- number Passed, Failed, or Inconclusive
      writeSeparator();

      //Write a summary, as in this example:
      //   Passed: 1  Failed: 0  Inconclusive: 2
      //
      for (RunStatus status = 0; status <= RunStatus.Max; status++)
      {
        Console.Write("{0}: {1}  "
          , status  //{0}
          , totals[status]  //{1}
          );
      }
      Console.WriteLine();
      writeSeparator();
      Console.WriteLine();

      if (isMismatched)
      {
        errorLevel = 1;  //Mismatched list

        Console.WriteLine(
@"  The TestMethodsToBeRun list does not match the [TestMethod] methods.");
      }
      else if (totals[RunStatus.Passed] > 0
       && totals[RunStatus.Failed] == 0
       && totals[RunStatus.Inconclusive] == 0)
      {
        errorLevel = 0;  //All tests passed

        Console.WriteLine(
              @"  All listed TDS test methods passed.");
      }
      else if (totals[RunStatus.Failed] > 0)
      {
        errorLevel = 3;  //A test failed

        Console.WriteLine(
              @"  At least one TDS test method failed.");
      }
      else if (totals[RunStatus.Inconclusive] > 0)
      {
        //A test was inconclusive
        errorLevel = 2;

        Console.WriteLine(
              @"  At least one TDS test was inconclusive.");
      }
      else if (totals[RunStatus.Passed] == 0)
      {
        //No test was run
        errorLevel = 1;

        Console.WriteLine(
              @"  No TDS tests were run.");
      }
      else
      {
        throw new ApplicationException(
              @"Internal error -- inconsistent conditions");
      }

      Console.WriteLine(@"
{0}(End of test summary)
"
            , TraceFlag  //{0} 
            );
      #endregion Write test summary -- number Passed, Failed, or Inconclusive
      return errorLevel;
    }  // end:DisplayTdsTestResults()

    /// <summary>
    ///getConditionalSymbols() returns a List of
    ///  <see cref="SymbolInfo"/> values,
    ///  reflecting the values of constants defined in 
    ///  the <c>"#region ReportSymbols"</c> code in 
    ///  each TDS source-code file and of any corresponding
    ///  <c>"#define ..."</c> directives in the file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For example, if the directive
    /// <c>"#define RunOnlySelectedTestData"</c>
    /// is used in file TDS.cs, then the constant
    /// <c>RunOnlySelectedTestData_TDS</c>
    /// has a value of True, and the returned list will include this item:
    /// <c>new SymbolInfo("RunOnlySelectedTestData", "TDS", True)</c></para>
    /// </remarks>
    /// <returns><c>List</c> of <see cref="SymbolInfo"/> identifying
    /// <c>"#define"</c> directives in TDS files.</returns>
    static List<SymbolInfo> GetConditionalSymbols()
    {
      var allReportingSymbols = (
            from reportingSymbol in typeof(Test)
              .GetFields(
                 BindingFlags.Public |
                 BindingFlags.Static |
                 BindingFlags.GetField |
                 BindingFlags.DeclaredOnly)
            where reportingSymbol.IsLiteral  //Select only constants
            select new
            {
              reportingSymbol.Name,
              IsDefined = reportingSymbol.GetValue(null)
                 as bool? ?? false
            });  //Collect all of these constants, even false ones.

      return new List<SymbolInfo>(
            from tdsFile in
              from fn in TdsFileNamesList
              select new { Name = fn.Item1, Ext = fn.Item2 }
            from symName in allReportingSymbols
            let suffix = "_" + tdsFile.Name  //Example: "_TDS"
            where symName.Name.EndsWith(suffix)
            // Example: "RunOnlySelectedTestData_TDS"
            select new SymbolInfo(
              Symbol: symName.Name.Substring(0,
                symName.Name.Length - suffix.Length)
              //Example: "RunOnlySelectedTestData"
              , File: tdsFile.Name  //Example: "TDS"
              , Ext: tdsFile.Ext  //Example: ".cs"
              , IsDefined: symName.IsDefined  //Example: True
              ));

    }  // end:GetConditionalSymbols() 

    /// <summary>
    /// This calls the classes under test,
    /// so that they can be initialized
    /// (for example, ensuring that each class's
    /// static constructor has been run to completion)
    /// before any test procedures are run.
    /// <para>It writes to the Console messages
    /// about selected conditional-compilation symbols
    /// that are defined in the TDS*.cs files.</para>
    /// </summary>
    /// <remarks>
    /// <para>If a unit test system, such as
    /// NUnit or Visual Studio Test, is being used, 
    /// and the <c>[ClassInitialize]</c> attribute is present,
    /// that system will call <see cref="InitializeClasses"/>()
    /// once before running any of the test methods,
    /// so all necessary initialization is complete by the time
    /// any test method is called.</para>
    /// <para>The reference to the static field
    /// <see cref="isInitialized"/> in each listed class
    /// is present only for the purpose of calling
    /// the class's static constructor.
    /// The intent is that this reference will set up
    /// any <c>static</c> fields referred to by the test methods,
    /// but using a reference to any other accessible
    /// <c>static</c> field or property,
    /// instead of to the class's
    /// <see cref="isInitialized"/> field,
    /// would accomplish the same purpose, and the
    /// definition of <see cref="isInitialized"/> could then
    /// be deleted without affecting
    /// <see cref="InitializeClasses"/>() .</para>
    /// </remarks>
#if NUnit_platform
    [ClassInitialize]
#endif //NUnit_platform
    public static void InitializeClasses()
    {

      Console.WriteLine(
@"{0}InitializeClasses() has begun running."
            , TraceFlag  //{0} 
            );

      //Edit the following statement to include Boolean expressions
      //  that refer to some accessible static field or property
      //  of each type to be tested, to ensure that
      //  that type's static constructor, if any, has been run
      //  before any of the TDS methods are run,
      //  and in the order specified.
      //Separate the expressions with single "|" or "&" operators,
      //  to ensure that all of them are utilized.
      //The expressions need not evaluate to (true).
      //This example initializes the types NewCode{},
      //  StaticCode{}, and TimeRounded{}.
      //Also consider adding similar expressions for
      //  other classes or structs defined in the working code.
      var callStaticConstructors = true
        ////TODO: InitializeClasses(), static variables -- For each testable type
        ////  in this Solution, specify a Boolean
        ////  expression that includes a reference to
        ////  some public static variable in that type.

        | NewCode.isInitialized
        | StaticCode.isInitialized
        | TimeRounded.Granularity == 0

        ;

      if (callStaticConstructors)  //This always-true expression avoids a Warning message
        CollectSymbols();

      //TODO: InitializeClasses() -- Add other class-setup code here

    }  // end: InitializeClasses()

    /// <summary>
    /// This is called by the Microsoft unit test framework, 
    /// similar to <see cref="InitializeClasses()"/>
    /// except that it includes a <paramref name="TestContext"/>
    /// parameter, which TDS ignores.
    /// </summary>
    /// <param name="context">This is intended to supply
    /// information from the Microsoft unit test framework
    /// to support data-driven tests.</param>
#if (!NUnit_platform && !TDS_platform)
    [ClassInitialize]
    public static void InitializeClasses(TestContext context)
    {
      InitializeClasses();
    }  //end: InitializeClasses(TestContext)
#endif //(!NUnit_platform & !TDS_platform)

    /// <summary>
    /// <para>This is intended to be run before, or at
    /// the beginning of, each <c>[TestMethod]</c> method,
    /// to initialize the code being tested.</para>
    /// <para>It calls <see cref="InitializeClasses"/>() and writes
    /// a message to the Console (visible on the test report),
    /// but, for ease of maintenance, any other code needed at the
    /// beginning of each test should also be located here.</para>
    /// </summary>
    /// <remarks>In standalone mode 
    /// (<see cref="IsUsingStandAloneTds"/> is true),
    /// <see cref="InitializeClasses"/>() has already
    /// been called by the static constructor, before this runs.</remarks>
    [TestInitialize]
    public void InitializeTestMethod()
    {

      Console.WriteLine(@"{0}InitializeTestMethod() was called at {1} ."
            , TraceFlag  //{0} 
            , DateTime.Now.ToString("o")  //{1}, high-precision time
            );

      //TODO: InitializeTestMethod() -- Add other test-setup code here



    }  // end: InitializeTestMethod()

    /// <summary>
    /// Return true iff the given <paramref name="idValue"/>
    /// begins with at least one of the tags listed
    /// in <paramref name="filter"/>.
    /// </summary>
    /// <remarks>
    /// For example, given <paramref name="filter"/> = "3 A2",
    /// this will return true only if
    /// <paramref name="idValue"/> begins with "A2" or "3".
    /// <para>A <paramref name="filter"/> value
    /// containing a tag of " 1 " will return true
    /// if <paramref name="idValue"/>
    /// begins with "1" or "  14" or "1hx"
    /// but not with "21".
    /// </para><para>
    /// The <paramref name="filter"/> string may 
    /// occupy more than one line (it may contain newline 
    /// characters, for example, to help with legibility).
    /// </para>
    /// </remarks>
    /// <param name="idValue">Value of the <c>Id</c>
    /// property of a test-case object</param>
    /// <param name="filter">Space-delimited list of tags
    /// appearing at the beginning of the value
    /// of <paramref name="idValue"/>
    /// if we want to use its test case.</param>
    /// <returns>True iff the value of <paramref name="idValue"/>
    /// begins with at least one of the tags
    /// listed in <paramref name="filter"/>.</returns>
    public static bool IsInSelectionList(string idValue, string filter)
    {
      var tagSet = filter.Split(new[] { ' ', '\r', '\n' },
          StringSplitOptions.RemoveEmptyEntries);
      
      return (from tag
              in tagSet
              where idValue.TrimStart().StartsWith(tag)
              select tag
            ).Count() > 0;
    }  // end:IsInSelectionList{}

    /// <summary>
    /// Run TDS tests and report on the results.<para>
    /// Exit code:
    /// </para><para>
    /// 0 = All tests passed
    /// </para><para>
    /// 1 = Tess to be run did not match
    /// the defined test methods,
    /// or no test was run
    /// </para><para>
    /// 2 = At least one test was Inconclusive
    /// </para><para>
    /// 3 = At least one test Failed
    /// </para>
    /// </summary>
    /// <remarks>
    /// This is the entry point when the
    /// TDS project is set as the StartUp project.
    /// <para>Note that, even though
    /// <see cref="RunTdsTests"/>() requires the
    /// report file's pathname, if any, to end with ".txt",
    /// that string is also the means by which <see cref="Main"/>()
    /// identifies the command-line parameter that specifies the file.
    /// </para>
    /// </remarks>
    /// <param name="args">Space-delimited list of parameters or switches<para>
    /// "/?" or "-?" displays help.
    /// (Actually, any unrecognized parameter displays help.)
    /// </para><para>
    /// Any argument ending with 
    /// "nopause" causes the program not to wait for
    /// keyboard input upon completion, useful when
    /// it is run from a *.BAT or *.ps1 file.
    /// </para></param>
    [STAThread]
    public static int Main(string[] args)
    {
      #region "helpText" usage information
      var helpText = @"
TDS runs the current unit-test suite.

Syntax: Use
   TDS -?
to display this Help information.

Use
    TDS
to display the test results and wait for keyboard input.

Use 
    TDS -nopause
to display the test report without pausing.

Use
    TDS <file>
  to write the entire test report to the specified <file>.
For example,
    TDS .\temp.txt
  will write the entire test report to file temp.txt .
Any text file name is suitable, but the "".txt"" is required.
If the specified file exists, an error message is displayed.

In Windows PowerShell, also specify the path, as in "".\TDS"".

Exit code values:
    0 = All tests passed.
    1 = Tests to be run did not match the defined test methods,
        or no test was run.
    2 = At least one test was Inconclusive.
    3 = At least one test Failed, or some other error occurred.
";
      #endregion "helpText" usage information

      #region TestReportPathname
      // Pathname to the file to contain output normally sent
      // to the Console during TDS tests, if an alternate text
      // file is specified on the command line.
      //
      // This may be useful if the output generated
      // by TDS is too lengthy for the Console window to display.
      //
      // The specified file name is expected to end with ".txt".
      // If no such argument is specified,
      // output goes to the Console window.
      var TestReportPathname = "";  //TODO: TestReportPathname -- Specify a pathname if needed
      #endregion TestReportPathname

      //Exit code; 0 = no errors
      var errorLevel = 0;

      //True to display Help text
      var isHelpRequest = false;

      //True to pause, with a message, before exiting
      var isPauseRequest = true;

      #region WaitForResponse() definition
      //If the "nopause" option is not specified,
      //  display a message and wait for a keypress.
      //With "nopause", do nothing.
      Action WaitForResponse = () =>
      {
        if (isPauseRequest)
        {
          Console.WriteLine(@"
Press the <Enter> key to finish . . .");
          Console.Read();  //Wait for the user's response.
        }
      };
      #endregion WaitForResponse() definition

      #region Collect command-line arguments
      foreach (var arg in args)
      {
        if (arg.ToLower().EndsWith("nopause"))
          isPauseRequest = false;
        else if (arg.ToLower().EndsWith(".txt"))
        {
          TestReportPathname = arg;
        }
        else
          //If any command-line argument is present, 
          //  and we don't recognize it,
          //  assume help is requested
          isHelpRequest = true;
      }  // end: foreach(...
      #endregion Collect command-line arguments

      #region Handle help request
      if (isHelpRequest)
      {
        //Display full Help information
        Console.WriteLine(helpText);

        //Always pause for Help requests, 
        //  even if "nopause" is specified
        isPauseRequest = true;

        WaitForResponse();
        return 0;
      }
      #endregion Handle help request

      //It's OK to run the tests
      try
      {
        errorLevel = RunTdsTests(TestReportPathname);
      }
      catch (ArgumentException e)
      {
        Console.WriteLine(e.Message);
        errorLevel = 3;  //Report that an error occurred
      }

      WaitForResponse();
      return errorLevel;
    }  // end: Main()

    /// <summary>
    /// Return an exception message suitable for reporting
    /// a function member's failure to raise an expected Exception.
    /// <para>Example: </para><para>
    /// TestableNoConsoleMethodTest(), test case 01 Sample test:
    /// The expected Exception, "Whoop",
    /// was not raised in this test case.</para>
    /// </summary>
    /// <param name="memberName">Name of the function member test
    /// containing this message</param>
    /// <param name="id">tCase.Id or similar test case label</param>
    /// <param name="expected">tCase.ExceptionExp, the expected exception message</param>
    /// <returns>Suitable exception message</returns>
    public static string MsgForMissingException(
          string memberName, string id, string expected)
    {
      return string.Format(@"
{0}Test(), test case {1}:
  No Exception was raised in this test case,
  but Exception ""{2}"" was expected."
              , memberName  //{0} Test name, without following "Test()" string
              , id          //{1} tCase.Id or similar test case label
              , expected    //{2}  tCase.ExceptionExp, the expected exception message
            );
    }  // end: MsgForMissingException()

    /// <summary>
    /// Return an exception message suitable for reporting
    /// the return of an unexpected Exception.
    /// <para>Example: </para><para>
    /// TestableNoConsoleMethodTest(), test case 01 Sample test:
    /// The expected exception should start with "Whoop".
    /// This unexpected exception was thrown:
    ///   "False Exception"
    /// </para>
    /// </summary>
    /// <param name="memberName">Name of the function member test
    /// containing this message</param>
    /// <param name="id">tCase.Id or similar test case label</param>
    /// <param name="expected">tCase.ExceptionExp, the expected exception message</param>
    /// <param name="actual">the exception that was improperly raised</param>
    /// <returns>Suitable exception message</returns>
    public static string MsgForUnexpectedException(
          string memberName, string id, string expected, string actual)
    {
      return string.Format(@"
{0}Test(), test case {1}:
  The expected exception should start with ""{2}"".
  This unexpected exception was thrown:
    ""{3}"""
              , memberName  //{0} Test name, without the following "Test()" string
              , id          //{1} tCase.Id or similar test case label
              , expected    //{2}  tCase.ExceptionExp, the expected exception message
              , actual      //{3}  the exception that was improperly raised
            );
    }  // end: MsgForUnexpectedException()

    /// <summary><para>
    /// Run the TDS test methods listed in
    /// <c>TestMethodsToBeRun</c> .
    /// </para><para>
    /// Return ERRORLEVEL 0 iff all tests passed.
    /// </para></summary>
    /// <remarks><para>
    /// This method may be called from <c>Main()</c>, 
    /// a static constructor, or some other suitable location,
    /// following the execution of all code that initializes
    /// any variables used by the code to be tested.
    /// </para><para>
    /// The text that this method writes to the Console mimics
    /// what should appear on NUnit's "Text Output" window
    /// or on Visual Studio Test Explorer's
    /// "Test Output" window for each test.
    /// This is followed by a report of which of the
    /// tested methods passed, failed, or were inconclusive,
    /// along with the text of any exceptions that they generated.
    /// </para><para>
    /// Although initialization code such as
    /// <see cref="InitializeClasses"/>()
    /// is run by both the TDS methods and a unit-test system such as NUnit,
    /// actions performed by <c>Main()</c> before the call to
    /// <see cref="RunTdsTests"/>() would be invisible to 
    /// a non-TDS test platform such as NUnit
    /// and could give rise to differing results.
    /// Placing the call to <see cref="RunTdsTests"/>()
    /// as early as possible following initialization
    /// may help to minimize such differences.
    /// </para><para>
    /// TDS is intended to be used as an aid to development
    /// and initial debugging of function members,
    /// rather than as a comprehensive testing system.
    /// The TDS test methods are intended to be easily
    /// ported to a full unit-test system.
    /// </para></remarks>
    /// <param name="testReportPathname">
    /// <para>
    /// Pathname to the file to contain output normally sent
    /// to the Console during TDS tests, if an alternate text
    /// file is specified on the command line.
    /// </para><para>
    /// This may be useful if the output generated
    /// by TDS is too lengthy for the Console window to display.
    /// </para><para>
    /// The specified file name is expected to end with ".txt".
    /// If no such argument is specified,
    /// output goes to the Console window.
    /// </para>
    /// </param>
    /// <returns>Returned value is suitable for ERRORLEVEL exit code:<para>
    /// 0 = All tests passed
    /// </para><para>
    /// 1 = Tess to be run did not match defined test methods,
    /// or no test was run
    /// </para><para>
    /// 2 = At least one test was Inconclusive
    /// </para><para>
    /// 3 = At least one test Failed
    /// </para></returns>
    /// <exception cref="ApplicationException">Coding error --
    /// the conditions on which the summary message and
    /// exit code are based are inconsistent.</exception>
    /// <exception cref="ArgumentException">testReportPathname has a value,
    /// but the pathname either does not end in ".txt" or it refers
    /// to an existing text file.
    /// </exception>
    public static int RunTdsTests(string testReportPathname = "")
    {

      #region Check parameters
      if (testReportPathname != "")
      {
        if (!testReportPathname.ToLower().EndsWith(".txt"))
        {
          throw new ArgumentException(string.Format(
@"testReportPathname = ""{0}"", but it should end with "".txt""."
                , testReportPathname  //{0}
              ));
        }

        //Don't run if the output file would be overwritten
        if (File.Exists(testReportPathname))
        {
          throw new ArgumentException(string.Format(
@"TDS report file ""{0}"" exists and would be overwritten.
    Please delete the file and try again, or use a different pathname."
                  , testReportPathname  //{0}
                ));
        }
      }
      #endregion Check parameters

      //Exit-code value
      int errorLevel;

      //Signal that this method is not expected to be used
      //  in a unit-test system run (such as NUnit)
      IsUsingStandAloneTds = true;

      //Output from tests having a given RunStatus
      var testStatusList = new List<TdsMethodResults>();

      //Number of tests that generated a given RunStatus
      var totals = new Dictionary<RunStatus, int>((int)RunStatus.Max + 1);

      if (testReportPathname != "")
      {

        //Send Console output to the specified text file
        using (StreamWriter sw = File.CreateText(testReportPathname))
        {

          Console.WriteLine(@"[The remaining Console output is written to file ""{0}"".]"
                  , testReportPathname  //{0}
                );

          Console.SetOut(sw);
          errorLevel = DisplayTdsTestResults(testStatusList, totals);
          sw.Close();
        }
      }
      else
      {
        errorLevel = DisplayTdsTestResults(testStatusList, totals);
      }

      //Restore the Console to its original state
      Console.SetOut(StdOut);
      Console.SetIn(StdIn);

      return errorLevel;

    }  // end:RunTdsTests()

    /// <summary>
    /// Run the specified test method and return 
    /// Item1 = its status
    /// (<see cref="RunStatus.Passed"/>, etc.);
    /// Item2 = its name; and
    /// Item3 = any <see cref="Exception"/> that it raises,
    /// such as an <see cref="InconclusiveException"/> exception.
    /// </summary>
    /// <param name="testObject">Normally = "this",
    /// this references a TDS.Test object
    /// containing the TDS methods to be run.</param>
    /// <param name="testMethod">Test method to be run</param>
    /// <returns>{Item1 = status from running the test,
    /// Item2 = test method name,
    /// Item3 = Exception from test failure}</returns>
    static internal TdsMethodResults RunTestMethod(
          object testObject, MethodInfo testMethod)
    {

      const string listItemHdr = "";  //List item header
      var testStatus = RunStatus.Passed;

      var nameOfMethodBeingRun = testMethod.Name;
      var displayName = listItemHdr +
              nameOfMethodBeingRun + "()";
      var declaringType =
              testMethod.DeclaringType.FullName;  //such as "TDS.Test"

      //Write the method's name in a format similar to what NUnit uses, for example
      //   ***** TDS.Test.TestableConsoleMethodTest()
      //  (but NUnit doesn't include the parentheses after the method name)
      //  to serve as a header introducing output from the method.
      Console.WriteLine();
      Console.WriteLine(@"{0}{1}.{2}()"
            , TraceFlag  //{0}
            , declaringType  //{1}
            , nameOfMethodBeingRun  //{2}
      );

      //Exception thrown, expected to be from an Assert statement.
      Exception assertEx = null;

      try
      {
        //Invoke the method using its MethodInfo description
        testMethod.Invoke(testObject, null);
      }
      catch (Exception e)
      {
        assertEx = e;
        if (e.InnerException.TargetSite.Name.Contains("Inconclusive"))
          testStatus = RunStatus.Inconclusive;
        else
          //Any exception except AssertInconclusiveException,
          //  we assume, indicates failure
          testStatus = RunStatus.Failed;
      }

      return new TdsMethodResults(testStatus, displayName, assertEx);
    }  // end:RunTestMethod()

    /// <summary>
    /// <para>
    /// These are used as elements of <c>List</c>
    /// <see cref="ConditionalSymbolsList"/> .
    /// </para><para>
    /// Each item corresponds to a constant defined in the
    /// <c>#region ReportSymbols</c> region of a TDS source-code file,
    /// and therefore, we expect, also to a matching
    /// <c>#define</c> directive and conditional-compilation
    /// symbol name in that file.
    /// </para><para>
    /// For example, if "#define RunOnlySelectedTestData"
    /// is active in file <c>TDS.cs</c>, then
    /// the constant "RunOnlySelectedTestData_TDS"
    /// should be defined in the "#region ReportSymbols"
    /// region of file <c>TDS.cs</c>
    /// and should have the value <c>True</c>, so
    /// the list should include this item:
    /// <c>new SymbolInfo(Symbol = "RunOnlySelectedTestData",
    /// File = "TDS", IsDefined = True)</c> .
    /// </para>
    /// </summary> 
    internal struct SymbolInfo
    {

      /// <summary>
      /// Backing field for file extension in <c>.File</c>
      /// (expected to be ".cs")
      /// </summary>
      string extension;

      /// <summary>
      /// Backing field for <c>.File</c>
      /// </summary>
      string file;

      /// <summary>
      /// Backing field for <c>.IsDefined</c>
      /// </summary>
      bool isDefined;

      /// <summary>
      /// Backing field for <c>.Symbol</c>
      /// </summary>
      string symbol;

      /// <summary>
      /// The name of the file, with no trailing ".cs".
      /// Example: "TDS"
      /// </summary>
      public string File { get { return file; } }

      /// <summary>
      /// The name of the file, including trailing ".cs".
      /// Example: "TDS.cs"
      /// </summary>
      public string FileWithExt { get { return file + extension; } }

      /// <summary>
      /// True iff a #define declaration of the
      /// conditional-compilation symbol
      /// is active in the file
      /// </summary>
      public bool IsDefined { get { return isDefined; } }

      /// <summary>
      /// The name of the conditional-compilation symbol
      /// </summary>
      public string Symbol { get { return symbol; } }

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="Symbol">Conditional-compilation symbol name</param>
      /// <param name="File">TDS source-file name, without the ".cs" extension.</param>
      /// <param name="Ext">File name extension, with leading '.' .
      /// Example: ".cs" identifies the type of file
      /// as C# source code.</param>
      /// <param name="IsDefined">True iff "#define" directive for the <paramref name="Symbol"/> is active in the file.</param>
      public SymbolInfo(string Symbol, string File, string Ext, bool IsDefined)
      {
        symbol = Symbol;
        file = File;
        extension = Ext;
        isDefined = IsDefined;
      }  // end:SymbolInfo()
    } // end:SymbolInfo{}

    /// <summary>
    /// Static constructor for the Test{} class.
    /// </summary>
    static Test()
    {
      Console.WriteLine(TraceFlag +
            "Test{} class's static constructor has been called.");
    }  // end: Test()

    //TODO: TestableConsoleMethodTest() example -- Delete the contents of the following #region if
    //      the example TestableConsoleMethodTest() method is not needed.
    #region TestableConsoleMethodTest() example
    /// <summary>
    /// Template for tests of methods that utilize the Console.  
    /// <para>The method actually called in this template is
    /// <see cref="TestableConsoleMethod"/>(), 
    /// but its only purpose is as a demonstration.</para>
    /// </summary>
    /// <remarks>
    /// <para>The test report should contain something like this:</para>
    /// <para> ---</para>
    /// <para> Beginning test of case #A1 One line of input</para>
    /// <para> " done</para>
    /// <para> ":</para>
    /// <para> Finished line 1, case #A1 One line of input:</para>
    /// <para>    Returned: "Returned: DONE"</para>
    /// <para>    To Console: "To the console: DONE</para>
    /// <para> "</para>
    /// <para> ---</para>
    /// <para> Beginning test of case #A2 Test throwing exception</para>
    /// <para> "I dislike</para>
    /// <para>    gnats, bedbugs, and mosquitoes.</para>
    /// <para> ":</para>
    /// <para> Finished line 1, case #A2 Test throwing exception:</para>
    /// <para>    Returned: "Returned: I DISLIKE"</para>
    /// <para>    To Console: "To the console: I DISLIKE</para>
    /// <para> "</para>
    /// <para> Finished line 2, case #A2 Test throwing exception:</para>
    /// <para>    Exception "Bugs are detected in this program."
    /// was correctly thrown.</para>
    /// <para> ---</para>
    /// <para> Beginning test of case #B1 Multiple input lines</para>
    /// <para> "  Say  hello</para>
    /// <para>  score</para>
    /// <para> ":</para>
    /// <para> Finished line 1, case #B1 Multiple input lines:</para>
    /// <para>    Returned: "Returned: SAY  HELLO"</para>
    /// <para>    To Console: "To the console: SAY  HELLO</para>
    /// <para> "</para>
    /// <para> Finished line 2, case #B1 Multiple input lines:</para>
    /// <para>    Returned: "Returned: You're a winner!"</para>
    /// <para>    To Console: "To the console: You're a winner!</para>
    /// <para> "</para>
    /// </remarks>
    [TestMethod]
    public void TestableConsoleMethodTest()
    {
      #region testValues[]

#if RunOnlySelectedTestData
      //Space-separated list of "Id" tags of test cases, to be run
      //  in the order in which they are defined in testValues[]
      const string testSelectionList = "B A3 ";
#endif //RunOnlySelectedTestData

      var testValues = new[] {
    #if !UseNamedObjectTypeInTestableConsoleMethodTest
        //These test case values are defined via
        //  anonymous object initializers, which is probably
        //  the most convenient way to define them when 
        //  only one or two instances are present.
        //If more than one array element is present,
        //  comments describing the properties are needed only
        //  in or near the initializer of the first one
        //  (testValues[0]), though additional comments
        //  describing specific values may be useful at times.
        //
        //Advantages: The type of each property need not be specified.
        //  All of the code defining an instance is located
        //    in one C# statement.
        //  No type name need appear, keeping the declaration short.
        //  No type definition is needed, keeping the program short.
        //Disadvantages: Each property must be given an explicit value.
        //  Changes in the data structure must be applied identically
        //    to each array element.
        //  Properties must be declared in the same order in each instance.
        //  Comments on a property, or the type itself, 
        //    are not visible in IntelliSense nor in the Object Browser.

        new {
          // Test case identifier (required), consisting of
          //  a unique 2- or 3-character tag, a space,
          //  and a short description of the test case.
          Id = "A1 One line of input", 

          // String (possibly containing new-line characters)
          //    simulating the user's input from the keyboard
          InputLines = " done\r\n",

          // OutputExp = expected output to the Console window.
          //   Output = string to be displayed
          //   Exception = exception expected to be thrown, if any.
          //      This specifies a string that the beginning
          //      of the exception message, if any, is expected to match.
          //      "" is treated as "No exception is expected".
          OutputExp = new[] {
              new{Output = "DONE",
                Exception = ""},
            },
          },

        //Since more than one array element is defined here,
        //  and comments describing these properties
        //  were included in the definition of testValues[0],
        //  comments describing the properties need not be repeated
        //  on these other instances.
        //However, you may find it useful to apply comments to some
        //  of the specific values in the test cases.
        //For example, a comment on the value given here
        //  for property "InputLines" of this instance, "A2",
        //  is included with the property's value; 
        //  a comment describing the entire instance is contained 
        //  in the value of the instance's "Id" property;
        //  and a comment describing what the "InputLines"
        //  property means for all instances is in or near
        //  the initializer for the first ("A1") instance.
        new {
          Id = "A2 Test throwing exception",
          InputLines =
@"I dislike
   gnats, bedbugs, and mosquitoes.
But none are here.
",            //The 2nd line includes the forbidden word!
          OutputExp = new[] {
            new{Output = "I DISLIKE",
              Exception=""},
            new{Output = "",
              Exception="Bugs are detected"},
            new{Output = "BUT NONE ARE HERE.",
              Exception=""},
            },
          },

        new {
          Id = "B1 Multiple input lines",
          InputLines =
@"  Say  hello  
  score   
",
          OutputExp = new[] {
            new{Output = "SAY  HELLO",
              Exception = ""},
            new{Output = "You're a winner!",
              Exception = ""},
            },
          },

    #else  //#if UseNamedObjectTypeInTestableConsoleMethodTest
            //Note: These test case values are defined as
            //    objects of a named type, which is probably
            //    the most convenient way to define them when 
            //    many instances are present.
            //
            //  Advantages: Properties may be given default values.
            //    Changes in the data structure are easy to implement.
            //    Property values may appear in any desired order.
            //    XML comments on each property, and on the type itself,
            //      including property types and default values,
            //      are visible in IntelliSense and in the Object Browser.
            //  Disadvantages: Property types must be explicitly specified.
            //    Some of the code defining the object is located
            //      in a remote part of the program.
            //    A type name must appear with each instance declaration,
            //      occupying space.
            //    The type must be explicitly declared somewhere
            //      in the namespace.

            new TestableConsoleMethodTestCase (
              Id : "A1 One line of input",
              InputLines : " done\r\n",
              OutputExp : new[] {
                  new TestableConsoleMethodTestCaseOutputExp(
                    Output : "DONE")
                  }
              ),

            //This constructor's parameters can be defined to match 
            //    the object's properties.
            //    They are documented in the constructor's XML comments
            //    and are visible in Visual Studio's IntelliSense.
            //  However, you may also find it useful to apply comments
            //    to some of the specific values in the test cases.
            //    For example, a comment on the value given here
            //    for property "InputLines" 
            //    of this instance, "A2", is included
            //    with the property's value; and a comment
            //    describing the entire instance is contained 
            //    in the value of the instance's "Id" property.
            new TestableConsoleMethodTestCase (
              Id: "A2 Test throwing exception",
              InputLines:
@"I dislike
   gnats, bedbugs, and mosquitoes.
But none are here.
",             //The 2nd line includes the forbidden word!
               OutputExp: new []{
                 new TestableConsoleMethodTestCaseOutputExp(
                   Output: "I DISLIKE"),
                 new TestableConsoleMethodTestCaseOutputExp(
                   Exception: "Bugs are detected"),
                 new TestableConsoleMethodTestCaseOutputExp(
                   Output: "BUT NONE ARE HERE.")
               }
              ),

              //For brevity, you may be able to omit
              //  some of the parameters' names.
            new TestableConsoleMethodTestCase (
              "B1 Multiple input lines",
@"  Say  hello
 score 
",
              new[] {
                new TestableConsoleMethodTestCaseOutputExp(
                  Output: "SAY  HELLO"),
                new TestableConsoleMethodTestCaseOutputExp(
                  Output: "You're a winner!"),
                }
              ),

    #endif  //UseNamedObjectTypeInTestableConsoleMethodTest
          };
      #endregion testValues[]

      //These are used to restore the Console after the test.
      var stdIn = Console.In;
      var stdOut = Console.Out;

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

          #region Set up the Console
          Console.WriteLine(@"
---
Beginning test of case #{0}
""{1}"":"
                  , tCase.Id  //{0}
                  , tCase.InputLines   //{1}
                );

          var sr = new StringReader(tCase.InputLines);
          Console.SetIn(sr);

          //Destination for output from the method to the Console
          var sBldr = new StringBuilder("");

          //# of input line within tCase.InputLines (first line is line 1)
          var lineNum = 0;

          //Read lines from the (simulated) Console one by one; stop at EOF
          while (sr.Peek() != -1)
          {
            ++lineNum;

            //Erase any previous output
            sBldr.Length = 0;

            //Capture what the method writes to the Console
            Console.SetOut(new StringWriter(sBldr));
            #endregion Set up the Console

            #region Invoke testable function members
            //Message value of an exception that the
            //  function member to be called might throw
            var exceptionThrown = DefaultExceptionMessage;

            //Expected exception message for this line
            var exceptionMsgExp = tCase.OutputExp[lineNum - 1].Exception;
            if (exceptionMsgExp == "")
              exceptionMsgExp = DefaultExceptionMessage;

            //Local variable to receive a value to be returned
            //  by the function member
            var actual = "";

            try
            {
              //Example of invoking a method that returns a testable
              //  value and has a side effect (writing to the Console).
              actual = NewCode.TestableConsoleMethod();

              //Before any tests are added (in the "#region Apply tests"
              //    region), the purpose of the above statement
              //    is only to invoke the function member (in this
              //    case, TestableConsoleMethod )
              //    while it is under development, to make it easy
              //    to observe the values of variables during tracing,
              //    so any returned value is unimportant here.
              //After the new function member's code is complete enough
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
                    "TestableConsoleMethod", tCase.Id,
                    exceptionMsgExp, exceptionThrown
                    ));

              // Write progress info to the test report.
              // Note that the "Standard Console Output" in the test report
              //   might not contain what the Console wrote to sBldr.
              Console.WriteLine(
@"Finished line {0}, case #{1}:
       Exception ""{2}"" was correctly thrown."
                      , lineNum  //{0}
                      , tCase.Id  //{1}
                      , exceptionThrown  //{2}
                   );

              //Skip the remaining tests for this input line;
              //  no useful values are returned after an Exception.
              continue;
            }
            #endregion Invoke testable function members

            Console.SetOut(stdOut);  //Resume writing to standard output

            #region Apply tests when no exception is raised

            //Test that if no exception occurred, none was expected.
            Assert.IsTrue(
                  exceptionMsgExp == DefaultExceptionMessage,
                  MsgForMissingException(
                      "TestableConsoleMethod", tCase.Id,
                      exceptionMsgExp
                    ));

            Assert.AreEqual(
                  "Returned: " + tCase.OutputExp[lineNum - 1].Output,
                  actual,
                  string.Format(@"
TestableConsoleMethodTest(), test case ""{0}"", Argument:
{1}"
                    , tCase.Id  //{0}
                    , tCase.InputLines   //{1}
                  ));

            //Even if the tested method does not return any values
            //  to its caller, we can test what it writes to the Console.
            Assert.AreEqual(
                  "To the console: "
                    + tCase.OutputExp[lineNum - 1].Output + "\r\n",
                  sBldr.ToString(),
                  "Value written to Console, case #" + tCase.Id);

            // Write progress info to the test report.
            // Note that the "Standard Console Output" in the test report
            //   does not contain what the Console wrote to sBldr.
            Console.WriteLine(
@"Finished line {0}, case #{1}:
       Returned: ""{2}""
       To Console: ""{3}"""
                    , lineNum  //{0} 
                    , tCase.Id  //{1}
                    , actual  //{2}
                    , sBldr.ToString()  //{3}
                  );
            #endregion Apply tests when no exception is raised

          }  // end:while(...)

        }  // end:foreach(var tCase...

      }
      finally
      {
        Console.SetOut(stdOut);  //Restore the Console state
        Console.SetIn(stdIn);

        if (IsUsingStandAloneTds)
          CleanupTestMethod();
      }

      ////TODO: TestableConsoleMethodTest() -- Remove the Assert.Inconclusive()
      ////  statement after this [TestMethod] is working:
      Assert.Inconclusive(
@"Verify the correctness of TestableConsoleMethodTest().");

    }  // end: TestableConsoleMethodTest()

#if UseNamedObjectTypeInTestableConsoleMethodTest
    /// <summary>
    /// This class is used in defining test cases
    /// for <see cref="TestableConsoleMethodTest"/>() .
    /// </summary>
    /// <remarks>
    /// <para>Using a constructor allows access to
    /// read-only properties and fields,
    /// and thus may be preferable to using an object initializer.</para>
    /// <para>To convert anonymous object initializers
    /// to named-type constructors, with properties
    /// to be replaced by same-named constructor parameters,</para>
    /// <para>- Include the type name.</para>
    /// <para>- Change braces to parentheses.</para>
    /// <para>- Change "=" to ":".</para>
    /// <para>- Delete a trailing comma, if present.</para>
    /// <para>For example, the following anonymous-object initializer:
    /// </para><para>|new {
    /// </para><para>|  Id = "A1 One line of input", 
    /// </para><para>|  InputLines = " done\r\n",
    /// </para><para>|  OutputExp = new[] {
    /// </para><para>|    new {Output = "DONE", 
    /// </para><para>|      Exception = ""},
    /// </para><para>|    },
    /// </para><para>|  },
    /// </para>
    /// <para>may be replaced by the following constructor call,
    /// if the constructor is suitably defined:
    /// </para><para>|  new TestableConsoleMethodTestCase (
    /// </para><para>|    Id : "A1 One line of input",
    /// </para><para>|    InputLines : " done\r\n",
    /// </para><para>|    OutputExp : new[] {
    /// </para><para>|        new TestableConsoleMethodTestCaseOutputExp(
    /// </para><para>|          Output : "DONE")
    /// </para><para>|        }
    /// </para><para>|    ),
    /// </para><para>
    /// Comments in the anonymous object initializers that describe
    /// the properties may be deleted, since they should have been copied
    /// to XML comments in the new type definition.  
    /// However, if the original initializers contained any
    /// comments that applied to specific values,
    /// it might be helpful to leave those comments in 
    /// the instance constructors, with the special values they describe.
    /// </para>
    /// <para>When you use this class as a template,
    /// you will probably want to delete the code in the
    /// <c>#region MembersThatDoNotNeedToBeCopied</c> region
    /// and make suitable changes to the constructor.</para>
    /// </remarks>
    internal class TestableConsoleMethodTestCase
    {
      /// <summary>
      /// Test case identifier, consisting of
      ///    a unique 2- or 3-character tag, a space,
      ///    and a short description of the test case.
      /// </summary>
      /// <remarks><para>This is useful in <c>Assert</c> statements
      /// to identify the failing test case.</para>
      /// <para>The short tag is used,
      /// when <c>RunOnlySelectedTestData</c> is defined,
      /// to identify which test cases are to be run.
      /// </para>
      /// </remarks>
      internal readonly string Id;

      /// <summary>
      /// <para>Constructor with named arguments.</para>
      /// </summary>
      /// <param name="Id">Test case identifier (required),
      /// consisting of a unique 2- or 3-character tag, a space,
      /// and a short description of the test case.
      /// </param>
      /// <param name="ExpectedException">Message of exception
      /// expected to be thrown, if any.
      /// If the value is "No exception is thrown" (the default value),
      /// then the test case is expected not to cause an exception.</param>
      /// <param name="OutputExp">Expected output to the Console window.
      /// Default value is an array containing a single element with a value
      /// <para>new{Output = "", Exception = ""}.
      /// </para><para>
      /// Output = string to be displayed.
      /// </para><para>
      /// Exception = exception expected to be thrown, if any.
      /// This specifies a string that the beginning
      /// of the exception message is expected to match.
      /// </para></param>
      /// <param name="InputLines">String (possibly containing
      /// new-line characters) simulating the user's input
      /// from the keyboard</param>
      internal TestableConsoleMethodTestCase(
            string Id,
            string InputLines = "\r\n",
            TestableConsoleMethodTestCaseOutputExp[] OutputExp = null
            )
      {
        this.Id = Id;
        this.InputLines = InputLines;
        this.OutputExp = OutputExp
              ?? new[] {
                new TestableConsoleMethodTestCaseOutputExp(
                  Output: "",
                  Exception: ""), };

      }  // end:TestableConsoleMethodTestCase()

      //The members in this #region are specific to the present example
      //  and are likely not to be useful in a general-purpose template.
    #region Members that do not need to be copied

      /// <summary>
      /// Message used to indicate that no exception was thrown
      /// </summary>
      const string DefaultExceptionMessage = Test.DefaultExceptionMessage;

      /// <summary>
      /// String (possibly containing new-line characters)
      ///    simulating the user's input from the keyboard
      /// </summary>
      /// <remarks>This could be defined instead as a readonly field;
      /// it's defined as a property here to serve as an illustration.
      /// </remarks>
      internal string InputLines { get; private set; }

      /// <summary>
      /// Example instance method that does nothing except
      /// to invoke static method <see cref="TestableConsoleMethod"/>()
      /// to read and write the <see cref="System.Console"/>.
      /// </summary>
      /// <returns>Same value returned by
      /// <see cref="TestableConsoleMethod"/>() .</returns>
      /// <remarks>This method is defined only in order to demonstrate
      /// the use of an instance method in the code.</remarks>
      internal string TestableConsoleMethodI()
      {
        return NewCode.TestableConsoleMethod();
      }  // end:TestableConsoleMethodI()

      /// <summary>
      /// Expected output to the Console window
      /// </summary>
      internal readonly TestableConsoleMethodTestCaseOutputExp[] OutputExp;
    #endregion Members that do not need to be copied

    }  // end:TestableConsoleMethodTestCase{}

    /// <summary>
    /// This is used in specifying values of elements of
    /// <see cref="TestableConsoleMethodTestCase().OutputExp[]"/>
    /// </summary>
    internal struct TestableConsoleMethodTestCaseOutputExp
    {

      /// <summary>
      /// Backing field for Exception property.
      /// Empty string represents <see cref="DefaultExceptionMessage"/>
      /// </summary>
      string exception;

      /// <summary>
      /// Exception expected to the thrown, if any.
      /// This specifies a string that is expected to match
      /// the beginning of the exception message.
      /// <para>Defaut value is
      /// <see cref="DefaultExceptionMessage"/>.</para>
      /// </summary>
      internal string Exception
      {
        get
        {
          return exception == ""
            ? DefaultExceptionMessage
            : exception;
        }
        set
        {
          if (value == DefaultExceptionMessage)
            exception = "";
          else exception = value;
        }
      }  // end:Exception

      /// <summary>
      /// Expected output to the Console window
      /// </summary>
      internal string Output { get; private set; }

      /// <summary>
      /// Constructor specifying expected response from one input line.
      /// </summary>
      /// <param name="Output">
      /// Expected output to the Console window
      /// </param>
      /// <param name="Exception">
      /// Exception expected to the thrown, if any.
      /// <para>Empty string is treated as
      /// <see cref="DefaultExceptionMessage"/>.</para>
      /// </param>
      internal TestableConsoleMethodTestCaseOutputExp(
        string Output = "",
        string Exception = "")
        : this()
      {
        this.Output = Output;
        this.Exception = Exception;
      }

    } // end:TestableConsoleMethodTestCaseOutputExp{}
#endif //UseNamedObjectTypeInTestableConsoleMethodTest
    #endregion TestableConsoleMethodTest() example

    //TODO: New TDS methods may be placed here:



  }  // end:Test{}

}  // end: Namespace TDS