namespace TheShop
{
	internal static class Messages
	{
		internal const string SellArticleArticleNull = "SellArticle: Article not provided.";
		internal const string SellArticleTryingToSell = "SellArticle: Trying to sell article id={0}.";
		internal const string SellArticleSold = "SellArticle: Article id={0} is sold.";
		internal const string SellArticleException = "SellArticle: Could not save article id={0}.";

		internal const string OrderArticleTryingToOrder =
			"OrderArticle: Trying to order article id={0} with max price={1}.";

		internal const string OrderArticleNotFound =
			"OrderArticle: Could not order article id={0} with max price={1}.";

		internal const string OrderArticleSupplierError =
			"OrderArticle: Supplier error for article id={0} with max price={1}.";

		internal const string OrderArticleReceived = "OrderArticle: Article id={0} received.";

		internal const string GetByIdReceived = "GetById: Article id={0} received.";
		internal const string GetByIdException = "GetById: Could not get Article id={0}.";
		internal const string GetByIdNull = "GetById: Database driver returned null for Article id={0}.";
	}
}