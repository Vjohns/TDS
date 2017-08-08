//This file contains example code for classes
//  called BitArray{}, NewCode{}, and StaticCode{},
//  representing code under development that may be called
//  by TDS test methods.
//It is intended to be used for a library (*.dll file)
//  to be produced by the solution's "NewCode" project.

using System;
using System.Linq;

namespace NewCodeNamespace
{

  /// <summary>
  /// This implements an indexer for accessing
  /// the individual bits in a bit array.
  /// </summary>
  [Serializable]
  public class BitArray
  {
    /// <summary>
    /// Array of Boolean values packed 32 per word.
    /// </summary>
    /// <remarks>This is based on an example in
    /// the _C# Language Specificaion_.</remarks>
    uint[] bits;

    /// <summary>
    /// # of bits stored in each word of <see cref="bits"/>[]
    /// <para>Current value is 5.</para>
    /// </summary>
    /// <remarks>This should be at most 32.</remarks>
    public int BitsPerWord { get { return 5; } }

    /// <summary>
    /// Constructor; this allocates one
    /// 32-bit word for every (BitsPerWord) Boolean values
    /// </summary>
    /// <param name="length">Number of bits in the bit array.</param>
    public BitArray(int length)
    {
      if (length < 0) throw new ArgumentException();
      bits = new uint[((length - 1) / BitsPerWord) + 1];
      this.Length = length;
    }  // end:BitArray()

    /// <summary>
    /// Length of this array, the number of Boolean
    /// values (1 bit each) to be stored.
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// Indexer that gets or sets the 
    /// Boolean value of the specified element.
    /// First element is [0].
    /// For each element, 1=true; 0=false.
    /// </summary>
    /// <param name="index">Array index</param>
    /// <returns>Boolean value of the specified bit</returns>
    public bool this[int index]
    {
      get
      {
        if (index < 0 || index >= Length)
          throw new IndexOutOfRangeException();

        return (bits[index / BitsPerWord]
              & 1u << index % BitsPerWord) != 0;
      }  // end: get
      set
      {
        if (index < 0 || index >= Length)
          throw new IndexOutOfRangeException();

        var selectedWord = index / BitsPerWord;
        var shiftedBit = 1u << index % BitsPerWord;

        if (value)
          bits[selectedWord] |= shiftedBit;
        else
          bits[selectedWord] &= ~shiftedBit;
      }  // end: set
    }  // end: this[int]

    /// <summary>
    /// Hexadecimal representation of the array
    /// </summary>
    /// <returns>Hex strings.
    /// <para>For example, if BitsPerWord = 5
    /// and there are 3 words in bits[],
    /// then the returned string might look like
    /// </para><para>
    /// "Hex. contents: 0x8, 0x0, 0x0"
    /// </para></returns>
    public override string ToString()
    {
      return bits.Aggregate(
              "Hex. contents: "
              , (partial, num) =>
                string.Format(@"{0}0x{1:X"
                    + ((BitsPerWord - 1) / 4 + 1)
                    + @"}, "
                  , partial  //{0}
                  , num  //{1}
                )
              , partial => partial.TrimEnd(',', ' ')
              );
    }  // end: ToString()

  }  // end: BitArray{}  

  /// <summary>
  /// This class contains a method
  /// <para>either to be tested by example TDS methods
  /// in class <see cref="TDS.Test{}"/>,</para>
  /// <para>or to be run independently of any TDS code
  /// via calls from <see cref="RunWithoutTds()"/> .</para>
  /// </summary>
  public class NewCode
  {

    /// <summary>
    /// The purpose of this field is 
    /// to allow the TDS code to run this class's static constructor. 
    /// </summary>
    readonly static public bool isInitialized = true;

    /// <summary>
    /// Example static constructor for the <c>NewCode{}</c> class.
    /// <para>If the class otherwise contains no
    /// <c>static</c> members, this may be omitted.</para>
    /// </summary>
    static NewCode()
    {
      Console.WriteLine(
            "***** NewCode{} class's static constructor has been called.");
    }  // end:NewCode()

