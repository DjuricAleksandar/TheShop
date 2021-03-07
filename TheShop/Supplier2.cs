namespace TheShop
{
	public class Supplier2 : ISupplier
	{
		public bool ArticleInInventory(int id)
		{
			return true;
		}

		public Article GetArticle(int id)
		{
			return new Article()
			{
				Id = 1,
				Name = "Article from supplier2",
				Price = 459
			};
		}
	}
}