using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmuPackDebug
{
    class ClientObject
    {
        private TcpClient _client;

        public ClientObject(TcpClient client)
        {
            _client = client;
        }

        public void GetMessage()
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
                Console.WriteLine(message);
            }
        }
    }
}
