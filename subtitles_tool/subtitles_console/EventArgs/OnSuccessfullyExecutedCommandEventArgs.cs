using subtitles_console.CommandsParsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.Events
{
    public class OnSuccessfullyExecutedCommandEventArgs : EventArgs
    {
        private Command _command;

        public OnSuccessfullyExecutedCommandEventArgs(Command command)
        {
            ExecutedCommand = command;
        }

        public Command ExecutedCommand
        {
            get
            {
                return _command;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(ExecutedCommand), "Cannot be null!");
                }

                _command = value;
            }
        }
    }
}
