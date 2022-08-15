using System;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Reversi_Final_Project.Réseau;

namespace Reversi_Final_Project
{
    public partial class ReversiMain : Form
    {
        #region Network

        private ServerTcpController serverTcp;

        #endregion

        private Board board;
        private SquareControl[,] squareControls;

        private string nomFic = "sauvegarde";
        private string transferPanel = "Transfert Panel";
        private string nomFicColor = "sauvColor";
        private string transferColor = "Transfert Color";

        // Game parameters.
        private int currentColor;
        private string Color ="";

        private int OnlineFlag = 0; // 0 if Offline, 1 if Online

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

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private async void StartGame()
        {
            // Initialize the board.
            board.SetForNewGame();
            UpdateBoardDisplay();

            // Set the first player.
            currentColor = -1;

            // Start the first turn.
            StartTurn();

            if(serverTcp != null)
                await InitializeClientAsync();
        }

        private async Task InitializeClientAsync()
        {
            DataToExchange dataToExchange = new DataToExchange
            {
                Command = Command.Initialization,
                Board = board
            };
            
            await serverTcp.SendAsync(dataToExchange);
        }

        private void StartTurn()
        {
            if (!board.HasAnyValidMove(currentColor))
            {
                MessageBox.Show("No Valid move");
                currentColor *= -1;
                if (!board.HasAnyValidMove(currentColor))
                {

                    EndGame();
                    return;
                }
            }

            Focus();
            HighlightValidMoves(currentColor);

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

        private void MakeMove(int row, int col)
        {
            // Make the move on the board.
            board.MakeMove(currentColor, row, col);

            // Notice every disc that is going to be flipped
            board.IsGoingToBeFlipped(row, col);

            // Update the display to reflect the board changes.
            UpdateBoardDisplay();

            EndMove();
        }


        private void EndMove()
        {
            // Switch players and start the next turn.

            if (currentColor == -1)
                Color = "Black";
            else
                Color = "White";
            lbEchanges.Items.Insert(0, Color.ToString() + " has played.");
            currentColor *= -1;
            if (currentColor == -1)
                Color = "Black";
            else
                Color = "White";
            lbEchanges.Items.Insert(0,"It's " + Color.ToString() + "'s turn.");
            StartTurn();

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
                   squareControl.PreviewContents = Board.Empty;
               }
           }

           // Redraw the board.
           Board1.Refresh();
            
        }
        private void SquareControl_Click(object sender, EventArgs e)
        {
            SquareControl squareControl = (SquareControl)sender;

            // If the move is valid, make it.
            if (board.IsValidMove(squareControl.Row, squareControl.Col, currentColor))
            {
                // Restore the cursor.
                squareControl.Cursor = Cursors.Default;

                // Make the move.
                MakeMove(squareControl.Row, squareControl.Col);
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

            if (squareControl.PreviewContents != Board.Empty)
            {
                // Clear the move preview.
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (squareControls[i, j].PreviewContents != Board.Empty)
                        {
                            squareControls[i, j].PreviewContents = Board.Empty;
                            squareControls[i, j].Refresh();
                        }
            }

            // Restore the cursor.
            squareControl.Cursor = Cursors.Default;
        }
        private void HighlightValidMoves(int color)
        {
            // Check each square.
            SquareControl squareControl;
            int i, j;
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                {
                    squareControl = (SquareControl)Board1.Controls[i * 8 + j];
                    if (board.IsValidMove(i, j, color))
                        squareControl.IsValid = true;
                    else
                        squareControl.IsValid = false;
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
                currentColor = (int)objColor;
                StartTurn();
            }
        }
        
        private static IPAddress GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }
        
        private async void mcSocket_Ecouter_Click(object sender, EventArgs e)
        {
            mcSocket_Ecouter.Enabled = mcSocket_Connecter.Enabled = false;
            mcSocket_deconnecter.Enabled = true;

            serverTcp = new ServerTcpController();
            
            bool res = await serverTcp.ListenAsync(new IPEndPoint(GetLocalIpAddress(), 1010));
            Console.WriteLine(res);
        }
        
        private async void mcSocket_Connecter_Click(object sender, EventArgs e)
        {
            mcSocket_Ecouter.Enabled = mcSocket_Connecter.Enabled = false;
            mcSocket_deconnecter.Enabled = true;
            
            // démarre le client tcp
            ClientTcpController client = new ClientTcpController();
            await client.ConnectAsync(new IPEndPoint(GetLocalIpAddress(), 1010));
            DataToExchange dataToExchange = await client.ReceiveAsync<DataToExchange>();

            board = dataToExchange.Board;
            UpdateBoardDisplay();
        }
    }
}
