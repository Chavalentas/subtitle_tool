using subtitles_lib.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace subtitles_lib
{
    public class SubtitleTool
    {
        public string[] ReadSubtitleFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "Cannot be null or empty!");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The file under the given path \"{path}\" does not exist!", path);
            }

            try
            {
                ExtensionFileReader extensionFileReader = new ExtensionFileReader(".srt");
                string[] lines = extensionFileReader.ReadFile(path);
                return lines;
            }
            catch (Exception ex)
            {
                throw new InvalidSubtitleFileException($"Invalid subtitle file format in \"{path}\" detected!", ex);
            }
        }

        public void MergeSubtitleFiles(string[] subtitleFiles, string targetFile)
        {
            if (subtitleFiles == null)
            {
                throw new ArgumentNullException(nameof(subtitleFiles), "Cannot be null!");
            }

            if (string.IsNullOrEmpty(targetFile))
            {
                throw new ArgumentNullException(nameof(targetFile), "Cannot be null or empty!");
            }

            List<string> subtitleFilesList = subtitleFiles.ToList();

            subtitleFilesList.ForEach(file =>
            {
                if (!File.Exists(file))
                {
                    throw new FileNotFoundException($"The file under the given path \"{file}\" does not exist!", file);
                }
            });

            List<string[]> subtitleContents = subtitleFilesList.Select(ReadSubtitleFile).ToList();
            string[] contentConcat = Helper.Concat(subtitleContents, "").ToArray();
            List<string[]> contentsSplitted = contentConcat.Split("").ToList();
            List<SubtitleMember> members = contentsSplitted.Select(c => ExtractSubtitleMember(c)).ToList();
            members.Sort((a, b) => a.TimeStampBegin.CompareTo(b.TimeStampBegin));
            members = EliminateDuplicatesByStartTime(members);
            Enumerable.Range(0, members.Count).ToList().ForEach((index) => {
                members[index].OrdinalNumber = index + 1;
            });
            string[] toLines = members.Select(m => MemberToLines(m)).ToList().Concat("").ToArray();
            File.WriteAllLines(targetFile, toLines);
        }

        public void DelaySubtitles(string subtitleFile, int delayHours, int delayMinutes, int delaySeconds, int delayMilli, string targetPath)
        {
            if (string.IsNullOrEmpty(subtitleFile))
            {
                throw new ArgumentNullException(nameof(subtitleFile), "Cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(targetPath))
            {
                throw new ArgumentNullException(nameof(targetPath), "Cannot be null or empty!");
            }

            if (!File.Exists(subtitleFile))
            {
                throw new FileNotFoundException($"The file under the given path \"{subtitleFile}\" does not exist!", subtitleFile);
            }

            string[] lines = ReadSubtitleFile(subtitleFile);
            var timeStampsLinesRegex = new Regex("[0-9][0-9]:[0-9][0-9]:[0-9][0-9],[0-9][0-9][0-9] --> [0-9][0-9]:[0-9][0-9]:[0-9][0-9],[0-9][0-9][0-9]");
            int[] indexes = lines.GetIndexesIfMatch(timeStampsLinesRegex.IsMatch).ToArray();
            string[] timeStampLines = indexes.Select(i => lines[i]).ToArray();
            string[] timeStampsAdapted = timeStampLines.Select(l => AdaptTimestamp(l, delayHours, delayMinutes, delaySeconds, delayMilli)).ToArray();
            string[] newLines = lines.ReplaceAtIndexes(indexes, timeStampsAdapted).ToArray();
            File.WriteAllLines(targetPath, newLines);
        }

        private SubtitleMember ExtractSubtitleMember(string[] memberContent)
        {
            if (memberContent == null)
            {
                throw new ArgumentNullException(nameof(memberContent), "Cannot be null!");
            }

            if (!(memberContent.Length >= 2))
            {
                throw new InvalidSubtitleMemberContentException(nameof(memberContent), "Has to contain at least subtitle number and timestamp!");
            }

            int subtitleNumber;

            if (!int.TryParse(memberContent[0], out subtitleNumber))
            {
                throw new InvalidSubtitleMemberContentException(nameof(memberContent), "The subtitle number had an invalid format!");
            }

            if (subtitleNumber <= 0)
            {
                throw new InvalidSubtitleMemberContentException(nameof(memberContent), "The subtitle number has to be greater than 0!");
            }

            DateTime startTime = ParseStartTime(memberContent[1]);
            DateTime endTime = ParseEndTime(memberContent[1]);
            string[] content = ParseContent(memberContent);
            SubtitleMember subtitleMember = new SubtitleMember(subtitleNumber, startTime, endTime, content);
            return subtitleMember;
        }

        private string[] ParseContent(string[] memberContent)
        {
            if (memberContent == null)
            {
                throw new ArgumentNullException(nameof(memberContent), "Cannot be null!");
            }

            if (!(memberContent.Length >= 2))
            {
                throw new InvalidSubtitleMemberContentException(nameof(memberContent), "Has to contain at least subtitle number and timestamp!");
            }

            List<string> contentToList = memberContent.ToList();
            
            if (memberContent.Length == 2)
            {
                return new string[] { "" };
            }

            List<string> result = contentToList.GetRange(2, memberContent.Length - 2);
            return result.ToArray();
        }

        private DateTime ParseStartTime(string timeStampLine)
        {
            if (string.IsNullOrEmpty(timeStampLine))
            {
                throw new ArgumentNullException(nameof(timeStampLine), "Cannot be null or empty!");
            }

            var wholeLineRegex = new Regex("[0-9][0-9]:[0-9][0-9]:[0-9][0-9],[0-9][0-9][0-9] --> [0-9][0-9]:[0-9][0-9]:[0-9][0-9],[0-9][0-9][0-9]");
            var timeStampRegex = new Regex("[0-9][0-9]:[0-9][0-9]:[0-9][0-9],[0-9][0-9][0-9]");
            var wholeLineMatches = wholeLineRegex.Matches(timeStampLine);
            var matches = timeStampRegex.Matches(timeStampLine);

            if (wholeLineMatches.Count != 1)
            {
                throw new InvalidSubtitleMemberContentException(nameof(timeStampLine), "Timestamp line had a wrong format!");
            }

            if (matches.Count != 2)
            {
                throw new InvalidSubtitleTimeStampException("The timestamp has to contain start and end time!");
            }

            DateTime startTime;

            if (!DateTime.TryParse(matches[0].Value, out startTime))
            {
                throw new InvalidSubtitleMemberContentException(nameof(timeStampLine), "The start time had an invalid type!");
            }

            return startTime;
        }

        private DateTime ParseEndTime(string timeStampLine)
        {
            if (string.IsNullOrEmpty(timeStampLine))
            {
                throw new ArgumentNullException(nameof(timeStampLine), "Cannot be null or empty!");
            }

            var wholeLineRegex = new Regex("[0-9][0-9]:[0-9][0-9]:[0-9][0-9],[0-9][0-9][0-9] --> [0-9][0-9]:[0-9][0-9]:[0-9][0-9],[0-9][0-9][0-9]");
            var timeStampRegex = new Regex("[0-9][0-9]:[0-9][0-9]:[0-9][0-9],[0-9][0-9][0-9]");
            var wholeLineMatches = wholeLineRegex.Matches(timeStampLine);
            var matches = timeStampRegex.Matches(timeStampLine);

            if (wholeLineMatches.Count != 1)
            {
                throw new InvalidSubtitleMemberContentException(nameof(timeStampLine), "Timestamp line had a wrong format!");
            }

            if (matches.Count != 2)
            {
                throw new InvalidSubtitleTimeStampException("The timestamp has to contain start and end time!");
            }

            DateTime endTime;

            if (!DateTime.TryParse(matches[1].Value, out endTime))
            {
                throw new InvalidSubtitleMemberContentException(nameof(timeStampLine), "The start time had an invalid type!");
            }

            return endTime;
        } 

        private string AdaptTimestamp(string timeStampTextFormat, int delayHours, int delayMinutes, int delaySeconds, int delayMilli)
        {
            if (string.IsNullOrEmpty(timeStampTextFormat))
            {
                throw new ArgumentNullException(nameof(timeStampTextFormat), "Cannot be null or empty!");
            }

            var timeStampRegex = new Regex("[0-9][0-9]:[0-9][0-9]:[0-9][0-9],[0-9][0-9][0-9]");
            var matches = timeStampRegex.Matches(timeStampTextFormat);
            
            if (matches.Count != 2)
            {
                throw new InvalidSubtitleTimeStampException("The timestamp has to contain start and end time!");
            }

            DateTime startTime;
            DateTime endTime;

            if (!DateTime.TryParse(matches[0].Value, out startTime))
            {
                throw new InvalidDataException("The start time had an invalid type!");
            }

            if (!DateTime.TryParse(matches[1].Value, out endTime))
            {
                throw new InvalidDataException("The end time had an invalid type!");
            }

            startTime = startTime.AddHours(delayHours);
            startTime = startTime.AddMinutes(delayMinutes);
            startTime = startTime.AddSeconds(delaySeconds);
            startTime = startTime.AddMilliseconds(delayMilli);
            endTime = endTime.AddHours(delayHours);
            endTime = endTime.AddMinutes(delayMinutes);
            endTime = endTime.AddSeconds(delaySeconds);
            endTime = endTime.AddMilliseconds(delayMilli);
            string startStringModified = startTime.ToString("HH:mm:ss,fff");
            string endStringModified = endTime.ToString("HH:mm:ss,fff");
            string result = startStringModified + " --> " + endStringModified;
            return result;
        }

        private List<SubtitleMember> EliminateDuplicatesByStartTime(List<SubtitleMember> subtitleMembers)
        {
            if (subtitleMembers == null)
            {
                throw new ArgumentNullException(nameof(subtitleMembers), "Cannot be null!");
            }

            List<SubtitleMember> result = new List<SubtitleMember>();

            for (int i = 0; i < subtitleMembers.Count; i++)
            {
                if (result.Any(m => m.TimeStampBegin == subtitleMembers[i].TimeStampBegin))
                {
                    continue;
                }

                result.Add(subtitleMembers[i]);
            }

            return result;
        }

        private string[] MemberToLines(SubtitleMember subtitleMember)
        {
            if (subtitleMember == null)
            {
                throw new ArgumentNullException(nameof(subtitleMember), "Cannot be null!");
            }

            List<string> result = new List<string>();
            result.Add(Convert.ToString(subtitleMember.OrdinalNumber));
            string startStringModified = subtitleMember.TimeStampBegin.ToString("HH:mm:ss,fff");
            string endStringModified = subtitleMember.TimeStampEnd.ToString("HH:mm:ss,fff");
            string finalTime = startStringModified + " --> " + endStringModified;
            result.Add(finalTime);
            subtitleMember.Content.ToList().ForEach(l => result.Add(l));
            return result.ToArray();
        }
    }
}
