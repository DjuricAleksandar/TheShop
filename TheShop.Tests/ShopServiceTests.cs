using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TheShop.Dal;
using TheShop.Logging;
using TheShop.Model;
using TheShop.Suppliers;
using Xunit;

namespace TheShop.Tests
{
	public class ShopServiceTests
	{
		private const int HighestPrice = 300;
		private const int LowestPrice = 100;
		private const int MiddlePrice = 200;
		private const int ThrowException = -5;
		private readonly ShopService _shop;

		public ShopServiceTests()
		{
			var mocker = new AutoMocker();

			var databaseDriverMock = mocker.GetMock<IDatabaseDriver>();
			databaseDriverMock
				.Setup(dd => dd.GetById(It.Is<int>(id => id > 0)))
				.Returns((int id) => new Article { Id = id });
			databaseDriverMock
				.Setup(dd => dd.GetById(ThrowException))
				.Throws(new Exception());
			databaseDriverMock
				.Setup(dd => dd.Save(It.Is<Article>(a => a.Id == ThrowException)))
				.Throws(new Exception());
			var databaseDriver = databaseDriverMock.Object;

			var logger = mocker.GetMock<ILogger>().Object;

			var suppliers = new List<ISupplier>
			{
				GetMockedSupplier(HighestPrice),
				GetMockedSupplier(MiddlePrice),
				GetMockedSupplier(LowestPrice)
			}.ToImmutableList();

			_shop = new ShopService(databaseDriver, logger, suppliers);
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
			Assert.Equal(id, result.Result.Id);
		}

		[Fact]
		public void GetByIdNegativeValueReturnsErrorMessage()
		{
			const int id = -1;
			var result = _shop.GetById(id);
			Assert.True(result.IsError);
			Assert.Equal(string.Format(Messages.GetByIdNull, id), result.Message);
		}

		[Theory]
		[InlineData(LowestPrice)]
		[InlineData(MiddlePrice)]
		[InlineData(HighestPrice)]
		public void OrderArticleArticleExistsAndGoodPriceReturnsArticle(int maxExpectedPrice)
		{
			var result = _shop.OrderArticle(1, maxExpectedPrice);
			Assert.False(result.IsError, "Expected no error.");
			Assert.Empty(result.Message);
			Assert.Equal(1, result.Result.Id);
			Assert.True(result.Result.Price <= maxExpectedPrice);
		}
		
		[Fact]
		public void OrderArticleTooLowPriceReturnsError()
		{
			var result = _shop.OrderArticle(1, LowestPrice - 1);
			Assert.True(result.IsError, "Expected error result.");
			Assert.Equal(string.Format(Messages.OrderArticleNotFound, 1, LowestPrice - 1), result.Message);
		}

		[Fact]
		public void OrderArticleNegativeValueReturnsErrorMessage()
		{
			const int id = -1;
			const int maxValue = int.MaxValue;
			var result = _shop.OrderArticle(id, maxValue);
			Assert.True(result.IsError, "Expected error result.");
			Assert.Equal(string.Format(Messages.OrderArticleNotFound, id, maxValue), result.Message);
		}

		[Fact]
		public void OrderArticleSupplierThrowsExceptionReturnsErrorMessage()
		{
			const int maxValue = int.MaxValue;
			var result = _shop.OrderArticle(ThrowException, maxValue);
			Assert.True(result.IsError, "Expected error result.");
			Assert.Equal(string.Format(Messages.OrderArticleNotFound, ThrowException, maxValue), result.Message);
		}

		[Fact]
		public void SellArticleCorrectArticleReturnsTrue()
		{
			var result = _shop.SellArticle(new Article() { Id = 5 }, 1);
			Assert.False(result.IsError, "Expected no error");
			Assert.Empty(result.Message);
			Assert.True(result.Result, "Expected true result");
		}

		[Fact]
		public void SellArticleDatabaseThrowsExceptionReturnsErrorMessage()
		{
			var result = _shop.SellArticle(new Article { Id = ThrowException }, 1);
			Assert.True(result.IsError, "Expected error result.");
			Assert.Equal(string.Format(Messages.SellArticleException, ThrowException), result.Message);
		}

		[Fact]
		public void SellArticleNullValueReturnsErrorMessage()
		{
			var result = _shop.SellArticle(null, 1);
			Assert.True(result.IsError, "Expected error result.");
			Assert.Equal(Messages.SellArticleArticleNull, result.Message);
		}

		private static ISupplier GetMockedSupplier(int articlePrice)
		{
			var mocker = new AutoMocker();
			var supplierMock = mocker.GetMock<ISupplier>();
			supplierMock
				.Setup(s => s.ArticleInInventory(It.Is<int>(id => id > 0)))
				.Returns(true);
			supplierMock
				.Setup(s => s.GetArticle(It.Is<int>(id => id > 0)))
				.Returns((int id) => new Article { Id = id, Price = articlePrice });
			supplierMock
				.Setup(dd => dd.ArticleInInventory(It.Is<int>(id => id == ThrowException)))
				.Throws(new Exception());
			return supplierMock.Object;
		}
	}
}