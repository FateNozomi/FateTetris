using System.Windows;
using System.Windows.Media;

namespace FateTetris.Models.Tetriminos
{
    public class TetriminoZ : Tetrimino
    {
        private static readonly Brush Red = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D32F2F"));

        public TetriminoZ()
        {
            Center = new Point(1, 1);
            Shape = new Point[]
            {
                new Point(0, 2),
                new Point(1, 2),
                new Point(1, 1),
                new Point(2, 1),
            };

            Color = Red;
        }
    }
}
