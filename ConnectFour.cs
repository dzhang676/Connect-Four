using System;
using static System.Console;

namespace Bme121.Pa2
{
    ///<StudentPlan>Biomedical Engineering</StudentPlan>
	///<StudentDept>Department of Systems Design Engineering</StudentDept>
	///<StudentInst>University of Waterloo</StudentInst>
	///<StudentName>Zhang,David</StudentName>
	///<StudentUserID>dbzhang</StudentUserID>
	///<StudentAcknowledgements>
	///I declare that, except as acknowledged below, this is my original work.
	///Acknowledged contributions of others:
	///</StudentAcknowledgements>

    // Procedural-programming implementation of the game Connect Four.
    // Note: Connect Four is (C) Hasbro, Inc.
    // This version is for educational use only.

    static partial class Program
    {
        // For random moves by AI players.
        static Random randomNumberGenerator = new Random();

        // The game board and intuitive names for its size.
        // Each element of the game board is initially null.
        // In valid play, an element may become "O" or "X".
        static string[,] gameBoard = new string[6, 7];
        static readonly int gameRows = gameBoard.GetLength(0);
        static readonly int gameCols = gameBoard.GetLength(1);

        // The playing piece colors can be altered.
        static readonly ConsoleColor fgColor = ForegroundColor;
        static readonly ConsoleColor xColor = ConsoleColor.Cyan;
        static readonly ConsoleColor oColor = ConsoleColor.Magenta;

        // Save the two player's names and kinds (human or AI).
        static string xName, oName;
        static string xKind, oKind;

        // The symbol (O or X), name, and kind of the current player.
        static string currentPlayer;
        static string currentPlayerName;
        static string currentPlayerKind;

        // Play the game, largely by calling game methods.
        static void Main()
        {
            WriteLine();
            WriteLine("BME 121 Connect Four!");
            WriteLine();
            WriteLine("The game of Connect Four is (C) Hasbro, Inc.");
            WriteLine("This version is for educational use only.");
            WriteLine();
            WriteLine("Play by stacking your token in any column with available space.");
            WriteLine("Win with four-in-a-row vertically, horizontally, or diagonally.");

            DrawGameBoard();

            GetPlayerNames();
            GetPlayerKinds();
            GetFirstPlayer();

            while (!IsBoardFull())
            {
                GetPlayerMove();
                DrawGameBoard();

                if (CurrentPlayerWins())
                {
                    WriteLine();
                    Write($"{currentPlayerName} - ");
                    ColorWrite(currentPlayer);
                    WriteLine(" - wins!");
                    WriteLine();
                    return;
                }

                SwitchPlayers();
            }

            WriteLine();
            WriteLine("Game is a draw!");
            WriteLine();
        }

        // Get the displayed names of the two players.
        static void GetPlayerNames()
        {
            WriteLine();
            Write("Enter player O name: ");
            oName = ReadLine();
            Write("Enter player X name: ");
            xName = ReadLine();
        }

        // Get the kinds (human or ai) of the two players.
        static void GetPlayerKinds()
        {
            WriteLine();

            while (true)
            {
                Write("Enter player O kind [human ai]: ");
                oKind = ReadLine().ToLower();
                if (oKind == "human") break;
                if (oKind == "ai") break;
                WriteLine("Must be one of 'human' or 'ai'.");
                WriteLine("Please try again.");
            }

            while (true)
            {
                Write("Enter player X kind [human ai]: ");
                xKind = ReadLine().ToLower();
                if (xKind == "human") break;
                if (xKind == "ai") break;
                WriteLine("Must be one of 'human' or 'ai'.");
                WriteLine("Please try again.");
            }
        }

        // Get and set up the player who will play first.
        static void GetFirstPlayer()
        {
            WriteLine();

            while (true)
            {
                Write("Enter first to play [O X]: ");
                currentPlayer = ReadLine().ToUpper();
                if (currentPlayer == "O") break;
                if (currentPlayer == "X") break;
                WriteLine("Must be one of 'O' or 'X'.");
                WriteLine("Please try again.");
            }

            if (currentPlayer == "O")
            {
                currentPlayerName = oName;
                currentPlayerKind = oKind;
            }

            if (currentPlayer == "X")
            {
                currentPlayerName = xName;
                currentPlayerKind = xKind;
            }
        }

