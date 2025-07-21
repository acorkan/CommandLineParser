using System.Diagnostics;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace CommandLineParser
{
    public class CLParser
    {
        private RootCommand _rootCommand;
        private string[] _cmdLineArgs;
        private ParseResult _parseResult;
        //_cmdLineArgs = Environment.GetCommandLineArgs();


        public string AppPath { get; }

        public CLParser()
        {
            var args = new List<string>(Environment.GetCommandLineArgs());
            AppPath = args[0];
            args.RemoveAt(0); // Remove the application path from the arguments
            _cmdLineArgs = args.ToArray();
        }

        private void ParseCommandLineArgs()
        {
            if (_parseResult == null)
            {
                _parseResult = _rootCommand.Parse(_cmdLineArgs);
            }
        }
        public void Add(Option option)
        {
            if(_rootCommand == null)
            {
                _rootCommand = new RootCommand();
            }
            _rootCommand.Add(option);
        }

        public bool HasErrors()
        {
            return GetErrors().Any();
        }

        public List<string> GetErrors()
        {
            ParseCommandLineArgs();
            return _parseResult.Errors.Select(e => e.Message).ToList();
        }

        public T GetValueForOption<T>(Option<T> option)
        {
            ParseCommandLineArgs();
            if (_parseResult.Errors.Any())
            {
                throw new ArgumentException("Error parsing command line arguments: " + string.Join(", ", _parseResult.Errors.Select(e => e.Message)));
            }
            return _parseResult.GetValue<T>(option);
        }
        public T GetValueForOption<T>(string optionName)
        {
            ParseCommandLineArgs();
            if (_parseResult.Errors.Any())
            {
                throw new ArgumentException("Error parsing command line arguments: " + string.Join(", ", _parseResult.Errors.Select(e => e.Message)));
            }
            var option = _rootCommand.Options.FirstOrDefault(o => o.Name == optionName);
            return _parseResult.GetValue<T>(option as Option<T>);
        }
    }
}
