using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing
{
    public abstract class Command
    {
        public abstract void AcceptVisitor(ICommandVisitor commandVisitor);

        public abstract T AcceptVisitor<T>(ICommandVisitor<T> commandVisitor);
    }
}
