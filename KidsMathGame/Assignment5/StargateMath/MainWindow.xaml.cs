using System.Diagnostics;
using System.Media;
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
///Teacher: Professor Cowder
///Date: 7/1/24
namespace StargateMath
{
    /// <summary>
    /// This class is in control of the inner workings of the main window page. The main window page is 
    /// in charge of getting the user's information, and allowing the user to decide what game mode is to be played.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Plays the background song for the main page
        /// </summary>
        private SoundPlayer themeSong;

        /// <summary>
        /// when the player enters the selected game mode, and when the begin is clicked, this sound effect happens
        /// </summary>
        private SoundPlayer EnterSound;

        /// <summary>
        /// MainWindow constructor
        /// </summary>
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                this.Loaded += MainWindow_Loaded;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in creating main window: {ex.Message}");
            }
        }

        /// <summary>
        /// Tells the PlayThemeSong method that the page has been loaded and to begin playing the background music.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PlayThemeSong();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load theme song: {ex.Message}");
            }
        }

        /// <summary>
        /// In control of the background sound when the main page loads.
        /// </summary>
        private void PlayThemeSong()
        {
            try
            {
                themeSong = new SoundPlayer("Stargate SG-1 Theme Song.wav");
                themeSong.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load theme song: {ex.Message}");
            }
        }

        /// <summary>
        /// When the BeginGameButton is clicked, this quick sound effect takes place
        /// </summary>
        private void PlayEnterSound()
        {
            try
            {
                EnterSound = new SoundPlayer("EnteringStargate.wav");
                EnterSound.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not play 'enter' sound: {ex.Message}");
            }
        }

        /// <summary>
        /// Stops the theme song when the main page closes
        /// </summary>
        private void StopThemeSong()
        {
            try
            {
                if (themeSong != null)
                {
                    themeSong.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error, could not stop theme song: {ex.Message}");
            }
        }

        /// <summary>
        /// This method is in control of making sure that the correct information has been inputted, as well as
        /// keeping track of the game mode's type. After the button is clicked, main page closes down, the background
        /// music stops, and the enter sound is played.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BeginGameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = NameTextBox.Text;
                if (string.IsNullOrEmpty(name))
                {
                    ShowError("Please enter your name to continue");
                    return;
                }

                if (!int.TryParse(AgeTextBox.Text, out int age) || age < 3 || age > 10)
                {
                    ShowError("Please enter a valid age between 3 and 10");
                    return;
                }

                string gameType = null;

                // checks which radio button is picked
                if (AddRadioButton.IsChecked == true)
                    gameType = "Add";
                else if (SubtractRadioButton.IsChecked == true)
                    gameType = "Subtract";
                else if (MultiplyRadioButton.IsChecked == true)
                    gameType = "Multiply";
                else if (DivideRadioButton.IsChecked == true)
                    gameType = "Divide";

                if (gameType == null)
                {
                    ShowError("Please select a game type");
                    return;
                }

                var gameWindow = new GameWindow(new UserInfo(name, age), gameType);
                StopThemeSong();
                gameWindow.Show();
                this.Hide();
                PlayEnterSound();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error, Could not start game: {ex.Message}");
            }
        }

        /// <summary>
        /// Helper method to display the correct error if something is not filled out correctly by the user
        /// </summary>
        /// <param name="message"></param>
        private void ShowError(string message)
        {
            try
            {
                ErrorLabel.Content = message;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in displaying error message: {ex.Message}");//love this lol
            }
        }
    }
}