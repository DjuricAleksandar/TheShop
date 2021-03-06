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

		public ServiceMethodResult(T result, bool isError, string message = "")
		{
			Result = result;
			IsError = isError;
			Message = message;
		}
	}
}