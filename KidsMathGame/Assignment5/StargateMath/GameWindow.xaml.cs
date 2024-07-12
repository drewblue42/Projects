using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
///Author: Andrew Winward
///Teacher: Professor Cowder
///Date: 7/1/24
namespace StargateMath
{
    /// <summary>
    /// All internal coding for the Game Window, where the player answers various mathematical questions.
    /// </summary>
    public partial class GameWindow : Window
    {
        /// <summary>
        /// The user's info from the user info class
        /// </summary>
        private UserInfo user;

        /// <summary>
        /// Keeps track of the chosen game type
        /// </summary>
        private string gameType;

        /// <summary>
        /// Initializes and allows access to the core game logic.
        /// </summary>
        private GameLogic game;

        /// <summary>
        /// Keeps track what Number of question the player is on
        /// </summary>
        private int currentQuestionNum;

        /// <summary>
        /// Keeps track of the amount that is correctly answered by the player
        /// </summary>
        private int CorrectNum = 0;

        /// <summary>
        /// Initializes and creates a timer to keep track of a game's duration
        /// </summary>
        private DispatcherTimer timer;

        /// <summary>
        /// Keeps track of the total elapsed time
        /// </summary>
        private int elapsedTime;

        /// <summary>
        /// Plays a sound when the answer is correct
        /// </summary>
        private SoundPlayer correctSound;

        /// <summary>
        /// Plays a sound when the answer us wrong
        /// </summary>
        private SoundPlayer incorrectSound;

        /// <summary>
        /// Plays a sound when the game is completed
        /// </summary>
        private SoundPlayer gameEndSound;

        /// <summary>
        /// Game Window's constructor 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="gameType"></param>
        public GameWindow(UserInfo user, string gameType)
        {
            try
            {
                InitializeComponent();
                this.user = user;
                this.gameType = gameType;
                game = new GameLogic(gameType);
                currentQuestionNum = 0;
                elapsedTime = 0;

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += TotalTime;
                timer.Start();

                DisplayNextQuestion();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in creating game: {ex.Message}");
            }
        }
        /// <summary>
        /// In control of playing sound if the answer is correct
        /// </summary>
        private void PlayCorrectSound()
        {
            try
            {
                correctSound = new SoundPlayer("correctAnswer.wav");
                correctSound.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing correct sound: {ex.Message}");
            }
        }
        /// <summary>
        /// In control of playing a sound if the answer is incorrect
        /// </summary>
        private void PlayIncorrectSound()
        {
            try
            {
                incorrectSound = new SoundPlayer("wrongAnswer.wav");
                incorrectSound.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing incorrect sound: {ex.Message}");
            }
        }
        /// <summary>
        /// In control of the background sound when the main page loads.
        /// </summary>
        private void PlayGameEndSound()
        {
            try
            {
                gameEndSound = new SoundPlayer("StargateClose.wav");
                gameEndSound.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing end song: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays the total time for the player during a game session.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TotalTime(object sender, EventArgs e)
        {
            try
            {
                elapsedTime++;
                TimerTextBlock.Text = $"Time: {elapsedTime} seconds";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting total time: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays the questions that are created in the game logic. When 10 questions is reached
        /// the game ends,a dn switches to the final score board.
        /// </summary>
        private void DisplayNextQuestion()
        {
            try
            {
                if (currentQuestionNum >= 10)
                {
                    PlayGameEndSound();
                    EndGame();
                    return;
                }

                var question = game.Questions[currentQuestionNum];
                QuestionText.Text = $"{question.QuestionText} = ";
                AnswerTextBox.Clear();
                AnswerTextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying next question: {ex.Message}");
            }
        }

        /// <summary>
        /// Helper method that ends the game, this entails stopping the game timer, switching and showing the final score window
        /// and closing the game window
        /// </summary>
        private void EndGame()
        {
            try
            {
                timer.Stop();
                var scoreWindow = new FinalScoreWindow(user, game.CorrectAnswers, game.IncorrectAnswers, elapsedTime);
                scoreWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ending game: {ex.Message}");
            }
        }

        /// <summary>
        /// When the submit button is activated it checks to see if the current input is of a numeric value. If it is not,
        /// an error message is displayed. If it is, the game logic checks to see if the answer in the Answer text box 
        /// is correct. If incorrect or correct, it is displayed in the results text box, and th amount that
        /// the player has answered correctly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (int.TryParse(AnswerTextBox.Text, out int answer))
                {
                    bool isCorrect = game.CheckAnswer(currentQuestionNum, answer);
                    if (isCorrect)
                    {
                        PlayCorrectSound();
                        CorrectNum++;
                        ResultTextBlock.Text = $"Correct! Number of correct answers: {CorrectNum}";
                    }
                    else
                    {
                        PlayIncorrectSound();
                        ResultTextBlock.Text = $"Incorrect! Number of correct answers: {CorrectNum}";
                    }
                    currentQuestionNum++;
                    DisplayNextQuestion();
                }
                else
                {
                    ResultTextBlock.Text = "Please enter a valid number";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending answer: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancels the game before the required number of questions is answered. Takes player to main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelGameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                timer.Stop();
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ending the game: {ex.Message}");
            }

        }
    }

}

