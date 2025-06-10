using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_lib.Exceptions
{
    public class InvalidSubtitleFileException : Exception
    {
        public InvalidSubtitleFileException(string message): base(message)
        {
        }

        public InvalidSubtitleFileException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}
