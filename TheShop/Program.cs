using System;

namespace TheShop
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var shopService = new ShopService();

			try
			{
				//order and sell
				var article = shopService.OrderArticle(1, 20);
				if(article == null)
					Console.WriteLine("Article not found.");
				else
				{
					var result = shopService.SellArticle(article, 10);
					if(result.IsError)
						Console.WriteLine(result.Message);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			try
			{
				//print article on console
				var article = shopService.GetById(1);
				Console.WriteLine("Found article with ID: " + article.ID);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Article not found: " + ex);
			}

			try
			{
				//print article on console				
				var article = shopService.GetById(12);
				Console.WriteLine("Found article with ID: " + article.ID);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Article not found: " + ex);
			}

			Console.ReadKey();
		}
	}
}