using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;


public class SecureProvider : Provider
{

    public static X509Certificate cert;

    private TcpListener socket = new TcpListener(10124);
    public SecureProvider() : base("using [Secured] provider type")
    {
        this.startSocket();
    }

    public override void startSocket()
    {
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (
                object s,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors
            )
            {
                return true;
            };

        X509Store store = new X509Store(StoreName.Root);
        store.Open(OpenFlags.ReadWrite);
        // Retrieve the certificate 
        X509Certificate2 cert = new X509Certificate2("certificate.pfx", "test123");


        store.Close(); // Close the storage area.



        socket.Start();
        Console.WriteLine("Listening");
        while (true)
        {
            TcpClient client = socket.AcceptTcpClient();
            Console.WriteLine("Client connected");
            SslStream stream = new SslStream(client.GetStream());
            stream.AuthenticateAsServer(cert);
            Logger.log("info", "client connected, now authenticating...");

            byte[] welcomeMesg = new byte[1024];   //any message must be serialized (converted to byte array)
            welcomeMesg = Encoding.Default.GetBytes("[Ex] client connected; Server hello\nauthenticating[pwd@root]");  //conversion string => byte array
            stream.Write(welcomeMesg, 0, welcomeMesg.Length);
            byte[] passwordBuffer = new Byte[2048];
            String password = String.Empty;

            Int32 bytes = stream.Read(passwordBuffer, 0, passwordBuffer.Length);
            password = System.Text.Encoding.ASCII.GetString(passwordBuffer, 0, bytes);
            password.Trim();
            password.ToLower();
            if (Authentication.IsLoginValid(password))
            {
                Logger.log("info", "#######################################");
                Logger.log("info", "client authenticated, starting session...");
                Logger.log("info", "#######################################");
                while (true)
                {
                    byte[] hello = new byte[1024];   //any message must be serialized (converted to byte array)
                    hello = Encoding.Default.GetBytes("[Ex] client authenticated; Server hello \n options for device \n 1) get Glucose Value \n 2) Get insulin value");  //conversion string => byte array
                    stream.Write(hello, 0, hello.Length);

                    while (client.Connected)  //while the client is connected, we look for incoming messages
                    {
                        byte[] data = new Byte[2048];

                        // String to store the response ASCII representation.
                        String responseData = String.Empty;

                        // Read the first batch of the TcpServer response bytes.
                        bytes = stream.Read(data, 0, data.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                        responseData.Trim();
                        responseData.ToLower();
                        Logger.log("info", "RECEIVED:" + responseData);
                        string _default = "[Ex]";

                        if (responseData == "1")
                        {
                            _default += " glucose value: " + this.getGlucoseValueFromDevice();
                        }
                        else if (responseData == "2")
                        {
                            _default += " insulin value: " + this.calculateInsulinValue();
                        }

                        Logger.log("info", "SENDING:" + _default);
                        data = Encoding.Default.GetBytes(_default);

                        stream.Write(data, 0, data.Length);

                    }
                }
            }
            else
            {
                client.Close();
            }
        }
    }

}
