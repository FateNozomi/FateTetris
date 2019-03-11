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
        private List<Tetrimino> _tetriminos = new List<Tetrimino>();

        public TetrisPreview(int previewCount)
        {
            PreviewCount = previewCount;

            Engine = new TetrisEngine(4, PreviewCount * 4);

            _tetriminos.AddRange(Engine.GenerateRandomizedTetriminoBag());
        }

        public int PreviewCount { get; }

        public TetrisEngine Engine { get; set; }

        public Tetrimino GetTetrimino()
        {
            int currentTetriminosCount = _tetriminos.Count;
            if (currentTetriminosCount != PreviewCount)
            {
                _tetriminos.AddRange(Engine.GenerateRandomizedTetriminoBag());
            }

            Tetrimino tetrimino = _tetriminos.First();
            _tetriminos.Remove(tetrimino);

            return tetrimino;
        }

        public void DisplayTetriminos()
        {
            foreach (var block in Engine.Grid)
            {
                block.Rectangle.Fill = null;
            }

            int index = 0;
            for (int i = 0; i < PreviewCount; i++)
            {
                _tetriminos[i].X = 1;
                _tetriminos[i].Y = 2 + (index * 4);
                index++;

                Engine.Renderer.DrawTetrimino(_tetriminos[i]);
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
