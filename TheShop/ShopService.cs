using System;

namespace TheShop
{
	public class ShopService
	{
		private DatabaseDriver DatabaseDriver;
		private Logger logger;

		private Supplier1 Supplier1;
		private Supplier2 Supplier2;
		private Supplier3 Supplier3;

		public ShopService()
		{
			DatabaseDriver = new DatabaseDriver();
			logger = new Logger();
			Supplier1 = new Supplier1();
			Supplier2 = new Supplier2();
			Supplier3 = new Supplier3();
		}

		public ServiceMethodResult<bool> SellArticle(Article article, int buyerId)
		{
			if (article == null)
			{
				logger.Debug(Messages.SellArticleArticleNull);
				return ServiceMethodResult<bool>.Error(Messages.SellArticleArticleNull);
			}

			logger.Debug(string.Format(Messages.SellArticleTryingToSell, article.ID));

			article.IsSold = true;
			article.SoldDate = DateTime.Now;
			article.BuyerUserId = buyerId;

			try
			{
				DatabaseDriver.Save(article);
				logger.Info(string.Format(Messages.SellArticleSold, article.ID));
				return ServiceMethodResult<bool>.True;
			}
			catch (Exception e)
			{
				var message = string.Format(Messages.SellArticleException, article.ID);
				logger.Error(message, e);
				return ServiceMethodResult<bool>.Error(message);
			}
		}

		public ServiceMethodResult<Article> OrderArticle(int id, int maxExpectedPrice)
		{
			logger.Debug(string.Format(Messages.OrderArticleTryingToOrder, id, maxExpectedPrice));

			Article article;
			Article tempArticle = null;
			string message;
			try
			{
				var articleExists = Supplier1.ArticleInInventory(id);
				if (articleExists)
				{
					tempArticle = Supplier1.GetArticle(id);
					if (maxExpectedPrice < tempArticle.ArticlePrice)
					{
						articleExists = Supplier2.ArticleInInventory(id);
						if (articleExists)
						{
							tempArticle = Supplier2.GetArticle(id);
							if (maxExpectedPrice < tempArticle.ArticlePrice)
							{
								articleExists = Supplier3.ArticleInInventory(id);
								if (articleExists)
								{
									tempArticle = Supplier3.GetArticle(id);
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
				logger.Error(message, e);
				return ServiceMethodResult<Article>.Error(message);
			}

			if (article != null)
			{
				logger.Debug(string.Format(Messages.OrderArticleReceived, id));
				return ServiceMethodResult<Article>.Ok(article);
			}

			message = string.Format(Messages.OrderArticleNotFound, id, maxExpectedPrice);
			logger.Debug(message);
			return ServiceMethodResult<Article>.Error(message);
		}

		public Article GetById(int id)
		{
			return DatabaseDriver.GetById(id);
		}
	}
}