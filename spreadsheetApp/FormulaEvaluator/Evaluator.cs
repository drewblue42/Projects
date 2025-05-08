
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary>
    /// Author: Andrew Winward
    /// Partner: -none-
    /// Date: 1/13/24
    /// Course: CS 3500
    /// 
    /// This library class evaluates a line of functions to turn into an algorithm.
    /// </summary>
    /// 

    public class Evaluator
    {
        /// <summary>
        /// this is a delegate method that allows us to look at a variable linked to a number
        /// </summary>
        /// <param name="variableName"></param>
        /// <returns>value of the variable</returns>
        public delegate int Lookup(string variableName);
       
        /// <summary>
        /// This class takes an expression that is turned into a set String array of tokens, the method then takes the tokens 
        /// analyzing each one in order to determine order of operations, and returns the calculated mathmatical expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="variable"></param>
        /// <returns>the value of the calculated expression</returns>
        public static int Evaluate(string expression, Lookup variable)
        {
            try
            {
                // Splits the expression into individual tokens for processing
               
                string[] tokens = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

                // Remove empty strings and trim whitespace
                List<string> filteredTokens = new List<string>();
                foreach (string token in tokens)
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        filteredTokens.Add(token.Trim());
                    }
                }
                //creates a stack for the converted string to ints, and the operation symbols for calculating
                Stack<int> valueStack = new Stack<int>();
                Stack<string> operatorStack = new Stack<string>();
                int curVal = 0;


                //check if the filtered token list is empty, and if it starts with an operator 
                if (filteredTokens.Count == 0) {
                    throw new ArgumentException("Empty Expression");
                }
                else if (filteredTokens[0] == "-" || filteredTokens[0] == "+" || filteredTokens[0] == "/" || filteredTokens[0] == "*")
                {
                    throw new ArgumentException("Cannot begin with an operator");
                }
                else if (filteredTokens.Count > 0 &&
                   (filteredTokens[filteredTokens.Count - 1] == "+" ||
                    filteredTokens[filteredTokens.Count - 1] == "-" ||
                    filteredTokens[filteredTokens.Count - 1] == "*" ||
                    filteredTokens[filteredTokens.Count - 1] == "/"))
                {
                    throw new ArgumentException("Cannot end with an operator");
                }


                //go through the whole List of Strings to determine each symbol and put it in the correct stack
                //and/or calculate depending on Order of Operations
                foreach (string token in filteredTokens)
                {
                    // Check if the token is an Integer
                    if (int.TryParse(token, out curVal))
                    {
                        //check if it contains a * ot /
                        if (ContainsMulDiv(operatorStack))
                        {
                            int tokVal1 = valueStack.Pop();
                            string tokOp = operatorStack.Pop();
                            int result = Result(curVal, tokVal1, tokOp);
                            valueStack.Push(result);
                        }
                        else
                        {
                            valueStack.Push(curVal);
                        }
                    }

                    // checks to see if the input variable follows the correct format of Letter-Digit
                    //ChatGPT was used to understand Reg Expression
                    else if (token.Length > 1)
                    {
                        if (Regex.IsMatch(token, "^[A-Za-z]+\\d+$")) //throwing error
                        {
                            try
                            {
                                if (ContainsMulDiv(operatorStack))
                                {
                                    int tokVal1 = valueStack.Pop();
                                    string tokOp = operatorStack.Pop();
                                    int result = Result(variable(token), tokVal1, tokOp);
                                    valueStack.Push(result);
                                }
                                else
                                {
                                    valueStack.Push(variable(token));
                                }
                            }
                            catch (ArgumentException)
                            {
                                // exception thrown if the variable is invalid
                                throw new ArgumentException("Invalid Variable");
                            }
                        }

                        else
                        {
                            throw new ArgumentException("Invalid Variable");
                        }

                    }

                    // Token is + or -
                    else if (token == "+" || token == "-")
                    {
                        if (valueStack.Count >= 2 && ContainsPlusMinus(operatorStack))
                        {
                            int val1 = valueStack.Pop();
                            int val2 = valueStack.Pop();
                            string opVal = operatorStack.Pop();
                            int results = Result(val2, val1, opVal);
                            valueStack.Push(results);
                        }
                        operatorStack.Push(token);
                    }

                    // Token is *
                    else if (token == "*")
                    {
                        operatorStack.Push(token);
                    }

                    // Token is /
                    else if (token == "/")
                    {
                        operatorStack.Push(token);
                    }

                    // Token is (
                    else if (token == "(")
                    {
                        operatorStack.Push(token);
                    }
                    // Token is )
                    else if (token == ")")
                    {
                        //checks to see if the Stack has at least two int variables in the value stack, and if it has a + or -
                        while (valueStack.Count >= 2 && ContainsPlusMinus(operatorStack))
                        {
                            int val1 = valueStack.Pop();
                            int val2 = valueStack.Pop();
                            string opVal = operatorStack.Pop();
                            int results = Result(val2, val1, opVal);
                            valueStack.Push(results);
                        }
                        // if the operation Stack is zero, throw argument
                        if (operatorStack.Count == 0)
                        {
                            throw new ArgumentException("Does not contain (");
                        }

                        operatorStack.Pop(); // Pop the (

                        while (valueStack.Count >= 2 && ContainsMulDiv(operatorStack))
                        {
                            int val1 = valueStack.Pop();
                            int val2 = valueStack.Pop();
                            string opVal = operatorStack.Pop();
                            int results = Result(val1, val2, opVal);
                            valueStack.Push(results);
                        }
                    }
                }
                //Checks to see if the Stack is not empty
                while (operatorStack.Count > 0)
                {
                    string oprat = operatorStack.Pop();
                   // Checks to see if there is a - ot + left
                    if (oprat == "+" || oprat == "-")
                    {
                        if (valueStack.Count >= 2)
                        {
                            int value2 = valueStack.Pop();
                            int value1 = valueStack.Pop();
                            int results = Result(value1, value2, oprat);
                            valueStack.Push(results);
                        }
                    }
                }
                //Checks to see if there is any value left, if there is not at least one, throw invalid expression 

                if (valueStack.Count != 1)
                {
                    throw new ArgumentException("Invalid expression");
                }


                return valueStack.Pop();
            }

            catch (ArgumentException)
            {
                throw new ArgumentException("Invalid expression");
              
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
        public static int Result(int val1, int val2, string op)
        {
            int value = 0;

            if (op == "*")
            {
                value = val1 * val2;
            }
            else if (op == "/")
            {
                try
                {
                    if (val1 == 0)
                    {
                        throw new DivideByZeroException("Not possible to divide by 0");
                        
                    }
                    value = val2 / val1;
                }
                catch (DivideByZeroException ex)
                {
                    throw new ArgumentException("Invalid Expression");
                }
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

    }
}

