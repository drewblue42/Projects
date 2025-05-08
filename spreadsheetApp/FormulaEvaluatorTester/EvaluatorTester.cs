/// <summary>
/// Author: Andrew Winward
/// Partner: -none-
/// Date: 1/13/24
/// Course: CS 3500
/// 
/// This Console App is the tester for the Evaluator class
/// </summary>
/// 
using FormulaEvaluator;
using System.Diagnostics;
using System.Linq.Expressions;
//Array takes in different expression to test different test cases to make sure they are valid answers
string[] formulas = {
                     "5+5",//addition
                     "5-6",//subtraction
                     "5*5",//multiplication
                     "5/5",//division
                     "6/5",//division with a bigger 
                     "5+3*7-8/(4+3)",//big expression with multiple operators
                     "5-6+8-9",//bring out a negative number
                     "95-45",//double digit numbers
                     "400+6000",//triple and quad digits
                     "400*6000",//triple and quad digits with multiplication
                     "4000/800",//triple and quad digits with division
                     "5+3-(8*5)/40",//All operators 
                     "5+3-(8*E2)/40",//All operators with variables
                     "4     +     7",//lots of white space
                     "3+5-A1",// valid variable expression
                     "6/8-(A1+L5*4)",//big expression with multiple operators and variables
                     "(5*(1+1)/10)", //multiple parenthesis
                     "((((((5+5)-(6-1))))))",//many many parenthesis
                     "A1-A2",//negative variable
                     "A1*A2",//multiplication variable
                     "E2/A1",//multiplication variable
                     "E2+TTT",//invalid variable
                     "(E2-2)+(45/3)", //mixed operations and double parenthesis
                     "A1-ASDasdqwdlkamsd123"//invalid long variable





};

//Console.WriteLine($"E2+TTT = {Evaluator.Evaluate("E2+TTT", Lookup)}");

//for each loop to take the formula string array into a function to calculate the called Evaluator function
foreach (string formula in formulas)
{
    try
    {
        int result = Evaluator.Evaluate(formula, Lookup);

        Console.WriteLine($"Test passed: {formula} = {result}");
    }
    catch (ArgumentException)
    {
        Console.WriteLine("Test failed: Invalid Expression");
    }

}
//checking an empty expression
try
{
    Console.WriteLine($" = {Evaluator.Evaluate("", null)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed: Empty Expression");
}
//checking a division by zero
try
{
    Console.WriteLine($"0/0 = {Evaluator.Evaluate("0/0", null)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed: 0/0");
}
//divsion by zero
try
{
    Console.WriteLine($"5-10/0 = {Evaluator.Evaluate("5-10/0", null)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed: 5-10/0 ");
}
//Checking for input of a single operator
try
{
    Console.WriteLine($"/ = {Evaluator.Evaluate("/", null)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed: /");
}
//Checking an initial input of a negative number
try
{
    Console.WriteLine($"-5 = {Evaluator.Evaluate("-5", null)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed");
}
//Checking white space and single right parenthesis
try
{
    Console.WriteLine($"6 - 5 + 45) = {Evaluator.Evaluate("6 - 5 + 45)", null)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed: 6 - 5 + 45)");
}
//Checking for wrong variable
try
{
    Console.WriteLine($"4+T1 = {Evaluator.Evaluate("4+T1", Lookup)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed: 4+T1");
}
//Checking for incorrect variable format variable
try
{
    Console.WriteLine($"4+T0 = {Evaluator.Evaluate("4+T0", Lookup)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed: 4+T0");

}
//Checking for incorrect variable format variable
try
{
    Console.WriteLine($"4+TT = {Evaluator.Evaluate("4+TT", Lookup)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed: 4+TY");
}
//Checking for a string with no number/variables
try
{
    Console.WriteLine($"My name is Andrew Winward = {Evaluator.Evaluate("My name is Andrew Winward", null)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed: My name is Andrew Winward");
}
//mismatched parenthesis
try
{
    Console.WriteLine($"((((((5+5)-(6-1))))) = {Evaluator.Evaluate("((((((5+5)-(6-1)))))", null)}");
}
catch (ArgumentException)
{
    Console.WriteLine("Test passed: ((((((5+5)-(6-1)))))");
}


/// <summary>
/// this is a delegate method that allows us to look at a variable linked to a number
/// </summary>
/// <param name="variable"></param>
/// <returns>A variable is looked up in the list, and the attached number is return</returns>
int Lookup(string variable)
{
  
        if (variable == "A1")
        {
            return 1;
        }
        if (variable == "A2")
        {
            return 2;
        }
        if (variable == "L5")
        {
            return 3;
        }
        if (variable == "F2")
        {
            return 4;
        }
        if (variable == "E2")
        {
            return 5;
        }
        if (variable == "A2")
        {
            return 6;
        }
        else
        {
            throw new ArgumentException ("Invalid Variable");
        }

}