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
///Date: 6/14/24
namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Keeps track of the current player
        /// </summary>
        private bool player1Turn = true;

        /// <summary>
        /// Initialization of the GameInstructions class
        /// </summary>
        private GameInstructions gameInstructions;

        /// <summary>
        /// Keeps track of the player 1 wins
        /// </summary>
        private int player1Wins;

        /// <summary>
        /// Keeps track of the player 2 wins
        /// </summary>
        private int player2Wins;

        /// <summary>
        /// Keeps track of the ties between the players
        /// </summary>
        private int ties;

        /// <summary>
        /// Keeps track if the game has been officially started by hitting the START GAME button
        /// </summary>
        private bool gameStarted;

        /// <summary>
        /// Starts up the main program
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            gameInstructions = new GameInstructions();
            player1Wins = 0;
            player2Wins = 0;
            ties = 0;

        }

        /// <summary>
        /// The main function of the program, when one of the grid buttons is clicked, the location is attached with the current
        /// player, and is filled with a colorized X or O depending on the player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (!gameStarted)
            {
                MessageBox.Show("Please start the game first!");
                return;
            }

            GameStatus_TxtBox.Text = " ";
            Button button = (Button)sender;
            string[] location = button.Name.Split('_');
            int row = int.Parse(location[1]);
            int col = int.Parse(location[2]);

            if (gameInstructions.GameBoard[row, col] == ' ')
            {
                TextBlock textBlock = new TextBlock();
                if (player1Turn)
                {
                    gameInstructions.GameBoard[row, col] = 'X';
                    textBlock.Text = "X";
                    textBlock.Foreground = Brushes.Red;
                    button.Content = textBlock;
                    GameStatus_TxtBox.Text = "Player 2's Turn";
                }
                else
                {
                    gameInstructions.GameBoard[row, col] = 'O';
                    textBlock.Text = "O";
                    textBlock.Foreground = Brushes.Blue;
                    button.Content = textBlock;
                    GameStatus_TxtBox.Text = "Player 1's Turn";
                }

                if (gameInstructions.Win(out List<(int, int)> winningCells))
                {
                    HighlightsWinningMove(winningCells);
                    if (player1Turn)
                    {
                        player1Wins++;
                        MessageBox.Show("Player 1 (X) Wins!");
                    }
                    else
                    {
                        player2Wins++;
                        MessageBox.Show("Player 2 (O) Wins!");
                    }
                    ResetGame();
                    GameStats();
                    return;
                }

                if (gameInstructions.IsTie())
                {
                    ties++;
                    MessageBox.Show("It's a Tie!");
                    ResetGame();
                    GameStats();
                    return;
                }

                player1Turn = !player1Turn; //Goes to next player
            }


            GameStats();

        }

        /// <summary>
        /// Highlights the winning row/column or diagonal set
        /// </summary>
        /// <param name="winningCells"></param>
        private void HighlightsWinningMove(List<(int, int)> winningCells)
        {
            foreach (var cell in winningCells)
            {
                string buttonName = $"Button_{cell.Item1}_{cell.Item2}";
                Button button = (Button)GameBoardGrid.FindName(buttonName);
                button.Background = Brushes.Yellow;
            }
        }

        /// <summary>
        /// Updates the game statistics of the number of player 1, 2 or ties.
        /// </summary>
        private void GameStats()
        {
            Player1WinsCount_Label.Content = player1Wins;
            Player2WinsCount_Label.Content = player2Wins;
            TiesCount_Label.Content = ties;
        }

        /// <summary>
        /// Resets the game when a round is finished. Sets the button contents to blank,
        /// and resets the highlighted winning move.
        /// </summary>
        private void ResetGame()
        {
            Button_0_0.Content = " ";
            Button_0_1.Content = " ";
            Button_0_2.Content = " ";
            Button_1_0.Content = " ";
            Button_1_1.Content = " ";
            Button_1_2.Content = " ";
            Button_2_0.Content = " ";
            Button_2_1.Content = " ";
            Button_2_2.Content = " ";

            Button_0_0.Background = Brushes.White;
            Button_0_1.Background = Brushes.White;
            Button_0_2.Background = Brushes.White;
            Button_1_0.Background = Brushes.White;
            Button_1_1.Background = Brushes.White;
            Button_1_2.Background = Brushes.White;
            Button_2_0.Background = Brushes.White;
            Button_2_1.Background = Brushes.White;
            Button_2_2.Background = Brushes.White;


            gameInstructions.ResetBoard();
            GameStatus_TxtBox.Text = "Player 1's Turn";
            EnableBoard();
            player1Turn = true;
        }

        /// <summary>
        /// Starts the game of Tic Tac Toe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGame_Button_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
            gameStarted = true;
            EnableBoard();
            GameStatus_TxtBox.Text = "Game Started. Player 1's Turn";

        }

        /// <summary>
        /// After the start button is clicked the player is than allowed to input into the game board
        /// </summary>
        private void EnableBoard()
        {
            foreach (Button button in GameBoardGrid.Children.OfType<Button>())
            {
                button.IsEnabled = true;
                button.FontSize = 100;
            }
        }
    }
}