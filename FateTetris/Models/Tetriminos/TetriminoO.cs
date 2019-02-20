using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FateTetris.Models.Tetriminos
{
    public class TetriminoO : Tetrimino
    {
        public TetriminoO()
        {
            Center = new Point(1.5, 1.5);
            Shape = new Point[]
            {
                new Point(1, 1),
                new Point(2, 1),
                new Point(1, 2),
                new Point(2, 2),
            };

            Color = Brushes.Yellow;
        }
    }
}
