using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FateTetris.Models.Tetriminos
{
    public class TetriminoJ : Tetrimino
    {
        public TetriminoJ()
            : base()
        {
            Center = new Point(1, 1);
            Shape = new Point[]
            {
                new Point(0, 2),
                new Point(0, 1),
                new Point(1, 1),
                new Point(2, 1),
            };

            Color = Brushes.Blue;
        }
    }
}
