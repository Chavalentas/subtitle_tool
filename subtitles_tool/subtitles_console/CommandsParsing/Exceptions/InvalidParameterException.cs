using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing.Exceptions
{
    public class InvalidParameterException : ArgumentException
    {
        public InvalidParameterException(string message, string paramName) : base(message, paramName)
        {
        }
    }
}
