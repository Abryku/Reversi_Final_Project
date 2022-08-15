using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi_Final_Project
{
    public partial class SquareControl : UserControl
    {

        // These reflect the position of the square on the board.
        public int Row => this.row;

        public int Col => this.col;

        // These reflect the public row and column properties.
        private int row;
        private int col;

        public int Contents;
		public int PreviewContents;
        public int IsFlipped = 0;

        public bool IsValid = false;
        public bool IsActive = false;


        public static readonly Color NormalBackColorDefault = Color.Green;
        public static Color NormalBackColor = NormalBackColorDefault;
        public static Color ActiveSquareBackColor = Color.FromArgb(0, 224, 0);
        public static Color ValidMoveBackColor = Color.FromArgb(0, 176, 0);


        private static Pen pen = new Pen(Color.Black);
        private static SolidBrush solidBrush = new SolidBrush(Color.Black);

        public SquareControl(int row, int col)
        {
            InitializeComponent();

            this.Contents = Board.Empty;
            this.row = row;
            this.col = col;

            // Prevent the control from receiving focus via the TAB key.
            this.TabStop = false;

            // Set the background color.
            this.BackColor = SquareControl.NormalBackColor;

            // Redraw the control on a resize.
            this.ResizeRedraw = true;

		}

        // Paint event handler.
        private void SquareControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Clear the square, filling with the appropriate background color.
            Color backColor = SquareControl.NormalBackColor;
            if (this.IsValid)
                backColor = SquareControl.ValidMoveBackColor;
            if (this.IsActive)
                backColor = SquareControl.ActiveSquareBackColor;

            e.Graphics.Clear(backColor);


            // Draw the border.
            Point topLeft = new Point(0, 0);
            Point topRight = new Point(this.Width - 1, 0);
            Point bottomLeft = new Point(0, this.Height - 1);
            Point bottomRight = new Point(this.Width - 1, this.Height - 1);
            SquareControl.pen.Color = Color.Black;
            SquareControl.pen.Width = 1;
            e.Graphics.DrawLine(SquareControl.pen, bottomLeft, topLeft);
            e.Graphics.DrawLine(SquareControl.pen, topLeft, topRight);
            SquareControl.pen.Color = Color.Black;
            e.Graphics.DrawLine(SquareControl.pen, bottomLeft, bottomRight);
            e.Graphics.DrawLine(SquareControl.pen, bottomRight, topRight);

            // Draw the disc, if any.
            if (this.Contents != Board.Empty || this.PreviewContents != Board.Empty)
            {
                // Get size and position parameters based on the control size and animation state.
                int size = (int)(this.Width * 0.8);
                int offset = (int)((this.Width - size) / 2);
                int thickness = (int)(size * 0.08);
                int width = size;
                int height = Math.Max(thickness, size);
                int left = offset;
                int top = offset + (int)Math.Round((double)(size - height) / 2.0);

                // Draw the disc face
                if (this.PreviewContents == Board.Empty)
                {
                    if (this.Contents == Board.Black)
                    {
                        SquareControl.solidBrush.Color = Color.Black;
                        if (IsFlipped == 1)
                        {
                            SquareControl.solidBrush.Color = Color.White;
                        }
                    }  
                    else
                    {
                        SquareControl.solidBrush.Color = Color.White;
                        if (IsFlipped == 1)
                        {
                            SquareControl.solidBrush.Color = Color.Black;
                        }
                    }
                        
                }
                else
                {
                    if (this.PreviewContents == Board.Black)
                        SquareControl.solidBrush.Color = Color.FromArgb(96, Color.Black);
                    else
                        SquareControl.solidBrush.Color = Color.FromArgb(96, Color.White);
                }
                e.Graphics.FillEllipse(SquareControl.solidBrush, left, top, width, height);

            }
        }
    }
}
