using EmuPackDebug.Commands;
using EmuPackDebug.Machine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EmuPackDebug
{
    class Program
    {
        const int port = 8888;
        static TcpListener listener;

        static void Main(string[] args)
        {
            EmulatedMachine machine = new EmulatedMachine();
            machine.ReceiveMessage("INC1M100000");
            machine.SendMessage();
            //listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            //listener.Start();
            //Console.WriteLine("Ожидание подключений...");

            //while (true)
            //{
            //    TcpClient client = listener.AcceptTcpClient();
            //    ClientObject clientObject = new ClientObject(client);

            //    Thread clientThread = new Thread(new ThreadStart(clientObject.GetMessage));
            //    clientThread.Start();
            //}
        }
    }
}
