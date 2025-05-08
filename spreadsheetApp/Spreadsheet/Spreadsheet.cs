
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using String = System.String;

namespace SS
{
    /// <summary>
    /// Author: Andrew Winward
    /// Partner: -none-
    /// Date: 2/11/24
    /// Course: CS 3500
    /// 
    /// This class is the internals of the SpreadSheet. It makes use of the previously developed
    /// Formula and DependencyGraph classes. 
    /// </summary>


    /// <summary>
    /// <para>
    ///     An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    ///     spreadsheet consists of an infinite number of named cells.
    /// </para>
    /// <para>
    ///     A string is a valid cell name if and only if:
    /// </para>
    /// <list type="number">
    ///      <item> its first character is an underscore or a letter</item>
    ///      <item> its remaining characters (if any) are underscores and/or letters and/or digits</item>
    /// </list>   
    /// <para>
    ///     Note that this is the same as the definition of valid variable from the Formula class assignment.
    /// </para>
    /// 
    /// <para>
    ///     For example, "x", "_", "x2", "y_15", and "___" are all valid cell  names, but
    ///     "25", "2x", and "&amp;" are not.  Cell names are case sensitive, so "x" and "X" are
    ///     different cell names.
    /// </para>
    /// 
    /// <para>
    ///     A spreadsheet contains a cell corresponding to every possible cell name.  (This
    ///     means that a spreadsheet contains an infinite number of cells.)  In addition to 
    ///     a name, each cell has a contents and a value.  The distinction is important.
    /// </para>
    /// 
    /// <para>
    ///     The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    ///     contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    ///     of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// </para>
    /// 
    /// <para>
    ///     In a new spreadsheet, the contents of every cell is the empty string. Note: 
    ///     this is by definition (it is IMPLIED, not stored).
    /// </para>
    /// 
    /// <para>
    ///     The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    ///     (By analogy, the value of an Excel cell is what is displayed in that cell's position
    ///     in the grid.)
    /// </para>
    /// 
    /// <list type="number">
    ///   <item>If a cell's contents is a string, its value is that string.</item>
    /// 
    ///   <item>If a cell's contents is a double, its value is that double.</item>
    /// 
    ///   <item>
    ///      If a cell's contents is a Formula, its value is either a double or a FormulaError,
    ///      as reported by the Evaluate method of the Formula class.  The value of a Formula,
    ///      of course, can depend on the values of variables.  The value of a variable is the 
    ///      value of the spreadsheet cell it names (if that cell's value is a double) or 
    ///      is undefined (otherwise).
    ///   </item>
    /// 
    /// </list>
    /// 
    /// <para>
    ///     Spreadsheets are never allowed to contain a combination of Formulas that establish
    ///     a circular dependency.  A circular dependency exists when a cell depends on itself.
    ///     For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    ///     A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    ///     dependency.
    /// </para>
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, object> Cells = new Dictionary<string, object>();
        private DependencyGraph dependencyGraph;

        /// <summary>
        /// This is a constructor method for the SpreadSheet class
        /// </summary>
        public Spreadsheet()
        {
            Cells = new Dictionary<string, object>();
            dependencyGraph = new DependencyGraph();
        }

        /// <summary>
        /// Private class that defines what is a cell and what certain
        /// "type" specific cells hold and how they handle their contents
        /// </summary>
        private class Cell
        {
            public object Contents { get; set; }

            public object Value { get; set; }

