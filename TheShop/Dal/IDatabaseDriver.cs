using TheShop.Model;

namespace TheShop.Dal
{
	public interface IDatabaseDriver
	{
		Article GetById(int id);

		void Save(Article article);
	}
}