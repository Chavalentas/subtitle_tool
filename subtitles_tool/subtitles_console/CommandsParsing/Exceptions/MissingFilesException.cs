using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing.Exceptions
{
    public class MissingFilesException : Exception
    {
        public MissingFilesException(string message): base(message)
        {
        }
    }
}
