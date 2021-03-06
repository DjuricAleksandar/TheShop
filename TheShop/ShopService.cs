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
		        const string message = "SellArticle: Article not provided.";
                   logger.Debug(message);
		        return new ServiceMethodResult<bool>(false, true, message);
	        }

	        logger.Debug("SellArticle: Trying to sell article with id=" + article.ID);

            article.IsSold = true;
            article.SoldDate = DateTime.Now;
            article.BuyerUserId = buyerId;

            try
            {
                DatabaseDriver.Save(article);
                logger.Info("SellArticle: Article with id=" + article.ID + " is sold.");
                return new ServiceMethodResult<bool>(true, false);
            }
            catch (Exception e)
            {
	            var message = "SellArticle: Could not save article with id=" + article.ID;
                logger.Error(message, e);
                return new ServiceMethodResult<bool>(false, true, message);
            }
        }

        public Article OrderArticle(int id, int maxExpectedPrice)
        {
            Article article = null;
            Article tempArticle = null;
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

            if (article == null)
            {
	            throw new Exception("Could not order article");
            }

            return article;
        }

        public Article GetById(int id)
        {
            return DatabaseDriver.GetById(id);
        }
    }
}