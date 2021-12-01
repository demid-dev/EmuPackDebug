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
            ExecuteCommand(_commandHandler.ExecuteCommand(MachineState, "PRC1M100192I00910510TUTU                          5000011RUTU                          5000012RURU                          5000013TURU                          5000014RURT                          50000"));
            ExecuteCommand(_commandHandler.ExecuteCommand(MachineState, "FLC1M100023P0091A0MC03130512031304"));
            ExecuteCommand(_commandHandler.ExecuteCommand(MachineState, "FLC1M100023P0091A0MC03130512031304"));
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
                Thread.Sleep(1000);
                if(_responsesQueue.Any())
                {
                    Console.WriteLine(_responsesQueue.Dequeue().Response);
                    MachineState.RegistredPrescriptions.ForEach(prescription =>
                    {
                        Console.WriteLine($"Id: {prescription.Id}");
                        prescription.RegistredCassettes.ForEach(cassette =>
                        {
                            Console.WriteLine($"Cassette id: {cassette.CassetteId}");
                            Console.WriteLine($"Drug name: {cassette.DrugName}");
                            Console.WriteLine($"Drug quantity: {cassette.DrugQuantity}");
                        });
                        MachineState.Adaptor.DrugPack.DrugCells.ForEach(cell =>
                        {
                            Console.WriteLine($"\n Cell {cell.CellName}");
                            cell.DrugsInCell.ForEach(drug =>
                            {
                                Console.WriteLine($"Name: {drug.DrugName}; Quantity: {drug.DrugQuantity}");
                            });
                        });
                    });

                }
            }
        }
    }
}
