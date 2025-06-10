using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_lib.Exceptions
{
    public class InvalidSubtitleMemberContentException : ArgumentException
    {
        public InvalidSubtitleMemberContentException(string paramName, string message): base(message, paramName)
        {
        }
    }
}
