using System;

public class Logger
{
    public static void log(string type, string message)
    {
        Console.WriteLine("[LOG] - (Type) " + type + " [msg:] " + message);
    }
}