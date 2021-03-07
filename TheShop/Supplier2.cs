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
                ID = 1,
                Name_of_article = "Article from supplier2",
                ArticlePrice = 459
            };
        }
    }
}