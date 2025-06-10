using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing.Exceptions
{
    public class InvalidFormatException: Exception
    {
        public InvalidFormatException(string message): base(message)
        {
        }
    }
}
