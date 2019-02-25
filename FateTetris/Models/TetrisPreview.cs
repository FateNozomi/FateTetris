using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FateTetris.Models.Tetriminos;

namespace FateTetris.Models
{
    public class TetrisPreview
    {
        private Queue<Tetrimino> _tetriminos = new Queue<Tetrimino>();

        public TetrisPreview(int previewCount)
        {
            PreviewCount = previewCount;

            Engine = new TetrisEngine(4, PreviewCount * 4);

            for (int i = 0; i < PreviewCount; i++)
            {
                _tetriminos.Enqueue(Engine.GenerateRandomTetrimino());
            }
        }

        public int PreviewCount { get; }

        public TetrisEngine Engine { get; set; }

        public Tetrimino GetTetrimino()
        {
            int currentTetriminosCount = _tetriminos.Count;
            if (currentTetriminosCount != PreviewCount)
            {
                for (int i = 0; i < PreviewCount - currentTetriminosCount; i++)
                {
                    _tetriminos.Enqueue(Engine.GenerateRandomTetrimino());
                }
            }

            _tetriminos.Enqueue(Engine.GenerateRandomTetrimino());
            return _tetriminos.Dequeue();
        }

        public void DisplayTetriminos()
        {
            foreach (var block in Engine.Grid)
            {
                block.Rectangle.Fill = null;
            }

            int index = 0;
            foreach (var tetrimino in _tetriminos)
            {
                tetrimino.X = 1;
                tetrimino.Y = 2 + (index * 4);
                index++;

                Engine.Renderer.DrawTetrimino(tetrimino);
            }
        }

        public void ClearTetriminos()
        {
            foreach (var block in Engine.Grid)
            {
                block.Rectangle.Fill = null;
            }

            _tetriminos.Clear();
        }
    }
}
