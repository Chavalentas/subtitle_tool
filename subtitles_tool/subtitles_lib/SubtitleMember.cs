using subtitles_lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_lib
{
    public class SubtitleMember
    {
        private int _ordinalNumber;

        private string[] _content;

        public SubtitleMember(int ordinalNumber, DateTime timeStampBegin, DateTime timeStampEnd, string[] content)
        {
            OrdinalNumber = ordinalNumber;
            TimeStampBegin = timeStampBegin;
            TimeStampEnd = timeStampEnd;
            Content = content;
        }

        public int OrdinalNumber
        {
            get
            {
                return _ordinalNumber;
            }

            set
            {
                if (value <= 0)
                {
                    throw new SubtitleNumberOutOfRangeException(nameof(OrdinalNumber), "Cannot be less or equal to 0!");
                }

                _ordinalNumber = value;
            }
        }

        public DateTime TimeStampBegin
        {
            get;
            set;
        }

        public DateTime TimeStampEnd
        {
            get;
            set;
        }

        public string[] Content
        {
            get
            {
                return _content;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Content), "Cannot be null!");
                }

                _content = value;
            }
        }
    }
}
