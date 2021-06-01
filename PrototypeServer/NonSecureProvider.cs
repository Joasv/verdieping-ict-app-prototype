using System;
using System.Net.Sockets;
using System.Text;

public class NonSecureProvider : Provider
{

    private TcpListener socket = new TcpListener(10124);
    public NonSecureProvider() : base("using [Unsecured] provider type")
    {
        this.startSocket();
    }

    public override void startSocket()
    {
        socket.Start();
        while (true)
        {
            TcpClient client = socket.AcceptTcpClient();  //if a connection exists, the server will accept it

            NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

            byte[] hello = new byte[1024];   //any message must be serialized (converted to byte array)
            hello = Encoding.Default.GetBytes("[Ex] client connected; Server hello \n options for device \n 1) get Glucose Value \n 2) Get insulin value");  //conversion string => byte array
            ns.Write(hello, 0, hello.Length);

            while (client.Connected)  //while the client is connected, we look for incoming messages
            {
                byte[] data = new Byte[2048];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = ns.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                responseData.Trim();
                responseData.ToLower();
                string _default = "[Ex]";

                if (responseData == "1")
                {
                    _default += " glucose value: " + this.getGlucoseValueFromDevice();
                }
                else if (responseData == "2")
                {
                    _default += " insulin value: " + this.calculateInsulinValue();
                }
                Console.WriteLine("Client: {0}", responseData);


                data = Encoding.Default.GetBytes(_default);

                ns.Socket.Send(data);

            }
        }
    }
}