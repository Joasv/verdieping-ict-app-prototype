using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

class SecureClient
{
    TcpClient client = new TcpClient();

    public SecureClient()
    {
        Console.WriteLine("...waiting...connecting...");
        //! ensure sever is running before running the client!
        client.Connect("127.0.0.1", 10124);
        SslStream stream = new SslStream(client.GetStream());
        stream.AuthenticateAsClient("JN");

        Console.WriteLine("! Connected");
        while (true)
        {

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            Byte[] response = new Byte[1024];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(response, 0, response.Length);
            responseData = System.Text.Encoding.ASCII.GetString(response, 0, bytes);
            Console.WriteLine(responseData);
            Console.Write("> ");
            string input = Console.ReadLine();
            byte[] data = Encoding.ASCII.GetBytes(input);
            stream.Write(data);
        }
    }
}