using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing
{
    public class CommandIdentifier : ICommandVisitor<string>
    {
        public string Visit(DelaySubtitlesCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Cannot be null!");
            }

            return "delay";
        }

        public string Visit(MergeSubtitlesCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Cannot be null!");
            }

            return "merge";
        }
    }
}
