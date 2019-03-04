using System.Windows;
using System.Windows.Media;

namespace FateTetris.Models.Tetriminos
{
    public class TetriminoT : Tetrimino
    {
        private static readonly Brush Purple = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8E24AA"));

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

            Color = Purple;
        }
    }
}
