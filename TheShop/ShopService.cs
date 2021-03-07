using System;
using System.Collections.Generic;
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

		public ServiceMethodResult<bool> SellArticle(Article article, int buyerId)
		{
			if (article == null)
			{
				_logger.Debug(Messages.SellArticleArticleNull);
				return ServiceMethodResult<bool>.Error(Messages.SellArticleArticleNull);
			}

			_logger.Debug(string.Format(Messages.SellArticleTryingToSell, article.ID));

			article.IsSold = true;
			article.SoldDate = DateTime.Now;
			article.BuyerUserId = buyerId;

			try
			{
				_databaseDriver.Save(article);
				_logger.Info(string.Format(Messages.SellArticleSold, article.ID));
				return ServiceMethodResult<bool>.True;
			}
			catch (Exception e)
			{
				var message = string.Format(Messages.SellArticleException, article.ID);
				_logger.Error(message, e);
				return ServiceMethodResult<bool>.Error(message);
			}
		}

		public ServiceMethodResult<Article> OrderArticle(int id, int maxExpectedPrice)
		{
			_logger.Debug(string.Format(Messages.OrderArticleTryingToOrder, id, maxExpectedPrice));

			Article article;
			Article tempArticle = null;
			string message;
			try
			{
				var articleExists = _suppliers[0].ArticleInInventory(id);
				if (articleExists)
				{
					tempArticle = _suppliers[0].GetArticle(id);
					if (maxExpectedPrice < tempArticle.ArticlePrice)
					{
						articleExists = _suppliers[1].ArticleInInventory(id);
						if (articleExists)
						{
							tempArticle = _suppliers[1].GetArticle(id);
							if (maxExpectedPrice < tempArticle.ArticlePrice)
							{
								articleExists = _suppliers[2].ArticleInInventory(id);
								if (articleExists)
								{
									tempArticle = _suppliers[2].GetArticle(id);
									if (maxExpectedPrice < tempArticle.ArticlePrice)
									{
										article = tempArticle;
									}
								}
							}
						}
					}
				}

				article = tempArticle;
			}
			catch (Exception e)
			{
				message = string.Format(Messages.OrderArticleSupplierError, id, maxExpectedPrice);
				_logger.Error(message, e);
				return ServiceMethodResult<Article>.Error(message);
			}

			if (article != null)
			{
				_logger.Debug(string.Format(Messages.OrderArticleReceived, id));
				return ServiceMethodResult<Article>.Ok(article);
			}

			message = string.Format(Messages.OrderArticleNotFound, id, maxExpectedPrice);
			_logger.Debug(message);
			return ServiceMethodResult<Article>.Error(message);
		}

		public ServiceMethodResult<Article> GetById(int id)
		{
			try
			{
				var article = _databaseDriver.GetById(id);
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
	}
}