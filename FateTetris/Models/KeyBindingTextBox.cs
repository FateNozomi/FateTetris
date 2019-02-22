using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FateTetris.Models
{
    public class KeyBindingTextBox : TextBox
    {
        // Using a DependencyProperty as the backing store for Key.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(Key), typeof(KeyBindingTextBox), new FrameworkPropertyMetadata(default(Key), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(KeyPropertyChangedCallback)));

        public KeyBindingTextBox()
        {
            IsReadOnly = true;
            MaxLength = 1;

            PreviewKeyDown += TextBox_PreviewKeyDown;
            GotFocus += TextBox_GotFocus;
            LostFocus += TextBox_LostFocus;
        }

        public Key Key
        {
            get { return (Key)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        private static void KeyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KeyBindingTextBox control)
            {
                control.Text = control.Key.ToString();
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            FontStyle = FontStyles.Normal;
            Key = e.Key;
            Text = e.Key.ToString();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            FontStyle = FontStyles.Italic;
            Text = "Please Enter Key Binding...";
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            FontStyle = FontStyles.Normal;
            Text = Key.ToString();
        }
    }
}
