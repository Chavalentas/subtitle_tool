using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing
{
    public class DelaySubtitlesCommand : Command
    {
        private string _sourceFile;

        private string _targetFile;

        private int _delayHours;

        private int _delayMinutes;

        private int _delaySeconds;

        private int _delayMilliseconds;

        public DelaySubtitlesCommand(string sourceFile, string targetFile, int delayHours, int delaySeconds, int delayMinutes, int delayMilliseconds)
        {
            DelayHours = delayHours;
            DelayMinutes = delayMinutes;
            DelaySeconds = delaySeconds;
            DelayMilliseconds = delayMilliseconds;
            SourceFile = sourceFile;
            TargetFile = targetFile;
        }

        public int DelayHours
        {
            get
            {
                return _delayHours;
            }

            set
            {
                _delayHours = value;
            }
        }

        public int DelayMinutes
        {
            get
            {
                return _delayMinutes;
            }

            set
            {
                _delayMinutes = value;
            }
        }

        public int DelaySeconds
        {
            get
            {
                return _delaySeconds;
            }

            set
            {
                _delaySeconds = value;
            }
        }

        public int DelayMilliseconds
        {
            get
            {
                return _delayMilliseconds;
            }

            set
            {
                _delayMilliseconds = value;
            }
        }

        public string SourceFile
        {
            get
            {
                return _sourceFile;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(SourceFile), "Cannot be null or empty!");
                }

                _sourceFile = value;
            }
        }

        public string TargetFile
        {
            get
            {
                return _targetFile;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(TargetFile), "Cannot be null or empty!");
                }

                _targetFile = value;
            }
        }

        public override void AcceptVisitor(ICommandVisitor commandVisitor)
        {
            if (commandVisitor  == null)
            {
                throw new ArgumentNullException(nameof(commandVisitor), "Cannot be null!");
            }

            commandVisitor.Visit(this);
        }

        public override T AcceptVisitor<T>(ICommandVisitor<T> commandVisitor)
        {
            if (commandVisitor == null)
            {
                throw new ArgumentNullException(nameof(commandVisitor), "Cannot be null!");
            }

            return commandVisitor.Visit(this);
        }
    }
}
