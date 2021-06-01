using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    TcpClient client = new TcpClient();

    public Client()
    {
        Console.WriteLine("...waiting...connecting...");
        client.Connect("127.0.0.1", 10124);
        Console.WriteLine("! Connected");
        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            byte[] data = Encoding.ASCII.GetBytes(input);
            NetworkStream stream = client.GetStream();
            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            Byte[] response = new Byte[1024];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(response, 0, response.Length);
            responseData = System.Text.Encoding.ASCII.GetString(response, 0, bytes);
            Console.WriteLine("Device: {0}", responseData);

            stream.Socket.Send(data);
        }
    }
}