using RssFeed.Models;

using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Data;
using System.Net;
using System.Xml.Linq;

namespace RssFeed.Services
{
    public class FeedService
    {
        private readonly IMongoCollection<Feed> _feeds;

        public FeedService(IRssFeedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            


            _feeds = database.GetCollection<Feed>(settings.FeedCollectionName);

        }

        public List<Feed> Get()
        {
            return _feeds.AsQueryable().ToList();
        }

        public Feed Get(string id) =>
            _feeds.Find(feed => feed.Id == id).FirstOrDefault();

        public Feed Create(Feed feed)
        {
            _feeds.InsertOne(feed);
            return feed;
        }

        public Feed Update(string id, Feed feedNew)           
        {
            //feedNew.UpdateDate = feedNew.UpdateDate?.ToUniversalTime();
            _feeds.ReplaceOne(feed => feed.Id == id, feedNew);

            return Get(id);
        }

        public void Remove(string id)
        {
            _feeds.DeleteOne(feed => feed.Id == id);
        }

        public int GetNumberOfArticles(Feed feed)
        {
            //WebClient wclient = new WebClient();
            //string RssData = wclient.DownloadString(feed.Url);
            //XDocument xml = XDocument.Parse(RssData);
            //feed.NumberOfArticles = xml.Descendants().Count();

            //return feed.NumberOfArticles;
            if (feed != null)
            {
                using (XmlReader reader = XmlReader.Create(feed.Url))
                {
                    feed.NumberOfArticles = SyndicationFeed.Load(reader).Items.Count();
                    return feed.NumberOfArticles;
                }
            }

            else
            {
                return 1;
            }
           
            
                
            
        }


    }
}
