using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
///Author: Andrew Winward
///Professor: Shawn Cowder
///Date:6/1/24
///
namespace GradeInputter
{
    /// <summary>
    /// Functionality for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Keeps track of the number of students
        /// </summary>
        private int numStudents = 0;

        /// <summary>
        /// Keeps track of the number of assignments
        /// </summary>
        private int numAssign = 0;

        /// <summary>
        /// Multidimensional array that keeps track of the students name and the assignment scores
        /// </summary>
        private double[,] assignmentScores;

        /// <summary>
        /// Array that keeps track of the student's name
        /// </summary>
        private string[] studentNames;

        /// <summary>
        /// Keeps track of the current student when using the first, prev, next, and last buttons
        /// </summary>
        private int currentStudent = 0;

        /// <summary>
        /// Initializes the program
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Button that submits the amount of students and assignments to the overall program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitCounts_Button_Click(object sender, RoutedEventArgs e)
        {
            studentNames = new string[numStudents];
            assignmentScores = new double[numStudents, numAssign];

            for (int i = 0; i < numStudents; i++)
            {
                studentNames[i] = $"Student #{i + 1}";
                for(int j = 0; j < numAssign; j++)
                {
                    assignmentScores[i, j] = 0 ;
                }

            }
            EnterAssignNum_Label.Content = "Enter Assignment Number(1-" + numAssign + ")";
            errorLabel.Content = "Students and Assignment numbers submitted successfully!";
           

        }

