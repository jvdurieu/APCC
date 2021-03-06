﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClientServer_Client
{
    class Client
    {

        private static String server = "localhost";
        private static String message = "sub;0;3;4;SocketClientServer_Client;a first test";

        public static void Main()
        {

            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);

                String dataString = null;

                // Receive the TcpServer.response.
                while (true)
                {
                    int i;
                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // String to store the response ASCII representation.
                    String responseData = String.Empty;

                    while ((i = stream.Read(data, 0, data.Length)) != 0)
                    {

                        // Read the first batch of the TcpServer response bytes.
                        // Int32 bytes = stream.Read(data, 0, data.Length);

                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, i);
                        Console.WriteLine("Received: {0}", responseData);

                    }
                }

                // Close everything.
                client.Close();

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}