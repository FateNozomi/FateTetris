using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using FateTetris.Models;
using Framework.MVVM;

namespace FateTetris.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        private Tetris _tetris;
        private List<KeyCommand> _keyCommands;
        private InputEngine _inputEngine;

        private uint _level;
        private uint _score;

        public MainWindowViewModel()
        {
            Init();
            WireCommands();
        }

        public ICollectionView TetrisGrid { get; set; }

        public uint Level { get => _level; set => SetProperty(ref _level, value); }

        public uint Score { get => _score; set => SetProperty(ref _score, value); }

        public RelayCommand StartCommand { get; private set; }

        public RelayCommand EndCommand { get; private set; }

        public RelayCommand PauseCommand { get; private set; }

        public RelayCommand KeyDownCommand { get; private set; }

        public RelayCommand KeyUpCommand { get; private set; }

        public void Init()
        {
            _tetris = new Tetris();
            _tetris.ScoreUpdated += Tetris_ScoreUpdated;
            _tetris.GameOver += Tetris_GameOver;
            _inputEngine = new InputEngine(_tetris);

            _keyCommands = new List<KeyCommand>();
            _keyCommands.Add(new KeyCommand(MovementCommand.HardDrop, Key.E));
            _keyCommands.Add(new KeyCommand(MovementCommand.Up, Key.Space));
            _keyCommands.Add(new KeyCommand(MovementCommand.Down, Key.D));
            _keyCommands.Add(new KeyCommand(MovementCommand.Left, Key.S));
            _keyCommands.Add(new KeyCommand(MovementCommand.Right, Key.F));
            _keyCommands.Add(new KeyCommand(MovementCommand.RotateLeft, Key.W));
            _keyCommands.Add(new KeyCommand(MovementCommand.RotateRight, Key.R));

            TetrisGrid = CollectionViewSource.GetDefaultView(_tetris.Engine.Grid);
        }

        private void WireCommands()
        {
            StartCommand = new RelayCommand(
                param =>
                {
                    _tetris.Start();
                },
                param => !_tetris.IsRunning);

            EndCommand = new RelayCommand(
                param =>
                {
                    _tetris.End();
                },
                param => _tetris.IsRunning);

            PauseCommand = new RelayCommand(
                param =>
                {
                    if (_tetris.Timer.IsEnabled)
                    {
                        _tetris.Timer.Stop();
                        _inputEngine.ClearInputs();
                        MessageBox.Show("ZA WARUDO! TOKI YO TOMARE!", "TETRIS");
                    }
                    else
                    {
                        _tetris.Timer.Start();
                        _inputEngine.ClearInputs();
                        MessageBox.Show("ZA WARUDO! TOKI WA UGOKI DASU!", "TETRIS");
                    }
                });

            KeyDownCommand = new RelayCommand(
                param =>
                {
                    if (param is Key key)
                    {
                        var keyCommand = _keyCommands.FirstOrDefault(k => k.KeyBinding == key);
                        if (keyCommand != null)
                        {
                            _inputEngine.KeyDown(keyCommand.Command);
                        }

                        switch (key)
                        {
                            case Key.Escape:
                                if (PauseCommand.CanExecute(null))
                                {
                                    PauseCommand.Execute(null);
                                }

                                break;

                            default:
                                break;
                        }
                    }
                },
                param => _tetris.IsRunning);

            KeyUpCommand = new RelayCommand(
                param =>
                {
                    if (param is Key key)
                    {
                        var keyCommand = _keyCommands.FirstOrDefault(k => k.KeyBinding == key);
                        if (keyCommand != null)
                        {
                            _inputEngine.KeyUp(keyCommand.Command);
                        }
                    }
                },
                param => _tetris.IsRunning);
        }

        private void Tetris_ScoreUpdated(object sender, EventArgs e)
        {
            if (sender is Tetris tetris)
            {
                Level = tetris.ScoreSystem.Level;
                Score = tetris.ScoreSystem.Score;
            }
        }

        private void Tetris_GameOver(object sender, EventArgs e)
        {
            _inputEngine.ClearInputs();
            MessageBox.Show("GAME OVER!", "TETRIS");
        }
    }
}
