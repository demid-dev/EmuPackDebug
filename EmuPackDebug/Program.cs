﻿using EmuPackDebug.Commands;
using EmuPackDebug.Machine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EmuPackDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            EmulatedMachine machine = new EmulatedMachine("127.0.0.1", 8888);
            Thread.Sleep(10000);
            Console.WriteLine("sadasd");
            //machine.ReceiveMessage("PRC1M100192I00910510TUTU                          0000411RUTU                          0000412RURU                          0000413TURU                          0000414RURT                          00004");
            //FillCommand fill = new FillCommand("FLC1M100023P0090A0MC03010502030304");
            //PrescriptionRegistrationCommand drug = new PrescriptionRegistrationCommand("PRC1M100192I00910510TUTU                          0000411RUTU                          0000412RURU                          0000413TURU                          0000414RURT                          00004");
            //MachineActivityRequestCommand activity = new MachineActivityRequestCommand("MRC1M100003D01");
            //NotRecognizedRequestResponse response = new NotRecognizedRequestResponse();
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
