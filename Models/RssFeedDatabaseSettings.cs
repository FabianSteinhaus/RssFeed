namespace RssFeed.Models
{
    public class RssFeedDatabaseSettings : IRssFeedDatabaseSettings
    {                                                                      
        public string ArticleCollectionName { get; set; }                  
        public string FeedCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        
    }

    public interface IRssFeedDatabaseSettings  
    {
        string ArticleCollectionName { get; set; }
        public string FeedCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }

       
    }
}
