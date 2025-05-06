using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
///Author: Andrew Winward
///Teacher: Professor Cowder
///Date: 7/1/24
namespace StargateMath
{
    /// <summary>
    /// All the code that handles the Final Score Window. This is where the players results are displayed,
    /// as well as their info.
    /// </summary>
    public partial class FinalScoreWindow : Window
    {

        /// <summary>
        /// Plays an ending theme
        /// </summary>
        private SoundPlayer EndTheme;

        /// <summary>
        /// The final score window's constructor
        /// </summary>
        /// <param name="user"></param>
        /// <param name="correctAnswers"></param>
        /// <param name="incorrectAnswers"></param>
        /// <param name="elapsedSeconds"></param>
        public FinalScoreWindow(UserInfo user, int correctAnswers, int incorrectAnswers, int elapsedSeconds)
        {
            try
            {
                InitializeComponent();
                NameTextBlock.Text = $"Name: {user.Name}";
                AgeTextBlock.Text = $"Age: {user.Age}";
                CorrectAnswersTextBlock.Text = $"Correct Answers: {correctAnswers}";
                IncorrectAnswersTextBlock.Text = $"Incorrect Answers: {incorrectAnswers}";
                TimeTextBlock.Text = $"Time: {elapsedSeconds} seconds";
                this.Loaded += FinalScoreWindow_Loaded;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in creating final score window: {ex.Message}");
            }
        }
        /// <summary>
        /// Tells the PlayThemeSong method that the page has been loaded and to begin playing the background music.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinalScoreWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PlayEndThemeSong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in music loading: {ex.Message}");
            }
        }

        /// <summary>
        /// In control of the background sound when the final page loads.
        /// </summary>
        private void PlayEndThemeSong()
        {
            try
            {
                EndTheme = new SoundPlayer("Stargate SG-1 Credits Theme.wav");
                EndTheme.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing theme song: {ex.Message}");
            }
        }

        /// <summary>
        /// This button sends the user back to the home page, and closes out the final score page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error, could not return to main window: {ex.Message}");
            }
        }
    }
}
