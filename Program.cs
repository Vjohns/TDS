//This file contains example code for a class called Program{},
//  which contains the application's entry point, Main(),
//  which calls the code being developed,
//  either (depending on DEBUG) directly or via TDS code.
//It is intended to be used for an application (*.exe file)
//  to be produced by the solution's Startup project.

using System;
using Console = System.Console;

namespace NewCodeNamespace
{

  /// <summary>
  /// Example Program{} class.
  /// <para>This is the Visual Studio solution's "Startup" object.</para>
  /// </summary>
  /// <remarks>These XML comments may be viewed
  /// in Visual Studio's Object Browser.</remarks>
  internal class Program
  {

    /// <summary>
    /// This indirectly calls <see cref="TestableNoConsoleMethod"/>
    /// and <see cref="TestableConsoleMethod"/>, writing
    /// their output to the Console.
    /// <para>These methods are examples representing
    /// code being developed.</para>
    /// <para>Depending on configuration, this either</para>
    /// <para>(Debug) calls the TDS code, which then calls
    /// the code being developed and reports on results, or</para>
    /// <para>(Release) directly calls the code being developed,
    /// with no reference to TDS codde.</para>
    /// </summary>
    /// <param name="args">Command-line parameters</param>
    static void Main(string[] args)
    {

      try
      {
        NewCode.RunWithoutTds();
      }
      catch (ApplicationException e)
      {
        Console.WriteLine(@"
Error: Program bug is detected; extermination is needed.
     {0}"
              , e.Message  //{0} Exception message
            );
      }  //end: catch()

      Console.WriteLine(@"
Press the <Enter> key to finish . . .");
      Console.ReadKey(true);  //Wait for the user's response.

    }  // end:Main()

  }  // end:Program{}

  /// <summary>
  /// Value including time fields
  /// rounded to the nearest 5 minutes.
  /// <para>The granularity of the rounding
  /// is specified by <see cref="Granularity"/>.
  /// </para>
  /// </summary>
  /// <remarks>Note: This type is included
  /// to illustrate a means of using TDS to test
  /// multiple constructors for the same type.
  /// Two constructors are defined here,
  /// and this struct's test method,
  /// <see cref="TDS.Test.TimeRoundedTest()"/>, uses
  /// a <c>switch</c> statement to test both,
  /// selecting the appropriate constructor
  /// for each test case.
  /// </remarks>
  public struct TimeRounded
  {

    /// <summary>
    /// Hour of the day
    /// </summary>
    public int Hour
    {
      get; private set;
    }

    /// <summary>
    /// Minutes modulo 1 hour,
    /// rounded to the nearest 5 minutes.
    /// </summary>
    public int Minute
    {
      get; private set;
    }

    /// <summary>
    /// Time granularity, 
    /// number of minutes to which a given time
    /// is to be rounded.
    /// <para>Current value is 5 minutes.</para>
    /// </summary>
    public static double Granularity
    {
      get { return 5.0D; }
    }

    /// <summary>
    /// Round a number of minutes to
    /// the nearest multiple of <see cref="Granularity"/>.
    /// </summary>
    /// <param name="fMinutes">Fractional minutes</param>
    /// <returns>Integer number of minutes, rounded
    /// </returns>
    static int RoundFloatMinutes(float fMinutes)
    {
      return (int)(
            Math.Round(fMinutes / Granularity)
              * Granularity
            );
    }  // end: RoundFloatMinutes()

    /// <summary>
    /// Static constructor.
    /// <para>This displays a message on the Console.</para>
    /// </summary>
    static TimeRounded()
    {
      Console.WriteLine(
@"***** TimeRounded{} struct's static constructor has been called.");
    }

    /// <summary>
    /// Convert fractional day to hour and minute values.
    /// </summary>
    /// <param name="f">Floating-point number 
    /// representing fraction of a day.</param>
    /// <returns>Hours and minutes of time matching the
    /// fractional day.</returns>
    /// <exception cref="ArgumentException">Negative
    /// time value is not permitted.</exception>
    public TimeRounded(float f)
      : this(RoundFloatMinutes(f * 24F * 60F))
    //HACK: TimeRounded(): Change this parameter to see a failure message in the test report
    //  For example, change it to (f * 24F * 60F + 10),
    //    and activate TimeRoundedTest() in the
    //    TestMethodsToBeRun list.
    { }  // end: TimeRounded(float)

    /// <summary>
    /// Convert minutes past midnight to
    /// (rounded) hours and minutes.
    /// <para>Returned value is rounded
    /// to the nearest 5 minutes.</para>
    /// </summary>
    /// <param name="intMins">Number of minutes
    /// since midnight.</param>
    /// <returns>Hours and minutes of time period.</returns>
    /// <exception cref="ArgumentException">Negative
    /// time value is not permitted.</exception>
    public TimeRounded(int intMins)
    {
      if (intMins < 0)
        throw new ArgumentException(
              String.Format(
                @"TimeRounded(): The argument ({0}) "
                  + @"should represent a positive integer."
                , intMins  //{0}
              ));

      var MinutesSinceMidnight =
            RoundFloatMinutes((float)intMins);
      Hour = MinutesSinceMidnight / 60;
      Minute = MinutesSinceMidnight % 60;

    } // end: TimeRounded(int)
  }  // end: TimeRounded{}

}  // end:namespace NewCodeNamespace