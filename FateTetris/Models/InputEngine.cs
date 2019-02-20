using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FateTetris.Models
{
    public class InputEngine
    {
        private readonly TimeSpan _initialFrameTime = TimeSpan.FromMilliseconds(1000 / 8);
        private readonly TimeSpan _subsequentFrameTime = TimeSpan.FromMilliseconds(1000 / 60);

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
        }

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
                case MovementCommand.Down:
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
                case MovementCommand.Down:
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

        private void MoveDownTimer_Tick(object sender, EventArgs e)
        {
            _tetris.MoveBlockDown();
            _moveDownTimer.Interval = _subsequentFrameTime;
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
