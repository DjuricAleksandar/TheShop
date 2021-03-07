using System.Collections.Generic;
using System.Linq;
using TheShop.Model;

namespace TheShop.Dal
{
    //in memory implementation
    public class DatabaseDriver : IDatabaseDriver
    {
        private readonly List<Article> _articles = new List<Article>();

        public Article GetById(int id)
        {
            return _articles.Single(x => x.Id == id);
        }

        public void Save(Article article)
        {
            _articles.Add(article);
        }
    }
}