using System;
using System.Drawing;
using System.Windows.Forms;

namespace Reversi_Final_Project
{
    public partial class SquareControl : UserControl
    {

        // These reflect the position of the square on the board.
        public int Row { get; }

        public int Col { get; }

        // These reflect the public row and column properties.

        public int Contents;
		public int PreviewContents;
        public int IsFlipped = 0;

        public bool IsValid = false;
        public bool IsActive = false;


        private static readonly Color NormalBackColorDefault = Color.Green;
        private static readonly Color NormalBackColor = NormalBackColorDefault;
        private static readonly Color ActiveSquareBackColor = Color.FromArgb(0, 224, 0);
        private static Color ValidMoveBackColor = Color.FromArgb(0, 176, 0);


        private static readonly Pen pen = new Pen(Color.Black);
        private static readonly SolidBrush solidBrush = new SolidBrush(Color.Black);

        public SquareControl(int row, int col)
        {
            InitializeComponent();

            Contents = (int) Coin.Empty;
            this.Row = row;
            this.Col = col;

            // Prevent the control from receiving focus via the TAB key.
            TabStop = false;

            // Set the background color.
            BackColor = NormalBackColor;

            // Redraw the control on a resize.
            ResizeRedraw = true;

		}

        // Paint event handler.
        private void SquareControl_Paint(object sender, PaintEventArgs e)
        {
            // Clear the square, filling with the appropriate background color.
            Color backColor = NormalBackColor;
            if (IsValid)
                backColor = ValidMoveBackColor;
            if (IsActive)
                backColor = ActiveSquareBackColor;

            e.Graphics.Clear(backColor);


            // Draw the border.
            Point topLeft = new Point(0, 0);
            Point topRight = new Point(Width - 1, 0);
            Point bottomLeft = new Point(0, Height - 1);
            Point bottomRight = new Point(Width - 1, Height - 1);
            pen.Color = Color.Black;
            pen.Width = 1;
            e.Graphics.DrawLine(pen, bottomLeft, topLeft);
            e.Graphics.DrawLine(pen, topLeft, topRight);
            pen.Color = Color.Black;
            e.Graphics.DrawLine(pen, bottomLeft, bottomRight);
            e.Graphics.DrawLine(pen, bottomRight, topRight);

            // Draw the disc, if any.
            if (Contents != (int) Coin.Empty || PreviewContents != (int) Coin.Empty)
            {
                // Get size and position parameters based on the control size and animation state.
                int size = (int)(Width * 0.8);
                int offset = (int)((Width - size) / 2);
                int thickness = (int)(size * 0.08);
                int width = size;
                int height = Math.Max(thickness, size);
                int left = offset;
                int top = offset + (int)Math.Round((double)(size - height) / 2.0);

                // Draw the disc face
                if (PreviewContents == (int) Coin.Empty)
                {
                    if (Contents == (int) Coin.Black)
                    {
                        solidBrush.Color = Color.Black;
                        if (IsFlipped == 1)
                        {
                            solidBrush.Color = Color.White;
                        }
                    }  
                    else
                    {
                        solidBrush.Color = Color.White;
                        if (IsFlipped == 1)
                        {
                            solidBrush.Color = Color.Black;
                        }
                    }
                        
                }
                else
                {
                    if (PreviewContents == (int) Coin.Black)
                        solidBrush.Color = Color.FromArgb(96, Color.Black);
                    else
                        solidBrush.Color = Color.FromArgb(96, Color.White);
                }
                e.Graphics.FillEllipse(solidBrush, left, top, width, height);

            }
        }
    }
}
