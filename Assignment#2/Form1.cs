using System;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;
///Author:Andrew Winward
///Date:5/18/24
///
namespace Assignment_2
{
    /// <summary>
    /// Methods for the inner functionality of the dice roll game.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// random number
        /// </summary>
        private Random rnd = new Random();

        /// <summary>
        /// Keeps track of the random number to be compared with guess
        /// </summary>
        private int randomNumber;

        /// <summary>
        /// Keeps track of each number rolled
        /// </summary>
        private int[] rollCounts = new int[6];

        /// <summary>
        /// keeps track of the total number of rolls
        /// </summary>
        private int totalRolls = 0;

        /// <summary>
        /// keeps track of the total wins
        /// </summary>
        private int totalWins = 0;

        /// <summary>
        /// keeps track of the total losses
        /// </summary>
        private int totalLosses = 0;

        /// <summary>
        /// keeps track of the guess that the player inputted
        /// </summary>
        private int guess = 0;

        /// <summary>
        /// keeps track of the guess inputted by the user
        /// </summary>
        private int[] guessCounts = new int[6];

        /// <summary>
        /// constructor for the Form1 class
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            ListFormat();
        }

        /// <summary>
        /// A helper method that formats the List view
        /// for the top portion of the stats
        /// </summary>
        private void ListFormat()
        {
            diceStats.Columns.Add("FACE", 70, HorizontalAlignment.Left);
            diceStats.Columns.Add("FREQUENCY", 100, HorizontalAlignment.Left);
            diceStats.Columns.Add("PERCENT", 100, HorizontalAlignment.Left);
            diceStats.Columns.Add("NUMBER OF TIMES GUESSED", 200, HorizontalAlignment.Left);
            diceStats.View = View.Details;
        }

        /// <summary>
        /// Generates a random number
        /// </summary>
        private void GetRandomNumber()
        {
            randomNumber = rnd.Next(1, 7); 
        }

        /// <summary>
        /// The button that initiates the game, and rolls the dice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonRoll_Click(object sender, EventArgs e)
        {
            await DisplayDiceRoll();
            if (guess == randomNumber)
            {
                totalWins++;
                guessCounts[guess-1]++;
            }
            else
            {
                totalLosses++;
                guessCounts[guess-1]++;
            }

            UpdateStats();
            UpdateGameBoard();
        }

        /// <summary>
        /// Updates the game board
        /// </summary>
        private void UpdateGameBoard()
        {
            totalRollsLabel.Text = totalRolls.ToString();
            totalWinsLabel.Text = totalWins.ToString();
            totalLossesLabel.Text = totalLosses.ToString();
        }

        /// <summary>
        /// Rolls the dice and displays it to the program
        /// </summary>
        /// <returns>Dice Roll</returns>
        private async Task DisplayDiceRoll()
        {
            dicePic.SizeMode = PictureBoxSizeMode.StretchImage;
            for (int i = 0; i < 7; i++)
            {
                GetRandomNumber();

                dicePic.Image = Image.FromFile("die" + randomNumber + ".gif");
                dicePic.Refresh();

                await Task.Delay(300);
            }

            rollCounts[randomNumber - 1]++;
            totalRolls++;
        }

        /// <summary>
        /// Updates the stats on the board
        /// </summary>
        private void UpdateStats()
        {
            diceStats.Items.Clear();

            for (int i = 0; i < 6; i++)
            {
                int face = i + 1;
                int frequency = rollCounts[i];
                int guesses = guessCounts[i];
                double percent = (double)frequency / totalRolls * 100;
                ListViewItem stats = new ListViewItem(new string[] {
                    face.ToString(),
                    frequency.ToString(),
                    percent.ToString("F2") + "%",
                    guesses.ToString(),
                });
                diceStats.Items.Add(stats);
            }
        }

        /// <summary>
        /// Resets the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonReset_Click(object sender, EventArgs e)
        {
            userGuessInput.Clear();
            diceStats.Items.Clear();
            totalRolls = 0;
            totalLosses = 0;
            totalWins = 0;
            errorLabel.Text = "";
            dicePic.Image = null;
            Array.Clear(rollCounts, 0, rollCounts.Length);
            Array.Clear(guessCounts, 0, guessCounts.Length);



        }

        /// <summary>
        /// Takes the user input and checks to see if the inputted number is valid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userInput_TextBox(object sender, EventArgs e)
        {
            userGuessInput.MaxLength = 1;


            if (!int.TryParse(userGuessInput.Text, out int myGuess) || myGuess < 1 || myGuess > 6)
            {
                errorLabel.Text = "Invalid entry! Must be a number between 1-6.";
            }
            else
            {
                errorLabel.Text = "";
                guess = myGuess;
                
            }
        }
    }
}
