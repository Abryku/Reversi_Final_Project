using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Reversi_Final_Project
{
    public partial class ReversiMain : Form
    {
        #region Mode Debug
        delegate void RenvoiVersInserer(string sTexte);
        private void SynchroPanelThread(Board bPanel)
        {
            Thread ThreadSynchroPanel = new Thread(new ParameterizedThreadStart(SynchroPanel));
            ThreadSynchroPanel.Start(bPanel);
        }
        private void SynchroPanel(object oPanel)
        {
            Object obj = Serialise.Recup(transferPanel);
            if (obj != null)
            {
                board = (Board)obj;
                UpdateBoardDisplay();
            }
            Object objColor = Serialise.Recup(transferColor);
            if (objColor != null)
            {
                currentColor = (int)objColor;
            }
        }
        private void InsererItemThread(string sTexte)
        {
            Thread ThreadInsererItem = new Thread(new ParameterizedThreadStart(InsererItem));
            ThreadInsererItem.Start(sTexte);
        }
        private void InsererItem(object oTexte)
        {
            if (lbEchanges.InvokeRequired)
            {
                RenvoiVersInserer f = new RenvoiVersInserer(InsererItem);
                Invoke(f, new object[] { (string)oTexte });
            }
            else
                lbEchanges.Items.Insert(0, (string)oTexte); // A MODIFIER
        }
        #endregion

        private Socket Serveur, Client;
        private int DrapeauSocket = 0;
        private byte[] Buffer = new byte[256];

        private Board board;
        private SquareControl[,] squareControls;

        private string nomFic = "sauvegarde";
        private string transferPanel = "Transfert Panel";
        private string nomFicColor = "sauvColor";
        private string transferColor = "Transfert Color";

        // Game parameters.
        private int currentColor;
        private string Color ="";
        private int moveNumber;

        // Used to track which player made the last move.
        private int lastMoveColor;

        private int OnlineFlag = 0; // 0 if Offline, 1 if Online

        public ReversiMain()
        {
            InitializeComponent();

            mcSocket_Ecouter.Enabled = mcSocket_Connecter.Enabled = true;
            mcSocket_deconnecter.Enabled = false;

            // Create the game board.
            this.board = new Board();

            // Create the controls for each square, add them to the squares
            // panel and set up event handling.
            this.squareControls = new SquareControl[8, 8];
            int i, j;
            for (i = 0; i < 8; i++)
            {
                for (j = 0; j < 8; j++)
                {
                    // Create it.
                    this.squareControls[i, j] = new SquareControl(i, j);
                    // Position it.
                    this.squareControls[i, j].Left = j * this.squareControls[i, j].Width;
                    this.squareControls[i, j].Top = i * this.squareControls[i, j].Height;
                    // Add it.
                    this.Board1.Controls.Add(this.squareControls[i, j]);
                    // Set up event handling for it.
                    this.squareControls[i, j].MouseMove += new MouseEventHandler(this.SquareControl_MouseMove);
                    this.squareControls[i, j].MouseLeave += new EventHandler(this.SquareControl_MouseLeave);
                    this.squareControls[i, j].Click += new EventHandler(this.SquareControl_Click);
                }
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            this.moveNumber = 1;

            // Initialize the board.
            this.board.SetForNewGame();
            this.UpdateBoardDisplay();

            // Initialize the last move color.
            this.lastMoveColor = Board.Empty;

            // Set the first player.
            this.currentColor = -1;

            // Start the first turn.
            this.StartTurn();

            if (OnlineFlag == 1 && DrapeauSocket == 1)
            {
                Serialise.Sauve(transferPanel, board);
                Serialise.Sauve(transferColor, currentColor);
                Client.SendFile(transferPanel);
                Client.SendFile(transferColor);
            }
        }

        private void StartTurn()
        {
            if (!this.board.HasAnyValidMove(this.currentColor))
            {
                MessageBox.Show("No Valid move");
                this.currentColor *= -1;
                if (!this.board.HasAnyValidMove(this.currentColor))
                {

                    EndGame();
                    return;
                }
            }

            if (this.currentColor == 1)
            {
                // Set focus on the form so it will receive key presses.
                this.Focus();
            }
            else
            {
                // Set focus on the form so it will receive key presses.
                this.Focus();
            }
            this.HighlightValidMoves(this.currentColor);

            this.Board1.Refresh();


        }

        private void EndGame()
        {
            // Update the status message.
            if (this.board.BlackCount > this.board.WhiteCount)
                MessageBox.Show("Black wins.");
            else if (this.board.WhiteCount > this.board.BlackCount)
                MessageBox.Show("White wins.");
            else
                MessageBox.Show("Draw.");

        }

        private void MakeMove(int row, int col)
        {
            // Bump the move number.
            this.moveNumber++;

            // Make the move on the board.
            this.board.MakeMove(this.currentColor, row, col);

            // Notice every disc that is going to be flipped
            this.board.IsGoingToBeFlipped(row, col);

            // Update the display to reflect the board changes.
            this.UpdateBoardDisplay();

            // Save the player color.
            this.lastMoveColor = this.currentColor;

            this.EndMove();
        }


        private void EndMove()
        {
            // Switch players and start the next turn.

            if (this.currentColor == -1)
                this.Color = "Black";
            else
                this.Color = "White";
            lbEchanges.Items.Insert(0, this.Color.ToString() + " has played.");
            this.currentColor *= -1;
            if (this.currentColor == -1)
                this.Color = "Black";
            else
                this.Color = "White";
            lbEchanges.Items.Insert(0,"It's " + this.Color.ToString() + "'s turn.");
            this.StartTurn();

        }
        //
        // Updates the display to reflect the current game board.
        //
        private void UpdateBoardDisplay()
        {
            if(DrapeauSocket != 2 )
            {
                // Map the current game board to the square controls.
               SquareControl squareControl;
               int i, j;
               for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                {
                    squareControl = (SquareControl)this.Board1.Controls[i * 8 + j];
                    squareControl.Contents = this.board.GetSquareContents(i, j);
                    squareControl.PreviewContents = Board.Empty;
                }
    
                 // Redraw the board.
                 this.Board1.Refresh();

            }
            
        }
        private void SquareControl_Click(object sender, System.EventArgs e)
        {
            SquareControl squareControl = (SquareControl)sender;

            // If the move is valid, make it.
            if (this.board.IsValidMove(squareControl.Row, squareControl.Col, this.currentColor))
            {
                // Restore the cursor.
                squareControl.Cursor = System.Windows.Forms.Cursors.Default;

                // Make the move.
                this.MakeMove(squareControl.Row, squareControl.Col);
            }
        }
        private void SquareControl_MouseMove(object sender, MouseEventArgs e)
        {
            SquareControl squareControl = (SquareControl)sender;

            // If the square is a valid move for the current player,
            // indicate it.
            if (this.board.IsValidMove(squareControl.Row, squareControl.Col, this.currentColor))
            {

                // Change the cursor.
                squareControl.Cursor = System.Windows.Forms.Cursors.Hand;
            }
        }
        private void SquareControl_MouseLeave(object sender, System.EventArgs e)
        {
            SquareControl squareControl = (SquareControl)sender;

            

            // If the move is being previewed, clear all affected squares.

            if (squareControl.PreviewContents != Board.Empty)
            {
                // Clear the move preview.
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (this.squareControls[i, j].PreviewContents != Board.Empty)
                        {
                            this.squareControls[i, j].PreviewContents = Board.Empty;
                            this.squareControls[i, j].Refresh();
                        }
            }

            // Restore the cursor.
            squareControl.Cursor = System.Windows.Forms.Cursors.Default;
        }
        private void HighlightValidMoves(int color)
        {
            // Check each square.
            SquareControl squareControl;
            int i, j;
            for (i = 0; i < 8; i++)
                for (j = 0; j < 8; j++)
                {
                    squareControl = (SquareControl)this.Board1.Controls[i * 8 + j];
                    if (this.board.IsValidMove(i, j, color))
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
        private void mcSocket_Ecouter_Click(object sender, EventArgs e)
        {
            mcSocket_Ecouter.Enabled = mcSocket_Connecter.Enabled = false;
            mcSocket_deconnecter.Enabled = true;
            DrapeauSocket = 1;
            Client = null;
            IPAddress IPServeur = AdresseValide(Dns.GetHostName());
            Serveur = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Serveur.Bind(new IPEndPoint(IPServeur, 8000));
            Serveur.Listen(1);
            Serveur.BeginAccept(new AsyncCallback(DemandeConnexion), Serveur);
        }
        public void DemandeConnexion(IAsyncResult iar)
        {
            if(DrapeauSocket == 1)
            {
                Socket tmp = (Socket)iar.AsyncState;
                Client = tmp.EndAccept(iar);
                Client.Send(Encoding.Unicode.GetBytes("Connexion accepté"));
                OnlineFlag = 1;
                InsererItemThread("Connexion effectuée par " + ((IPEndPoint)Client.RemoteEndPoint).Address.ToString());
                SynchroPanelThread(board);
                //lbEchanges.Items.Insert(0, "Connexion effectuée par " + ((IPEndPoint)Client.RemoteEndPoint).Address.ToString());
                InitialiserReception(Client);
            }
        }
        public void InitialiserReception(Socket soc)
        {
            soc.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(Reception), soc);
        }
        public void ReceptionPanel(IAsyncResult iar)
        {
            if (DrapeauSocket > 0)
            {
                Socket tmp = (Socket)iar.AsyncState;
                int recu = tmp.EndReceive(iar);
                if (recu > 0)
                {
                    Object obj = Serialise.Recup(transferPanel);
                    Board NewPanel = new Board();
                    NewPanel = (Board)obj;
                    SynchroPanelThread(NewPanel);
                    InitialiserReception(tmp);
                    Array.Clear(Buffer, 0, Buffer.Length);

                }
                else
                {
                    tmp.Disconnect(true);
                    tmp.Close();
                    Serveur.BeginAccept(new AsyncCallback(DemandeConnexion), Serveur);
                    Client = null;
                }
            }
        }

        public void Reception(IAsyncResult iar)
        {
            if (DrapeauSocket > 0)
            {
                Socket tmp = (Socket)iar.AsyncState;
                int recu = tmp.EndReceive(iar);
                if (recu > 0)
                {
                    InsererItemThread(Encoding.Unicode.GetString(Buffer));
                    //lbEchanges.Items.Insert(0, Encoding.Unicode.GetString(Buffer));
                    InitialiserReception(tmp);
                    Array.Clear(Buffer, 0, Buffer.Length);

                }
                else
                {
                    tmp.Disconnect(true);
                    tmp.Close();
                    Serveur.BeginAccept(new AsyncCallback(DemandeConnexion), Serveur);
                    Client = null;
                }
            }
        }
        private void mcSocket_Connecter_Click(object sender, EventArgs e)
        {
            if (tServeur.Text.Length > 0)
            {
                mcSocket_Ecouter.Enabled = mcSocket_Connecter.Enabled = false;
                mcSocket_deconnecter.Enabled = true;
                DrapeauSocket = 2;
                Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Client.Blocking = false;
                IPAddress IPserveur = AdresseValide(tServeur.Text);
                Client.BeginConnect(new IPEndPoint(IPserveur, 8000), new AsyncCallback(SurConnexion), Client);
                gameToolStripMenuItem.Enabled = false;
            }
            else 
            {
                MessageBox.Show("Renseigner le serveur");
            }
        }
        public void SurConnexion(IAsyncResult iar)
        {
            Socket tmp = (Socket)iar.AsyncState;
            if (tmp.Connected)
            {
                InitialiserReception(tmp);
            }
            else 
            {
                MessageBox.Show("Serveur inaccessible");
            }
        }
        private void bEnvoyer_Click(object sender, EventArgs e)
        {
            Client.Send(Encoding.Unicode.GetBytes(tbMessage.Text));
        }
        private void mcSocket_deconnecter_Click(object sender, EventArgs e)
        {
            if(DrapeauSocket == 2)
            {
                Client.Send(Encoding.Unicode.GetBytes("Déconnexion (client)"));
                Client.Shutdown(SocketShutdown.Both);
                DrapeauSocket = 0;
                Client.BeginDisconnect(false, new AsyncCallback(DemandeDeconnexion), Client);
                mcSocket_Ecouter.Enabled = mcSocket_Connecter.Enabled = true;
                mcSocket_deconnecter.Enabled = false;
                gameToolStripMenuItem.Enabled = true;
            }
            else if(Client == null)
            {
                Serveur.Close();
                DrapeauSocket = 0;
                mcSocket_Ecouter.Enabled = mcSocket_Connecter.Enabled = true;
                mcSocket_deconnecter.Enabled = false;
            }
            else
            {
                MessageBox.Show("Client toujours connecté !");
            }
        }
        public void DemandeDeconnexion(IAsyncResult iar)
        {
            if (DrapeauSocket == 2)
                OnlineFlag = 0;
            Socket tmp = (Socket)iar.AsyncState;
            tmp.EndDisconnect(iar);
        }
        private IPAddress AdresseValide(string nPC)
        {
            IPAddress ipReponse = null;
            if(nPC.Length >0)
            {
                IPAddress[] IPserveur = Dns.GetHostEntry(nPC).AddressList;
                for(int i=0; i<IPserveur.Length;i++)
                {
                    Ping pingServeur = new Ping();
                    PingReply pingReponse = pingServeur.Send(IPserveur[i]);
                    if(pingReponse.Status == IPStatus.Success)
                        if(IPserveur[i].AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipReponse = IPserveur[i];
                            break;
                        }
                }

            }
            
            return ipReponse;
        }
    }
}
