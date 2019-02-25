using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FateTetris.Models.Tetriminos;

namespace FateTetris.Models
{
    public class TetrisRenderer
    {
        private TetrisEngine _engine;

        public TetrisRenderer(TetrisEngine engine)
        {
            _engine = engine;
        }

        public void CommandRender(Action<Tetrimino> command, Tetrimino tetrimino)
        {
            ClearGhostTetrimino(tetrimino);
            ClearTetrimino(tetrimino);

            command(tetrimino);

            DrawGhostTetrimino(tetrimino);
            DrawTetrimino(tetrimino);
        }

        public T CommandRender<T>(Func<Tetrimino, T> command, Tetrimino tetrimino)
        {
            ClearGhostTetrimino(tetrimino);
            ClearTetrimino(tetrimino);

            T result = command(tetrimino);

            DrawGhostTetrimino(tetrimino);
            DrawTetrimino(tetrimino);

            return result;
        }

        public void DrawTetrimino(Tetrimino tetrimino)
        {
            var blocks = tetrimino.GetShapeOnGrid();
            foreach (var block in blocks)
            {
                var b = _engine.Grid.FirstOrDefault(r => r.Coordinate == new Point(block.X, block.Y));
                if (b != null)
                {
                    b.Rectangle.Fill = tetrimino.Color;
                }
            }
        }

        public void DrawGhostTetrimino(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            while (true)
            {
                if (!_engine.MoveDown(clone))
                {
                    break;
                }
            }

            var blocks = clone.GetShapeOnGrid();
            foreach (var block in blocks)
            {
                var b = _engine.Grid.FirstOrDefault(r => r.Coordinate == new Point(block.X, block.Y));
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

        public void ClearTetrimino(Tetrimino tetrimino)
        {
            var blocks = tetrimino.GetShapeOnGrid();
            foreach (var block in blocks)
            {
                var b = _engine.Grid.FirstOrDefault(r => r.Coordinate == new Point(block.X, block.Y));
                if (b != null)
                {
                    b.Rectangle.Fill = null;
                    b.IsLocked = false;
                }
            }
        }

        public void ClearGhostTetrimino(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            while (true)
            {
                if (!_engine.MoveDown(clone))
                {
                    break;
                }
            }

            var blocks = clone.GetShapeOnGrid();
            foreach (var block in blocks)
            {
                var b = _engine.Grid.FirstOrDefault(r => r.Coordinate == new Point(block.X, block.Y));
                if (b != null)
                {
                    b.Rectangle.Fill = null;
                }
            }
        }
    }
}
