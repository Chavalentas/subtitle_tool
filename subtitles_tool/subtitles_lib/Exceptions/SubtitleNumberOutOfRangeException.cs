using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_lib.Exceptions
{
    public class SubtitleNumberOutOfRangeException : ArgumentOutOfRangeException
    {
        public SubtitleNumberOutOfRangeException(string paramName, string message): base(paramName, message)
        {
        }
    }
}
