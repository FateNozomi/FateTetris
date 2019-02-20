using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FateTetris.Models.Tetriminos
{
    public class TetriminoI : Tetrimino
    {
        public TetriminoI()
        {
            Center = new Point(1.5, 1.5);
            Shape = new Point[]
            {
                new Point(0, 2),
                new Point(1, 2),
                new Point(2, 2),
                new Point(3, 2),
            };

            Color = Brushes.Cyan;
        }

        public override Point[] WallKickData()
        {
            switch (PreviousState)
            {
                case RotationState.SpawnState:
                    if (CurrentState == RotationState.Right)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(-2, 0),
                            new Point(+1, 0),
                            new Point(-2, -1),
                            new Point(1, 2),
                        };
                    }

                    if (CurrentState == RotationState.Left)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(-1, 0),
                            new Point(2, 0),
                            new Point(-1, 2),
                            new Point(2, -1),
                        };
                    }

                    break;

                case RotationState.Right:
                    if (CurrentState == RotationState.SpawnState)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(2, 0),
                            new Point(-1, 0),
                            new Point(2, 1),
                            new Point(-1, -2),
                        };
                    }

                    if (CurrentState == RotationState.InvertedSpawnState)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(-1, 0),
                            new Point(2, 0),
                            new Point(-1, 2),
                            new Point(2, -1),
                        };
                    }

                    break;

                case RotationState.Left:
                    if (CurrentState == RotationState.SpawnState)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(-2, 0),
                            new Point(1, -2),
                            new Point(-2, 1),
                        };
                    }

                    if (CurrentState == RotationState.InvertedSpawnState)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(-2, 0),
                            new Point(+1, 0),
                            new Point(-2, -1),
                            new Point(1, 2),
                        };
                    }

                    break;

                case RotationState.InvertedSpawnState:
                    if (CurrentState == RotationState.Right)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(-2, 0),
                            new Point(1, -2),
                            new Point(-2, 1),
                        };
                    }

                    if (CurrentState == RotationState.Left)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(2, 0),
                            new Point(-1, 0),
                            new Point(2, 1),
                            new Point(-1, -2),
                        };
                    }

                    break;

                default:
                    break;
            }

            throw new ArgumentOutOfRangeException("Rotation state out of range.");
        }
    }
}
