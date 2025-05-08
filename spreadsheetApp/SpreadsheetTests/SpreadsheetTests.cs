using SpreadsheetUtilities;
using SS;

namespace SpreadsheetTests
{
    /// <summary>
    /// Author: Andrew Winward
    /// Partner: -none-
    /// Date: 2/11/24
    /// Course: CS 3500
    /// 
    /// This MSTEST file is a tester for the SpreadSheet program.
    /// </summary>
    /// 
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// This method tests if the SS is empty
        /// </summary>
        [TestMethod]
        public void emptySSTest()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual(0, s.GetNamesOfAllNonemptyCells().Count());
        }

        /// <summary>
        /// This method checks the if a unknown cell is empty
        /// </summary>
        [TestMethod]
        public void emptyGetCellContentsTest()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A1"));
        }

        /// <summary>
        /// This tests the String method for SetCellContents
        /// </summary>
        [TestMethod]
        public void setCellContentsStringTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", "Me llamo Antonio");
            Assert.AreEqual("Me llamo Antonio", s.GetCellContents("A1"));
        }
        /// <summary>
        /// This tests the double method for SetCellContents
        /// </summary>
        [TestMethod]
        public void setCellContentsDoubleTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", 1.17);
            Assert.AreEqual(1.17, s.GetCellContents("A1"));
        }

        /// <summary>
        /// This tests the formula method for SetCellContents
        /// </summary>
        [TestMethod]
        public void SetCellContentsFormulaTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("Z1 + 5"));

            var result = s.GetCellContents("A1");

            Assert.IsInstanceOfType(result, typeof(Formula));
            Assert.AreEqual("Z1+5", result.ToString());
        }

        /// <summary>
        /// This tests a invalid name on the GetCellContents method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void invalidNameExceptionTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1A");
        }

        /// <summary>
        /// This tests the string method for SetCellContents if it is an invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsString_InvalidNameTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("{}", "my name is Andrew");
        }

        /// <summary>
        /// This tests the Double method for SetCellContents if it is an invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsDouble_InvalidNameTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("{}", 12.3);
        }

        /// <summary>
        /// This tests the formula method for SetCellContents if it is an invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsFormula_InvalidNameTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("{}", new Formula("Z1 + 5"));
        }
        /// <summary>
        /// Tests for circular dependency
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void circularExceptionTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("B1+1"));
            s.SetCellContents("B1", new Formula("C1+1"));
            s.SetCellContents("C1", new Formula("A1+1"));
        }

        /// <summary>
        /// This method checks to see if a current cell already exists.
        /// If it does, it updates it
        /// </summary>
        [TestMethod]
        public void existingCellUpdate()
        {
            Spreadsheet s = new Spreadsheet();
            string cellName = "A1";
            double first = 5.0;
            string updated = "hello";

            s.SetCellContents(cellName, first);

            s.SetCellContents(cellName, updated);

            var realContent = s.GetCellContents(cellName);
            Assert.IsInstanceOfType(realContent, typeof(string));
            Assert.AreEqual(updated, realContent);
        }
        /// <summary>
        /// This method checks to see if a current formula cell already exists.
        /// If it does, it updates it
        /// </summary>
        [TestMethod]
        public void existingFormulaCellUpdate()
        {
            Spreadsheet s = new Spreadsheet();
            string cell = "A1";
            Formula first = new Formula("B1 + C1");
            Formula updated = new Formula("D1 + E1");

            s.SetCellContents(cell, first);

            Assert.AreEqual(first, s.GetCellContents(cell));

            s.SetCellContents(cell, updated);

            var realContent = s.GetCellContents(cell);
            Assert.IsInstanceOfType(realContent, typeof(Formula));
            Assert.AreEqual(updated, realContent);
        }

        /// <summary>
        /// Tests the null part of the string SetCellContents
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsString_NullTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, "This should fail");
        }

        /// <summary>
        /// Tests the null part of the double SetCellContents
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsDouble_NullTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetCellContents(null, 3.4);
        }
    }
}