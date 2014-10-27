using System;
using System.Text;

public class Board
{
    // $G$ NTT-999 (-3) This Array should be readonly.
    private eBoardCellTypes[,] m_GameBoard;
    private int[] m_FirstAvailablePoisitionInCol;
    private int m_NumOfOccupiedCells;
    private int m_NumOfTotalCells;
    private int m_ColSize;
    private int m_RowSize;

    public static char GetCharOfCell(eBoardCellTypes cellType)
    {
        char result;

        if (cellType == eBoardCellTypes.Circle)
        {
            result = 'O';
        }
        else if (cellType == eBoardCellTypes.X)
        {
            result = 'X';
        }
        else
        {
            result = ' ';
        }

        return result;
    }

    public Board(int i_RowSize, int i_ColSize)
    {
        m_RowSize = i_RowSize;
        m_ColSize = i_ColSize;
        m_GameBoard = new eBoardCellTypes[i_RowSize, i_ColSize];
        m_NumOfOccupiedCells = 0;
        m_NumOfTotalCells = i_RowSize * i_ColSize;

        for (int row = 0; row < i_RowSize; row++)
        {
            for (int col = 0; col < i_ColSize; col++)
            {
                m_GameBoard[row, col] = eBoardCellTypes.Empty;
            }
        }

        m_FirstAvailablePoisitionInCol = new int[i_ColSize];
        for (int i = 0; i < i_ColSize; i++)
        {
            m_FirstAvailablePoisitionInCol[i] = 0;
        }       
    }    

    public enum eBoardCellTypes
    {
        Circle,
        Empty,        
        X,                
    }

    public enum eBoardDirections
    {
        Horizontal,
        Vertical,        
        RightCross,
        LeftCross,
    }

    public eBoardCellTypes GetCellEnum(int i_Row, int i_Col)
    {
        return this.m_GameBoard[i_Row, i_Col];
    }

    public eBoardCellTypes[,] GameBoard
    {
        get { return m_GameBoard; }
    }
    
    public int RowSize
    {
        get { return m_RowSize; }
    }

    public int ColSize
    {
        get { return m_ColSize; }
    }

    public int[] FirstAvailablePoisitionInCol
    {
        get { return m_FirstAvailablePoisitionInCol; }
    }
             
    public void IncreaseOccupiedCells()
    {
        this.m_NumOfOccupiedCells++;
    }

    public int NumOfOccupiedCells 
    {
        get { return m_NumOfOccupiedCells; }
    }

    public int NumOfTotalCells
    {
        get { return m_NumOfTotalCells; }
    }
    
    public bool isBoardFull()
    {
        bool result;

        result = this.m_NumOfOccupiedCells == this.m_NumOfTotalCells;

        return result;
    }

    public bool CheckIsInsideBoardBoundaries(int i_Row, int i_Col)
    {
        bool result;

        result = !((i_Row < 0) || (i_Row > m_RowSize - 1) || (i_Col < 0) || (i_Col > m_ColSize - 1));
        
        return result;
    }        
}