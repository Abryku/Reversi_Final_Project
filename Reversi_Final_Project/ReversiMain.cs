using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using Reversi_Final_Project.Réseau;

namespace Reversi_Final_Project
{
    public partial class ReversiMain : Form
    {
        private NetworkUtilities networkUtilities;

        private Board board;
        private readonly SquareControl[,] squareControls;

        private readonly string nomFic = "sauvegarde";
        private readonly string nomFicColor = "sauvColor";

        // Game parameters.
        private Coin currentColor;

        private OnlineFlag OnlineFlag = OnlineFlag.Offline;

        public ReversiMain()
        {
            InitializeComponent();

            mcSocket_Ecouter.Enabled = mcSocket_Connecter.Enabled = true;
            mcSocket_deconnecter.Enabled = false;

            // Create the game board.
            board = new Board();

            // Create the controls for each square, add them to the Squares
            // panel and set up event handling.
            squareControls = new SquareControl[8, 8];
            int i, j;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    // Create it.
                    squareControls[i, j] = new SquareControl(i, j);
                    // Position it.
                    squareControls[i, j].Left = j * squareControls[i, j].Width;
                    squareControls[i, j].Top = i * squareControls[i, j].Height;
                    // Add it.
                    Board1.Controls.Add(squareControls[i, j]);
                    // Set up event handling for it.
                    squareControls[i, j].MouseMove += SquareControl_MouseMove;
                    squareControls[i, j].MouseLeave += SquareControl_MouseLeave;
                    squareControls[i, j].Click += SquareControl_Click;
                }
            }
        }

        private async void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await StartGameAsync();
        }

        private async Task StartGameAsync()
        {
            // Initialize the board.
            board.SetForNewGame();
            UpdateBoardDisplay();

            // Set the first player.
            currentColor = Coin.White;

            await networkUtilities.InitializeServerAsync(board);
            
            // Start the first turn.
            StartTurn();
        }

        private async Task EnAttenteDeDonneesAsync()
        {
            board = await networkUtilities.ReceiveAsync();
            if(board == null) return;

            lbEchanges.Items.Insert(0, "Données recues");
            
            UpdateBoardDisplay();
            Focus();
            StartTurn();
        }

        private void StartTurn()
        {
            if (networkUtilities.IsClient)
            {
                currentColor = Coin.White;
            }
            else
            {
                currentColor = Coin.Black;
            }
            
            if (!board.HasAnyValidMove(currentColor))
            {
                MessageBox.Show("No Valid move");
                //currentColor *= -1;
                if (!board.HasAnyValidMove(currentColor))
                {
                    EndGame();
                    return;
                }
            }

            Focus();
            HighlightValidMoves(currentColor);
            Board1.Enabled = true;
            Board1.Refresh();
        }

        private void EndGame()
        {
            // Update the status message.
            if (board.BlackCount > board.WhiteCount)
                MessageBox.Show("Black wins.");
            else if (board.WhiteCount > board.BlackCount)
                MessageBox.Show("White wins.");
            else
                MessageBox.Show("Draw.");

        }

        private async Task MakeMove(int row, int col)
        {
            // Make the move on the board.
            board.MakeMove(currentColor, row, col);

            // Notice every disc that is going to be flipped
            board.IsGoingToBeFlipped(row, col);

            // Update the display to reflect the board changes.
            UpdateBoardDisplay();

            await EndMoveAsync();
        }


        // Switch players and start the next turn.
        private async Task EndMoveAsync()
        {
            SetColor();

            await networkUtilities.SendAsync(board);

            StopTurn();
            
            UpdateBoardDisplay();
            
            await EnAttenteDeDonneesAsync();
        }

        private void StopTurn()
        {
            Board1.Enabled = false;
        }

        private void SetColor()
        {
            if (currentColor == Coin.Black)
            {
                lbEchanges.Items.Insert(0, "Black has played.");
                lbEchanges.Items.Insert(0,"It's White's turn.");
            }
            else
            {
                lbEchanges.Items.Insert(0, "White has played.");
                lbEchanges.Items.Insert(0,"It's Black's turn.");
            }
        }
        
        //
        // Updates the display to reflect the current game board.
        //
        private void UpdateBoardDisplay()
        {
            // Map the current game board to the square controls.
           SquareControl squareControl;
           int i, j;
           for (i = 0; i < 8; i++)
           {
               for (j = 0; j < 8; j++)
               {
                   squareControl = (SquareControl)Board1.Controls[i * 8 + j];
                   squareControl.Contents = board.GetSquareContents(i, j);
                   squareControl.PreviewContents = (int) Coin.Empty;
               }
           }

           // Redraw the board.
           Board1.Refresh();
        }
        
        private async void SquareControl_Click(object sender, EventArgs e)
        {
            SquareControl squareControl = (SquareControl)sender;

            // If the move is valid, make it.
            if (board.IsValidMove(squareControl.Row, squareControl.Col, currentColor))
            {
                // Restore the cursor.
                squareControl.Cursor = Cursors.Default;

                // Make the move.
                await MakeMove(squareControl.Row, squareControl.Col);
            }
        }
        
        private void SquareControl_MouseMove(object sender, MouseEventArgs e)
        {
            SquareControl squareControl = (SquareControl)sender;

            // If the square is a valid move for the current player,
            // indicate it.
            if (board.IsValidMove(squareControl.Row, squareControl.Col, currentColor))
            {
                // Change the cursor.
                squareControl.Cursor = Cursors.Hand;
            }
        }
        
        private void SquareControl_MouseLeave(object sender, EventArgs e)
        {
            SquareControl squareControl = (SquareControl)sender;

            // If the move is being previewed, clear all affected Squares.

            if (squareControl.PreviewContents != (int) Coin.Empty)
            {
                // Clear the move preview.
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (squareControls[i, j].PreviewContents != (int) Coin.Empty)
                        {
                            squareControls[i, j].PreviewContents = (int) Coin.Empty;
                            squareControls[i, j].Refresh();
                        }
                    }
                }
            }

            // Restore the cursor.
            squareControl.Cursor = Cursors.Default;
        }
        
        private void HighlightValidMoves(Coin color)
        {
            // Check each square.
            SquareControl squareControl;
            int i, j;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    squareControl = (SquareControl)Board1.Controls[i * 8 + j];
                    
                    squareControl.IsValid = board.IsValidMove(i, j, color);
                }
            }
        }
        
        private void sauvegarderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Serialise.Sauve(nomFic, board);
            Serialise.Sauve(nomFicColor, currentColor);
        }
        
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Object obj = Serialise.Recup(nomFic);
            if(obj != null)
            {
                board = (Board)obj;
                UpdateBoardDisplay();
            }
            Object objColor = Serialise.Recup(nomFicColor);
            if(objColor != null)
            {
                currentColor = (Coin) objColor;
                StartTurn();
            }
        }

        private async void mcSocket_Ecouter_Click(object sender, EventArgs e)
        {
            mcSocket_Ecouter.Enabled = mcSocket_Connecter.Enabled = false;
            mcSocket_deconnecter.Enabled = true;
            
            networkUtilities = new NetworkUtilities();
            await networkUtilities.StartServerAsync();
        }
        
        private async void mcSocket_Connecter_Click(object sender, EventArgs e)
        {
            mcSocket_Ecouter.Enabled = mcSocket_Connecter.Enabled = false;
            mcSocket_deconnecter.Enabled = true;

            networkUtilities = new NetworkUtilities();
            board = await networkUtilities.StartClientAsync();
            
            UpdateBoardDisplay();
            Focus();
            await EnAttenteDeDonneesAsync();
        }
    }
}
