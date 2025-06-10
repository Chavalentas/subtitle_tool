using subtitles_console.CommandsParsing;
using subtitles_console.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subtitles_console
{
    public class Application
    {
        public void Start(string[] args)
        {
            if  (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            try
            {
                var basicCommandParser = new BasicCommandParser();
                var commands = basicCommandParser.Parse(args);
                var commandsQueueExecutor = new CommandsQueueExecutor();
                commandsQueueExecutor.OnSuccessfullyExecutedCommand += ExecutedCommandCallback;
                commandsQueueExecutor.Execute(commands.ToList());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.Write(ParseArgumentOutOfRangeExceptionMessage(ex));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ParseArgumentExceptionMessage(ex));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ExecutedCommandCallback(object sender, OnSuccessfullyExecutedCommandEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args), "Cannot be null!");
            }

            var identifier = new CommandIdentifier();
            string id = args.ExecutedCommand.AcceptVisitor(identifier);

            if (id == "delay")
            {
                Console.WriteLine("Delay command was successfully executed!");
                return;
            }
            
            if (id == "merge")
            {
                Console.Write("Merge command was successfully executed!");
                return;
            }
        }

        private string ParseArgumentExceptionMessage(ArgumentException ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex), "Cannot be null!");
            }

            int indexOfParam = ex.Message.IndexOf("(Parameter");

            if (indexOfParam == -1)
            {
                return ex.Message;
            }

            string newString = ex.Message.Substring(0, indexOfParam - 1);
            return newString;
        }

        private string ParseArgumentOutOfRangeExceptionMessage(ArgumentOutOfRangeException ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex), "Cannot be null!");
            }

            int indexOfParam = ex.Message.IndexOf("(Parameter");

            if (indexOfParam == -1)
            {
                return ex.Message;
            }

            string newString = ex.Message.Substring(0, indexOfParam - 1);
            return newString;
        }
    }
}
