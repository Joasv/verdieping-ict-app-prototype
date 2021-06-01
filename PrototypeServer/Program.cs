using System;

namespace Prototype
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            Provider prov = new SecureProvider();
        }
    }
}
