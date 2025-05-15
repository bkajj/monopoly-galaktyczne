using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MonopolyGalaktyczneFull.containers
{
    public class GameGrid : Panel
    {
        public Control CenterControl { get; set; }
        public GameGrid()
        {
            Resize += (s, e) => PerformLayout();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            int totalRows = 7;
            int totalCols = 7;

            int cellWidth = ClientSize.Width / totalCols;
            int cellHeight = ClientSize.Height / totalRows;

            int index = 0;

            for (int row = 0; row < totalRows; row++)
            {
                for (int col = 0; col < totalCols; col++)
                {
                    if (row == 0 || row == totalRows - 1 || col == 0 || col == totalCols - 1)
                    {
                        if (index < Controls.Count)
                        {
                            Control ctrl = Controls[index++];
                            ctrl.Bounds = new Rectangle(col * cellWidth, row * cellHeight, cellWidth, cellHeight);
                        }
                    }
                }
            }

            if (CenterControl != null)
            {
                int left = cellWidth;
                int top = cellHeight;
                int width = cellWidth * 5;
                int height = cellHeight * 5;

                CenterControl.Bounds = new Rectangle(left, top, width, height);
            }
        }
    }
}
