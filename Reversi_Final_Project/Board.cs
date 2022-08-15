using System;

namespace Reversi_Final_Project
{
	[Serializable]
	public class Board
	{
		// This two-dimensional array represents the Squares on the board.
		public int[,] Squares { get; set; }

		// This two-dimensional array tracks which discs are safe (i.e.,
		// discs that cannot be outflanked in any direction).
		private bool[,] safeDiscs;

		// These counts reflect the current board situation.
		public int BlackCount => blackCount;

		public int WhiteCount => whiteCount;

		//Counts
		private int blackCount;
		private int whiteCount;

		public Board()
		{
			// Create the Squares and safe disc map.
			Squares = new int[8, 8];
			safeDiscs = new bool[8, 8];

			int i, j;

			for (i = 0; i < 8; i++)
			{
				for (j = 0; j < 8; j++)
				{
					Squares[i, j] = (int)Coin.Empty;
					safeDiscs[i, j] = false;
				}
			}

			// Update the counts.
			UpdateCounts();
		}

		public void SetForNewGame()
		{
			// Clear the board.
			int i, j;
			for (i = 0; i < 8; i++)
			{
				for (j = 0; j < 8; j++)
				{
					Squares[i, j] = (int)Coin.Empty;
					safeDiscs[i, j] = false;
				}
			}

			// Set two black and two white discs in the center.
			Squares[3, 3] = (int)Coin.White;
			Squares[3, 4] = (int)Coin.Black;
			Squares[4, 3] = (int)Coin.Black;
			Squares[4, 4] = (int)Coin.White;

			// Update the counts.
			UpdateCounts();
		}

		public int GetSquareContents(int row, int col)
		{
			return Squares[row, col];
		}

		public void MakeMove(Coin color, int row, int col)
		{
			// Set the disc on the square.
			Squares[row, col] = (int)color;

			// Update the counts.
			UpdateCounts();
		}

