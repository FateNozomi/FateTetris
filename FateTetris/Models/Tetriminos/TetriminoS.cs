using System.Windows;
using System.Windows.Media;

namespace FateTetris.Models.Tetriminos
{
    public class TetriminoS : Tetrimino
    {
        private static readonly Brush Green = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00C853"));

        public TetriminoS()
        {
            Center = new Point(1, 1);
            Shape = new Point[]
            {
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 2),
                new Point(2, 2),
            };

            Color = Green;
        }
    }
}
