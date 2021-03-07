using TheShop.Model;

namespace TheShop.Suppliers
{
	public class Supplier : ISupplier
	{
		private readonly string _articleName;
		private readonly int _articlePrice;

		public Supplier(string articleName, int articlePrice)
		{
			_articleName = articleName;
			_articlePrice = articlePrice;
		}

		public bool ArticleInInventory(int id)
		{
			return true;
		}

		public Article GetArticle(int id)
		{
			return new Article()
			{
				Id = 1,
				Name = _articleName,
				Price = _articlePrice
			};
		}
	}
}