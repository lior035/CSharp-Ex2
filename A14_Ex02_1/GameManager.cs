using System.Collections.Generic;

public class GameManager
{
    public static readonly int sr_MinBoardSize = 4;
    public static readonly int sr_MaxBoardSize = 8;
    public static readonly int sr_PlayerVsPlayer = 1;
    public static readonly int sr_PlayerVsComputer = 2;
    public static readonly int sr_Exit = 0;
    public static readonly int sr_NewGame = 1;
    public static readonly int sr_NumOfPlayers = 2;
    // $G$ NTT-999 (-5) These variables should be constants, use readonly when the values will be known at runtime.
    public readonly int r_HumanPlayer = 0;
    public readonly int r_ComputerPlayer = 1;     
    private List<Player> m_PlayersArray;
    private eGameStatus m_GameStatus;
    private Board m_GameBoard;
    private eMatchType m_MatchType;

    public GameManager()
    {
        int rowSize;
        int colSize;
        int intMatchType;

        Interface.GetBoardSizeAndMatchType(out rowSize, out colSize, out intMatchType);
        m_GameBoard = new Board(rowSize, colSize);
        m_GameStatus = eGameStatus.Player1Turn;
        m_PlayersArray = new List<Player>(sr_NumOfPlayers);
        m_PlayersArray.Insert(0, new Player(r_HumanPlayer));
        if (intMatchType == sr_PlayerVsPlayer)
        {
            m_MatchType = eMatchType.PlayerVsPlayer;
            m_PlayersArray.Insert(1, new Player(r_HumanPlayer));
        }
        else
        {                       
            m_MatchType = eMatchType.PlayerVsComputer;
            m_PlayersArray.Insert(1, new Player(r_ComputerPlayer));
        }
    }

    public enum eGameStatus
    {
        Player1Turn,
        Player2Turn,
        GameOver,
    }
    
    public eGameStatus GameStatus
    {
        get { return m_GameStatus; }
        set { m_GameStatus = value; }
    }

    public Board GameBoard
    {
        get { return m_GameBoard; }        
    }

    public enum eMatchType
    {
        PlayerVsPlayer,
        PlayerVsComputer,
    }

    public eMatchType MatchType
    {
        get { return m_MatchType; }
    }

    private void updateScore(bool i_GameIsWon)
    {
        if (i_GameIsWon)
        {
            if (m_GameStatus == eGameStatus.Player1Turn)
            {
                m_PlayersArray[0].IncreaseScore();
            }
            else
            {
                m_PlayersArray[1].IncreaseScore();
            }
        }
    }

    public bool GetPlayerScoreByIndex(int i_Index, out int o_Score)
    {
        bool result = false;
        o_Score = 0;

        if (i_Index >= 0 && i_Index < m_PlayersArray.Count)
        {
            o_Score = m_PlayersArray[i_Index].Score;
            result = true;
        }

        return result;
    }

    private void gameOver()
    {
        int newGameInput;

        Interface.AskNewGameInput(out newGameInput);
        if (newGameInput == sr_Exit)
        {
            Interface.ExitMessage();
        }
        else if (newGameInput == sr_NewGame)
        {
            startNewGame();
        }
    }

    private void startNewGame()
    {   
        m_GameBoard = new Board(m_GameBoard.RowSize, m_GameBoard.ColSize);
        m_GameStatus = eGameStatus.Player1Turn;
        this.RunGame();
    }

    public void RunGame()
    {
        Interface.DrawBoard(m_GameBoard);
        while (m_GameStatus != eGameStatus.GameOver)
        {     
            makeTurn();
        }

        gameOver();
    }

    private void makeTurn()
    {
        switch (m_GameStatus)
        {
            case eGameStatus.Player1Turn:
                m_PlayersArray[0].MakePlayerMove(this);
                break;
            case eGameStatus.Player2Turn:
                m_PlayersArray[1].MakePlayerMove(this);
                break;
        }
    }

    public void InsertMoveToGameBoard(int i_ColInput)
    {
        int indexOfFirstAvailableRowInInputCol = m_GameBoard.FirstAvailablePoisitionInCol[i_ColInput - 1];

        if (m_GameStatus == eGameStatus.Player1Turn)
        {
            m_GameBoard.GameBoard[m_GameBoard.RowSize - indexOfFirstAvailableRowInInputCol - 1, i_ColInput - 1] = Board.eBoardCellTypes.Circle;
        }
        else
        {
            m_GameBoard.GameBoard[m_GameBoard.RowSize - indexOfFirstAvailableRowInInputCol - 1, i_ColInput - 1] = Board.eBoardCellTypes.X;
        }

        m_GameBoard.FirstAvailablePoisitionInCol[i_ColInput - 1]++;
    }

    public void EraseMoveFromGameBoard(int i_ColInput)
    {
        int indexOfFirstAvailableRowInInputCol = m_GameBoard.FirstAvailablePoisitionInCol[i_ColInput - 1];
        m_GameBoard.GameBoard[m_GameBoard.RowSize - indexOfFirstAvailableRowInInputCol, i_ColInput - 1] = Board.eBoardCellTypes.Empty;
        m_GameBoard.FirstAvailablePoisitionInCol[i_ColInput - 1]--;
    }

    public void ChangePlayerTurn()
    {
        if (m_GameStatus == eGameStatus.Player1Turn)
        {
            m_GameStatus = eGameStatus.Player2Turn;
        }
        else if (m_GameStatus == eGameStatus.Player2Turn)
        {
            m_GameStatus = eGameStatus.Player1Turn;
        }
    }
     
    public void PerformMoveChanges(int i_ColInput)
    {
        this.InsertMoveToGameBoard(i_ColInput);
        m_GameBoard.IncreaseOccupiedCells();
        Interface.DrawBoard(m_GameBoard);
        updateGameStatus(i_ColInput);
    }

    private void updateGameStatus(int i_ColInput)
    {
        bool gameIsWon;

        if (GameLogic.IsGameOver(i_ColInput, m_GameBoard, out gameIsWon))
        {
            this.updateScore(gameIsWon);
            Interface.PrintGameResult(gameIsWon, m_GameStatus);            
            Interface.PrintScore(this);            
            m_GameStatus = eGameStatus.GameOver;
        }
        else
        {
            this.ChangePlayerTurn();
        }
    }
}
