using EmuPackDebug.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    class FillCommand: Command
    {
        public FillCommand(string commandString): base(commandString)
        {
            IsCommandValid = ValidateCommand(commandString);
        }

        public override CommandExecutionObject Execute(MachineState machineState)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateCommand(string commandString)
        {
            return base.ValidateCommand(commandString);
        }
    }

    class FillCommandResposne : CommandResponse
    {

    }
}
