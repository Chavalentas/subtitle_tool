using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing
{
    public class MergeSubtitlesCommand : Command
    {
        private string _targetPath;

        private string[] _sourcePaths;

        public MergeSubtitlesCommand(string[] sourcePaths, string targetPath)
        {
            SourcePaths = sourcePaths;
            TargetPath = targetPath;
        }

        public string[] SourcePaths
        {
            get
            {
                return _sourcePaths;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(SourcePaths), "Cannot be null!");
                }

                _sourcePaths = value;
            }
        }

        public string TargetPath
        {
            get
            {
                return _targetPath;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(TargetPath), "Cannot be null!");
                }

                _targetPath = value;
            }
        }

        public override void AcceptVisitor(ICommandVisitor commandVisitor)
        {
            if (commandVisitor == null)
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