		public void IsGoingToBeFlipped(int row, int col)
		{
			int dr, dc;
			int r, c, i;

			for (dr = -1; dr <= 1; dr++)
			{
				for (dc = -1; dc <= 1; dc++)
				{
					r = dr;
					c = dc;
					i = 0;

					if ((row + dr) >= 0 && (row + dr) <= 7 && (col + dc) >= 0 && (col + dc) <= 7)
					{
						if (Squares[row, col] == (int)Coin.White)
						{
							if (Squares[row + dr, col + dc] == (int)Coin.Black)
							{
								while (((row + r) >= 0) && ((row + r) <= 7) && ((col + c) <= 7) && ((col + c) >= 0) &&
								       (i == 0))
								{
									if (Squares[row + r, col + c] == (int)Coin.Black)
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
									if (Squares[row + r, col + c] == (int)Coin.White)
									{
										int rbis = dr;
										int cbis = dc;
										while (Squares[row + rbis, col + cbis] == (int)Coin.Black)
										{
											Squares[row + rbis, col + cbis] = (int)Coin.White;
											rbis = rbis + dr;
											cbis = cbis + dc;
										}
									}
								}
							}
						}
						else
						{
							if (Squares[row + dr, col + dc] == (int)Coin.White)
							{
								while (((row + r) >= 0) && ((row + r) <= 7) && ((col + c) <= 7) && ((col + c) >= 0) &&
								       (i == 0))
								{
									if (Squares[row + r, col + c] == (int)Coin.White)
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
									if (Squares[row + r, col + c] == (int)Coin.Black)
									{
										int rbis = dr;
										int cbis = dc;
										while (Squares[row + rbis, col + cbis] == (int)Coin.White)
										{
											Squares[row + rbis, col + cbis] = (int)Coin.Black;
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

		public bool HasAnyValidMove(Coin color)
		{
			// Check all board positions for a valid move.
			int r, c;
			for (r = 0; r < 8; r++)
			{
				for (c = 0; c < 8; c++)
				{
					if (IsValidMove(r, c, color))
						return true;
				}
			}

			// None found.
			return false;
		}

		public bool IsValidMove(int row, int col, Coin color)
		{
			// The square must be empty.
			if (Squares[row, col] != (int)Coin.Empty)
				return false;

			int direction_Row, direction_Col;
			for (direction_Row = -1; direction_Row <= 1; direction_Row++)
			{
				for (direction_Col = -1; direction_Col <= 1; direction_Col++)
				{
					if (!(direction_Row == 0 && direction_Col == 0) &&
					    IsOutFlanking(row, col, direction_Row, direction_Col, color))
						return true;
				}
			}

			return false;
		}

		public int GetValidMoveCount(Coin color)
		{
			int n = 0;

			// Check all board positions.
			int i, j;
			for (i = 0; i < 8; i++)
			{
				for (j = 0; j < 8; j++)
				{
					// If the move is valid for the color, bump the count.
					if (IsValidMove(i, j, color))
						n++;
				}
			}

			return n;
		}

		private bool IsOutFlanking(int row, int col, int direction_Row, int direction_Col, Coin color)
		{
			// Move in the given direction as long as we stay on the board and
			// land on a disc of the opposite color.
			int r = row + direction_Row;
			int c = col + direction_Col;
			while (r >= 0 && r < 8 && c >= 0 && c < 8 && Squares[r, c] == -(int)color)
			{
				r += direction_Row;
				c += direction_Col;
			}

			// If we ran off the board, only moved one space or didn't land on
			// a disc of the same color, return false.
			if (r < 0 || r > 7 || c < 0 || c > 7 || (r - direction_Row == row && c - direction_Col == col) ||
			    Squares[r, c] != (int)color)
				return false;

			return true;
		}

		private bool IsOutFlankable(int row, int col)
		{
			int color = Squares[row, col]; //Get the color of the disc

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
				if (Squares[row, j] == (int) Coin.Empty)
					LeftIsEmpty = true;
				if (Squares[row, j] != color || !safeDiscs[row, j])
					LeftIsUnsafe = true;
			}

			//Lets check the right side
			for (j = col + 1; j < 8 && !RightIsEmpty; j++)
			{
				if (Squares[row, j] == (int) Coin.Empty)
					RightIsEmpty = true;
				if (Squares[row, j] != color || !safeDiscs[row, j])
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
				if (Squares[i, col] == (int) Coin.Empty)
					LeftIsEmpty = true;
				if (Squares[i, col] != color || !safeDiscs[i, col])
					LeftIsUnsafe = true;
			}

			//Lets check bot side
			for (i = row + 1; i < 8 && !safeDiscs[i, col]; i++)
			{
				if (Squares[i, col] == (int) Coin.Empty)
					RightIsEmpty = true;
				if (Squares[i, col] != color || !safeDiscs[i, col])
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
				if (Squares[i, j] == (int) Coin.Empty)
					LeftIsEmpty = true;
				else if (Squares[i, j] != color || !safeDiscs[i, j])
					LeftIsUnsafe = true;
				i--;
				j--;
			}

			// Southeast side.
			i = row + 1;
			j = col + 1;
			while (i < 8 && j < 8 && !RightIsEmpty)
			{
				if (Squares[i, j] == (int) Coin.Empty)
					RightIsEmpty = true;
				else if (Squares[i, j] != color || !safeDiscs[i, j])
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
				if (Squares[i, j] == (int) Coin.Empty)
					LeftIsEmpty = true;
				if (Squares[i, j] != color || !safeDiscs[i, j])
					LeftIsUnsafe = true;
				i--;
				j++;
			}

			// Southwest side.
			i = row + 1;
			j = col - 1;
			while (i < 8 && j >= 0 && !RightIsEmpty)
			{
				if (Squares[i, j] == (int) Coin.Empty)
					RightIsEmpty = true;
				if (Squares[i, j] != color || !safeDiscs[i, j])
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
			blackCount = 0;
			whiteCount = 0;

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
					if (Squares[i, j] != (int) Coin.Empty && !safeDiscs[i, j] && !IsOutFlankable(i, j))
					{
						safeDiscs[i, j] = true;
						statusChanged = true;
					}
			}

			// Tally the counts.
			for (i = 0; i < 8; i++)
			for (j = 0; j < 8; j++)
			{
				// Update the counts.
				if (Squares[i, j] == (int) Coin.Black)
				{
					blackCount++;
				}
				else if (Squares[i, j] == (int) Coin.White)
				{
					whiteCount++;
				}
			}
		}
	}
}