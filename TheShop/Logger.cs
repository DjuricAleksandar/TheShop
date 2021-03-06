using System;

namespace TheShop
{
    public class Logger
    {
        public void Info(string message)
        {
            Console.WriteLine("Info: " + message);
        }

        public void Error(string message, Exception exception = null)
        {
            Console.WriteLine("Error: " + message + (exception != null ? " - " + exception.ToString() : ""));
        }

        public void Debug(string message)
        {
            Console.WriteLine("Debug: " + message);
        }
    }
}