            /// <summary>
            /// A number cell
            /// </summary>
            /// <param name="number"></param>
            public Cell(double number)
            {

                Contents = number;
                Value = number;
                Contents = Value;

            }
            /// <summary>
            /// A text cell
            /// </summary>
            /// <param name="text"></param>
            public Cell(string text)
            {

                Contents = text;
                Value = text;
                Contents = Value;

            }
            /// <summary>
            /// Formula cell
            /// </summary>
            /// <param name="formula"></param>
            public Cell(Formula formula)
            {

                Contents = formula;
                Value = formula.Evaluate(x => 1);


            }

        }
        /// <summary>
        ///   Returns the contents (as opposed to the value) of the named cell.
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   Thrown if the name is null or invalid
        /// </exception>
        /// 
        /// <param name="name">The name of the spreadsheet cell to query</param>
        /// 
        /// <returns>
        ///   The return value should be either a string, a double, or a Formula.
        ///   See the class header summary 
        /// </returns>
        public override object GetCellContents(string name)
        {
            if (name == null || !validCellName(name))
            {
                throw new InvalidNameException();
            }

            if (Cells.ContainsKey(name))
            {
                return ((Cell)Cells[name]).Contents;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns an Enumerable that can be used to enumerates 
        /// the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return Cells.Keys.ToArray();
        }

        /// <summary>
        ///  Set the contents of the named cell to the given number.  
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="number"> The new contents/value </param>
        /// 
        /// <returns>
        ///   <para>
        ///      The method returns a set consisting of name plus the names of all other cells whose value depends, 
        ///      directly or indirectly, on the named cell.
        ///   </para>
        /// 
        ///   <para>
        ///      For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///      set {A1, B1, C1} is returned.
        ///   </para>
        /// </returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            if (name == null || !validCellName(name))
            {
                throw new InvalidNameException();
            }

            return SetCellContentsHelper(name, new Cell(number));

        }

        /// <summary>
        /// The contents of the named cell becomes the text.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If text is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell </param>
        /// <param name="text"> The new content/value of the cell</param>
        /// 
        /// <returns>
        ///   The method returns a set consisting of name plus the names of all 
        ///   other cells whose value depends, directly or indirectly, on the 
        ///   named cell.
        /// 
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned.
        ///   </para>
        /// </returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            if (name == null || text == null || !validCellName(name))
            {
                throw new InvalidNameException();
            }
            return SetCellContentsHelper(name, new Cell(text));
        }

        /// <summary>
        /// Set the contents of the named cell to the formula.  
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If formula parameter is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name</param>
        /// <param name="formula"> The content of the cell</param>
        /// 
        /// <returns>
        ///   <para>
        ///     The method returns a Set consisting of name plus the names of all other 
        ///     cells whose value depends, directly or indirectly, on the named cell.
        ///   </para>
        ///   <para> 
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned.
        ///   </para>
        /// 
        /// </returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            if (name == null || formula == null || !validCellName(name))
            {
                throw new InvalidNameException();
            }

            dependencyGraph.ReplaceDependees(name, formula.GetVariables());

            try
            {
                dependencyGraph.ReplaceDependees(name, formula.GetVariables());

                HashSet<String> dependees = new HashSet<String>(GetCellsToRecalculate(name));

                Cell FormulaCell = new Cell(formula);

                if (!Cells.ContainsKey(name))
                {
                    Cells.Add(name, FormulaCell);
                }
                else
                {
                    Cells[name] = FormulaCell;
                }

                return dependees;
            }
            catch (CircularException)
            {
                throw new CircularException();
            }

        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell. 
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"> 
        ///   If the name is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"></param>
        /// <returns>
        ///   Returns an enumeration, without duplicates, of the names of all cells that contain
        ///   formulas containing name.
        /// 
        ///   <para>For example, suppose that: </para>
        ///   <list type="bullet">
        ///      <item>A1 contains 3</item>
        ///      <item>B1 contains the formula A1 * A1</item>
        ///      <item>C1 contains the formula B1 + A1</item>
        ///      <item>D1 contains the formula B1 - C1</item>
        ///   </list>
        /// 
        ///   <para>The direct dependents of A1 are B1 and C1</para>
        /// 
        /// </returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null || !validCellName(name))
            {
                throw new InvalidNameException();
            }

            return dependencyGraph.GetDependents(name);
        }

        /// <summary>
        /// Helper method for the SetCellContents. This method takes away some redundant code
        /// and makes it easier overall to follow the SetCellContents methods
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newCell"></param>
        /// <param name="newDependeesOfCell"></param>
        /// <returns></returns>//needed a little help from ChatGPT to get the right format for this helper method
        private ISet<string> SetCellContentsHelper(string name, Cell content, HashSet<string> Dependees = null)
        {
            if (!Cells.ContainsKey(name))
            {
                Cells.Add(name, content);
            }
            else
            {
                Cells[name] = content;
            }

            dependencyGraph.ReplaceDependees(name, new HashSet<string>());

            return new HashSet<string>(Dependees ?? GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Simple validation of a cells name. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool validCellName(string name)
        {

            string pattern = @"^[a-zA-Z_][a-zA-Z_0-9]*$";

            return Regex.IsMatch(name, pattern);
        }

    }
}

