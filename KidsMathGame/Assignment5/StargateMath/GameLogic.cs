using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
///Author: Andrew Winward
///Teacher: Professor Cowder
///Date: 7/1/24
namespace StargateMath
{
    /// <summary>
    /// This class is in charge of the games instructions. These instructions include creating the necessary questions
    /// for the user to answer, checks to make sure that the answer provided by the user is correct, or incorrect.
    /// </summary>
    public class GameLogic
    {
        /// <summary>
        /// Keeps track of the number of the correct answers for the final score window
        /// </summary>
        public int CorrectAnswers { get; private set; }
        /// <summary>
        /// Keeps track of the number of the incorrect answers for the final score window
        /// </summary>
        public int IncorrectAnswers { get; private set; }
        public List<(string QuestionText, int Answer)> Questions { get; private set; }


        /// <summary>
        /// Game logic constructor
        /// </summary>
        /// <param name="gameType"></param>
        public GameLogic(string gameType)
        {
            try
            {
                Questions = new List<(string, int)>();
                GenerateQuestions(gameType);
                CorrectAnswers = 0;
                IncorrectAnswers = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error, could not create game logic: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates the questions for the user. Each question is generated with the mathematical expression, along with
        /// its accompanying correct answer. These two outputs are then placed inside a Question array, which is then later
        /// accessed by the program to be displayed to the user.
        /// The questions follow the instructions given by the instructor to 
        /// follow numbers within 1-10 range, and that students could double the division numbers to
        /// give a broader range of questions.
        /// </summary>
        /// <param name="gameType"></param>
        private void GenerateQuestions(string gameType)
        {
            try
            {
                var random = new Random();

                for (int i = 0; i < 10; i++)
                {
                    int number1 = random.Next(1, 11);
                    int number2 = random.Next(1, 11);
                    int answer = 0;
                    string questionText = string.Empty;

                    if (gameType == "Add")
                    {
                        answer = number1 + number2;
                        questionText = $"{number1} + {number2}";
                    }
                    else if (gameType == "Subtract")
                    {
                        if (number1 < number2)
                        {
                            int temp = number1;
                            number1 = number2;
                            number2 = temp;
                        }
                        answer = number1 - number2;
                        questionText = $"{number1} - {number2}";
                    }
                    else if (gameType == "Multiply")
                    {
                        answer = number1 * number2;
                        questionText = $"{number1} * {number2}";
                    }
                    else if (gameType == "Divide")
                    {
                        number1 = number1 * number2;
                        answer = number1 / number2;
                        questionText = $"{number1} / {number2}";
                    }
                    Questions.Add((questionText, answer));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error, Could not generate questions: {ex.Message}");
            }



        }
        /// <summary>
        /// Takes the answer that was generated with the question, and then checks it against the user's answer.
        /// If the answer was correct than the method returns true, false otherwise.
        /// </summary>
        /// <param name="questionIndex"></param>
        /// <param name="userAnswer"></param>
        /// <returns></returns>
        public bool CheckAnswer(int questionIndex, int userAnswer)
        {
            try
            {
                if (Questions[questionIndex].Item2 == userAnswer)
                {
                    CorrectAnswers++;
                    return true;
                }
                else
                {
                    IncorrectAnswers++;
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in checking answers: {ex.Message}");
                return false;
            }
        }
    }

}
