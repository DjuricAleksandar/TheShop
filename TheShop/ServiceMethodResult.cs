namespace TheShop
{
	/// <summary>
	/// Return type of all ShopService methods.
	/// </summary>
	/// <typeparam name="T">Type of method result.</typeparam>
	public class ServiceMethodResult<T>
	{
		public T Result { get; }
		public bool IsError { get; }
		public string Message { get; }

		private ServiceMethodResult(T result, bool isError, string message = "")
		{
			Result = result;
			IsError = isError;
			Message = message;
		}

		/// <summary>
		/// Return error message and set isError flag to true.
		/// </summary>
		/// <param name="message">Error message.</param>
		/// <returns></returns>
		public static ServiceMethodResult<T> Error(string message)
		{
			return new ServiceMethodResult<T>(default, true, message);
		}

		/// <summary>
		/// Return result and set isError flag to false.
		/// </summary>
		/// <param name="result">Service method result.</param>
		/// <returns></returns>
		public static ServiceMethodResult<T> Ok(T result)
		{
			return new ServiceMethodResult<T>(result, false);
		}

		/// <summary>
		/// Method completed successfully.
		/// </summary>
		public static ServiceMethodResult<bool> True => new ServiceMethodResult<bool>(true, false);
	}
}