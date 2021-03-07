using System;

namespace TheShop
{
	public interface ILogger
	{
		void Debug(string message);

		void Error(string message, Exception exception = null);

		void Info(string message);
	}
}