using EmuPackDebug.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    class CommandHandler
    {
        public CommandResponse ExecuteCommand(MachineState machineState, string commandString)
        {
            string index = GetCommandStringIndex(commandString);
            bool indexIsValid = ValidateCommandIndex(index);
            if (!indexIsValid)
            {
                return GetNotRecongnizedCommand();
            }

            return GetCommand(index, commandString).Execute(machineState);
        }

        private string GetCommandStringIndex(string commandString)
        {
            return string.Join("",
                commandString.Skip(CommandValues.IndexStartIndex)
                    .Take(CommandValues.IndexLength));
        }

        private bool ValidateCommandIndex(string index)
        {
            return CommandHandlerValues.CommandsIndexes.Keys.Contains(index);
        }

        private CommandResponse GetNotRecongnizedCommand()
        {
            return new NotRecognizedRequestResponse();
        }

        private Command GetCommand(string index, string commandString)
        {
            CommandHandlerValues.CommandsIndexes.TryGetValue(index, out string commandClass);
            string namespaceString = MethodBase.GetCurrentMethod().ReflectedType.Namespace;
            Type commandType = Type.GetType(namespaceString + "." + commandClass);
            ConstructorInfo constructor = commandType.GetConstructor(new[] { typeof(string) });
            object commandObject = constructor.Invoke(new object[] { commandString });

            return commandObject as Command;
        }
    }

    static class CommandHandlerValues
    {
        static public Dictionary<string, string> CommandsIndexes { private set; get; }

        static CommandHandlerValues()
        {
            CommandsIndexes = new Dictionary<string, string>
            {
                ["IN"] = "InitializationCommand",
                ["FL"] = "FillCommand",
                ["PR"] = "PrescriptionRegistrationCommand",
                ["MR"] = "MachineActivityRequestCommand",
                ["SR"] = "StatusRequestCommand"
            };
        }
    }
}
