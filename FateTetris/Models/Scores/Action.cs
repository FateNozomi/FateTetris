using System.Windows.Media;

namespace FateTetris.Models.Scores
{
    public class Action
    {
        public Action(string action = default(string), uint points = default(uint), Brush color = default(Brush))
        {
            Type = action;
            Points = points;
            Color = color ?? Brushes.Black;
        }

        public string Type { get; set; }

        public uint Points { get; set; }

        public Brush Color { get; set; }
    }
}
