using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FateTetris.Models.Tetriminos;

namespace FateTetris.Models
{
    public class TetrisEngine
    {
        private Random _random = new Random();

        public TetrisEngine(int x_Cells, int y_Cells)
        {
            X = x_Cells;
            Y = y_Cells;

            for (int x = 0; x < X; x++)
            {
                for (int y = 0; y < Y; y++)
                {
                    Grid.Add(new Block(new Point(x, y)));
                }
            }
        }

        public int X { get; }

        public int Y { get; }

        public List<Block> Grid { get; } = new List<Block>();

        public Tetrimino GenerateRandomTetrimino()
        {
            Tetrimino block;
            int rng = _random.Next(7);
            switch (rng)
            {
                case 0:
                    block = new TetriminoI();
                    break;
                case 1:
                    block = new TetriminoJ();
                    break;
                case 2:
                    block = new TetriminoL();
                    break;
                case 3:
                    block = new TetriminoO();
                    break;
                case 4:
                    block = new TetriminoS();
                    break;
                case 5:
                    block = new TetriminoT();
                    break;
                case 6:
                    block = new TetriminoZ();
                    break;
                default:
                    block = null;
                    break;
            }

            block.X = (X / 2) - 1;
            block.Y = -1;
            return block;
        }

        public void HardDrop(Tetrimino tetrimino)
        {
            while (true)
            {
                if (!MoveDown(tetrimino))
                {
                    break;
                }
            }
        }

        public void MoveUp(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            clone.MoveUp();
            if (CanPlace(clone))
            {
                tetrimino.MoveUp();
            }
        }

        public bool MoveDown(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            clone.MoveDown();
            if (CanPlace(clone))
            {
                tetrimino.MoveDown();
                tetrimino.LinesDropped += 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MoveLeft(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            clone.MoveLeft();
            if (CanPlace(clone))
            {
                tetrimino.MoveLeft();
            }
        }

        public void MoveRight(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            clone.MoveRight();
            if (CanPlace(clone))
            {
                tetrimino.MoveRight();
            }
        }

        public void RotateLeft(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            clone.RotateLeft();
            var testSequence = clone.WallKickData();
            foreach (var offset in testSequence)
            {
                var offsetClone = clone.Clone();
                offsetClone.X += (int)offset.X;
                offsetClone.Y -= (int)offset.Y;
                if (CanPlace(offsetClone))
                {
                    tetrimino.X += (int)offset.X;
                    tetrimino.Y -= (int)offset.Y;
                    tetrimino.RotateLeft();
                    break;
                }
            }
        }

        public void RotateRight(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            clone.RotateRight();
            var testSequence = clone.WallKickData();
            foreach (var offset in testSequence)
            {
                var offsetClone = clone.Clone();
                offsetClone.X += (int)offset.X;
                offsetClone.Y -= (int)offset.Y;
                if (CanPlace(offsetClone))
                {
                    tetrimino.X += (int)offset.X;
                    tetrimino.Y -= (int)offset.Y;
                    tetrimino.RotateRight();
                    break;
                }
            }
        }

        private bool CanPlace(Tetrimino tetrimino)
        {
            var blocks = tetrimino.GetShapeOnGrid();
            foreach (var block in blocks)
            {
                if (block.Y >= 0)
                {
                    var b = Grid.FirstOrDefault(r => r.Coordinate == new Point(block.X, block.Y));
                    if (b == null)
                    {
                        return false;
                    }
                    else if (b.IsLocked)
                    {
                        return false;
                    }
                }
                else if (block.X < 0 || block.X >= X)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
