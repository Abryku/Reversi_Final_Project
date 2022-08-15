namespace Reversi_Final_Project
{
    partial class ReversiMain
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.Board1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sauvegarderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mcSocket_Ecouter = new System.Windows.Forms.ToolStripMenuItem();
            this.mcSocket_Connecter = new System.Windows.Forms.ToolStripMenuItem();
            this.mcSocket_deconnecter = new System.Windows.Forms.ToolStripMenuItem();
            this.lbEchanges = new System.Windows.Forms.ListBox();
            this.tServeur = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.bEnvoyer = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Board1
            // 
            this.Board1.Location = new System.Drawing.Point(27, 43);
            this.Board1.Name = "Board1";
            this.Board1.Size = new System.Drawing.Size(256, 256);
            this.Board1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.hostToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(475, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.sauvegarderToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.newGameToolStripMenuItem.Text = "New game";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // sauvegarderToolStripMenuItem
            // 
            this.sauvegarderToolStripMenuItem.Name = "sauvegarderToolStripMenuItem";
            this.sauvegarderToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.sauvegarderToolStripMenuItem.Text = "Save";
            this.sauvegarderToolStripMenuItem.Click += new System.EventHandler(this.sauvegarderToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // hostToolStripMenuItem
            // 
            this.hostToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mcSocket_Ecouter,
            this.mcSocket_Connecter,
            this.mcSocket_deconnecter});
            this.hostToolStripMenuItem.Name = "hostToolStripMenuItem";
            this.hostToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.hostToolStripMenuItem.Text = "Host";
            // 
            // mcSocket_Ecouter
            // 
            this.mcSocket_Ecouter.Name = "mcSocket_Ecouter";
            this.mcSocket_Ecouter.Size = new System.Drawing.Size(141, 22);
            this.mcSocket_Ecouter.Text = "Ecouter";
            this.mcSocket_Ecouter.Click += new System.EventHandler(this.mcSocket_Ecouter_Click);
            // 
            // mcSocket_Connecter
            // 
            this.mcSocket_Connecter.Name = "mcSocket_Connecter";
            this.mcSocket_Connecter.Size = new System.Drawing.Size(141, 22);
            this.mcSocket_Connecter.Text = "Connecter";
            this.mcSocket_Connecter.Click += new System.EventHandler(this.mcSocket_Connecter_Click);
            // 
            // mcSocket_deconnecter
            // 
            this.mcSocket_deconnecter.Name = "mcSocket_deconnecter";
            this.mcSocket_deconnecter.Size = new System.Drawing.Size(141, 22);
            this.mcSocket_deconnecter.Text = "Déconnecter";
            this.mcSocket_deconnecter.Click += new System.EventHandler(this.mcSocket_deconnecter_Click);
            // 
            // lbEchanges
            // 
            this.lbEchanges.FormattingEnabled = true;
            this.lbEchanges.Location = new System.Drawing.Point(289, 139);
            this.lbEchanges.Name = "lbEchanges";
            this.lbEchanges.Size = new System.Drawing.Size(174, 160);
            this.lbEchanges.TabIndex = 2;
            // 
            // tServeur
            // 
            this.tServeur.Location = new System.Drawing.Point(289, 63);
            this.tServeur.Name = "tServeur";
            this.tServeur.Size = new System.Drawing.Size(174, 20);
            this.tServeur.TabIndex = 3;
            this.tServeur.Text = "DESKTOP-SJ6UBPR";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(289, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Serveur";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(289, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Message";
            // 
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(289, 102);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(115, 20);
            this.tbMessage.TabIndex = 6;
            // 
            // bEnvoyer
            // 
            this.bEnvoyer.Location = new System.Drawing.Point(410, 100);
            this.bEnvoyer.Name = "bEnvoyer";
            this.bEnvoyer.Size = new System.Drawing.Size(53, 23);
            this.bEnvoyer.TabIndex = 7;
            this.bEnvoyer.Text = "Send";
            this.bEnvoyer.UseVisualStyleBackColor = true;
            this.bEnvoyer.Click += new System.EventHandler(this.bEnvoyer_Click);
            // 
            // ReversiMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 343);
            this.Controls.Add(this.bEnvoyer);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tServeur);
            this.Controls.Add(this.lbEchanges);
            this.Controls.Add(this.Board1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ReversiMain";
            this.Text = "Reversi";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Board1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sauvegarderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mcSocket_Ecouter;
        private System.Windows.Forms.ToolStripMenuItem mcSocket_Connecter;
        private System.Windows.Forms.ToolStripMenuItem mcSocket_deconnecter;
        private System.Windows.Forms.ListBox lbEchanges;
        private System.Windows.Forms.TextBox tServeur;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button bEnvoyer;
    }
}

