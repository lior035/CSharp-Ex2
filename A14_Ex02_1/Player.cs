using System;
using System.Collections;
using System.Collections.Generic;

public class Player
{
    // $G$ NTT-999 (0) These variables should be constants, use readonly when the values will be known at runtime.
    private readonly int r_HumanPlayer = 0;
    private readonly int r_ComputerPlayer = 1;
    private ePlayerType m_PlayerType;
    private int m_Score;

    public Player(int i_IntPlayerType)
    {
        if (i_IntPlayerType == r_HumanPlayer)
        {
            m_PlayerType = ePlayerType.HumanPlayer;
        }
        else if (i_IntPlayerType == r_ComputerPlayer)
        {
            m_PlayerType = ePlayerType.CoumputerPlayer;
        }

        m_Score = 0;
    }

    public int Score
    {
        get { return m_Score; }
    }

    public void IncreaseScore()
    {
        this.m_Score++;
    }
    
    public enum ePlayerType
    {
        HumanPlayer,
        CoumputerPlayer,
    }

    public void MakePlayerMove(GameManager i_GameManager)
    {
        if (this.m_PlayerType == ePlayerType.HumanPlayer)
        {
            makeHumanMove(i_GameManager);
        }
        else
        {
            makeComputerMove(i_GameManager);
        }
    }

    private void makeHumanMove(GameManager i_GameManager)
    {
        int colNumber;
        bool isLegalMove;

        isLegalMove = Interface.GetHumanMove(i_GameManager, out colNumber);        
        if (isLegalMove) 
        {
            i_GameManager.PerformMoveChanges(colNumber);
            if (i_GameManager.MatchType == GameManager.eMatchType.PlayerVsComputer)
            {
                System.Threading.Thread.Sleep(850);
            }
        }               
        else 
        {
            // 'Q' was entered to exit game
            i_GameManager.GameStatus = GameManager.eGameStatus.GameOver;
        }
    }

    // $G$ SFN-013 (+25) Implemented AI
    private void makeComputerMove(GameManager i_GameManager)
    {
        int winningMoveIndex;
        int randMove;
        // $G$ NTT-007 (-10) There's no need to re-instantiate the Random instance each time it is used.
        Random rand = new Random();
        List<int> legalMovesArray;
        List<int> legalNonLosingMovesArray = new List<int>();
        
        if (GameLogic.WinningMoveExists(i_GameManager, out winningMoveIndex))
        {
            i_GameManager.PerformMoveChanges(winningMoveIndex);
        }
        else
        {           
            legalNonLosingMovesArray = generateNonLosingMoveArray(i_GameManager, out legalMovesArray);
            if (legalNonLosingMovesArray.Count > 0)
            {
                randMove = legalNonLosingMovesArray[rand.Next(legalNonLosingMovesArray.Count)];
                i_GameManager.PerformMoveChanges(randMove);
            }
            else 
            {
                randMove = legalMovesArray[rand.Next(legalNonLosingMovesArray.Count)];
                i_GameManager.PerformMoveChanges(randMove);
            }
        }
    }

    private List<int> generateNonLosingMoveArray(GameManager i_GameManager, out List<int> o_LegalMovesArray)
    {
        o_LegalMovesArray = new List<int>();
        List<int> legalNonLosingMovesArray = new List<int>();
        int winningMoveIndex;

        for (int i = 1; i <= i_GameManager.GameBoard.ColSize; i++)
        {
            if (GameLogic.IsLegalMove(i, i_GameManager.GameBoard))
            {
                o_LegalMovesArray.Add(i);
                i_GameManager.InsertMoveToGameBoard(i);
                i_GameManager.ChangePlayerTurn();
                if (!GameLogic.WinningMoveExists(i_GameManager, out winningMoveIndex))
                {
                    legalNonLosingMovesArray.Add(i);
                }

                i_GameManager.ChangePlayerTurn();
                i_GameManager.EraseMoveFromGameBoard(i);
            }
        }

        return legalNonLosingMovesArray;
    }
}