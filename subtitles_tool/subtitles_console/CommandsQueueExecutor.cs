using subtitles_console.CommandsParsing;
using subtitles_console.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console
{
    public class CommandsQueueExecutor
    {
        public event EventHandler<OnSuccessfullyExecutedCommandEventArgs> OnSuccessfullyExecutedCommand;

        public void Execute(List<Command> commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands), "Cannot be null!");
            }

            var commandExecutor = new CommandExecutor();

            foreach (var command in commands)
            {
                command.AcceptVisitor(commandExecutor);
                FireOnSuccessfullyExecutedCommand(new OnSuccessfullyExecutedCommandEventArgs(command));
            }
        }

        protected virtual void FireOnSuccessfullyExecutedCommand(OnSuccessfullyExecutedCommandEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            OnSuccessfullyExecutedCommand?.Invoke(this, args);
        }
    }
}
