using System;
using System.Collections.Immutable;

namespace TheShop
{
	public class ShopService
	{
		private readonly IDatabaseDriver _databaseDriver;
		private readonly ILogger _logger;
		private readonly IImmutableList<ISupplier> _suppliers;

		public ShopService(IDatabaseDriver databaseDriver, ILogger logger, IImmutableList<ISupplier> suppliers)
		{
			_databaseDriver = databaseDriver;
			_logger = logger;
			_suppliers = suppliers;
		}

		/// <summary>
		/// Get sold article by Id.
		/// </summary>
		/// <param name="id">Article Id</param>
		/// <returns>ServiceMethodResult</returns>
		public ServiceMethodResult<Article> GetById(int id)
		{
			try
			{
				var article = _databaseDriver.GetById(id);
				if (article == null)
				{
					var message = string.Format(Messages.GetByIdNull, id);
					_logger.Debug(message);
					return ServiceMethodResult<Article>.Error(message);
				}
				_logger.Debug(string.Format(Messages.GetByIdReceived, id));
				return ServiceMethodResult<Article>.Ok(article);
			}
			catch (Exception e)
			{
				var message = string.Format(Messages.GetByIdException, id);
				_logger.Error(message, e);
				return ServiceMethodResult<Article>.Error(message);
			}
		}

		/// <summary>
		/// Check if article exists in any of the suppliers storage,
		/// and returns it if it's price lesser or equal the maxExpectedPrice.
		/// </summary>
		/// <param name="id">Article Id</param>
		/// <param name="maxExpectedPrice">Max allowed article price</param>
		/// <returns>ServiceMethodResult</returns>
		public ServiceMethodResult<Article> OrderArticle(int id, int maxExpectedPrice)
		{
			_logger.Debug(string.Format(Messages.OrderArticleTryingToOrder, id, maxExpectedPrice));

			foreach (var supplier in _suppliers)
			{
				try
				{
					if (!supplier.ArticleInInventory(id)) continue;

					var article = supplier.GetArticle(id);
					if (!(article?.Price <= maxExpectedPrice)) continue;

					_logger.Debug(string.Format(Messages.OrderArticleReceived, id));
					return ServiceMethodResult<Article>.Ok(article);
				}
				catch (Exception e)
				{
					_logger.Error(string.Format(Messages.OrderArticleSupplierError, id, maxExpectedPrice), e);
				}
			}

			var message = string.Format(Messages.OrderArticleNotFound, id, maxExpectedPrice);
			_logger.Debug(message);
			return ServiceMethodResult<Article>.Error(message);
		}

		/// <summary>
		/// Sells the article.
		/// </summary>
		/// <param name="article">Article to sell.</param>
		/// <param name="buyerId">Id of the buyer of the article.</param>
		/// <returns>ServiceMethodResult</returns>
		public ServiceMethodResult<bool> SellArticle(Article article, int buyerId)
		{
			if (article == null)
			{
				_logger.Debug(Messages.SellArticleArticleNull);
				return ServiceMethodResult<bool>.Error(Messages.SellArticleArticleNull);
			}

			_logger.Debug(string.Format(Messages.SellArticleTryingToSell, article.Id));

			article.IsSold = true;
			article.SoldDate = DateTime.Now;
			article.BuyerUserId = buyerId;

			try
			{
				_databaseDriver.Save(article);
				_logger.Info(string.Format(Messages.SellArticleSold, article.Id));
				return ServiceMethodResult<bool>.True;
			}
			catch (Exception e)
			{
				var message = string.Format(Messages.SellArticleException, article.Id);
				_logger.Error(message, e);
				return ServiceMethodResult<bool>.Error(message);
			}
		}
	}
}