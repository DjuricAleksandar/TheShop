using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace TheShop.Tests
{
	public class ShopServiceTests
	{
		private readonly ShopService _shop;
		private const int ThrowException = -5;

		public ShopServiceTests()
		{
			var mocker = new AutoMocker();

			var databaseDriverMock = mocker.GetMock<IDatabaseDriver>();
			databaseDriverMock
				.Setup(dd => dd.GetById(It.Is<int>(id => id > 0)))
				.Returns((int id) => new Article {ID = id});
			databaseDriverMock
				.Setup(dd => dd.GetById(ThrowException))
				.Throws(new Exception());
			databaseDriverMock
				.Setup(dd => dd.Save(It.Is<Article>(a => a.ID == ThrowException)))
				.Throws(new Exception());
			var databaseDriver = databaseDriverMock.Object;

			var logger = mocker.GetMock<ILogger>().Object;

			var supplier1 = mocker.GetMock<ISupplier>().Object;
			var supplier2 = mocker.GetMock<ISupplier>().Object;
			var supplier3 = mocker.GetMock<ISupplier>().Object;
			var suppliers = new List<ISupplier>
			{
				supplier1,
				supplier2,
				supplier3
			}.ToImmutableList();

			_shop = new ShopService(databaseDriver, logger, suppliers);
		}

		[Fact]
		public void GetByIdNegativeValueReturnsErrorMessage()
		{
			const int id = -1;
			var result = _shop.GetById(id);
			Assert.True(result.IsError);
			Assert.Equal(string.Format(Messages.GetByIdNull, id), result.Message);
		}

		[Fact]
		public void GetByIdDatabaseThrowsExceptionReturnsErrorMessage()
		{
			var result = _shop.GetById(ThrowException);
			Assert.True(result.IsError);
			Assert.Equal(string.Format(Messages.GetByIdException, ThrowException), result.Message);
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
		public void SellArticleDatabaseThrowsExceptionReturnsErrorMessage()
		{
			var result = _shop.SellArticle(new Article {ID = ThrowException}, 1);
			Assert.True(result.IsError);
			Assert.Equal(string.Format(Messages.SellArticleException, ThrowException), result.Message);
		}

		[Fact]
		public void SellArticleCorrectArticleReturnsTrue()
		{
			var result = _shop.SellArticle(new Article() { ID = 5 }, 1);
			Assert.False(result.IsError);
			Assert.Empty(result.Message);
			Assert.True(result.Result);
		}
	}
}