using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing
{
    public interface ICommandVisitor
    {
        void Visit(DelaySubtitlesCommand command);

        void Visit(MergeSubtitlesCommand command);
    }
}
