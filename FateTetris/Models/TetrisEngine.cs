using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FateTetris.Models.Extensions;
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

            Renderer = new TetrisRenderer(this);
        }

        public TetrisRenderer Renderer { get; }

        public int X { get; }

        public int Y { get; }

        public List<Block> Grid { get; } = new List<Block>();

        public IEnumerable<Tetrimino> GenerateRandomizedTetriminoBag()
        {
            List<Tetrimino> tetriminos = new List<Tetrimino>();
            tetriminos.Add(new TetriminoI());
            tetriminos.Add(new TetriminoJ());
            tetriminos.Add(new TetriminoL());
            tetriminos.Add(new TetriminoO());
            tetriminos.Add(new TetriminoS());
            tetriminos.Add(new TetriminoT());
            tetriminos.Add(new TetriminoZ());

            return tetriminos.Shuffle(_random).Shuffle(_random);
        }

        public void HardDrop(Tetrimino tetrimino)
        {
            while (true)
            {
                if (!MoveDown(tetrimino))
                {
                    break;
                }

                tetrimino.LinesHardDropped += 1;
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
                tetrimino.LinesSoftDropped += 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MoveLeft(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            clone.MoveLeft();
            if (CanPlace(clone))
            {
                tetrimino.MoveLeft();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MoveRight(Tetrimino tetrimino)
        {
            var clone = tetrimino.Clone();
            clone.MoveRight();
            if (CanPlace(clone))
            {
                tetrimino.MoveRight();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RotateLeft(Tetrimino tetrimino)
        {
            bool rotated = false;

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
                    rotated = true;
                    break;
                }
            }

            return rotated;
        }

        public bool RotateRight(Tetrimino tetrimino)
        {
            bool rotated = false;

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
                    rotated = true;
                    break;
                }
            }

            return rotated;
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
