using System;
using System.Text;

public class Interface
{
    public static void DrawBoard(Board i_GameBoard)
    {
        StringBuilder boardString = new StringBuilder();

        Ex02.ConsoleUtils.Screen.Clear();       
        for (int row = 0; row < i_GameBoard.RowSize; row++)
        {
            boardString.Append('|');
            for (int col = 0; col < i_GameBoard.ColSize; col++)
            {
                boardString.Append(' ').Append(Board.GetCharOfCell(i_GameBoard.GameBoard[row, col])).Append(" |");
            }

            boardString.AppendLine().Append('=', (i_GameBoard.ColSize * 4) + 1).AppendLine();                      
        }

        System.Console.WriteLine(boardString.ToString());
    }

    public static void PrintScore(GameManager i_GameManager)
    {
        int p1Score;
        int p2Score;

        i_GameManager.GetPlayerScoreByIndex(0, out p1Score);
        i_GameManager.GetPlayerScoreByIndex(1, out p2Score);        
        System.Console.WriteLine(@"
The Score is:
Player 1: {0}
Player 2: {1}", p1Score, p2Score);
    }

    // $G$ NTT-999 (-3) Why using an out parameter here? You can simply use a return value.
    public static void AskNewGameInput(out int o_NewGameInput)
    {
        bool isLegalInt;
        StringBuilder msg = new StringBuilder();

        msg.Append("Would you like to play another game?").Append(Environment.NewLine).Append("Please enter 0 to exit or 1 for new game");
        System.Console.WriteLine(msg.ToString());
        do
        {
            isLegalInt = int.TryParse(System.Console.ReadLine(), out o_NewGameInput);
            if (!isLegalInt)
            {
                System.Console.WriteLine("Please enter 0 to exit or 1 for new game");
            }
            else
            {
                if ((o_NewGameInput != GameManager.sr_Exit) && (o_NewGameInput != GameManager.sr_NewGame))
                {
                    System.Console.WriteLine("Please enter 0 to exit or 1 for new game");
                }
                else
                {
                    break;
                }
            }
        }
        while (true);
    }
 
    public static void ExitMessage()
    {
        Ex02.ConsoleUtils.Screen.Clear();
        System.Console.WriteLine("Thank you Goodbye!");
        System.Console.WriteLine("Press any key to exit...");
        System.Console.ReadLine();
    }

    public static void PrintGameResult(bool i_GameIsWon, GameManager.eGameStatus i_GameStatus)
    {                 
        if (i_GameIsWon)
        {
            if (i_GameStatus == GameManager.eGameStatus.Player1Turn)
            {
                System.Console.WriteLine("Player 1 Wins!");
            }
            else
            {
                System.Console.WriteLine("Player 2 Wins!");
            }
        }
        else
        {
            System.Console.WriteLine("Game board was full - Draw!");
        }
    }

    private static void getBoardSize(out int o_RowSize, out int o_ColSize)
    {
        bool isLegalIntRow;
        bool isLegalIntCol;
        bool isLegalInput;

        System.Console.WriteLine(@"
Please enter the size of the board:
Enter number of Board rows and Board columns (between {0} - {1}):", GameManager.sr_MinBoardSize, GameManager.sr_MaxBoardSize);
        do
        {
            isLegalIntRow = int.TryParse(System.Console.ReadLine(), out o_RowSize);
            isLegalIntCol = int.TryParse(System.Console.ReadLine(), out o_ColSize);
            if (!(isLegalIntRow && isLegalIntCol))
            {
                System.Console.WriteLine("Illegal input, please enter legal integers");
            }
            else
            {
                isLegalInput = !(((o_RowSize < GameManager.sr_MinBoardSize) || (o_RowSize > GameManager.sr_MaxBoardSize))
                            || ((o_ColSize < GameManager.sr_MinBoardSize) || (o_ColSize > GameManager.sr_MaxBoardSize)));
                if (!isLegalInput)
                {
                    System.Console.WriteLine("Illegal input, please insert numbers between ({0} - {1}):", GameManager.sr_MinBoardSize, GameManager.sr_MaxBoardSize);
                }
                else
                {
                    break;
                }
            }
        }
        while (true);
    }

    private static void getMatchType(out int o_IntMatchType)
    {
        bool isLegalIntMatchType;
        bool isLegalInput;

        System.Console.WriteLine(@"
Enter match type:
For PlayerVsPlayer match enter 1, for PlayerVsComputer match enter 2: ");
        do
        {
            isLegalIntMatchType = int.TryParse(System.Console.ReadLine(), out o_IntMatchType);
            if (!isLegalIntMatchType)
            {
                System.Console.WriteLine("Illegal input, please enter legal integer");
            }
            else
            {
                isLegalInput = (o_IntMatchType == GameManager.sr_PlayerVsPlayer) || (o_IntMatchType == GameManager.sr_PlayerVsComputer);
                if (!isLegalInput)
                {
                    System.Console.WriteLine("Illgeal input, please enter 1 for PlayerVsPlayer or 2 for PlayerVsComputer");
                }
                else
                {
                    break;
                }
            }
        }
        while (true);
    }

    public static void GetBoardSizeAndMatchType(out int o_RowSize, out int o_ColSize, out int o_IntMatchType)
    {
        getBoardSize(out o_RowSize, out o_ColSize);
        getMatchType(out o_IntMatchType);
    }

    public static bool GetHumanMove(GameManager i_GameManager, out int o_ColNumber)
    {
        bool result = false;
        bool continueGetInput = true;
        string userInput;        
        
        o_ColNumber = 0;       
        System.Console.WriteLine("Please enter number of column or 'Q' to quit game");
        while (continueGetInput)
        {
            userInput = System.Console.ReadLine();
            bool isLegalInt = int.TryParse(userInput, out o_ColNumber);
            if (userInput.Equals("Q"))
            {
                continueGetInput = false;
                result = false;
            }
            else
            {
                if (isLegalInt)
                {
                    if (GameLogic.IsLegalMove(o_ColNumber, i_GameManager.GameBoard))
                    {
                        result = true;
                        continueGetInput = false;
                    }
                    else
                    {
                        Ex02.ConsoleUtils.Screen.Clear();
                        DrawBoard(i_GameManager.GameBoard);
                        if (o_ColNumber >= 1 && o_ColNumber <= i_GameManager.GameBoard.ColSize)
                        {
                            System.Console.WriteLine("Illegal move, column is full. Please enter number of column or 'Q' to quit game");
                        }
                        else
                        {
                            System.Console.WriteLine("Illegal move, out of range. Please enter number of column or 'Q' to quit game");
                        }
                    }
                }
                else
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    DrawBoard(i_GameManager.GameBoard);
                    System.Console.WriteLine("Illegal input, please enter number of column or 'Q' to quit game");
                }
            }
        }

        return result;
    }
}