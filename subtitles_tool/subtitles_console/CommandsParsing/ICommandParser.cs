using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing
{
    public interface ICommandParser
    {
        IEnumerable<Command> Parse(string[] commands);
    }
}
