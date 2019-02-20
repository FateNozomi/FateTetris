using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FateTetris.Models
{
    public class Block
    {
        public Block(Point coordinate)
        {
            Coordinate = coordinate;
            Rectangle = new Rectangle();
        }

        public Point Coordinate { get; set; }

        public Rectangle Rectangle { get; set; }

        public bool IsLocked { get; set; }
    }
}
