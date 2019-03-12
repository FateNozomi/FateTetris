using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace FateTetris.Models.Tetriminos
{
    public abstract class Tetrimino
    {
        public int X { get; set; }

        public int Y { get; set; }

        public uint LinesSoftDropped { get; set; }

        public uint LinesHardDropped { get; set; }

        public Point[] Shape { get; protected set; }

        public Point Center { get; protected set; }

        public RotationState PreviousState { get; protected set; }

        public RotationState CurrentState { get; protected set; }

        public Brush Color { get; protected set; }

        public bool IsLocked { get; set; }

        public int LockDelayedCount { get; set; }

        public void MoveUp()
        {
            Y -= 1;
        }

        public void MoveDown()
        {
            Y += 1;
        }

        public void MoveLeft()
        {
            X -= 1;
        }

        public void MoveRight()
        {
            X += 1;
        }

        public void RotateLeft()
        {
            PreviousState = CurrentState;
            switch (CurrentState)
            {
                case RotationState.SpawnState:
                    CurrentState = RotationState.Left;
                    break;
                case RotationState.Right:
                    CurrentState = RotationState.SpawnState;
                    break;
                case RotationState.Left:
                    CurrentState = RotationState.InvertedSpawnState;
                    break;
                case RotationState.InvertedSpawnState:
                    CurrentState = RotationState.Right;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < Shape.Length; i++)
            {
                Point product = new Point(Shape[i].X - Center.X, Shape[i].Y - Center.Y);
                Shape[i] = new Point(-product.Y + Center.X, product.X + Center.Y);
            }
        }

        public void RotateRight()
        {
            PreviousState = CurrentState;
            switch (CurrentState)
            {
                case RotationState.SpawnState:
                    CurrentState = RotationState.Right;
                    break;
                case RotationState.Right:
                    CurrentState = RotationState.InvertedSpawnState;
                    break;
                case RotationState.Left:
                    CurrentState = RotationState.SpawnState;
                    break;
                case RotationState.InvertedSpawnState:
                    CurrentState = RotationState.Left;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < Shape.Length; i++)
            {
                Point product = new Point(Shape[i].X - Center.X, Shape[i].Y - Center.Y);
                Shape[i] = new Point(product.Y + Center.X, -product.X + Center.Y);
            }
        }

        public List<Point> GetShapeOnGrid()
        {
            var translatedShape = new List<Point>();

            foreach (var block in Shape)
            {
                int x = (int)(X + block.X - Math.Floor(Center.X));
                int y = (int)(Y - block.Y + Math.Floor(Center.Y));
                translatedShape.Add(new Point(x, y));
            }

            return translatedShape;
        }

        public Tetrimino Clone()
        {
            var clone = (Tetrimino)Activator.CreateInstance(GetType());
            clone.X = X;
            clone.Y = Y;
            clone.Shape = (Point[])Shape.Clone();
            clone.Center = Center;
            clone.PreviousState = PreviousState;
            clone.CurrentState = CurrentState;
            clone.Color = Color.Clone();

            return clone;
        }

        public virtual Point[] WallKickData()
        {
            switch (PreviousState)
            {
                case RotationState.SpawnState:
                case RotationState.InvertedSpawnState:
                    if (CurrentState == RotationState.Right)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(-1, 0),
                            new Point(-1, 1),
                            new Point(0, -2),
                            new Point(-1, -2),
                        };
                    }

                    if (CurrentState == RotationState.Left)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(1, 1),
                            new Point(0, -2),
                            new Point(1, -2),
                        };
                    }

                    break;

                case RotationState.Right:
                    if (CurrentState == RotationState.SpawnState ||
                        CurrentState == RotationState.InvertedSpawnState)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(1, -1),
                            new Point(0, 2),
                            new Point(1, 2),
                        };
                    }

                    break;

                case RotationState.Left:
                    if (CurrentState == RotationState.SpawnState ||
                        CurrentState == RotationState.InvertedSpawnState)
                    {
                        return new Point[]
                        {
                            new Point(0, 0),
                            new Point(-1, 0),
                            new Point(-1, -1),
                            new Point(0, 2),
                            new Point(-1, 2),
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
