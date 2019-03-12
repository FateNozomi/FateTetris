using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FateTetris.Models
{
    public class ScoreSystem
    {
        public uint Level { get; private set; }

        public uint TotalLines { get; private set; }

        public uint Score { get; private set; }

        public void SetLevel(uint level)
        {
            Level = level;
        }

        public TimeSpan GetGravity()
        {
            double seconds = Math.Pow(0.8 - (Level * 0.007), Level);
            return TimeSpan.FromMilliseconds(seconds * 1000);
        }

        public void IncrementScore(uint linesCleared, uint linesDropped)
        {
            if (linesCleared != 0)
            {
                double multiplier = 0;

                switch (linesCleared)
                {
                    case 1:
                        multiplier = 100;
                        break;
                    case 2:
                        multiplier = 300;
                        break;
                    case 3:
                        multiplier = 500;
                        break;
                    case 4:
                        multiplier = 800;
                        break;
                    default:
                        break;
                }

                Score += (uint)(multiplier * (Level + 1)) + linesDropped;

                TotalLines += linesCleared;
                if (TotalLines >= (Level * 10) + 10)
                {
                    Level++;
                }
            }
        }

        public void ClearScore()
        {
            TotalLines = 0;
            Score = 0;
        }
    }
}
