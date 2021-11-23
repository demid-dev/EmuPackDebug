using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug.Commands
{
    class CommandExecutionObject
    {
        public Func<CommandResponse> CommandToExecute { get; private set; }
        public int ExecutionTime { get; private set; }
        public bool MachineMechanicalPartUsed { get; private set;  }

        public CommandExecutionObject(Func<CommandResponse> commandToExecute,
            int executionTime, bool machineMechanicalPartUsed)
        {
            CommandToExecute = commandToExecute;
            ExecutionTime = executionTime;
            MachineMechanicalPartUsed = machineMechanicalPartUsed;
        }
    }
}