    /// <summary>
    /// Example method that calls <see cref="TestableConsoleMethod()"/>
    /// and <see cref="TestableNoConsoleMethod()"/> directly from
    /// <see cref="Program.Main()"/>, bypassing all of the TDS code.
    /// </summary>
    /// <remarks>This is example code intended to run
    /// as if the TDS code were not present.
    /// <para>Example output to the Console (assuming some keyboard input):
    /// </para><para>|
    /// </para><para>| (This is a test of example method
    /// TestableNoConsoleMethod().)
    /// </para><para>| 
    /// </para><para>|   One plus 3 is 4.
    /// </para><para>| 
    /// </para><para>| 
    /// </para><para>| (This is a test of example method
    /// TestableConsoleMethod().)
    /// </para><para>| 
    /// </para><para>|   Type any words except "bugs",
    /// followed by &lt;Enter&gt;.
    /// </para><para>|     Do this twice.
    /// </para><para>| 
    /// </para><para>|    hElLo   WOrld
    /// </para><para>| To the console: HELLO   WORLD
    /// </para><para>|   2nd line
    /// </para><para>| To the console: 2ND LINE
    /// </para><para>| 
    /// </para><para>| Press the &lt;Enter&gt; key to finish . . .
    /// </para>
    /// </remarks>
    public static void RunWithoutTds()
    {
      #region TestableNoConsoleMethod()
      var three = 3;
      Console.WriteLine(@"
(This is a test of example method TestableNoConsoleMethod().)
    
  One plus {0} is {1}.
"
              , three  //{0}
              , three.TestableNoConsoleMethod()  //{1}
            );

      #endregion TestableNoConsoleMethod()

      #region TestableConsoleMethod()
      Console.WriteLine(@"
(This is a test of example method TestableConsoleMethod().)
    
  Type any words except ""bugs"", followed by <Enter>.
    Do this twice.
");
      for (var n = 1; n <= 2; n++)
        TestableConsoleMethod();

      #endregion TestableConsoleMethod()

    }  // end:RunWithoutTds()

    /// <summary>
    /// Example method to be called by 
    /// TDS method <see cref="TestableConsoleMethodTest"/>().
    /// <para>This reads a line of text from the Console, 
    /// trims and upper-cases it,</para>
    /// <para>writes to the Console that line preceded by "To the console: ",
    /// </para><para>and returns to the caller the same line
    /// preceded by "Returned: ".
    /// </para><para>Example:
    /// </para><para>If the line read from the Console
    /// consists of the string "  done ",
    /// </para><para>then this method writes to the Console
    /// "To the console: DONE"
    /// </para><para>and returns to the caller the string
    /// "Returned: DONE".</para>
    /// </summary>
    /// <returns>"Returned: " + trimmed &amp; upper-cased line
    /// read from the Console.</returns>
    /// <exception cref="ApplicationException">Exception
    /// "Bugs are detected in this line."
    /// is raised if the word "BUGS" appears anywhere in the input.
    /// </exception>
    public static string TestableConsoleMethod()
    {
      var nextLine = Console.ReadLine().Trim().ToUpper();

      //HACK: TestableConsoleMethod() -- Change string to "B UGS" to check test method
      if (nextLine.Contains("BUGS"))
        throw new ApplicationException("Bugs are detected in this line.");

      if (nextLine == "SCORE")
        nextLine = "You're a winner!";

      Console.WriteLine(
              "To the console: {0}"
              , nextLine  //{0}
            );

      return string.Format(
              "Returned: {0}"
              , nextLine  //{0}
            );

    }  // end:TestableConsoleMethod()

  }  // end: NewCode{}

  /// <summary>
  /// This class contains static members
  /// used by <see cref="NewCode"/>{}.
  /// </summary>
  /// <remarks>This class is present, and <c>static</c>,
  /// to allow methods it contains
  /// to be defined and used as extension methods.</remarks>
  public static class StaticCode
  {

    /// <summary>
    /// Access this to run the static constructor 
    /// (unless you can be sure that it has already been run).
    /// </summary>
    readonly static public bool isInitialized = true;

    /// <summary>
    /// Example static constructor for
    /// the <c>StaticCode{}</c> class.
    /// </summary>
    static StaticCode()
    {
      Console.WriteLine(
            "***** StaticCode{} class's static constructor has been called.");
    }  // end:StaticCode()

    /// <summary>
    /// Example method to be called by TDS method
    /// <see cref="TestableNoConsoleMethodTest"/>().
    /// <para>This is an extension method,
    /// returning the value of its argument plus one.
    /// </para><para>
    /// For example, (3).TestableNoConsoleMethod() = 4 .
    /// </para>
    /// </summary>
    /// <param name="param1">Non-negative integer value
    /// whose successor is to be returned.</param>
    /// <returns>Value of <paramref name="param1"/> plus 1.</returns>
    /// <exception cref="ApplicationException">Negative
    /// argument was specified</exception>
    public static int TestableNoConsoleMethod(this int param1)
    {
      //HACK: TestableNoConsoleMethod() -- Remove this line, which raises the wrong Exception:
      throw new ApplicationException("False exception.");

      //HACK: TestableNoConsoleMethod() -- Remove this line, which fails to raise an Exception:
      return param1 + 1;

      if (param1 < 0)
        throw new ApplicationException(
              string.Format(
                @"Whoopsie -- negative argument ({0} in this case) is not allowed!"
                , param1  //{0}
              ));

      return
            //HACK: TestableNoConsoleMethod() -- Remove this line, which is intended to give a wrong answer:
            1000 +

            param1 + 1;
    }  // end:TestableNoConsoleMethod()

  }  // end:StaticCode{}
}  // end:namespace NewCodeNamespace