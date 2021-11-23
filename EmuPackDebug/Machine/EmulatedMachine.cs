using EmuPackDebug.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmuPackDebug.Machine
{
    class EmulatedMachine
    {
        private Thread _commandsInputThread;
        private Thread _commandsOutputThread;
        private Queue<CommandResponse> _responsesQueue;
        private CommandHandler _commandHandler;

        public MachineState MachineState { get; private set; }

        public EmulatedMachine()
        {
            _responsesQueue = new Queue<CommandResponse>();
            _commandHandler = new CommandHandler();
            MachineState = new MachineState();
        }

        public void ReceiveMessage(string message)
        {
            ExecuteCommand(_commandHandler.ExecuteCommand(MachineState, "INC1M100000"));
        }

        private void ExecuteCommand(CommandExecutionObject executionObject)
        {
            Task task = Task.Run(async delegate
            {
                if (executionObject.MachineMechanicalPartUsed)
                {
                    Console.WriteLine("try");
                }
                await Task.Delay(executionObject.ExecutionTime);
                if (executionObject.MachineMechanicalPartUsed)
                {
                    Console.WriteLine("try2");
                }
                _responsesQueue.Enqueue(executionObject.CommandToExecute.Invoke());
            });
        }

        public void SendMessage()
        {
            while (true)
            {
                if(_responsesQueue.Count != 0)
                {
                    Console.WriteLine(_responsesQueue.Dequeue().Response);
                }
            }
        }
    }
}
