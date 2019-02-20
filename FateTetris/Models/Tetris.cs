using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using FateTetris.Models.Tetriminos;

namespace FateTetris.Models
{
    public class Tetris
    {
        public Tetris()
        {
            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
        }

        public event EventHandler ScoreUpdated;

        public event EventHandler GameOver;

        public bool IsRunning { get; set; }

        public DispatcherTimer Timer { get; set; }

        public TetrisEngine Engine { get; set; } = new TetrisEngine(10, 20);

        public ScoreSystem ScoreSystem { get; set; } = new ScoreSystem();

        public Tetrimino CurrentTetrimino { get; set; }

        public void Start()
        {
            IsRunning = true;

            foreach (var block in Engine.Grid)
            {
                block.Rectangle.Fill = null;
                block.IsLocked = false;
            }

            ScoreSystem.SetLevel(0);
            ScoreSystem.ClearScore();
            ScoreUpdated?.Invoke(this, null);

            CurrentTetrimino = Engine.GenerateRandomTetrimino();
            Timer.Interval = default(TimeSpan);
            Timer.Start();
        }

        public void End()
        {
            Timer.Stop();
            IsRunning = false;
        }

        public void HardDropBlock()
        {
            ClearTetrimino(CurrentTetrimino);
            Engine.HardDrop(CurrentTetrimino);
            DrawTetrimino(CurrentTetrimino);

            Timer.Stop();
            Timer.Interval = default(TimeSpan);
            Timer.Start();
        }

        public void MoveBlockUp()
        {
            ClearTetrimino(CurrentTetrimino);
            Engine.MoveUp(CurrentTetrimino);
            DrawTetrimino(CurrentTetrimino);
        }

        public bool MoveBlockDown()
        {
            bool moved = MoveDown(CurrentTetrimino);
            if (moved)
            {
                LockDelay();
                return true;
            }
            else
            {
                Timer.Stop();
                Timer.Interval = default(TimeSpan);
                Timer.Start();
                return false;
            }
        }

        public void MoveBlockLeft()
        {
            ClearTetrimino(CurrentTetrimino);
            Engine.MoveLeft(CurrentTetrimino);
            DrawTetrimino(CurrentTetrimino);
        }

        public void MoveBlockRight()
        {
            ClearTetrimino(CurrentTetrimino);
            Engine.MoveRight(CurrentTetrimino);
            DrawTetrimino(CurrentTetrimino);
        }

        public void RotateBlockLeft()
        {
            ClearTetrimino(CurrentTetrimino);
            Engine.RotateLeft(CurrentTetrimino);
            DrawTetrimino(CurrentTetrimino);
        }

        public void RotateBlockRight()
        {
            ClearTetrimino(CurrentTetrimino);
            Engine.RotateRight(CurrentTetrimino);
            DrawTetrimino(CurrentTetrimino);
        }

        public void ClearFullRows()
        {
            uint linesCleared = 0;
            for (int row = Engine.Y - 1; row >= 0; row--)
            {
                var blocks = Engine.Grid.Where(b => b.Coordinate.Y == row).ToList();
                bool isFull = !blocks.Any(b => !b.IsLocked);
                if (isFull)
                {
                    foreach (var block in blocks)
                    {
                        block.Rectangle.Fill = null;
                        block.IsLocked = false;
                    }

                    ShiftRows(row);
                    row++;
                    linesCleared++;
                }
            }

            ScoreSystem.IncrementScore(linesCleared, CurrentTetrimino.LinesDropped);
            ScoreUpdated?.Invoke(this, null);
        }

        private void ShiftRows(int startingRow)
        {
            for (int row = startingRow; row > 0; row--)
            {
                var blocks = Engine.Grid.Where(b => b.Coordinate.Y == row).ToList();
                var blocksPrime = Engine.Grid.Where(b => b.Coordinate.Y == row - 1).ToList();
                for (int i = 0; i < blocks.Count; i++)
                {
                    blocks[i].Rectangle.Fill = blocksPrime[i].Rectangle.Fill;
                    blocks[i].IsLocked = blocksPrime[i].IsLocked;
                }
            }
        }

        private void DrawTetrimino(Tetrimino tetrimino)
        {
            DrawGhostTetrimino(tetrimino);

            var blocks = tetrimino.GetShapeOnGrid();
            foreach (var block in blocks)
            {
                var b = Engine.Grid.FirstOrDefault(r => r.Coordinate == new Point(block.X, block.Y));
                if (b != null)
                {
                    b.Rectangle.Fill = tetrimino.Color;
                }
            }
        }

        private void DrawGhostTetrimino(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            while (true)
            {
                if (!Engine.MoveDown(clone))
                {
                    break;
                }
            }

            var blocks = clone.GetShapeOnGrid();
            foreach (var block in blocks)
            {
                var b = Engine.Grid.FirstOrDefault(r => r.Coordinate == new Point(block.X, block.Y));
                if (b != null)
                {
                    b.Rectangle.Fill = tetrimino.Color;
                    if (b.Rectangle.Fill.IsFrozen)
                    {
                        b.Rectangle.Fill = b.Rectangle.Fill.Clone();
                    }

                    b.Rectangle.Fill.Opacity = 0.35;
                }
            }
        }

        private void ClearTetrimino(Tetrimino tetrimino)
        {
            ClearGhostTetrimino(tetrimino);
            var blocks = tetrimino.GetShapeOnGrid();
            foreach (var block in blocks)
            {
                var b = Engine.Grid.FirstOrDefault(r => r.Coordinate == new Point(block.X, block.Y));
                if (b != null)
                {
                    b.Rectangle.Fill = null;
                    b.IsLocked = false;
                }
            }
        }

        private void ClearGhostTetrimino(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            while (true)
            {
                if (!Engine.MoveDown(clone))
                {
                    break;
                }
            }

            var blocks = clone.GetShapeOnGrid();
            foreach (var block in blocks)
            {
                var b = Engine.Grid.FirstOrDefault(r => r.Coordinate == new Point(block.X, block.Y));
                if (b != null)
                {
                    b.Rectangle.Fill = null;
                }
            }
        }

        private void LockTetrimino(Tetrimino tetrimino)
        {
            var blocks = tetrimino.GetShapeOnGrid();
            foreach (var block in blocks)
            {
                var b = Engine.Grid.FirstOrDefault(r => r.Coordinate == new Point(block.X, block.Y));
                if (b != null)
                {
                    b.IsLocked = true;
                }
            }
        }

        private void LockDelay()
        {
            var clone = CurrentTetrimino.Clone();
            if (!Engine.MoveDown(clone))
            {
                Timer.Stop();
                Timer.Interval = TimeSpan.FromMilliseconds(500);
                Timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Interval = ScoreSystem.GetGravity();

            bool moved = MoveDown(CurrentTetrimino);
            if (moved)
            {
                LockDelay();
            }
            else
            {
                LockTetrimino(CurrentTetrimino);
                ClearFullRows();

                var blocks = CurrentTetrimino.GetShapeOnGrid();
                bool topOut = !blocks.Any(b => b.Y > 0);
                if (topOut)
                {
                    GameOver?.Invoke(this, null);
                    End();
                }
                else
                {
                    CurrentTetrimino = Engine.GenerateRandomTetrimino();
                    Timer.Interval = default(TimeSpan);
                }
            }
        }

        private bool MoveDown(Tetrimino tetrimino)
        {
            ClearTetrimino(tetrimino);
            bool moved = Engine.MoveDown(tetrimino);
            DrawTetrimino(tetrimino);
            return moved;
        }
    }
}
