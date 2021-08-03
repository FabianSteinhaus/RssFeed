using RssFeed.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Data;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using RssFeed.Services;

using Microsoft.AspNetCore.Mvc.Rendering;
//using ArticleFrontEnd.Models;
using System.Net;
using System.Web;
//using System.Web.Mvc;
using System.Xml.Linq;
using MongoDB.Bson;

namespace RssFeed.Services
{
    public class ArticleService
    {

        private readonly IMongoCollection<Article> _articles;
        private readonly FeedService _feedService;

        public ArticleService(IRssFeedDatabaseSettings settings, FeedService feedService)                            
        {
            var client = new MongoClient(settings.ConnectionString);                       
            var database = client.GetDatabase(settings.DatabaseName);
            _feedService = feedService;


            _articles = database.GetCollection<Article>(settings.ArticleCollectionName);
        }

        public List<Article> GetAllFromDb()
        {                                                    
            return _articles.AsQueryable().ToList();
        }

        public Article GetArticleFromDb(string id) => _articles.Find(article => article.Id == id).FirstOrDefault();

        public Article Create(Article article)
        {
            _articles.InsertOne(article);                                                         
            return article;
        }

        public Article Update(string id, Article articleNew)
        {                                    
            _articles.ReplaceOne(article => article.Id == id, articleNew);

            return GetArticleFromDb(id);
        }

        public void Remove(string id)
        {
            _articles.DeleteOne(article => article.Id == id);

        }


        public void Reload(Feed feed)
        {
            WebClient wclient = new WebClient();
            string RssData = wclient.DownloadString(feed.Url);
            XDocument xml = XDocument.Parse(RssData);

           


            var RSSFeedData = from x in xml.Descendants("item")
                              //where !string.IsNullOrWhiteSpace(x.Element("description")?.ToString())
                              select new Article


                              {
                                  ArticleName =  (string)x.Element("title"),
                                  Link = (string)x.Element("link"),
                                  //Link = x.Element("link") != null ? (string)x.Element("link") : "",
                                  //Wenn links neben ? TRUE dann rechts neben ? sonst rechts neben :
                                  Author = ((string)x.Element("author")),
                                  
                                  PubDate = (DateTime?)x.Element("pubDate"),
                                  //Bei Nullable Variablen überall ? ergänzen
                                  Category = (string)x.Element("category"),
                                  Description = (string)x.Element("description")

                              };

            foreach (var article in RSSFeedData)
            {
                article.FeedUrl = feed.Url;
                article.FeedName = feed.FeedName;

                if (article.Category == null)
                {
                    article.Category = feed.Category;
                }

                var articleExists = IsArticleExist(article);
                var articleValid = IsArticleValid(article);

                if (!articleExists && articleValid)
                {
                    Create(article);
                }
            }
        }

        /// <summary>
        /// Checks if an article already exists
        /// </summary>
        /// <param name="article">article to check</param>
        /// <returns></returns>
        private bool IsArticleExist(Article article)
        {
            var articles = GetAllFromDb();
            bool articleExists = articles.Exists(x => x.Link == article.Link);

            if (articleExists)
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        private bool IsArticleValid(Article article)
        {
            bool validArticle = !String.IsNullOrEmpty(article.Description) && !String.IsNullOrEmpty(article.ArticleName);

            if (validArticle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       
        public void FilteredReload(Feed feed)
        {

            WebClient wclient = new WebClient();
            string RssData = wclient.DownloadString(feed.Url);
            XDocument xml = XDocument.Parse(RssData);



            var RSSFeedData = from x in xml.Descendants("item")
                              select new Article

                              {
                                  ArticleName = (string)x.Element("title"),
                                  Link = (string)x.Element("link"),
                                  Author = ((string)x.Element("author")),
                                  PubDate = (DateTime?)x.Element("pubDate"),
                                  Category = (string)x.Element("category"),
                                  Description = (string)x.Element("description")
                              };

            foreach (var article in RSSFeedData)
            {
                article.FeedUrl = feed.Url;
                article.FeedName = feed.FeedName;

                if (article.Category == null)
                {
                    article.Category = feed.Category;
                }

                var articleExists = IsArticleExist(article);
                var articleValid = IsArticleValid(article);

                if (!articleExists && articleValid)
                {
                    Create(article);
                }
            }
        }

    }
}
