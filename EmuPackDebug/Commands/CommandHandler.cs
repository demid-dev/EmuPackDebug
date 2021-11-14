using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    class CommandHandler
    {
        public CommandResponse ExecuteCommand(string commandString)
        {
            string index = GetCommandStringIndex(commandString);
            bool indexIsValid = ValidateCommandIndex(index);
            if (!indexIsValid)
            {
                NotRecognizedRequestResponse response = new NotRecognizedRequestResponse();
                return response;
            }

            ParseAndExecuteCommand(index, commandString);
        }

        private string GetCommandStringIndex(string commandString)
        {
            return commandString.Substring(CommandValues.IndexStartIndex, 
                CommandValues.IndexLength);
        }
        
        private bool ValidateCommandIndex(string index)
        {
            return CommandHandlerValues.CommandIndexes.Contains(index);
        }


    }

    static class CommandHandlerValues
    {
        static public Dictionary<string, string> CommandsIndexes { private set; get; }

        static CommandHandlerValues()
        {
            CommandsIndexes = new Dictionary<string, string>
            {
                ["IN"] = ""
            }
        }
    }
}
