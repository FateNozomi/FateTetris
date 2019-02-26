using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FateTetris.Models.Tetriminos
{
    public class TetriminoL : Tetrimino
    {
        private static readonly Brush Orange = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6D00"));

        public TetriminoL()
            : base()
        {
            Center = new Point(1, 1);
            Shape = new Point[]
            {
                new Point(0, 1),
                new Point(1, 1),
                new Point(2, 1),
                new Point(2, 2),
        };

            Color = Orange;
        }
    }
}
