using System.Collections;
using System.Collections.Generic;

public class GameLogic
{
    private static readonly int sr_WinningStreakSize = 4;
    private static readonly int sr_LineRadiusToCheck = 3;

    private static bool isFourInLine(Board.eBoardCellTypes[] i_LineOfEnums, Board.eBoardCellTypes i_CurrCellEnum)
    {
        bool fourInLineFound = false;
        int streakOfSameElement = 0;

        for (int i = 0;  i < (sr_LineRadiusToCheck * 2) + 1; i++)
        {
            if (i_CurrCellEnum.Equals(i_LineOfEnums[i]))
            {
                streakOfSameElement++;
                if (streakOfSameElement == sr_WinningStreakSize)
                {
                    fourInLineFound = true;
                    break;
                }
            }
            else
            {
                streakOfSameElement = 0;
            }
        }

        return fourInLineFound;
    }
        
    public static bool IsWinningMove(int i_CurrentInsertCellCol, Board i_GameBoard)
    {
        int currentInsertCellRow = i_GameBoard.FirstAvailablePoisitionInCol[i_CurrentInsertCellCol - 1] - 1;
        Board.eBoardCellTypes currentInsertCellEnum = i_GameBoard.GetCellEnum(i_GameBoard.RowSize - 1 - currentInsertCellRow, i_CurrentInsertCellCol - 1);
        bool result = isFourInLine(generateLineOfCellEnumsFromBoardByDirection(i_GameBoard, Board.eBoardDirections.Horizontal, i_CurrentInsertCellCol), currentInsertCellEnum)
                   || isFourInLine(generateLineOfCellEnumsFromBoardByDirection(i_GameBoard, Board.eBoardDirections.Vertical, i_CurrentInsertCellCol), currentInsertCellEnum)
                   || isFourInLine(generateLineOfCellEnumsFromBoardByDirection(i_GameBoard, Board.eBoardDirections.RightCross, i_CurrentInsertCellCol), currentInsertCellEnum)
                   || isFourInLine(generateLineOfCellEnumsFromBoardByDirection(i_GameBoard, Board.eBoardDirections.LeftCross, i_CurrentInsertCellCol), currentInsertCellEnum);

        return result;
    }

    private static Board.eBoardCellTypes[] generateLineOfCellEnumsFromBoardByDirection(Board i_GameBoard, Board.eBoardDirections i_Direction, int i_CurrentInsertCellCol)
    {        
        int rowSize = i_GameBoard.RowSize;
        int currentInsertCellRow = i_GameBoard.FirstAvailablePoisitionInCol[i_CurrentInsertCellCol - 1] - 1;            
        int startOfLineRow, startOfLineCol, rowDirection, colDirection;
        Board.eBoardCellTypes[] result = new Board.eBoardCellTypes[(sr_LineRadiusToCheck * 2) + 1];       

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = Board.eBoardCellTypes.Empty;
        }

        switch (i_Direction)
        {
            case Board.eBoardDirections.Horizontal:
                startOfLineCol = i_CurrentInsertCellCol - 1 - 3;
                startOfLineRow = currentInsertCellRow;
                rowDirection = 0;
                colDirection = 1;
                break;
            case Board.eBoardDirections.Vertical:
                startOfLineCol = i_CurrentInsertCellCol - 1;
                startOfLineRow = currentInsertCellRow - 3;
                rowDirection = 1;
                colDirection = 0;
                break;
            case Board.eBoardDirections.RightCross:
                startOfLineCol = i_CurrentInsertCellCol - 1 - 3;
                startOfLineRow = currentInsertCellRow - 3;
                rowDirection = 1;
                colDirection = 1; 
                break;
            case Board.eBoardDirections.LeftCross:
                startOfLineCol = i_CurrentInsertCellCol - 1 - 3;
                startOfLineRow = currentInsertCellRow + 3;
                rowDirection = -1;
                colDirection = 1; 
                break;
            default:
                startOfLineRow = startOfLineCol = rowDirection = colDirection = 0;
                break;
        }

        for (int i = 0; i < (sr_LineRadiusToCheck * 2) + 1; i++)
        {
            int currentCellToCheckRow = startOfLineRow + (i * rowDirection);
            int currentCellToCheckCol = startOfLineCol + (i * colDirection);
            if (i_GameBoard.CheckIsInsideBoardBoundaries(currentCellToCheckRow, currentCellToCheckCol))
            {
                result[i] = i_GameBoard.GetCellEnum(rowSize - 1 - currentCellToCheckRow, currentCellToCheckCol);
            }
        }

        return result;
    }   
    
    public static bool IsLegalMove(int i_ColInput, Board i_GameBoard)
    {
        bool result;

        if (i_ColInput < 1 || i_ColInput > i_GameBoard.ColSize)
        {
            result = false;            
        }
        else
        {
            int indexOfFirstAvailableRowInInputCol = i_GameBoard.FirstAvailablePoisitionInCol[i_ColInput - 1];
            result = indexOfFirstAvailableRowInInputCol < i_GameBoard.RowSize;            
        }

        return result;
    }        
    
    public static bool IsGameOver(int i_ColInput, Board i_GameBoard, out bool o_GameIsWon)
    {
        bool result;

        o_GameIsWon = IsWinningMove(i_ColInput, i_GameBoard);
        result = o_GameIsWon || i_GameBoard.isBoardFull();

        return result;
    }

    public static bool WinningMoveExists(GameManager i_GameManager, out int o_WinningMoveIndex)
    {
        bool result = false;

        o_WinningMoveIndex = 0;
        for (int i = 1; i <= i_GameManager.GameBoard.ColSize; i++)
        {
            if (GameLogic.IsLegalMove(i, i_GameManager.GameBoard))
            {
                i_GameManager.InsertMoveToGameBoard(i);
                if (GameLogic.IsWinningMove(i, i_GameManager.GameBoard))
                {
                    result = true;
                    o_WinningMoveIndex = i;                   
                }

                i_GameManager.EraseMoveFromGameBoard(i);
                if (result)
                {
                    break;
                }
            }
        }        

        return result;
    }
}