        /// <summary>
        /// Keeps track if the number for the amount of students is either a valid or invalid entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumStudents_TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int numberStuds;
            if (!(int.TryParse(NumStudents_TxtBox.Text, out numberStuds) && numberStuds > 0 && numberStuds <= 10))
            {
                errorLabel.Content = "Invalid entry! Must be a number between 1-10.";
            }
            else
            {
                errorLabel.Content = "";
                numStudents = numberStuds;
            }
        }

        /// <summary>
        /// Keeps track if the number of assignments is a valid entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumClasses_TxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int numAssignments;

            if (!(int.TryParse(NumAssign_TxtBox.Text, out numAssignments) && numAssignments > 0 && numAssignments <= 99))
            {
                errorLabel.Content = "Invalid entry! Must be a number between 1-99.";
            }
            else
            {
                errorLabel.Content = "";
                numAssign = numAssignments;
            }
        }

        /// <summary>
        /// Button that displays all the input data from the user 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayScores_Button_Click(object sender, RoutedEventArgs e)
        {
            string display = "STUDENT\t";

            for (int i = 0; i < numAssign; i++)
            {
                display += $"#{i + 1}\t";
            }
            display += "AVG\tGrade\n";

            for (int i = 0; i < numStudents; i++)
            {
                display += $"{studentNames[i]}\t";

                double totalScore = 0;
                for (int j = 0; j < numAssign; j++)
                {
                    display += $"{assignmentScores[i, j]}\t";
                    totalScore += assignmentScores[i, j];
                }

                double averageScore = totalScore / numAssign;
                string letterGrade = LetterGradeCalculator(averageScore);

                display += $"{averageScore:F2}\t{letterGrade}\n";
            }

            dataDisplay_TxtBox.Text = display;
        }
    

        /// <summary>
        /// Gets the letter grade from the average Score from a students assignments
        /// </summary>
        /// <param name="averageScore"></param>
        /// <returns></returns>
        private string LetterGradeCalculator(double averageScore)
        {
            if (averageScore >= 90)
                return "A";
            else if (averageScore >= 80)
                return "B";
            else if (averageScore >= 70)
                return "C";
            else if (averageScore >= 60)
                return "D";
            else
                return "F";
        }

        /// <summary>
        /// Button that displays the first student in the array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FirstStudent_Button_Click(object sender, RoutedEventArgs e)
        {
            if (studentNames != null && studentNames.Length > 0)
            {
                currentStudent = 0;
                StudentName_txtBox.Text = studentNames[currentStudent];
                errorLabel.Content = "";
            }
            else
            {
                errorLabel.Content = "Empty array found. PLease enter number of students and assignments.";
            }
        }

        /// <summary>
        /// Button that displays the previous student in an array 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrevStudent_Button_Click(object sender, RoutedEventArgs e)
        {
            if (studentNames != null && studentNames.Length > 0)
            {
                if (currentStudent > 0)
                {
                    currentStudent -= 1;
                    StudentName_txtBox.Text = studentNames[currentStudent];
                    errorLabel.Content = "";
                }
                else
                {
                    errorLabel.Content = "Cannot go below 0";
                }
            }
            else
            {
                errorLabel.Content = "Empty array found. PLease enter number of students and assignments.";
            }
        }

        /// <summary>
        /// Button that displays the last student in an array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastStudent_Button_Click(object sender, RoutedEventArgs e)
        {
            if (studentNames != null && studentNames.Length > 0)
            {
                currentStudent = studentNames.Length - 1;
                StudentName_txtBox.Text = studentNames[currentStudent];
                errorLabel.Content = "";
            }
            else
            {
                errorLabel.Content = "Empty array found. PLease enter number of students and assignments";
            }
        }

        /// <summary>
        /// Button that displays the next student in an array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextStudent_Button_Click(object sender, RoutedEventArgs e)
        {
            if (studentNames != null && studentNames.Length > 0)
            {
                if (currentStudent < studentNames.Length - 1)
                {
                    currentStudent += 1;
                    StudentName_txtBox.Text = studentNames[currentStudent];
                    errorLabel.Content = "";
                }
                else
                {
                    errorLabel.Content = "Cannot go above student count";
                }
            }
            else
            {
                errorLabel.Content = "Empty array found. PLease enter number of students and assignments";
            }
        }

        /// <summary>
        /// The user can change the name of each student. 
        /// This button takes the input in the StudentName_txtBox and changes it from the default.
        /// Default name is Student#(1-10)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveName_Button_Click(object sender, RoutedEventArgs e)
        {
            if (studentNames != null && studentNames.Length > 0)
            {
                if (currentStudent >= 0 && currentStudent < studentNames.Length)
                {
                    studentNames[currentStudent] = StudentName_txtBox.Text;
                    errorLabel.Content = "Student name saved successfully!";
                }
                else
                {
                    errorLabel.Content = "Invalid student index";
                }
            }
            else
            {
                errorLabel.Content = "Empty array found. PLease enter number of students and assignments";
            }
        }

        /// <summary>
        /// User enters the specific assignment into the AssignNum_txtBox, and then inputs the score into the
        /// AssignScore_txtBox. The button then saves the data into the assingmentScores array to be displayed
        ///  and calculated later.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveScore_Button_Click(object sender, RoutedEventArgs e)
        {
            int assignmentNumber;
            double assignmentScore;

            if (int.TryParse(AssignNum_txtBox.Text, out assignmentNumber) && double.TryParse(AssignScore_txtBox.Text, out assignmentScore))
            {
                if (assignmentNumber > 0 && assignmentNumber <= numAssign && assignmentScore >= 0 && assignmentScore <= 100)
                {
                    assignmentScores[currentStudent, assignmentNumber - 1] = assignmentScore;
                    errorLabel.Content = "Assignment score saved successfully!";
                }
                else
                {
                    errorLabel.Content = "Invalid assignment number or score. Ensure assignment number is within range and score is between 0-100.";
                }
            }
            else
            {
                errorLabel.Content = "Invalid input for assignment number or score.";
            }
        }

        /// <summary>
        /// Resets and clears all the data for the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetScores_Button_Click(object sender, RoutedEventArgs e)
        {
            studentNames= new string[0];
            assignmentScores= new double[0,0];
            dataDisplay_TxtBox.Clear();
            StudentName_txtBox.Clear();
            NumStudents_TxtBox.Clear();
            NumAssign_TxtBox.Clear();
            numAssign = 0;
            numStudents = 0;
            AssignScore_txtBox.Clear();
            AssignNum_txtBox.Clear();
        }
    }
}