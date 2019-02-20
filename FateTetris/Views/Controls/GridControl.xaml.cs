using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FateTetris.Views.Controls
{
    /// <summary>
    /// Interaction logic for GridControl.xaml
    /// </summary>
    public partial class GridControl : UserControl
    {
        // Using a DependencyProperty as the backing store for TetrisGrid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TetrisGridProperty =
            DependencyProperty.Register("TetrisGrid", typeof(ICollectionView), typeof(GridControl), new PropertyMetadata(default(ICollectionView), UpdateTetrisGridCallbacks));

        private List<Tuple<Point, Rectangle>> _rectangles = new List<Tuple<Point, Rectangle>>();

        public GridControl()
        {
            InitializeComponent();
        }

        public ICollectionView TetrisGrid
        {
            get { return (ICollectionView)GetValue(TetrisGridProperty); }
            set { SetValue(TetrisGridProperty, value); }
        }

        private static void UpdateTetrisGridCallbacks(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GridControl gridControl)
            {
                gridControl.DrawTetrisGrid();
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridTetris.Width =
                GridTetris.ActualHeight /
                GridTetris.RowDefinitions.Count *
                GridTetris.ColumnDefinitions.Count;
        }

        private void DrawTetrisGrid()
        {
            GridTetris.Children.Clear();
            GridTetris.Background = Brushes.Black;
            var grid = TetrisGrid.OfType<Models.Block>();
            foreach (var block in grid)
            {
                block.Rectangle.Stroke = Brushes.Gray;
                block.Rectangle.StrokeThickness = 0.5;
                Grid.SetColumn(block.Rectangle, (int)block.Coordinate.X);
                Grid.SetRow(block.Rectangle, (int)block.Coordinate.Y);
                GridTetris.Children.Add(block.Rectangle);
            }
        }
    }
}