        // Get and perform the desired move by the current player.
        static void GetPlayerMove()
        {
            if (currentPlayerKind == "ai")
            {
                WriteLine();
                Write($"{currentPlayerName} - ");
                ColorWrite(currentPlayer);
                Write(" - choose a column: ");
                int column = SelectRandomColumn();
                System.Threading.Thread.Sleep(1000);
                Write(column);
                System.Threading.Thread.Sleep(1000);
                WriteLine();
                PlayInColumn(column);
            }

            if (currentPlayerKind == "human")
            {
                while (true)
                {
                    WriteLine();
                    Write($"{currentPlayerName} - ");
                    ColorWrite(currentPlayer);
                    Write(" - choose a column: ");
                    int column;
                    if (!int.TryParse(ReadLine(), out column) || !IsValidPlay(column))
                    {
                        WriteLine("Not a valid column or column is full.");
                        WriteLine("Please try again.");
                    }
                    else
                    {
                        PlayInColumn(column);
                        break;
                    }
                }
            }
        }

        // Detect whether the current player has won by looking for a vertical,
        // horizontal, or diagonal run of four of the current player's symbols.
        static bool CurrentPlayerWins()
        {

            //Checking all cases of four in a row in columns
            for (int col = 0; col < 7; col+= 1)
            {
                if (gameBoard[0, col] == currentPlayer && gameBoard[1, col] == currentPlayer
                && gameBoard[2, col] == currentPlayer && gameBoard[3, col] == currentPlayer)
                {
                    return true;
                }
                else if (gameBoard[1, col] == currentPlayer && gameBoard[2, col] == currentPlayer 
                && gameBoard[3, col] == currentPlayer && gameBoard[4, col] == currentPlayer)
                {
                    return true;
                }
                else if (gameBoard[2, col] == currentPlayer && gameBoard[3, col] == currentPlayer 
                && gameBoard[4, col] == currentPlayer && gameBoard[5, col] == currentPlayer)
                {
                    return true;
                }
            }

            //Checking all cases of four in a row in rows
            for (int row = 0; row < 6; row+=1)
            {
                if (gameBoard[row, 0] == currentPlayer && gameBoard[row, 1] == currentPlayer 
                && gameBoard[row, 2] == currentPlayer && gameBoard[row, 3] == currentPlayer)
                {
                    return true;
                }
                else if (gameBoard[row, 1] == currentPlayer && gameBoard[row, 2] == currentPlayer 
                && gameBoard[row, 3] == currentPlayer && gameBoard[row, 4] == currentPlayer)
                {
                    return true;
                }
                else if (gameBoard[row, 2] == currentPlayer && gameBoard[row, 3] == currentPlayer 
                && gameBoard[row, 4] == currentPlayer && gameBoard[row, 5] == currentPlayer)
                {
                    return true;
                }
                else if (gameBoard[row, 3] == currentPlayer && gameBoard[row, 4] == currentPlayer 
                && gameBoard[row, 5] == currentPlayer && gameBoard[row, 6] == currentPlayer)
                {
                    return true;
                }
            }

            //Checking right diagonals
            for (int col = 0; col < 4; col+=1)
            {
                if (gameBoard[0, col] == currentPlayer && gameBoard[1, col + 1] == currentPlayer
                && gameBoard[2, col + 2] == currentPlayer && gameBoard[3, col + 3] == currentPlayer)
                {
                    return true;
                }
                else if (gameBoard[1, col] == currentPlayer && gameBoard[2, col + 1] == currentPlayer 
                && gameBoard[3, col + 2] == currentPlayer && gameBoard[4, col + 3] == currentPlayer)
                {
                    return true;
                }
                else if (gameBoard[2, col] == currentPlayer && gameBoard[3, col + 1] == currentPlayer 
                && gameBoard[4, col + 2] == currentPlayer && gameBoard[5, col + 3] == currentPlayer)
                {
                    return true;
                }
            }

            //Checking left diagonals
            for (int col = 6; col > 2; col--)
            {
                if (gameBoard[0, col] == currentPlayer && gameBoard[1, col - 1] == currentPlayer 
                && gameBoard[2, col - 2] == currentPlayer && gameBoard[3, col - 3] == currentPlayer)
                {
                    return true;
                }
                else if (gameBoard[1, col] == currentPlayer && gameBoard[2, col - 1] == currentPlayer 
                && gameBoard[3, col - 2] == currentPlayer && gameBoard[4, col - 3] == currentPlayer)
                {
                    return true;
                }
                else if (gameBoard[2, col] == currentPlayer && gameBoard[3, col - 1] == currentPlayer
                && gameBoard[4, col - 2] == currentPlayer && gameBoard[5, col - 3] == currentPlayer)
                {
                    return true;
                }
            }
            
            return false;
        }

