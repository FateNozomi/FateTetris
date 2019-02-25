using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace FateTetris.Models
{
    public class InputEngine
    {
        private readonly TimeSpan _initialFrameTime = TimeSpan.FromMilliseconds(1000 / 8);
        private readonly TimeSpan _subsequentFrameTime = TimeSpan.FromMilliseconds(1000 / 60);
        private readonly TimeSpan _moveDownSubsequentFrameTime = TimeSpan.FromMilliseconds(1000 / 30);

        private Tetris _tetris;
        private DispatcherTimer _moveDownTimer;
        private DispatcherTimer _moveLeftTimer;
        private DispatcherTimer _moveRightTimer;

        public InputEngine(Tetris tetris)
        {
            _tetris = tetris;

            _moveDownTimer = new DispatcherTimer();
            _moveDownTimer.Interval = _initialFrameTime;
            _moveDownTimer.Tick += MoveDownTimer_Tick;

            _moveLeftTimer = new DispatcherTimer();
            _moveLeftTimer.Interval = _initialFrameTime;
            _moveLeftTimer.Tick += MoveLeftTimer_Tick;

            _moveRightTimer = new DispatcherTimer();
            _moveRightTimer.Interval = _initialFrameTime;
            _moveRightTimer.Tick += MoveRightTimer_Tick;

            LoadKeyBindings();
        }

        public List<KeyCommand> KeyCommands { get; set; }

        public void KeyDown(MovementCommand command)
        {
            switch (command)
            {
                case MovementCommand.HardDrop:
                    _tetris.HardDropBlock();
                    break;
                case MovementCommand.Up:
                    _tetris.MoveBlockUp();
                    break;
                case MovementCommand.SoftDrop:
                    if (!_moveDownTimer.IsEnabled)
                    {
                        _tetris.MoveBlockDown();
                        _moveDownTimer.Start();
                    }

                    break;
                case MovementCommand.Left:
                    if (!_moveLeftTimer.IsEnabled)
                    {
                        _tetris.MoveBlockLeft();
                        _moveLeftTimer.Start();
                    }

                    break;
                case MovementCommand.Right:
                    if (!_moveRightTimer.IsEnabled)
                    {
                        _tetris.MoveBlockRight();
                        _moveRightTimer.Start();
                    }

                    break;
                case MovementCommand.RotateLeft:
                    _tetris.RotateBlockLeft();
                    break;
                case MovementCommand.RotateRight:
                    _tetris.RotateBlockRight();
                    break;
                default:
                    break;
            }
        }

        public void KeyUp(MovementCommand command)
        {
            switch (command)
            {
                case MovementCommand.HardDrop:
                    break;
                case MovementCommand.Up:
                    break;
                case MovementCommand.SoftDrop:
                    _moveDownTimer.Stop();
                    _moveDownTimer.Interval = _initialFrameTime;
                    break;
                case MovementCommand.Left:
                    _moveLeftTimer.Stop();
                    _moveLeftTimer.Interval = _initialFrameTime;
                    break;
                case MovementCommand.Right:
                    _moveRightTimer.Stop();
                    _moveRightTimer.Interval = _initialFrameTime;
                    break;
                case MovementCommand.RotateLeft:
                    break;
                case MovementCommand.RotateRight:
                    break;
                default:
                    break;
            }
        }

        public void ClearInputs()
        {
            _moveDownTimer.Stop();
            _moveLeftTimer.Stop();
            _moveRightTimer.Stop();
        }

        public void LoadKeyBindings()
        {
            KeyCommands = new List<KeyCommand>();

            KeyCommands.Add(new KeyCommand(MovementCommand.HardDrop, (Key)Properties.Settings.Default.HardDrop));

            KeyCommands.Add(new KeyCommand(MovementCommand.Up, Key.V));
            KeyCommands.Add(new KeyCommand(MovementCommand.SoftDrop, (Key)Properties.Settings.Default.SoftDrop));
            KeyCommands.Add(new KeyCommand(MovementCommand.Left, (Key)Properties.Settings.Default.Left));
            KeyCommands.Add(new KeyCommand(MovementCommand.Right, (Key)Properties.Settings.Default.Right));
            KeyCommands.Add(new KeyCommand(MovementCommand.RotateLeft, (Key)Properties.Settings.Default.RotateLeft));
            KeyCommands.Add(new KeyCommand(MovementCommand.RotateRight, (Key)Properties.Settings.Default.RotateRight));
        }

        private void MoveDownTimer_Tick(object sender, EventArgs e)
        {
            _tetris.MoveBlockDown();
            _moveDownTimer.Interval = _moveDownSubsequentFrameTime;
        }

        private void MoveLeftTimer_Tick(object sender, EventArgs e)
        {
            _tetris.MoveBlockLeft();
            _moveLeftTimer.Interval = _subsequentFrameTime;
        }

        private void MoveRightTimer_Tick(object sender, EventArgs e)
        {
            _tetris.MoveBlockRight();
            _moveRightTimer.Interval = _subsequentFrameTime;
        }
    }
}
