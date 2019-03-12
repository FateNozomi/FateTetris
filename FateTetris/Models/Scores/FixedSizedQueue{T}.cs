using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FateTetris.Models.Scores
{
    public class FixedSizedQueue<T> : Queue<T>
    {
        public FixedSizedQueue(int size)
        {
            Size = size;
        }

        public int Size { get; }

        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            while (Count > Size)
            {
                Dequeue();
            }
        }
    }
}
