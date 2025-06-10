using subtitles_lib;
using subtitles_lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing
{
    public class CommandExecutor : ICommandVisitor
    {
        public void Visit(DelaySubtitlesCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Cannot be null!");
            }

            try
            {
                SubtitleTool subtitleTool = new SubtitleTool();
                subtitleTool.DelaySubtitles(command.SourceFile, command.DelayHours, command.DelayMinutes, command.DelaySeconds, command.DelayMilliseconds, command.TargetFile);
            }
            catch (InvalidSubtitleFileException ex)
            {
                throw;
            }
            catch (InvalidSubtitleTimeStampException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Visit(MergeSubtitlesCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command), "Cannot be null!");
            }

            try
            {
                SubtitleTool subtitleTool = new SubtitleTool();
                subtitleTool.MergeSubtitleFiles(command.SourcePaths, command.TargetPath);
            }
            catch (SubtitleNumberOutOfRangeException ex)
            {
                throw;
            }
            catch (InvalidSubtitleMemberContentException ex)
            {
                throw;
            }
            catch (InvalidSubtitleFileException ex)
            {
                throw;
            }
            catch (InvalidSubtitleTimeStampException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
