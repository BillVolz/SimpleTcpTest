using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SimpleTcpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("SimpleTcpTest <Server> <Port> <Iterations");
                return;
            }
            var destination = args[0];
            var desitnationPort = Convert.ToInt16(args[1]);
            var count = Convert.ToInt16(args[2]);

            int goodConnections = 0;
            int badConnections = 0;
            int totalConnections = 0;

            for(int x=0; x<count; count++) { 
                using(var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { NoDelay = true })
                {
                    totalConnections++;
                    try
                    {
                        socket.Connect(IPAddress.Parse(destination), desitnationPort);
                        socket.Disconnect(false);
                        goodConnections++;
                    }
                    catch (Exception)
                    {
                        badConnections++;
                    }
                }
                if (totalConnections%10 == 0)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0,0);
                    Console.WriteLine("{0} good connections {1} bad connections out of {2}",goodConnections,badConnections,totalConnections);
                }
            }



        }
    }
}
