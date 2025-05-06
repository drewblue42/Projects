using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///Author: Andrew Winward
///Teacher: Professor Cowder
///Date: 6/14/24
namespace TicTacToe
{
    /// <summary>
    /// The overall Game Logic for Tic Tac Toe
    /// </summary>
    internal class GameInstructions
    {
        /// <summary>
        /// Sets up the game board's grid for input
        /// </summary>
        public char[,] GameBoard { get; set; }

        /// <summary>
        /// Main constructor, here is where the game board is initialized.
        /// </summary>
        public GameInstructions()
        {

            GameBoard = new char[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    GameBoard[i, j] = ' ';
            }
        }

        /// <summary>
        /// Method that determines if the a player won the round
        /// </summary>
        /// <param name="winningCells"></param>
        /// <returns></returns>
        public bool Win(out List<(int, int)> winningCells)
        {

            winningCells = new List<(int, int)>();
            //////////////////////////////////////////// Checks Row for Win//////////////////////////////////////////////////////
            for (int i = 0; i < 3; i++)
            {
                if (GameBoard[i, 0] != ' ' && GameBoard[i, 0] == GameBoard[i, 1] && GameBoard[i, 1] == GameBoard[i, 2])
                {
                    winningCells.Add((i, 0));
                    winningCells.Add((i, 1));
                    winningCells.Add((i, 2));
                    return true;
                }
            }

            //////////////////////////////////////////// Checks Column for Win//////////////////////////////////////////////////////
            for (int i = 0; i < 3; i++)
            {
                if (GameBoard[0, i] != ' ' && GameBoard[0, i] == GameBoard[1, i] && GameBoard[1, i] == GameBoard[2, i])
                {
                    winningCells.Add((0, i));
                    winningCells.Add((1, i));
                    winningCells.Add((2, i));
                    return true;
                }
            }

            //////////////////////////////////////////// Checks Diagonal for Win//////////////////////////////////////////////////////
            if (GameBoard[0, 0] != ' ' && GameBoard[0, 0] == GameBoard[1, 1] && GameBoard[1, 1] == GameBoard[2, 2])
            {
                winningCells.Add((0, 0));
                winningCells.Add((1, 1));
                winningCells.Add((2, 2));
                return true;
            }
            if (GameBoard[2, 0] != ' ' && GameBoard[2, 0] == GameBoard[1, 1] && GameBoard[1, 1] == GameBoard[0, 2])
            {
                winningCells.Add((2, 0));
                winningCells.Add((1, 1));
                winningCells.Add((0, 2));
                return true;
            }

            return false;


        }

        /// <summary>
        /// Checks the gameboard for a tie
        /// </summary>
        /// <returns></returns>
        public bool IsTie()
        {
            foreach (char cell in GameBoard)
            {
                if (cell == ' ')
                {
                    return false;
                }
            }

            return !Win(out _);

        }

        /// <summary>
        /// Erases the previous gameboard to be reused for the next game
        /// </summary>
        public void ResetBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    GameBoard[i, j] = ' ';
                }
            }
        }


    }
}
