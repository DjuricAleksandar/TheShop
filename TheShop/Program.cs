using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TheShop.Dal;
using TheShop.Logging;
using TheShop.Suppliers;

namespace TheShop
{
	internal static class Program
	{
		private static void Main()
		{
			var shopService = new ShopService(new DatabaseDriver(), new Logger(), new List<ISupplier>
			{
				new Supplier("Article from supplier1", 458),
				new Supplier("Article from supplier2", 459),
				new Supplier("Article from supplier3", 460)
			}.ToImmutableList());

			try
			{
				//order and sell
				var orderResult = shopService.OrderArticle(1, 459);
				if (orderResult.IsError)
					Console.WriteLine(orderResult.Message);
				else
				{
					var sellResult = shopService.SellArticle(orderResult.Result, 10);
					if (sellResult.IsError)
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
				if (articleResult.IsError)
					Console.WriteLine(articleResult.Message);
				else
					Console.WriteLine("Found article with ID: " + articleResult.Result.Id);
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
					Console.WriteLine("Found article with ID: " + articleResult.Result.Id);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Article not found: " + ex);
			}

			Console.ReadKey();
		}
	}
}