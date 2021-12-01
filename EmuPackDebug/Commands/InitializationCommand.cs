using EmuPackDebug.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    class InitializationCommand : Command
    {
        public InitializationCommand(string commandString) : base(commandString)
        {
            IsCommandValid = ValidateCommand(commandString);
            Console.WriteLine(IsCommandValid);
            Console.WriteLine(CommandId + SendFrom + SendTo + DataLength);
        }

        public override bool ValidateCommand(string commandString)
        {
            if (!base.ValidateCommand(commandString))
                return false;
            if (CommandId != InitializationCommandValues.CommandId)
                return false;

            return true;
        }

        public override CommandExecutionObject Execute(MachineState machineState)
        {
            Func<InitialiaztionCommandResposne> commandToExecute = () =>
            {
                return new InitialiaztionCommandResposne(CommandResponseCodes.Sucess);
            };
            int executionTime = InitializationCommandValues.CommandExecutionTime;
            bool machineMechanicalPartUsed = true;

            return new CommandExecutionObject(commandToExecute,
                executionTime, machineMechanicalPartUsed);
        }
    }

    static class InitializationCommandValues
    {
        static public string CommandId { get; private set; }
        static public int CommandExecutionTime { get; private set; }

        static InitializationCommandValues()
        {
            CommandId = "IN";
            CommandExecutionTime = 30000;
        }
    }

    class InitialiaztionCommandResposne : CommandResponse
    {
        public InitialiaztionCommandResposne(CommandResponseCodes code) : base(code)
        {
            CommandId = InitializationCommandValues.CommandId;
            DataLength = "00002";
        }
    }
}
