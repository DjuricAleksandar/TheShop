using System;

namespace TheShop.Logging
{
	public class Logger : ILogger
	{
		public void Debug(string message)
		{
			Console.WriteLine("Debug: " + message);
		}

		public void Error(string message, Exception exception = null)
		{
			Console.WriteLine("Error: " + message + (exception != null ? " - " + exception.ToString() : ""));
		}

		public void Info(string message)
		{
			Console.WriteLine("Info: " + message);
		}
	}
}