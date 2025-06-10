// See https://aka.ms/new-console-template for more information
using subtitles_console;
using subtitles_console.CommandsParsing;
using subtitles_lib;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

List<string> commandLineArgs = Environment.GetCommandLineArgs().ToList();
commandLineArgs.RemoveAt(0);
Application application = new Application();
application.Start(commandLineArgs.ToArray());