This is a mechanism for quickly creating and using C# unit-test methods, with the help of Visual Studio.

This project originated at tds.codeplex.com.

Please open file "TDS User Guide 2_0b.pdf" for documentation, examples, tutorial, etc.  
File "TDS User Guide 2_0b.docx" has the same contents, in MS Office Word format.
File "TDS User Guide 2_0.pdf" also has the same contents, except that it does not contain the bookmarks.

===============

The following material supplements (or possibly corrects) the contents of the
TDS User Guide 2_0b and will be incorporated into the next version (if any)
of that document.

References to the location of the source-code files will be updated to 
refer to their GitHub locations instead of the old CodePlex locations.

[The following is added 4/9/2018 and will probably be included into the
User Guide as an additional example, perhaps in a new section 5.5
or in an expanded version of section 4.8.4.2.]

Example: Testing a class or struct that has multiple constructors

Suppose we wish to test public class S_Exp{...} that includes, among
others, definitions of the following constructors:

  public S_Exp(S_Exp L, S_Exp R){...}

  public S_Exp(string L, string R){...}

and contains a suitable ToString() method, perhaps something like this:

    public override string ToString()
    {
      if (...) return L.ToString();
      return String.Format ("({0} . {1})"
              , L.ToString()  //{0}
              , R.ToString()  //{1}
            );
    }  //end: ToString()
 
We want to test various combinations of constructors, with each test in the
testValues[] array specifying a calling expression, instead of placing several
possible calling expressions into if or switch statements in the
          #region Invoke testable function members
region, as we did in section 4.8.4.2.

For example, we could specify a calling statement like this:

            //TODO: S_ExpTest() -- Provide a suitable calling expression
            actual = tCase.Lambda();

to invoke some of the constructors, using test cases like this:

        new {
          Id = "11 With two Atoms",
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

The code in Lambda isn't executed until the
            actual = tCase.Lambda();
statement is reached, making this a possibly helpful location for a breakpoint.

Using this technique, each (maybe convoluted) calling expression can be kept close
to the expected value returned by ToString(), making it easy to read both at the same time.
