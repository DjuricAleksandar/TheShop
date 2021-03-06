using System.Collections.Generic;
using System.Linq;

namespace TheShop
{
    //in memory implementation
    public class DatabaseDriver
    {
        private readonly List<Article> _articles = new List<Article>();

        public Article GetById(int id)
        {
            return _articles.Single(x => x.ID == id);
        }

        public void Save(Article article)
        {
            _articles.Add(article);
        }
    }
}