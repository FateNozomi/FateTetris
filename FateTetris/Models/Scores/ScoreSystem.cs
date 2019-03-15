using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FateTetris.Models.Scores
{
    public class ScoreSystem
    {
        public ScoreSystem()
        {
            Actions = new FixedSizedQueue<Action>(5);
            for (int i = 0; i < Actions.Size; i++)
            {
                Actions.Enqueue(null);
            }
        }

        public uint LevelCap { get; set; }

        public uint Level { get; set; }

        public uint TotalLines { get; private set; }

        public uint Score { get; private set; }

        public FixedSizedQueue<Action> Actions { get; private set; }

        public TimeSpan GetGravity()
        {
            double seconds = Math.Pow(0.8 - (Level * 0.007), Level);
            return TimeSpan.FromMilliseconds(seconds * 1000);
        }

        public void IncrementScore(uint linesCleared, uint linesDropped)
        {
            Action action = new Action();

            if (linesCleared != 0)
            {
                double multiplier = 0;

                switch (linesCleared)
                {
                    case 1:
                        multiplier = 100;
                        action.Type = "Single";
                        break;
                    case 2:
                        multiplier = 300;
                        action.Type = "Double";
                        break;
                    case 3:
                        multiplier = 500;
                        action.Type = "Triple";
                        break;
                    case 4:
                        multiplier = 800;
                        action.Type = "Tetris";
                        action.Color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00E5FF"));
                        break;
                    default:
                        break;
                }

                action.Points = (uint)(multiplier * (Level + 1)) + linesDropped;
                Score += action.Points;

                TotalLines += linesCleared;
                if (TotalLines >= (Level * 10) + 10)
                {
                    if (LevelCap < Level)
                    {
                        Level++;
                    }
                }

                Actions.Enqueue(action);
            }
        }

        public void ClearScore()
        {
            TotalLines = 0;
            Score = 0;
            for (int i = 0; i < Actions.Size; i++)
            {
                Actions.Enqueue(null);
            }
        }
    }
}
