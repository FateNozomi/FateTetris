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
            Engine = new TetrisEngine(10, 20);
            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
        }

        public event EventHandler ScoreUpdated;

        public event EventHandler GameOver;

        public bool IsRunning { get; set; }

        public DispatcherTimer Timer { get; set; }

        public TetrisEngine Engine { get; set; }

        public ScoreSystem ScoreSystem { get; set; } = new ScoreSystem();

        public TetrisPreview Preview { get; set; } = new TetrisPreview(4);

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

            Preview.ClearTetriminos();
            CurrentTetrimino = NextTetrimino();

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
            Engine.Renderer.CommandRender(Engine.HardDrop, CurrentTetrimino);
            Timer.Stop();
            Timer.Interval = default(TimeSpan);
            Timer.Start();
        }

        public void MoveBlockUp()
        {
            Engine.Renderer.CommandRender(Engine.MoveUp, CurrentTetrimino);
        }

        public void MoveBlockDown()
        {
            if (!CurrentTetrimino.IsLocked)
            {
                bool moved = MoveDown(CurrentTetrimino);
                if (moved)
                {
                    LockDelay();
                }
                else
                {
                    if (Timer.IsEnabled)
                    {
                        Timer.Stop();
                        Timer.Interval = default(TimeSpan);
                        Timer.Start();
                    }
                }
            }
        }

        public void MoveBlockLeft()
        {
            Engine.Renderer.CommandRender(Engine.MoveLeft, CurrentTetrimino);
        }

        public void MoveBlockRight()
        {
            Engine.Renderer.CommandRender(Engine.MoveRight, CurrentTetrimino);
        }

        public void RotateBlockLeft()
        {
            Engine.Renderer.CommandRender(Engine.RotateLeft, CurrentTetrimino);
        }

        public void RotateBlockRight()
        {
            Engine.Renderer.CommandRender(Engine.RotateRight, CurrentTetrimino);
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

            tetrimino.IsLocked = true;
        }

        private void LockDelay()
        {
            var clone = CurrentTetrimino.Clone();
            if (!Engine.MoveDown(clone))
            {
                if (Timer.IsEnabled)
                {
                    Timer.Stop();
                    Timer.Interval = TimeSpan.FromMilliseconds(500);
                    Timer.Start();
                }
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
                if (!CurrentTetrimino.IsLocked)
                {
                    LockTetrimino(CurrentTetrimino);
                    Timer.Interval = TimeSpan.FromMilliseconds(250);
                    return;
                }

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
                    CurrentTetrimino = NextTetrimino();
                    Timer.Interval = default(TimeSpan);
                }
            }
        }

        private bool MoveDown(Tetrimino tetrimino)
        {
            return Engine.Renderer.CommandRender(Engine.MoveDown, tetrimino);
        }

        private Tetrimino NextTetrimino()
        {
            var tetrimino = Preview.GetTetrimino();
            tetrimino.X = (Engine.X / 2) - 1;
            tetrimino.Y = -1;

            Preview.DisplayTetriminos();
            return tetrimino;
        }
    }
}
