using subtitles_console.CommandsParsing.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console.CommandsParsing
{
    public class BasicCommandParser : ICommandParser
    {
        public IEnumerable<Command> Parse(string[] commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands), "Cannot be null!");
            }

            if (commands.Length == 0)
            {
                throw new ArgumentException("Cannot be empty!", nameof(commands));
            }

            if (commands.Any(string.IsNullOrEmpty))
            {
                throw new ArgumentNullException(nameof(commands), "Cannot contain null arguments!");
            }

            List<string> commandsList = commands.ToList();
            List<Command> result = new List<Command>();

            while (commandsList.Count > 0)
            {
                var currentCommand = commandsList[0].ToLower();
                commandsList.RemoveAt(0);

                try
                {

                    if (currentCommand == "--delay")
                    {
                        result.Add(GetDelaySubtitlesCommand(commandsList));
                        commandsList = GetNewCommandsAfterSubtitlesDelay(commandsList);
                    } 
                    else if (currentCommand == "--merge")
                    {
                        result.Add(GetMergeSubtitlesCommand(commandsList));
                        commandsList = GetNewCommandsAfterSubtitlesMerge(commandsList);
                    }
                    else 
                    { 
                        throw new InvalidCommandException($"Invalid command {currentCommand} detected!", nameof(currentCommand));
                    }
                }
                catch (InvalidParameterValueException)
                {
                    throw;
                }
                catch (InvalidCommandException)
                {
                    throw;
                }
                catch (InvalidParameterException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }

        private List<string> GetNewCommandsAfterSubtitlesMerge(List<string> commands)
        {
            string[] validParameters = new string[] { "-sources", "-target" };
            bool sourcePathsAlreadyProcessed = false;
            bool targetAlreadyProcessed = false;
            List<string> currentCommands = new List<string>();
            commands.ForEach(currentCommands.Add);
            List<string> sourcePaths = new List<string>();
            string targetPath = string.Empty;

            while (currentCommands.Count > 0)
            {
                string currentParam = currentCommands[0].ToLower();

                if (sourcePathsAlreadyProcessed && targetAlreadyProcessed)
                {
                    return currentCommands;
                }

                if (!validParameters.Contains(currentParam) && !(sourcePathsAlreadyProcessed && targetAlreadyProcessed))
                {
                    throw new InvalidParameterException($"Invalid parameter \"{currentCommands[0]}\" for the subtitle merge detected!", nameof(currentCommands));
                }

                if (!validParameters.Contains(currentParam) && sourcePathsAlreadyProcessed && targetAlreadyProcessed)
                {
                    return currentCommands;
                }

                currentCommands.RemoveAt(0);

                if (currentParam == "-sources")
                {
                    string missingFilesErrorMessage = "The parameter -sources in --merge requires at least one file!";
                    string invalidFormatErrorMessage = "Invalid subtitle format in -sources in --merge was detected!";
                    sourcePaths = GetSourcePaths(currentCommands.ToArray(), invalidFormatErrorMessage, missingFilesErrorMessage);
                    sourcePathsAlreadyProcessed = true;
                    currentCommands = GetCommandsAfterSourcePaths(currentCommands.ToArray(), invalidFormatErrorMessage, missingFilesErrorMessage);
                }

                if (currentParam == "-target")
                {
                    string missingFilesErrorMessage = "The parameter -target in --merge requires one file!";
                    string invalidFormatErrorMessage = "Invalid subtitle format in -target in --merge was detected!";
                    targetPath = GetTargetPath(currentCommands.ToArray(), missingFilesErrorMessage, invalidFormatErrorMessage);
                    targetAlreadyProcessed = true;
                    currentCommands = GetCommandsAfterTargetPath(currentCommands.ToArray(), invalidFormatErrorMessage, missingFilesErrorMessage);
                }
            }

            if (sourcePathsAlreadyProcessed && targetAlreadyProcessed)
            {
                return currentCommands;
            }

            throw new InvalidCommandException("Invalid parameter in merge subtitles command detected!", nameof(commands));
        }

        private List<string> GetNewCommandsAfterSubtitlesDelay(List<string> commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands), "Cannot be null!");
            }

            string[] validParameters = new string[] { "-source", "-target", "-hours", "-minutes", "-seconds", "-milliseconds" };
            bool validTimeParameterAlreadyProcessed = false;
            bool sourceAlreadyProcessed = false;
            bool targetAlreadyProcessed = false;
            List<string> currentCommands = new List<string>();
            commands.ForEach(currentCommands.Add);

            while (currentCommands.Count > 0)
            {
                string currentParam = currentCommands[0];

                if (!validParameters.Contains(currentParam.ToLower()) && !(validTimeParameterAlreadyProcessed && targetAlreadyProcessed && sourceAlreadyProcessed))
                {
                    throw new InvalidParameterException($"Invalid parameter \"{currentCommands[0]}\" for the subtitle delay detected!", nameof(currentCommands));
                }

                if (!validParameters.Contains(currentParam.ToLower()) && validTimeParameterAlreadyProcessed && targetAlreadyProcessed && sourceAlreadyProcessed)
                {
                    return currentCommands;
                }

                currentCommands.RemoveAt(0);

                if (currentParam == "-source")
                {
                    currentCommands.RemoveAt(0);
                    sourceAlreadyProcessed = true;
                }

                if (currentParam == "-target")
                {
                    currentCommands.RemoveAt(0);
                    targetAlreadyProcessed = true;
                }

                if (currentParam == "-hours")
                {
                    currentCommands.RemoveAt(0);
                    validTimeParameterAlreadyProcessed = true;
                }

                if (currentParam == "-minutes")
                {
                    currentCommands.RemoveAt(0);
                    validTimeParameterAlreadyProcessed = true;
                }

                if (currentParam == "-seconds")
                {
                    currentCommands.RemoveAt(0);
                    validTimeParameterAlreadyProcessed = true;
                }

                if (currentParam == "-milliseconds")
                {
                    currentCommands.RemoveAt(0);
                    validTimeParameterAlreadyProcessed = true;
                }
            }

            if (validTimeParameterAlreadyProcessed && sourceAlreadyProcessed && targetAlreadyProcessed)
            {
                return currentCommands;
            }

            throw new InvalidCommandException("Invalid parameter in delay subtitle command detected!", nameof(commands));
        }

        private Command GetDelaySubtitlesCommand(List<string> commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands), "Cannot be null!");
            }

            string[] validParameters = new string[] { "-source", "-target", "-hours", "-minutes", "-seconds", "-milliseconds" };
            bool validTimeParameterAlreadyProcessed = false;
            bool sourceAlreadyProcessed = false;
            bool targetAlreadyProcessed = false;
            List<string> currentCommands = new List<string>();
            commands.ForEach(currentCommands.Add);
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            int milliseconds = 0;
            string sourcePath = string.Empty;
            string targetPath = string.Empty;

            while (currentCommands.Count > 0)
            {
                string currentParam = currentCommands[0].ToLower();

                if (!validParameters.Contains(currentParam) && !(validTimeParameterAlreadyProcessed && targetAlreadyProcessed && sourceAlreadyProcessed))
                {
                    throw new InvalidParameterException($"Invalid parameter \"{currentCommands[0]}\" for the subtitle delay detected!", nameof(currentCommands));
                }

                if (!validParameters.Contains(currentParam) && validTimeParameterAlreadyProcessed && targetAlreadyProcessed && sourceAlreadyProcessed)
                {
                    return new DelaySubtitlesCommand(sourcePath, targetPath, hours, seconds, minutes, milliseconds);
                }

                currentCommands.RemoveAt(0);

                if (currentParam == "-source")
                {
                    string missingParamMessage = "Source file was not specified!";
                    sourcePath = GetStringParameter(currentCommands.ToArray(), missingParamMessage);
                    sourceAlreadyProcessed = true;
                    currentCommands = GetCommandsAfterStringParameter(currentCommands.ToArray(), missingParamMessage);
                }

                if (currentParam == "-target")
                {
                    string missingParamMessage = "Target file was not specified!";
                    targetPath = GetStringParameter(currentCommands.ToArray(), missingParamMessage);
                    targetAlreadyProcessed = true;
                    currentCommands = GetCommandsAfterStringParameter(currentCommands.ToArray(), missingParamMessage);
                }

                if (currentParam == "-hours")
                {
                    string missingParamMessage = "Hours value was not specified!";
                    string wrongFormatMessage = "Hours value had an invalid format!";
                    hours = GetValidNumberParameter(currentCommands.ToArray(), missingParamMessage, wrongFormatMessage);
                    validTimeParameterAlreadyProcessed = true;
                    currentCommands = GetCommandsAfterValidNumberParameter(currentCommands.ToArray(), missingParamMessage, wrongFormatMessage);
                }

                if (currentParam == "-minutes")
                {
                    string missingParamMessage = "Minutes value was not specified!";
                    string wrongFormatMessage = "Minutes value had an invalid format!";
                    minutes = GetValidNumberParameter(currentCommands.ToArray(), missingParamMessage, wrongFormatMessage);
                    validTimeParameterAlreadyProcessed = true;
                    currentCommands = GetCommandsAfterValidNumberParameter(currentCommands.ToArray(), missingParamMessage, wrongFormatMessage);
                }

                if (currentParam == "-seconds")
                {
                    string missingParamMessage = "Seconds value was not specified!";
                    string wrongFormatMessage = "Seconds value had an invalid format!";
                    seconds = GetValidNumberParameter(currentCommands.ToArray(), missingParamMessage, wrongFormatMessage);
                    validTimeParameterAlreadyProcessed = true;
                    currentCommands = GetCommandsAfterValidNumberParameter(currentCommands.ToArray(), missingParamMessage, wrongFormatMessage);
                }

                if (currentParam == "-milliseconds")
                {
                    string missingParamMessage = "Milliseconds value was not specified!";
                    string wrongFormatMessage = "Milliseconds value had an invalid format!";
                    milliseconds = GetValidNumberParameter(currentCommands.ToArray(), missingParamMessage, wrongFormatMessage);
                    validTimeParameterAlreadyProcessed = true;
                    currentCommands = GetCommandsAfterValidNumberParameter(currentCommands.ToArray(), missingParamMessage, wrongFormatMessage);
                }
            }

            if (validTimeParameterAlreadyProcessed && sourceAlreadyProcessed && targetAlreadyProcessed)
            {
                return new DelaySubtitlesCommand(sourcePath, targetPath, hours, seconds, minutes, milliseconds);
            }

            throw new InvalidCommandException("Invalid parameter in delay subtitle command detected!", nameof(commands));
        } 

        private Command GetMergeSubtitlesCommand(List<string> commands)
        {
            string[] validParameters = new string[] { "-sources", "-target" };
            bool sourcePathsAlreadyProcessed = false;
            bool targetAlreadyProcessed = false;
            List<string> currentCommands = new List<string>();
            commands.ForEach(currentCommands.Add);
            List<string> sourcePaths = new List<string>();
            string targetPath = string.Empty;

            while (currentCommands.Count > 0)
            {
                string currentParam = currentCommands[0].ToLower();

                if (sourcePathsAlreadyProcessed && targetAlreadyProcessed)
                {
                    return new MergeSubtitlesCommand(sourcePaths.ToArray(), targetPath);
                }

                if (!validParameters.Contains(currentParam) && !(sourcePathsAlreadyProcessed && targetAlreadyProcessed))
                {
                    throw new InvalidParameterException($"Invalid parameter \"{currentCommands[0]}\" for the subtitle merge detected!", nameof(currentCommands));
                }

                if (!validParameters.Contains(currentParam) && sourcePathsAlreadyProcessed && targetAlreadyProcessed)
                {
                    return new MergeSubtitlesCommand(sourcePaths.ToArray(), targetPath);
                }

                currentCommands.RemoveAt(0);

                if (currentParam == "-sources")
                {
                    string missingFilesErrorMessage = "The parameter -sources in --merge requires at least one file!";
                    string invalidFormatErrorMessage = "Invalid subtitle format in -sources in --merge was detected!";
                    sourcePaths = GetSourcePaths(currentCommands.ToArray(), invalidFormatErrorMessage, missingFilesErrorMessage);
                    sourcePathsAlreadyProcessed = true;
                    currentCommands = GetCommandsAfterSourcePaths(currentCommands.ToArray(), invalidFormatErrorMessage, missingFilesErrorMessage);
                }

                if (currentParam == "-target")
                {
                    string missingFilesErrorMessage = "The parameter -target in --merge requires one file!";
                    string invalidFormatErrorMessage = "Invalid subtitle format in -target in --merge was detected!";
                    targetPath = GetTargetPath(currentCommands.ToArray(), missingFilesErrorMessage, invalidFormatErrorMessage);
                    targetAlreadyProcessed = true;
                    currentCommands = GetCommandsAfterTargetPath(currentCommands.ToArray(), invalidFormatErrorMessage, missingFilesErrorMessage);
                }
            }

            if (sourcePathsAlreadyProcessed && targetAlreadyProcessed)
            {
                return new MergeSubtitlesCommand(sourcePaths.ToArray(), targetPath);
            }

            throw new InvalidCommandException("Invalid parameter in merge subtitles command detected!", nameof(commands));
        }

        private List<string> GetSourcePaths(string[] commands, string invalidFileErrorMessage, string missingFilesErrorMessage)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands), "Cannot be null!");
            }

            if (commands.Length == 0)
            {
                throw new MissingFilesException(missingFilesErrorMessage);
            }

            List<string> files = new List<string>();

            foreach (var command in commands)
            {
                if (Path.GetExtension(command) != ".srt")
                {
                    if (files.Count == 0)
                    {
                        throw new InvalidFormatException(invalidFileErrorMessage);
                    }
                    else
                    {
                        break;
                    }
                }

                files.Add(command);
            }

            return files;
        }

        private List<string> GetCommandsAfterSourcePaths(string[] commands, string invalidFileErrorMessage, string missingFilesErrorMessage)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands), "Cannot be null!");
            }

            if (commands.Length == 0)
            {
                throw new MissingFilesException(missingFilesErrorMessage);
            }

            List<string> commandsNew = commands.ToList();
            List<string> files = new List<string>();

            while (commandsNew.Count > 0)
            {
                if (Path.GetExtension(commandsNew[0]) != ".srt")
                {
                    if (files.Count == 0)
                    {
                        throw new InvalidFormatException(invalidFileErrorMessage);
                    }
                    else
                    {
                        break;
                    }
                }

                files.Add(commandsNew[0]);
                commandsNew.RemoveAt(0);
            }

            return commandsNew;
        }

        private string GetTargetPath(string[] commands, string invalidFileErrorMessage, string missingFilesErrorMessage)
        {
            if (commands.Length == 0)
            {
                throw new MissingFilesException(missingFilesErrorMessage);
            }

            if (Path.GetExtension(commands[0]) != ".srt")
            {
                throw new InvalidFormatException(invalidFileErrorMessage);
            }

            return commands[0];
        }

        private List<string> GetCommandsAfterTargetPath(string[] commands, string invalidFileErrorMessage, string missingFilesErrorMessage)
        {
            if (commands.Length == 0)
            {
                throw new MissingFilesException(missingFilesErrorMessage);
            }

            if (Path.GetExtension(commands[0]) != ".srt")
            {
                throw new InvalidFormatException(invalidFileErrorMessage);
            }

            List<string> commandsNew = commands.ToList();
            commandsNew.RemoveAt(0);
            return commandsNew;
        }

        private List<string> GetCommandsAfterStringParameter(string[] commands, string missingParameterErrorMessage)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands), "Cannot be null!");
            }

            if (commands.Length == 0)
            {
                throw new MissingParameterValueException(missingParameterErrorMessage, nameof(commands));
            }

            List<string> commandsList = commands.ToList();
            commandsList.RemoveAt(0);
            return commandsList;
        }

        private string GetStringParameter(string[] commands, string missingParameterErrorMessage)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands), "Cannot be null!");
            }

            if (string.IsNullOrEmpty(missingParameterErrorMessage))
            {
                throw new ArgumentNullException(nameof(missingParameterErrorMessage), "Cannot be null or empty!");
            }

            if (commands.Length == 0)
            {
                throw new MissingParameterValueException(missingParameterErrorMessage, nameof(commands));
            }

            return commands[0];
        }

        private List<string> GetCommandsAfterValidNumberParameter(string[] commands, string missingParameterErrorMessage, string wrongFormatErrorMessage)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands), "Cannot be null!");
            }

            if (commands.Length == 0)
            {
                throw new MissingParameterValueException(missingParameterErrorMessage, nameof(commands));
            }

            if (string.IsNullOrEmpty(missingParameterErrorMessage))
            {
                throw new ArgumentNullException(nameof(missingParameterErrorMessage), "Cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(wrongFormatErrorMessage))
            {
                throw new ArgumentNullException(nameof(wrongFormatErrorMessage), "Cannot be null or empty!");
            }

            List<string> commandsList = commands.ToList();
            commandsList.RemoveAt(0);
            return commandsList;
        }

        private int GetValidNumberParameter(string[] commands, string missingParameterErrorMessage, string wrongFormatErrorMessage)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands), "Cannot be null!");
            }

            if (commands.Length == 0)
            {
                throw new MissingParameterValueException(missingParameterErrorMessage, nameof(commands));
            }

            if (string.IsNullOrEmpty(missingParameterErrorMessage))
            {
                throw new ArgumentNullException(nameof(missingParameterErrorMessage), "Cannot be null or empty!");
            }

            if (string.IsNullOrEmpty(wrongFormatErrorMessage))
            {
                throw new ArgumentNullException(nameof(wrongFormatErrorMessage), "Cannot be null or empty!");
            }

            int value;

            if (!int.TryParse(commands[0], out value))
            {
                throw new InvalidParameterValueException(wrongFormatErrorMessage, nameof(commands));
            }

            return value;
        }
    }
}
