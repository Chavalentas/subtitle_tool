using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_lib.Exceptions
{
    public class InvalidSubtitleTimeStampException : Exception
    {
        public InvalidSubtitleTimeStampException(string message): base(message)
        {
        }
    }
}
