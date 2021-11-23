using EmuPackDebug.Commands;
using EmuPackDebug.Machine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug
{
    class ClientObject
    {
        private TcpClient _client;
        private TcpListener _listener;

        public ClientObject()
        {
            _client = new TcpClient();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
        }

        public void GetMessage(MachineState machineState, CommandHandler commandHandler)
        {
            NetworkStream stream = null;
            stream = _client.GetStream();
            byte[] data = new byte[999];
            while (true)
            {
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);

                string message = builder.ToString();
                commandHandler.ExecuteCommand(machineState, message);
            }
        }
    }
}
