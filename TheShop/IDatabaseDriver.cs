namespace TheShop
{
	public interface IDatabaseDriver
	{
		Article GetById(int id);

		void Save(Article article);
	}
}