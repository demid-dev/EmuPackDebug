using EmuPackDebug.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmuPackDebug.Machine
{
    class EmulatedMachine
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private TcpListener _listener;
        private CommandHandler _commandHandler;

        public MachineState MachineState { get; private set; }

        public EmulatedMachine(string hostname, int port)
        {
            _commandHandler = new CommandHandler();
            MachineState = new MachineState();

            _client = new TcpClient();
            _listener = new TcpListener(IPAddress.Parse(hostname), port);

            Thread acceptConnectionThhread = new Thread(new ThreadStart(AcceptConnection));
            acceptConnectionThhread.Start();
        }

        public void AcceptConnection()
        {
            _listener.Start();
            while (true)
            {
                _client = _listener.AcceptTcpClient();
                _stream = _client.GetStream();
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start();
            }
        }

        public void ReceiveMessage()
        {
            byte[] data = new byte[99999];
            try
            {
                while (true)
                {
                    StringBuilder builder = new StringBuilder();

                    int bytes = 0;
                    do
                    {
                        bytes = _stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);

                    string message = builder.ToString();
                    string response = GetResponseFromProcessedMessage(message);
                    _stream.Write(Encoding.ASCII.GetBytes(response), 0, response.Length);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (_stream != null)
                    _stream.Close();
                if (_client != null)
                    _client.Close();
            }
        }

        public string GetResponseFromProcessedMessage(string message)
        {
            string messageToReturn = _commandHandler.ExecuteCommand(MachineState, message).Response;
            return messageToReturn;
        }

        public void SendMessage(CommandResponse response)
        {
            Console.WriteLine(response.Response);
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