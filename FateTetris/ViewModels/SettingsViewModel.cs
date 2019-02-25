using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Framework.MVVM;

namespace FateTetris.ViewModels
{
    public class SettingsViewModel : PropertyChangedBase
    {
        private Key _hardDrop;
        private Key _softDrop;
        private Key _left;
        private Key _right;
        private Key _rotateLeft;
        private Key _rotateRight;
        private Key _hold;
        private uint _highScore;

        public SettingsViewModel()
        {
            WireCommands();
        }

        public event EventHandler Closing;

        public Key HardDrop { get => _hardDrop; set => SetProperty(ref _hardDrop, value); }

        public Key SoftDrop { get => _softDrop; set => SetProperty(ref _softDrop, value); }

        public Key Left { get => _left; set => SetProperty(ref _left, value); }

        public Key Right { get => _right; set => SetProperty(ref _right, value); }

        public Key RotateLeft { get => _rotateLeft; set => SetProperty(ref _rotateLeft, value); }

        public Key RotateRight { get => _rotateRight; set => SetProperty(ref _rotateRight, value); }

        public Key Hold { get => _hold; set => SetProperty(ref _hold, value); }

        public uint HighScore { get => _highScore; set => SetProperty(ref _highScore, value); }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand ResetHighScoreCommand { get; private set; }

        public void Init()
        {
            HardDrop = (Key)Properties.Settings.Default.HardDrop;
            SoftDrop = (Key)Properties.Settings.Default.SoftDrop;
            Left = (Key)Properties.Settings.Default.Left;
            Right = (Key)Properties.Settings.Default.Right;
            RotateLeft = (Key)Properties.Settings.Default.RotateLeft;
            RotateRight = (Key)Properties.Settings.Default.RotateRight;
            Hold = (Key)Properties.Settings.Default.Hold;

            HighScore = Properties.Settings.Default.HighScore;
        }

        private void WireCommands()
        {
            OkCommand = new RelayCommand(
                param =>
                {
                    Properties.Settings.Default.HardDrop = (int)HardDrop;
                    Properties.Settings.Default.SoftDrop = (int)SoftDrop;
                    Properties.Settings.Default.Left = (int)Left;
                    Properties.Settings.Default.Right = (int)Right;
                    Properties.Settings.Default.RotateLeft = (int)RotateLeft;
                    Properties.Settings.Default.RotateRight = (int)RotateRight;
                    Properties.Settings.Default.Hold = (int)Hold;

                    Properties.Settings.Default.HighScore = HighScore;

                    Properties.Settings.Default.Save();
                    Closing?.Invoke(this, null);
                });

            ResetHighScoreCommand = new RelayCommand(
                param =>
                {
                    HighScore = 0;
                });
        }
    }
}
