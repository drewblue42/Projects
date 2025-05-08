// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// Author: Andrew Winward
    /// Partner: -none-
    /// Date: 2/4/24
    /// Course: CS 3500
    /// 
    /// This library class evaluates a line of functions to turn into a useable formula.
    /// </summary>


    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private List<string> formulaTokens;
        private List<string> normalTokens = new List<string>();
        private List<string> normalVar = new List<string>();

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
        this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            formulaTokens = new List<string>(GetTokens(formula));

            //empty formula
            if (formulaTokens.Count == 0)
            {
                throw new FormulaFormatException("Need at least one token");
            }
            // starts with Operator
            else if (IsOp(formulaTokens[0]))
            {
                throw new FormulaFormatException("Cannot begin with an operator");
            }
            //Ends with Operator
            else if (formulaTokens.Count > 0 && IsOp(formulaTokens[formulaTokens.Count - 1]))
            {
                throw new FormulaFormatException("Cannot end with an operator");
            }

            string prevToken = "";
            int openParenthesesCount = 0;
            int closeParenthesesCount = 0;

            foreach (string token in formulaTokens)
            {
                // validate and normalize tokens
                validToken(token, normalize, isValid);

                // check for correct placement of operators and operands
                nextOP(token, prevToken);
                nextNum(token, prevToken);

                if (token == "(")
                {
                    openParenthesesCount++;
                }
                else if (token == ")")
                {
                    closeParenthesesCount++;

                    if (closeParenthesesCount > openParenthesesCount)
                    {
                        throw new FormulaFormatException("Does not follow right parenthesis rule");
                    }
                }

                if (IsVar(token))
                {
                    string normalizedVariable = normalize(token);

                    if (isValid(normalizedVariable))
                    {

                        normalTokens.Add(normalizedVariable);
                    }
                }
                else
                {
                    normalTokens.Add(token);
                }

                prevToken = token;
            }

            if (openParenthesesCount != closeParenthesesCount)
            {
                throw new FormulaFormatException("Does not follow balanced parenthesis rule");
            }

        }

        ///////////////////////////Formula Helper Methods/////////////////////

        /// <summary>
        /// validates the tokens of a formula to determine if they are a number, variable, and/or operator(+-*/)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="normalize"></param>
        /// <param name="isValid"></param>
        /// <exception cref="FormulaFormatException"></exception>
        public void validToken(string token, Func<string, string> normalize, Func<string, bool> isValid)
        {
            if (IsNum(token) || IsOp(token) || IsVar(token) || token == "(" || token == ")")
            {
                if (IsVar(token))
                {
                    string normalizedVariable = normalize(token);

                    if (isValid(normalizedVariable))
                    {

                        normalVar.Add(normalizedVariable);
                        Console.WriteLine("Token: " + normalizedVariable);
                        return;
                    }
                    else
                    {
                        throw new FormulaFormatException($"Invalid variable after normalization: {normalizedVariable}");
                    }
                }

                return;
            }
            else
            {
                throw new FormulaFormatException($"Invalid token: {token}");
            }


        }
        /// <summary>
        ///  Needs to be any token that immediately follows an opening parenthesis or 
        ///  an operator must be either a number, a variable, or an opening parenthesis.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="prevToken"></param>
        /// <exception cref="FormulaFormatException"></exception>
        private void nextOP(string token, string prevToken)
        {
            if (prevToken == "(" || IsOp(prevToken))
            {
                if (!IsNum(token) && !IsVar(token) && token != "(")
                {
                    throw new FormulaFormatException("Invalid token following opening parenthesis or operator.");
                }
            }
        }
        /// <summary>
        /// Needs to be any token that immediately follows a number, a variable, 
        /// or a closing parenthesis must be either an operator or a closing parenthesis.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="prevToken"></param>
        /// <exception cref="FormulaFormatException"></exception>
        private void nextNum(string token, string prevToken)
        {
            // Implement the Extra Following Rule
            if (prevToken == ")" || IsNum(prevToken) || IsVar(prevToken))
            {
                if (token != ")" && !IsOp(token))
                {
                    throw new FormulaFormatException("Invalid token following number, variable, or closing parenthesis.");
                }
            }
        }
        /// <summary>
        /// Checks to see if the token is a number
        /// </summary>
        /// <param name="token"></param>
        /// <returns>True or False</returns>
        public static bool IsNum(string token) => Double.TryParse(token, out _);
        /// <summary>
        /// Checks to see if the token is an operator
        /// </summary>
        /// <param name="token"></param>
        /// <returns>True or False</returns>
        public static bool IsOp(string token) => token == "-" || token == "+" || token == "/" || token == "*";
        /// <summary>
        /// Checks to see if the token is a variable
        /// </summary>
        /// <param name="token"></param>
        /// <returns>True or False</returns>
        public static bool IsVar(string token) => Regex.IsMatch(token, "[a-zA-Z_](?: [a-zA-Z_]|\\d)*");
        /////////////////////////////////End of Formula Helper Methods/////////////////////////////////////

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        /// <param name="lookup"></param>
        /// <returns>an evaluated function</returns>
        public object Evaluate(Func<string, double> variableLookup)
        {
            try
            {
                Stack<double> valueStack = new Stack<double>();
                Stack<string> operatorStack = new Stack<string>();

                foreach (string token in normalTokens)
                {
                    if (Double.TryParse(token, out double curValue))
                    {
                        NumericToken(valueStack, operatorStack, curValue);
                    }
                    else if (IsVar(token))
                    {
                        VariableToken(variableLookup, valueStack, operatorStack, token);
                    }
                    else
                    {
                        OperatorToken(valueStack, operatorStack, token);
                    }
                }

                RemainingOperators(valueStack, operatorStack);

                return valueStack.Pop();
            }
            catch (FormulaFormatException ex)
            {
                return new FormulaError(ex.Message);
            }
            catch (DivideByZeroException)
            {
                return new FormulaError("Cannot divide by zero");
            }
        }

        ////////////////////////////////////Evaluate Helper Methods///////////////////////////////////////

        /// <summary>
        /// This is a helper method for the evaluator method that handles number tokens
        /// </summary>
        /// <param name="valueStack"></param>
        /// <param name="operatorStack"></param>
        /// <param name="curValue"></param>
        private void NumericToken(Stack<double> valueStack, Stack<string> operatorStack, double curValue)
        {
            if (ContainsMulDiv(operatorStack))
            {
                double tokValue1 = valueStack.Pop();
                string tokOp = operatorStack.Pop();
                double result = Result(curValue, tokValue1, tokOp);
                valueStack.Push(result);
            }
            else
            {
                valueStack.Push(curValue);
            }
        }
        /// <summary>
        /// This is a helper method for the evaluator method that handles Variable tokens
        /// </summary>
        /// <param name="variableLookup"></param>
        /// <param name="valueStack"></param>
        /// <param name="operatorStack"></param>
        /// <param name="token"></param>
        /// <exception cref="FormulaFormatException"></exception>
        private void VariableToken(Func<string, double> variableLookup, Stack<double> valueStack, Stack<string> operatorStack, string token)
        {
            double variableValue;
            try
            {
                variableValue = variableLookup(token);
            }
            catch (ArgumentException)
            {
                throw new FormulaFormatException("Unknown variable");
            }

            if (ContainsMulDiv(operatorStack))
            {
                double tokValue1 = valueStack.Pop();
                string tokOp = operatorStack.Pop();
                double result = Result(variableValue, tokValue1, tokOp);
                valueStack.Push(result);
            }
            else
            {
                valueStack.Push(variableValue);
            }
        
        }
        /// <summary>
        /// This is a helper method for the evaluator method that handles that operator token
        /// </summary>
        /// <param name="valueStack"></param>
        /// <param name="operatorStack"></param>
        /// <param name="token"></param>
        private void OperatorToken(Stack<double> valueStack, Stack<string> operatorStack, string token)
        {
            if (token == "+" || token == "-")
            {
                if (valueStack.Count >= 2 && ContainsPlusMinus(operatorStack))
                {
                    double val1 = valueStack.Pop();
                    double val2 = valueStack.Pop();
                    string opVal = operatorStack.Pop();
                    double result = Result(val2, val1, opVal);
                    valueStack.Push(result);
                }
                operatorStack.Push(token);
            }
            else if (token == "*" || token == "/")
            {
                operatorStack.Push(token);
            }
            else if (token == "(")
            {
                operatorStack.Push(token);
            }
            else if (token == ")")
            {
                ClosingParenthesis(valueStack, operatorStack);
            }
        }
        /// <summary>
        /// This is a helper method for the evaluator method that handles remaining operators
        /// </summary>
        /// <param name="valueStack"></param>
        /// <param name="operatorStack"></param>
        /// <exception cref="FormulaFormatException"></exception>
        private void RemainingOperators(Stack<double> valueStack, Stack<string> operatorStack)
        {
            while (operatorStack.Count > 0)
            {
                string oprat = operatorStack.Pop();

                if (oprat == "+" || oprat == "-")
                {
                    if (valueStack.Count >= 2)
                    {
                        double value2 = valueStack.Pop();
                        double value1 = valueStack.Pop();
                        double results = Result(value1, value2, oprat);
                        valueStack.Push(results);
                    }
                }
            }

            if (valueStack.Count != 1)
            {
                throw new FormulaFormatException("Invalid Expression");
            }
        }

        /// <summary>
        /// This is a helper method for the evaluator method that handles the closing parenthesis
        /// </summary>
        /// <param name="valueStack"></param>
        /// <param name="operatorStack"></param>
        /// <exception cref="FormulaFormatException"></exception>
        private void ClosingParenthesis(Stack<double> valueStack, Stack<string> operatorStack)
        {
            // Handle closing parenthesis ")"
            while (valueStack.Count >= 2 && ContainsPlusMinus(operatorStack))
            {
                double val1 = valueStack.Pop();
                double val2 = valueStack.Pop();
                string opVal = operatorStack.Pop();
                double results = Result(val2, val1, opVal);
                valueStack.Push(results);
            }

            if (operatorStack.Count == 0)
            {
                throw new FormulaFormatException("Mismatched Parentheses");
            }

            operatorStack.Pop(); // Pop the (

            while (valueStack.Count >= 2 && ContainsMulDiv(operatorStack))
            {
                double val1 = valueStack.Pop();
                double val2 = valueStack.Pop();
                string opVal = operatorStack.Pop();
                double results = Result(val1, val2, opVal);
                valueStack.Push(results);
            }
        }


        /// <summary>
        /// This methods checks to see if the token is a multiple or a divisor
        /// </summary>
        /// <param name="oper"></param>
        /// <returns> when this function is called it returns true or false if the expression contains a * or a / </returns>
        public static bool ContainsMulDiv(Stack<string> oper)
        {
            if (oper.Count == 0)
            {
                return false;
            }

            string top = oper.Peek();
            return top == "*" || top == "/";
        }

        /// <summary>
        /// this method checks to see if it contains and addition or a subtraction token
        /// </summary>
        /// <param name="oper"></param>
        /// <returns>when this function is called it returns true or false if the expression contains a + or a -</returns>
        public static bool ContainsPlusMinus(Stack<string> oper)
        {
            if (oper.Count == 0)
            {
                return false;
            }

            string top = oper.Peek();
            return top == "+" || top == "-";
        }
        /// <summary>
        /// This method takes two values and an operator and performs the correct operation.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="t"></param>
        /// <param name="op"></param>
        /// <returns>the result of a calculated expression</returns>
        /// <exception cref="ArgumentException"></exception>
        public static double Result(double val1, double val2, string op)
        {
            double value = 0;

            if (op == "*")
            {
                value = val1 * val2;
            }
            else if (op == "/")
            {
                if (val1 == 0)
                {
                    throw new DivideByZeroException("Not possible to divide by 0");

                }
                value = val2 / val1;
            }

            else if (op == "+")
            {
                value = val1 + val2;
            }
            else if (op == "-")
            {
                value = val1 - val2;
            }
            return value;
        }

        ////////////////////////////////////////End of Evaluate Helper Methods//////////////////////////////////

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<string> GetVariables()
        {
            HashSet<string> uniqueVar = new HashSet<string>();

            //double checks the variables
            foreach (string token in normalVar)
            {
                if (IsVar(token))
                {
                    uniqueVar.Add(token);
                }
            }

            return uniqueVar;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {

            StringBuilder strFormula = new StringBuilder();

            foreach (string token in normalTokens)
            {
                if (IsNum(token))
                {
                    strFormula.Append(double.Parse(token).ToString());
                }
                else
                {
                    strFormula.Append(token);
                }
            }

            return strFormula.ToString();
        }



        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        /// <param name="obj"></param>
        /// <returns> True or False</returns>
        public override bool Equals(object? obj)
        {
            if (obj is null || !(obj is Formula otherFormula))
                return false;

            return ToString() == otherFormula.ToString();
        }




        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>true or false</returns>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>true or false</returns>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        /// <returns>the hash code</returns>
        public override int GetHashCode()
        {
            int hash = 5;
            hash = hash * ToString().Trim().GetHashCode();

            return hash.GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        /// <param name="formula"></param>
        /// <returns>tokenated formula</returns>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }


        /// <summary>
        /// Used to report syntactic errors in the argument to the Formula constructor.
        /// </summary>
        public class FormulaFormatException : Exception
        {
            /// <summary>
            /// Constructs a FormulaFormatException containing the explanatory message.
            /// </summary>
            public FormulaFormatException(String message)
                : base(message)
            {
            }
        }

        /// <summary>
        /// Used as a possible return value of the Formula.Evaluate method.
        /// </summary>
        public struct FormulaError
        {
            /// <summary>
            /// Constructs a FormulaError containing the explanatory reason.
            /// </summary>
            /// <param name="reason"></param>
            public FormulaError(String reason)
                : this()
            {
                Reason = reason;
            }

            /// <summary>
            ///  The reason why this FormulaError was created.
            /// </summary>
            public string Reason { get; private set; }
        }
    
}




// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
