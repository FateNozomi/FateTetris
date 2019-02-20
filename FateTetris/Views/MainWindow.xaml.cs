using System;
using System.Collections.Generic;
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
using FateTetris.ViewModels;

namespace FateTetris.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                if (vm.KeyDownCommand.CanExecute(null))
                {
                    vm.KeyDownCommand.Execute(e.Key);
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                if (vm.KeyUpCommand.CanExecute(null))
                {
                    vm.KeyUpCommand.Execute(e.Key);
                }
            }
        }
    }
}
