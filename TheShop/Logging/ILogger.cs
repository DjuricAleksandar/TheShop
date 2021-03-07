using System;

namespace TheShop.Logging
{
	public interface ILogger
	{
		void Debug(string message);

		void Error(string message, Exception exception = null);

		void Info(string message);
	}
}