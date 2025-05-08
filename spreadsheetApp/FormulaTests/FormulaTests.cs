using SpreadsheetUtilities;
using static SpreadsheetUtilities.Formula;

namespace FormulaTests
{
    /// <summary>
    /// Author: Andrew Winward
    /// Partner: -none-
    /// Date: 2/24/24
    /// Course: CS 3500
    /// 
    /// This MSTEST file is the tester for the Formula class
    /// </summary>  
    /// 
    [TestClass]
    public class FormulaTests
    {

        /////////////////////////////Random Method Tests///////////////////////////////
        /// <summary>
        /// Test the nextNum method
        /// </summary>
        [TestMethod]
        public void nextNum()
        {
            Assert.ThrowsException<FormulaFormatException>(() => new Formula("(2 + 3 * x) 3"));

        }

        /// <summary>
        /// Test the parenthesis thrown exception
        /// </summary>
        [TestMethod]
        public void closingError()
        {
            Assert.ThrowsException<FormulaFormatException>(() => new Formula("(2 + 3 * x) + )"));

        }

        /// <summary>
        /// Test the operator token method
        /// </summary>
        [TestMethod]
        public void operatorTokenTest()
        {

            Formula formula = new Formula("2 + 3 + 2 -3 +4", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(8, result);

        }

        /// <summary>
        /// Test the closing operator method
        /// </summary>
        [TestMethod]
        public void closingParenthesisTokenTest()
        {

            Formula formula = new Formula("(2 * 3) / (2 *3)", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(1, result);

        }

        /// <summary>
        /// Checks if a formula is null in the equals method
        /// </summary>
        [TestMethod]
        public void equalsNull()
        {
            Formula formula = new Formula("2 + x");
            Assert.IsFalse(formula.Equals(null));
        }

        /// <summary>
        /// Checks if one of the formulas is not a formula
        /// </summary>
        [TestMethod]
        public void notAFormulaTest()
        {
            Formula formula = new Formula("2 + x");

            Assert.IsFalse(formula.Equals("not a formula"));
        }

        /// <summary>
        /// Checks a single varaible
        /// </summary>
        [TestMethod]
        public void singleVariable()
        {
            Formula formula = new Formula("x + y * z", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate(variable => variable == "X" ? 10.0 : 0.0);
            Assert.AreEqual(10.0, result);
        }

        /// <summary>
        /// evaluates a formula with multiple variables
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [TestMethod]
        public void evaluateMultiVariables()
        {
            Formula formula = new Formula("x + y * z", s => s.ToUpper(), s => true);

            //ChatGPT helped me set this one up. 
            //did not know how to do a switch method in testing
            double result = (double)formula.Evaluate(variable => variable switch
            {
                "X" => 2.0,
                "Y" => 3.0,
                "Z" => 5.0,
                _ => throw new ArgumentException("Unexpected variable"),
            });
            Assert.AreEqual(17.0, result);
        }

        //////////////////////////////////////////Formula Tests///////////////////////////////////////
        /// <summary>
        /// Testing the basic constructor
        /// </summary>
        [TestMethod]
        public void basicConstructor()
        {
            new Formula("2 + 3 * x");

        }
        /// <summary>
        /// Testing the longer constructor
        /// </summary>
        [TestMethod]
        public void secondConstructorTest()
        {
            new Formula("2 + 3 * x", s => s.ToUpper(), s => true);

        }
        /// <summary>
        /// Testing for a false constructed formula
        /// </summary>
        [TestMethod]
        public void constructorThrowsException()
        {
            Assert.ThrowsException<FormulaFormatException>(() => new Formula("2 + * x", s => s.ToUpper(), s => true));
        }

        /// <summary>
        /// Test for throw of exception when formula is empty
        /// </summary>
        [TestMethod]
        public void emptyFormula()
        {
            Assert.ThrowsException<FormulaFormatException>(() => new Formula(""));
        }

        /// <summary>
        /// Tests for a starting operator
        /// </summary>
        [TestMethod]
        public void formulaStartsWithOP()
        {
            Assert.ThrowsException<FormulaFormatException>(() => new Formula("+2"));
        }

        /// <summary>
        /// tests for an exception if the formula ends with an operator
        /// </summary>
        [TestMethod]
        public void formulaEndsWithOP()
        {
            Assert.ThrowsException<FormulaFormatException>(() => new Formula("2+"));
        }

        /// <summary>
        /// test for a basic parenthesis formula evaluation
        /// </summary>
        [TestMethod]
        public void parenthesisCheck()
        {
            Formula formula = new Formula("(1 + 5) + (6-5)", s => s.ToUpper(), s => true);
        }

        /// <summary>
        /// Test of exception check of an unbalanced left formula
        /// </summary>
        [TestMethod]
        public void unbalancedLeftParenthesisCheck()
        {
            Assert.ThrowsException<FormulaFormatException>(() => new Formula("(2+3"));
        }

        /// <summary>
        /// Test of exception check of an unbalanced right formula
        /// </summary>
        [TestMethod]
        public void unbalancedRightParenthesisCheck()
        {
            Assert.ThrowsException<FormulaFormatException>(() => new Formula("2+3)"));
        }

        /// <summary>
        /// Check to see if it throws an exception after a normailzed variable is not valid
        /// </summary>
        [TestMethod]
        public void invalidVaraibleAfterNormailzation()
        {
            Func<string, string> normalize = s => s.ToUpper();
            Func<string, bool> isValid = s => s.Length >= 2;

            Assert.ThrowsException<FormulaFormatException>(() => new Formula("x+y", normalize, isValid));
        }
        ///////////////////////////////////////Evaluate Tests /////////////////////////////////////////////
        /// <summary>
        /// Testing the evaluate method
        /// </summary>
        [TestMethod]
        public void evaluateDivisionByZero()
        {
            Formula formula = new Formula("1 / x", s => s.ToUpper(), s => true);

            FormulaError error = (FormulaError)formula.Evaluate(variable => variable == "X" ? 0 : 5);

            Assert.AreEqual("Cannot divide by zero", error.Reason);
        }

        /// <summary>
        /// evaluate an addition formula
        /// </summary>
        [TestMethod]
        public void evaluateAddition()
        {
            Formula formula = new Formula("2 + 3", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(5, result);
        }

        /// <summary>
        /// evaluate a multiplication formula
        /// </summary>
        [TestMethod]
        public void evaluateMultiplication()
        {
            Formula formula = new Formula("2 * 3", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(6, result);
        }

        /// <summary>
        /// evaluate a division formula
        /// </summary>
        [TestMethod]
        public void evaluateDivision()
        {
            Formula formula = new Formula("6 / 3", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(2, result);
        }

        /// <summary>
        ///  /// <summary>
        /// evaluate a more complex formula expression
        /// </summary>
        /// </summary>
        [TestMethod]
        public void evaluateBigExpression()
        {
            Formula formula = new Formula("(2 + 3) * 4 / 2", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(10, result);
        }

        /// <summary>
        /// tests to see if it handles a bigger addition 
        /// </summary>
        [TestMethod]
        public void evaluateBiggerAddition()
        {
            Formula formula = new Formula("400+6000", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(6400, result);
        }

        /// <summary>
        /// tests to see if it handles a bigger multiplication
        /// </summary>
        [TestMethod]
        public void evaluateBiggerMultiplication()
        {
            Formula formula = new Formula("400*6000", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(2400000, result);
        }

        /// <summary>
        /// tests to see if it handles a bigger division
        /// </summary>
        [TestMethod]
        public void evaluateBiggerDivision()
        {
            Formula formula = new Formula("6000/6000", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(1, result);
        }

        /// <summary>
        /// evaluates a formula with multiple parenthesis
        /// </summary>
        [TestMethod]
        public void evaluateMultipleParenthesis()
        {
            Formula formula = new Formula("((((((5+5)-(6-1))))))", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(5, result);
        }

        /// <summary>
        /// evaluates to a negative sum 
        /// </summary>
        [TestMethod]
        public void evaluateNegativeNumber()
        {
            Formula formula = new Formula("5-6", s => s.ToUpper(), s => true);

            double result = (double)formula.Evaluate((variable) => 0);

            Assert.AreEqual(-1, result);
        }
        ////////////////////////Get Variables method tests///////////////////////////////////


        /// <summary>
        /// Tests the variables
        /// </summary>
        [TestMethod]
        public void getVariables()
        {
            // Test getting variables
            Formula formula1 = new Formula("2 + 3 * x", s => s.ToUpper(), s => true);
            var variables = formula1.GetVariables();
            CollectionAssert.AreEquivalent(new List<string> { "X" }, variables.ToList());
        }
        /// <summary>
        /// Another tests for the variables
        /// </summary>

        [TestMethod]
        public void getVariables2()
        {
            Formula formula2 = new Formula("a + b * C", s => s.ToUpper(), s => true);
            var variables2 = formula2.GetVariables();
            CollectionAssert.AreEquivalent(new List<string> { "A", "B", "C" }, variables2.ToList());
        }

        ////////////////////////////////Test ToString Method/////////////////////////////////////////

        /// <summary>
        /// Tests the ToString Method
        /// </summary>
        [TestMethod]
        public void toStringTest()
        {
            Formula formula1 = new Formula("2 + 3 * x", s => s.ToUpper(), s => true);
            Assert.AreEqual("2+3*X", formula1.ToString());
        }

        /// <summary>
        /// Tests the ToString Method again
        /// </summary>
        [TestMethod]
        public void ToStringTest2()
        {
            Formula formula2 = new Formula("a + b * C", s => s.ToUpper(), s => true);
            Assert.AreEqual("A+B*C", formula2.ToString());
        }

        /// <summary>
        /// Tests for a failure of the ToString method
        /// </summary>
        [TestMethod()]
        public void formulaFalseToString()
        {
            Formula frmla1 = new Formula("4 + x1-3+45/23+U23");
            Formula frmla2 = new Formula("4+X1 -   3 + 45 / 23   +  u3");
            string forPhrase = frmla1.ToString();
            string otherPhrase = frmla2.ToString();

            Assert.IsFalse(forPhrase.Equals(otherPhrase));
        }

        /// <summary>
        /// This method tests ToString method in a different way
        /// </summary>
        [TestMethod()]
        public void formulaBasicToString()
        {
            Formula formula1 = new Formula("4+x1", s => s.ToUpper(), s => true);
            Formula formula2 = new Formula("4+x1", s => s.ToUpper(), s => true);
            string forPhrase = formula1.ToString();
            string otherPhrase = formula2.ToString();

            Assert.IsTrue(formula1.Equals(formula2));
        }

        /////////////////////////////////////////Test ToEquals method//////////////////////////////////////

        /// <summary>
        /// Tests the Equals Method if it passes
        /// </summary>
        [TestMethod]
        public void equalsTest()
        {
            Formula formula1 = new Formula("2 + 3 * x", s => s.ToUpper(), s => true);
            Formula formula2 = new Formula("2 + 3 * x", s => s.ToUpper(), s => true);
            Assert.IsTrue(formula1.Equals(formula2));
        }

        /// <summary>
        /// Tests the Equals Method if it fails
        /// </summary>
        [TestMethod]
        public void falseEquals()
        {
            Formula formula1 = new Formula("2 + 3 * x", s => s.ToUpper(), s => true);
            Formula formula2 = new Formula("a + b * C", s => s.ToUpper(), s => true);
            Assert.IsFalse(formula1.Equals(formula2));
        }

        /// <summary>
        /// Tests the equals method with  case provide from the Assignment page
        /// </summary>
        [TestMethod()]
        public void formulaEquals()
        {
            Assert.IsTrue(new Formula("x1+y2", s => s.ToUpper(), s => true).Equals(new Formula("X1 + Y2")));
        }
        ////////////////////////////////HashCode, ==, != method test//////////////////////////////////////////

        /// <summary>
        /// Tests the == method
        /// </summary>
        [TestMethod()]
        public void equalityTestonEquals()
        {
            Formula formula1 = new Formula("2 + 3 * x");
            Formula formula2 = new Formula("2 + 3 * x");

            Assert.IsTrue(formula1 == formula2);
        }

        [TestMethod()]
        public void equalityTestOnEqualsFalse()
        {
            Formula formula1 = new Formula("2 + 3 * x");
            Formula formula2 = new Formula("a + b * C");

            Assert.IsFalse(formula1 == formula2);
        }

        [TestMethod()]
        public void inequalityTestOnEqualsFalse()
        {
            Formula formula1 = new Formula("2 + 3 * x");
            Formula formula2 = new Formula("2 + 3 * x");

            Assert.IsFalse(formula1 != formula2);
        }

        [TestMethod()]
        public void inequalityTestOnEquals()
        {
            Formula formula1 = new Formula("2 + 3 * x");
            Formula formula2 = new Formula("a + b * C");

            Assert.IsTrue(formula1 != formula2);
        }

        [TestMethod()]
        public void HashCodeTest()
        {
            Formula formula1 = new Formula("2 + 3 * x");
            Formula formula2 = new Formula("2 + 3 * x");

            Assert.AreEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }

        [TestMethod()]
        public void HashCodeTestFail()
        {
            Formula formula1 = new Formula("2 + 3 * x");
            Formula formula2 = new Formula("a + b * C");

            Assert.AreNotEqual(formula1.GetHashCode(), formula2.GetHashCode());
        }
    }
}