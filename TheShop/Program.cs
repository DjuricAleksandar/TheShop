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
				var orderResult = shopService.OrderArticle(1, 20);
				if(orderResult.IsError)
					Console.WriteLine(orderResult.Message);
				else
				{
					var sellResult = shopService.SellArticle(orderResult.Result, 10);
					if(sellResult.IsError)
						Console.WriteLine(sellResult.Message);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			try
			{
				//print article on console
				var articleResult = shopService.GetById(1);
				if(articleResult.IsError)
					Console.WriteLine(articleResult.Message);
				else
					Console.WriteLine("Found article with ID: " + articleResult.Result.ID);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Article not found: " + ex);
			}

			try
			{
				//print article on console				
				var articleResult = shopService.GetById(12);
				if (articleResult.IsError)
					Console.WriteLine(articleResult.Message);
				else
					Console.WriteLine("Found article with ID: " + articleResult.Result.ID);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Article not found: " + ex);
			}

			Console.ReadKey();
		}
	}
}