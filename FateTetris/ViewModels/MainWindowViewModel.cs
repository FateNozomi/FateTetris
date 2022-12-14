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
using FateTetris.Views;
using Framework.MVVM;

namespace FateTetris.ViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        private Tetris _tetris;
        private InputEngine _inputEngine;

        private ICollectionView _tetrisGrid;
        private ICollectionView _holdGrid;
        private ICollectionView _previewGrid;
        private ICollectionView _actions;

        private uint _level;
        private uint _score;
        private uint _lastHighScore;

        public MainWindowViewModel()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            BuildVersion = version.ToString();

            Init(10, 20);
            WireCommands();
        }

        public string BuildVersion { get; set; }

        public IWindowService WindowService { get; set; } = new WindowService();

        public ICollectionView TetrisGrid { get => _tetrisGrid; set => SetProperty(ref _tetrisGrid, value); }

        public ICollectionView HoldGrid { get => _holdGrid; set => SetProperty(ref _holdGrid, value); }

        public ICollectionView PreviewGrid { get => _previewGrid; set => SetProperty(ref _previewGrid, value); }

        public ICollectionView Actions { get => _actions; set => SetProperty(ref _actions, value); }

        public uint Level { get => _level; set => SetProperty(ref _level, value); }

        public uint Score { get => _score; set => SetProperty(ref _score, value); }

        public uint LastHighScore { get => _lastHighScore; set => SetProperty(ref _lastHighScore, value); }

        public RelayCommand StartCommand { get; private set; }

        public RelayCommand EndCommand { get; private set; }

        public RelayCommand SettingsCommand { get; private set; }

        public RelayCommand PauseCommand { get; private set; }

        public RelayCommand KeyDownCommand { get; private set; }

        public RelayCommand KeyUpCommand { get; private set; }

        public void Init(int x, int y)
        {
            _tetris = new Tetris(x, y);
            _tetris.ScoreUpdated += Tetris_ScoreUpdated;
            _tetris.GameOver += Tetris_GameOver;

            TetrisGrid = CollectionViewSource.GetDefaultView(_tetris.Engine.Grid);
            PreviewGrid = CollectionViewSource.GetDefaultView(_tetris.Preview.Engine.Grid);
            HoldGrid = CollectionViewSource.GetDefaultView(_tetris.Holder.Engine.Grid);
            Actions = CollectionViewSource.GetDefaultView(_tetris.ScoreSystem.Actions.Reverse());

            _inputEngine = new InputEngine(_tetris);

            LastHighScore = Properties.Settings.Default.HighScore;
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

            SettingsCommand = new RelayCommand(
                param =>
                {
                    var vm = new SettingsViewModel();
                    vm.Columns = _tetris.Engine.X;
                    vm.Rows = _tetris.Engine.Y;
                    vm.Init();
                    var result = WindowService.ShowDialog<SettingsView>(vm);
                    if (result == true)
                    {
                        _inputEngine.LoadKeyBindings();
                        _tetris.ScoreSystem.LevelCap = Properties.Settings.Default.LevelCap;
                        LastHighScore = Properties.Settings.Default.HighScore;

                        Init(vm.Columns, vm.Rows);
                        var windowState = Application.Current.MainWindow.WindowState;
                        Application.Current.MainWindow.WindowState = WindowState.Maximized;
                        Application.Current.MainWindow.WindowState = WindowState.Normal;
                        Application.Current.MainWindow.WindowState = windowState;
                    }
                },
                param => !_tetris.IsRunning);

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
                        var keyCommand = _inputEngine.KeyCommands.FirstOrDefault(k => k.KeyBinding == key);
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
                        var keyCommand = _inputEngine.KeyCommands.FirstOrDefault(k => k.KeyBinding == key);
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
                Actions.Refresh();

                if (Score > LastHighScore)
                {
                    LastHighScore = Score;
                    Properties.Settings.Default.HighScore = LastHighScore;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void Tetris_GameOver(object sender, EventArgs e)
        {
            _inputEngine.ClearInputs();
            MessageBox.Show("GAME OVER!", "TETRIS");
        }
    }
}
