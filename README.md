This is a mechanism for quickly creating and using C# unit-test methods, with the help of Visual Studio.

For instructions, download the files using the green "Clone or download" button as TDS-master.zip,
then unpack the contents of TDS-master.zip and
open file "TDS User Guide 2_0b.pdf" for documentation, examples, tutorial, etc.
For a quick, updated summary, please see document "Updated summary, supplement to UG 2_0b.pdf".

File "TDS User Guide 2_0b.docx" has the same contents, in MS Office Word format.

This project originated at tds.codeplex.com.

---------------

TDS is intended to provide a lightweight unit-test platform to define and run unit-test
methods that are easy to set up and that are also compatible with MS Test and NUnit.

TDS, after setup is complete, allows one to create a unit-test method by invoking a Visual Studio snippet and customizing it, using items in the Task List to assist with navigation.  (The setup includes installing Visual Studio if needed, importing the snippet, and satisfying some //TODO: Tasks.)

If you use the TDS test platform (instead of the VS Test or NUnit platform) and wish to run only a subset of the TDS tests defined for the Visual Studio Solution, you may specify a subset of the TDS unit tests, and/or a subset of the test cases within a TDS test, to be run by editing the TDS.cs source code (as specified in its //TODO: Tasks).  The use of such filterings is noted within the test report, giving a warning that some of the (possibly failing) test cases have been skipped.  The TDS platform also provides methods for setup and teardown of the test environment.

In accordance with traditional unit testing (see, for example, https://docs.microsoft.com/en-us/visualstudio/test/intellitest-manual/test-generation?view=vs-2017), each TDS unit-test method includes a standard set of "//TODO:" Tasks identifying
 (1) test-case data, defining inputs and expected outputs, located in the testValues[] array near the beginning of each TDS test method's code;
 (2) a statement that calls the C# function member (e.g., indexer, method, or property) to be tested, using the specified data; and
 (3) "Assert" statements to perform the tests, including tests that the expected outputs are received and that all expected exceptions, and no unexpected ones, are thrown.

TDS is designed for use from within Visual Studio, but it may be used independently of Visual Studio, by editing the snippet file or one of the provided example test methods, to serve as a C# method template.  Suitably edited, the TDS.cs file contains everything needed to define and run a collection of TDS unit tests.

---------------

The following material supplements (or possibly corrects) the contents of the
TDS User Guide 2_0b and will be incorporated into the next version (if any)
of that document.

References to the location of the source-code files will be updated to 
refer to their GitHub locations instead of the old CodePlex locations.

---------------
[The following is added 4/11/2018 and will probably be included into the
User Guide as an additional example, perhaps in a new section 5.5
or in an expanded version of section 4.8.4.2.]

Example: Testing a class or struct that has multiple constructors

Suppose we wish to test public class S_Exp{...} that includes, among
others, definitions of the following constructors:

     public S_Exp(S_Exp L, S_Exp R){...}

     public S_Exp(string L, string R){...}

and that contains a suitable ToString() method, perhaps something like this:

    public override string ToString()
    {
      if (...) return L.ToString();
      return String.Format ("({0} . {1})"
              , L.ToString()  //{0}
              , R.ToString()  //{1}
            );
    }  //end: ToString()
 
We want to test various combinations of constructors of this class, with each test in
the testValues[] array specifying a calling expression.  We could place several possible
calling expressions into "if" or "switch" statements in the
          #region Invoke testable function members
region of our test method, as we did in section 4.8.4.2, but this would locate the code
that invokes the function member at some distance from the code identifying the
expected results.  We could instead place the calling code into the test cases in the
TestValues[] array, along with the expected results.  For example, we could specify
a calling statement like this:

            //TODO: S_ExpTest() -- Provide a suitable calling expression
            actual = tCase.Lambda();

to invoke some of the constructors, using test cases like this:

        new {
          Id = "11 With one level",
          ExceptionExp = "",
          Lambda = new Func<S_Exp>(() => new S_Exp("A", "B")),
          ValueExp = "(A . B)",
        },

or, with a bit more complexity, like this:

        new {
          Id = "24 Nested 4 levels",
          ExceptionExp = "",
          Lambda = new Func<S_Exp>(() =>
                    new S_Exp(
                      new S_Exp(
                        new S_Exp(
                          new S_Exp("A", "B"),
                          new S_Exp("C", "D")
                        )
                        , new S_Exp("E", "F")
                      ),
                      new S_Exp("G",
                        new S_Exp(
                          new S_Exp("H", "I"),
                          new S_Exp("J", "K")
                        )
                      )
                    )
                  ),
          ValueExp = "((((A . B) . (C . D)) . (E . F)) . (G . ((H . I) . (J . K))))",
        },

The expression in the tCase.Lambda property isn't evaluated until the

            actual = tCase.Lambda();

statement is reached.  (For tracing, this statement may be a suitable location for a breakpoint.)

Note: in this example, the parentheses are significant; the statement

   actual = tCase.Lambda.ToString()  
   
would fetch only the name of the type, whereas

   actual = tCase.Lambda().ToString()
   
will capture the result of running the code.
   We want the result of running it to match the value of the tCase.ValueExp string.
   The parentheses are empty in this example merely becaue tCase.Lambda() takes
   no parameters.  All of the parameters used in calls to S_Exp() in this example
   are specified as constants, to make their values in each test case easy to compare
   with the expected result.

When Assert.AreEqual() runs this test, it will report that the test "Passed" iff
  the value of tCase.Lambda().ToString()
  (in this example, it's the same as actual.ToString() )
  matches that of tCase.ValueExp .

In each test case (each element of the testValues[] array),
   by placing the calling expression (in this example, specified by
   the Lambda property) close to the expected result (the value of
   the ValueExp property), both can be visible at the same time,
   making it easy to compare them and analyze any differences.

---------------
