using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FateTetris.Models.Tetriminos
{
    public class TetriminoT : Tetrimino
    {
        public TetriminoT()
        {
            Center = new Point(1, 1);
            Shape = new Point[]
            {
                new Point(0, 1),
                new Point(1, 2),
                new Point(1, 1),
                new Point(2, 1),
            };

            Color = Brushes.Purple;
        }
    }
}
