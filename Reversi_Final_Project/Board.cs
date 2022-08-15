using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Reversi_Final_Project
{
    [Serializable()]
    class Board 
    {
        // These constants represent the contents of a board square.
        public static readonly int Black = -1;
        public static readonly int White = 1;
        public static readonly int Empty = 0;

        // This two-dimensional array represents the squares on the board.
        private int[,] squares;

        // This two-dimensional array tracks which discs are safe (i.e.,
        // discs that cannot be outflanked in any direction).
        private bool[,] safeDiscs;

        // These counts reflect the current board situation.
        public int BlackCount
        {
            get { return this.blackCount; }
        }
        public int WhiteCount
        {
            get { return this.whiteCount; }
        }
        public int EmptyCount
        {
            get { return this.emptyCount; }
        }
        public int BlackFrontierCount
        {
            get { return this.blackFrontierCount; }
        }
        public int WhiteFrontierCount
        {
            get { return this.whiteFrontierCount; }
        }
        public int BlackSafeCount
        {
            get { return this.blackSafeCount; }
        }
        public int WhiteSafeCount
        {
            get { return this.whiteSafeCount; }
        }

        //Counts
        private int blackCount;
        private int whiteCount;
        private int emptyCount;
        private int blackFrontierCount;
        private int whiteFrontierCount;
        private int blackSafeCount;
        private int whiteSafeCount;

        public Board()
        {
            // Create the squares and safe disc map.
            this.squares = new int[8, 8];
            this.safeDiscs = new bool[8, 8];

            int i, j;

            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    this.squares[i, j] = Board.Empty;
                    this.safeDiscs[i, j] = false;
                }
            }

            // Update the counts.
            this.UpdateCounts();
        }

        public void SetForNewGame()
        {
            // Clear the board.
            int i, j;
            for (i = 0; i < 8; i++)
            for (j = 0; j < 8; j++)
            {
                this.squares[i, j] = Board.Empty;
                this.safeDiscs[i, j] = false;
            }

            // Set two black and two white discs in the center.
            this.squares[3, 3] = White;
            this.squares[3, 4] = Black;
            this.squares[4, 3] = Black;
            this.squares[4, 4] = White;

            // Update the counts.
            this.UpdateCounts();
        }
        public int GetSquareContents(int row, int col)
        {
            return this.squares[row, col];
        }

        public void MakeMove(int color, int row, int col)
		{
			// Set the disc on the square.
			this.squares[row, col] = color;

			// Update the counts.
			this.UpdateCounts();
		}

        public void IsGoingToBeFlipped(int row, int col)
        {
            int dr, dc;
            int r, c, i;

            for (dr = -1; dr<=1;dr++)
            {
                for (dc = -1; dc <= 1; dc++)
                {
                    r = dr;
                    c = dc;
                    i = 0;

                    if ((row + dr) >= 0 && (row + dr) <= 7 && (col + dc) >= 0 && (col + dc) <= 7)
                    {
                        if (this.squares[row, col] == White)
                        {
                            if (this.squares[row + dr, col + dc] == Black)
                            {
                                
                                while (((row + r) >= 0) && ((row + r) <= 7) && ((col + c) <= 7) && ((col + c) >= 0) && (i==0))
                                {
                                    if(this.squares[row + r, col + c] == Black)
                                    {
                                        r = r + dr;
                                        c = c + dc;
                                    }
                                    else
                                    {
                                        i = 1;
                                    }
                                }
                                if(((row + r) >= 0) && ((row + r) <= 7) && ((col + c) <= 7) && ((col + c) >= 0))
                                {
                                    if (this.squares[row + r, col + c] == White)
                                    {
                                        int rbis = dr;
                                        int cbis = dc;
                                        while (this.squares[row + rbis, col + cbis] == Black)
                                        {
                                            this.squares[row + rbis, col + cbis] = White;
                                            rbis = rbis + dr;
                                            cbis = cbis + dc;
                                        }
                                    }

                                }
                                
                            }
                        }
                        else
                        {
                            if (this.squares[row + dr, col + dc] == White)
                            {

                                while (((row + r) >= 0) && ((row + r) <= 7) && ((col + c) <= 7) && ((col + c) >= 0) && (i == 0))
                                {
                                    if (this.squares[row + r, col + c] == White)
                                    {
                                        r = r + dr;
                                        c = c + dc;
                                    }
                                    else
                                    {
                                        i = 1;
                                    }
                                }
                                if (((row + r) >= 0) && ((row + r) <= 7) && ((col + c) <= 7) && ((col + c) >= 0))
                                {
                                    if (this.squares[row + r, col + c] == Black)
                                    {
                                        int rbis = dr;
                                        int cbis = dc;
                                        while (this.squares[row + rbis, col + cbis] == White)
                                        {
                                            this.squares[row + rbis, col + cbis] = Black;
                                            rbis = rbis + dr;
                                            cbis = cbis + dc;
                                        }
                                    }

                                }

                            }
                        }

                    }
                    
                    
                }
            }
        }

        // Check all board positions for a valid move.

        public bool HasAnyValidMove(int color)
        {
            // Check all board positions for a valid move.
            int r, c;
            for (r = 0; r < 8; r++)
            for (c = 0; c < 8; c++)
                if (this.IsValidMove(r, c, color))
                    return true;

            // None found.
            return false;
        }
        public bool IsValidMove(int row, int col, int color)
        {
            // The square must be empty.
            if (this.squares[row, col] != Board.Empty)
                return false;

            int direction_Row, direction_Col;
            for (direction_Row = -1; direction_Row <= 1; direction_Row++)
            {
                for (direction_Col = -1; direction_Col <= 1; direction_Col++)
                {
                    if (!(direction_Row == 0 && direction_Col == 0) && this.IsOutFlanking(row, col, direction_Row, direction_Col, color))
                        return true;
                }
            }
            
            return false;
        }
        public int GetValidMoveCount(int color)
        {
            int n = 0;

            // Check all board positions.
            int i, j;
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                    // If the move is valid for the color, bump the count.
                    if (this.IsValidMove(i, j, color))
                        n++;
            return n;
        }

        private bool IsOutFlanking(int row, int col, int direction_Row, int direction_Col, int color)
        {
            // Move in the given direction as long as we stay on the board and
            // land on a disc of the opposite color.
            int r = row + direction_Row;
            int c = col + direction_Col;
            while (r >= 0 && r < 8 && c >= 0 && c < 8 && this.squares[r, c] == -color)
            {
                r += direction_Row;
                c += direction_Col;
            }

            // If we ran off the board, only moved one space or didn't land on
            // a disc of the same color, return false.
            if (r < 0 || r > 7 || c < 0 || c > 7 || (r - direction_Row == row && c - direction_Col == col) || this.squares[r, c] != color)
                return false;

            // Otherwise, return true;
            return true;
        }

        private bool IsOutFlankable(int row, int col)
        {
            int color = this.squares[row, col]; //Get the color of the disc

            //Condition to be outflankable
            int i, j;
            bool LeftIsEmpty = false;
            bool RightIsEmpty = false;
            bool LeftIsUnsafe = false;
            bool RightIsUnsafe = false;

            //Horizontal
            //Lets check the left side
            for (j = 0; j < col && !LeftIsEmpty; j++)
            {
                if (this.squares[row, j] == Board.Empty)
                    LeftIsEmpty = true;
                if (this.squares[row, j] != color || !this.safeDiscs[row, j])
                    LeftIsUnsafe = true;
            }

            //Lets check the right side
            for (j = col + 1; j < 8 && !RightIsEmpty; j++)
            {
                if (this.squares[row, j] == Board.Empty)
                    RightIsEmpty = true;
                if (this.squares[row, j] != color || !this.safeDiscs[row, j])
                    RightIsUnsafe = true;
            }

            if ((LeftIsUnsafe && RightIsEmpty) ||
                (LeftIsEmpty && RightIsUnsafe) ||
                (LeftIsEmpty && RightIsEmpty))
                return true;

            //Vertical
            LeftIsEmpty = false;
            RightIsEmpty = false;
            LeftIsUnsafe = false;
            RightIsUnsafe = false;
            //Lets check top side
            for (i = 0; i < row && !LeftIsEmpty; i++)
            {
                if (this.squares[i, col] == Board.Empty)
                    LeftIsEmpty = true;
                if (this.squares[i, col] != color || !this.safeDiscs[i, col])
                    LeftIsUnsafe = true;
            }

            //Lets check bot side
            for (i = row + 1; i < 8 && !this.safeDiscs[i, col]; i++)
            {
                if (this.squares[i, col] == Board.Empty)
                    RightIsEmpty = true;
                if (this.squares[i, col] != color || !this.safeDiscs[i, col])
                    RightIsUnsafe = true;
            }

            if ((LeftIsEmpty && RightIsEmpty) ||
                (LeftIsUnsafe && RightIsEmpty) ||
                (LeftIsEmpty && RightIsUnsafe))
                return true;

            // Check the Northwest-Southeast diagonal line through the disc.
            LeftIsEmpty = false;
            RightIsEmpty = false;
            LeftIsUnsafe = false;
            RightIsUnsafe = false;
            // Northwest side.
            i = row - 1;
            j = col - 1;
            while (i >= 0 && j >= 0 && !LeftIsEmpty)
            {
                if (this.squares[i, j] == Board.Empty)
                    LeftIsEmpty = true;
                else if (this.squares[i, j] != color || !this.safeDiscs[i, j])
                    LeftIsUnsafe = true;
                i--;
                j--;
            }
            // Southeast side.
            i = row + 1;
            j = col + 1;
            while (i < 8 && j < 8 && !RightIsEmpty)
            {
                if (this.squares[i, j] == Board.Empty)
                    RightIsEmpty = true;
                else if (this.squares[i, j] != color || !this.safeDiscs[i, j])
                    RightIsUnsafe = true;
                i++;
                j++;
            }
            if ((LeftIsEmpty && RightIsEmpty) ||
                (LeftIsEmpty && RightIsUnsafe) ||
                (LeftIsUnsafe && RightIsEmpty))
                return true;

            // Check the Northeast-Southwest diagonal line through the disc.
            LeftIsEmpty = false;
            RightIsEmpty = false;
            LeftIsUnsafe = false;
            RightIsUnsafe = false;
            // Northeast side.
            i = row - 1;
            j = col + 1;
            while (i >= 0 && j < 8 && !LeftIsEmpty)
            {
                if (this.squares[i, j] == Board.Empty)
                    LeftIsEmpty = true;
                if (this.squares[i, j] != color || !this.safeDiscs[i, j])
                    LeftIsUnsafe = true;
                i--;
                j++;
            }
            // Southwest side.
            i = row + 1;
            j = col - 1;
            while (i < 8 && j >= 0 && !RightIsEmpty)
            {
                if (this.squares[i, j] == Board.Empty)
                    RightIsEmpty = true;
                if (this.squares[i, j] != color || !this.safeDiscs[i, j])
                    RightIsUnsafe = true;
                i++;
                j--;
            }
            if ((LeftIsEmpty && RightIsEmpty) ||
                (LeftIsEmpty && RightIsUnsafe) ||
                (LeftIsUnsafe && RightIsEmpty))
                return true;

            //Every lines are safe
            return false;
        }
        private void UpdateCounts()
        {
            // Reset all counts.
            this.blackCount = 0;
            this.whiteCount = 0;
            this.emptyCount = 0;
            this.blackFrontierCount = 0;
            this.whiteFrontierCount = 0;
            this.whiteSafeCount = 0;
            this.blackSafeCount = 0;

            int i, j;

            // Update the safe disc map.
            //
            // All currently unsafe discs are checked to see if they are still
            // outflankable. Those that are not are marked as safe.
            // If any new safe discs were found, the process is repeated
            // because this change may have made other discs safe as well. The
            // loop exits when no new safe discs are found.
            bool statusChanged = true;
            while (statusChanged)
            {
                statusChanged = false;
                for (i = 0; i < 8; i++)
                    for (j = 0; j < 8; j++)
                        if (this.squares[i, j] != Board.Empty && !this.safeDiscs[i, j] && !this.IsOutFlankable(i, j))
                        {
                            this.safeDiscs[i, j] = true;
                            statusChanged = true;
                        }
            }

            // Tally the counts.
            int dr, dc;
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                {
                    // If there is a disc at this position, determine if it is
                    // on the frontier (i.e., adjacent to an empty square).
                    bool isFrontier = false;
                    if (this.squares[i, j] != Board.Empty)
                    {
                        for (dr = -1; dr <= 1; dr++)
                            for (dc = -1; dc <= 1; dc++)
                                if (!(dr == 0 && dc == 0) && i + dr >= 0 && i + dr < 8 && j + dc >= 0 && j + dc < 8 && this.squares[i + dr, j + dc] == Board.Empty)
                                    isFrontier = true;
                    }

                    // Update the counts.
                    if (this.squares[i, j] == Board.Black)
                    {
                        this.blackCount++;
                        if (isFrontier)
                            this.blackFrontierCount++;
                        if (this.safeDiscs[i, j])
                            this.blackSafeCount++;
                    }
                    else if (this.squares[i, j] == Board.White)
                    {
                        this.whiteCount++;
                        if (isFrontier)
                            this.whiteFrontierCount++;
                        if (this.safeDiscs[i, j])
                            this.whiteSafeCount++;
                    }
                    else
                        this.emptyCount++;
                }
        }

    }
}