        // Detect whether the game board is completely filled.
        
        static bool IsBoardFull()
        {
            int col = 0;
            int row = 5;
			bool[] colFull = new bool[7] ;
			
				while (col < 7)
				{
					if (gameBoard[row,col] != null)
					{
						colFull[col] = true;
					}
					col += 1;
				}
		
			if (colFull[0] == true && colFull[1] == true && colFull[2] == true 
			&& colFull[3] == true && colFull[4] == true && colFull[5] == true && colFull[6] == true  )
			{
				return true;
			}
			else
			{
				return false;
			}
		}
					
			
           
        // Detect whether given column is on the board and has space remaining.
        static bool IsValidPlay(int col)
        {
            bool colValid = false;
            if (col >= 0 && col < 7)
            {
                colValid = true;
            }
            else
            {
                return false;
            }

            bool rowValid = false;
            int row = 0;
            while (row < 6)
            {
                if (gameBoard[row, col] == null)
                {
                    rowValid = true;
                    break;
                    
                }
                else
                {
                    rowValid = false;
                    row += 1;
                    
                }
            }

            if (rowValid == true && colValid == true)
            {
                return true;
            }
            else
            {
                return false;
            }     
        }

        // Play current player's symbol on top of existing plays in the selected column.
        static void PlayInColumn(int col)
        {
            int row = 0;
            while (true)
            {
                if (gameBoard[row, col] == null)
                {
                    gameBoard[row, col] = currentPlayer;
                    break;
                }
                else
                {
                    row += 1;
                }
            }
        }

        // Select a column at random until a valid play is found.
        static int SelectRandomColumn()
        {
            return randomNumberGenerator.Next(0, 7);
        }

        // Change the current player from player O to player X or vice versa.
        static void SwitchPlayers( )
        {
            if (currentPlayer == "O")
            {
				currentPlayer = "X";
				currentPlayerName = xName;
				currentPlayerKind = xKind;
			}
			else
			{
				currentPlayer = "O";
				currentPlayerName = oName;
				currentPlayerKind = oKind;
			}
        }

        // Display the current game board on the console.
        // This version uses only ASCII characters for portability.
        static void DrawGameBoard()
        {
            WriteLine();
            for (int row = gameRows - 1; row >= 0; row--)
            {
                Write("   |");
                for (int col = 0; col < gameCols; col++) Write("   |");
                WriteLine();
                Write($"{row,2} |");
                for (int col = 0; col < gameCols; col++)
                {
                    Write(" ");
                    ColorWrite(gameBoard[row, col]);
                    Write(" |");
                }
                WriteLine();
            }
            Write("   |");
            for (int col = 0; col < gameCols; col++) Write("___|");
            WriteLine();
            WriteLine();
            Write("    ");
            for (int col = 0; col < gameCols; col++) Write($"{col,2}  ");
            WriteLine();
        }

        // Display O or X in their special color.
        static void ColorWrite(string symbol)
        {
            if (symbol == "O") ForegroundColor = oColor;
            if (symbol == "X") ForegroundColor = xColor;
            // Empty cells in the game board use null but
            // we still want them to display using one space.
            Write($"{symbol,1}");
            ForegroundColor = fgColor;
        }
    }
}

