using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace SimpleTcpTest
{
    class Program
    {
        static int _goodConnections = 0;
        static int _badConnections = 0;
        static int _totalConnections = 0;

        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("SimpleTcpTest <Destination Server> < Destination Port> <Source Port Start> <Iterations>");
                return;
            }
            var destination = args[0];
            var destinationPort = Convert.ToInt32(args[1]);
            var startPort = Convert.ToInt32(args[2]);
            var count = Convert.ToInt32(args[3]);

            for (int x = 0; x < count; x++)
            {
                BuildConnectClose(destination, destinationPort, startPort + x);   
                Console.Clear();
                Console.SetCursorPosition(0,0);
                Console.WriteLine("{0} good connections {1} bad connections out of {2}.  Port: {3}",_goodConnections,_badConnections,_totalConnections,x+startPort);   
            }
        }

        static  bool BuildConnectClose(string destination, int destinationPort, int port, bool shutDown = false)
        {
            using (
                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                    {
                        NoDelay = true
                    })
            {
                _totalConnections++;
                try
                {
                    try
                    {
                        socket.Bind(new IPEndPoint(IPAddress.Any,port));
                    }
                    catch (Exception err)
                    {
                        //Ignore binding issues, port may be in uese.
                        Console.WriteLine("Bind error: " + err.ToString());
                        return false;
                    }
                    socket.Connect(IPAddress.Parse(destination), destinationPort);
                    if(shutDown)
                        socket.Shutdown(SocketShutdown.Both);

                    socket.Disconnect(false);
                    _goodConnections++;
                    return true;
                }
                catch (Exception err)
                {
                    File.AppendAllText(@"badports.txt", "Failed Port: " + port.ToString() + "\r\n");
                    Console.WriteLine(err.ToString());
                    Thread.Sleep(2000);
                    _badConnections++;
                    return false;
                }
            }
        }
    }
}
