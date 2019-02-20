using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FateTetris.Models
{
    public class KeyCommand
    {
        public KeyCommand(MovementCommand movementCommand, Key keyBinding)
        {
            Command = movementCommand;
            KeyBinding = keyBinding;
        }

        public MovementCommand Command { get; }

        public Key KeyBinding { get; private set; }
    }
}
