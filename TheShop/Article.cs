using System;

namespace TheShop
{
	public class Article
	{
		public int BuyerUserId { get; set; }
		public int Id { get; set; }
		public bool IsSold { get; set; }
		public string Name { get; set; }
		public int Price { get; set; }
		public DateTime SoldDate { get; set; }
	}
}