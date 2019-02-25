using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FateTetris.Models.Tetriminos;

namespace FateTetris.Models
{
    public class TetrisHolder
    {
        private Tetrimino _tetrimino;

        public TetrisHolder()
        {
            Engine = new TetrisEngine(4, 4);
            CanSwap = true;
        }

        public bool CanSwap { get; private set; }

        public TetrisEngine Engine { get; set; }

        public Tetrimino Swap(Tetrimino tetrimino)
        {
            CanSwap = false;
            var held = _tetrimino;
            _tetrimino = tetrimino;
            return held;
        }

        public void DisplayTetrimino()
        {
            Engine.Grid.ForEach(block => block.Rectangle.Fill = null);

            _tetrimino.X = 1;
            _tetrimino.Y = 2;
            Engine.Renderer.DrawTetrimino(_tetrimino);
        }

        public void ResetSwap()
        {
            CanSwap = true;
        }

        public void ClearTetrimino()
        {
            Engine.Grid.ForEach(block => block.Rectangle.Fill = null);
            _tetrimino = null;
        }
    }
}
