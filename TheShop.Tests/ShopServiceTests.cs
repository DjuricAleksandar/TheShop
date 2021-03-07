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

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void GetByIdExistingIdArticleReturnedSuccessfully(int id)
		{
			var orderResult = _shop.OrderArticle(1, int.MaxValue);
			_shop.SellArticle(orderResult.Result, 1);
			var result = _shop.GetById(id);
			Assert.False(result.IsError);
			Assert.Empty(result.Message);
			Assert.Equal(id, result.Result.ID);
		}

		[Fact]
		public void OrderArticleNegativeValueReturnsErrorMessage()
		{
			const int id = -1;
			const int maxValue = int.MaxValue;
			var result = _shop.OrderArticle(id, maxValue);
			Assert.True(result.IsError);
			Assert.Equal(string.Format(Messages.OrderArticleNotFound, id, maxValue), result.Message);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void OrderArticleExistingIdReturnsArticle(int id)
		{
			const int maxValue = int.MaxValue;
			var result = _shop.OrderArticle(id, maxValue);
			Assert.False(result.IsError);
			Assert.Empty(result.Message);
			Assert.Equal(id, result.Result.ID);
		}

		[Fact]
		public void SellArticleNullValueReturnsErrorMessage()
		{
			var result = _shop.SellArticle(null, 1);
			Assert.True(result.IsError);
			Assert.Equal(Messages.SellArticleArticleNull, result.Message);
		}

		[Fact]
		public void SellArticleCorrectArticleReturnsTrue()
		{
			var result = _shop.SellArticle(new Article() {ID = 5}, 1);
			Assert.False(result.IsError);
			Assert.Empty(result.Message);
			Assert.True(result.Result);
		}
	}
}
