using System;
using Xunit;

namespace TheShop.Tests
{
	public class ShopServiceTests
	{
		private readonly ShopService _shop = new ShopService();

		[Fact]
		public void GetByIdNegativeValueReturnsErrorMessage()
		{
			const int id = -1;
			var result = _shop.GetById(id);
			Assert.True(result.IsError);
			Assert.Equal(string.Format(Messages.GetByIdException, id), result.Message);
		}

		[Fact]
		public void GetByIdExistingIdArticleReturnedSuccessfully()
		{
			const int id = 1;
			var result = _shop.GetById(id);
			Assert.False(result.IsError);
			Assert.Empty(result.Message);
			Assert.Equal(id, result.Result.ID);
		}
	}
}
