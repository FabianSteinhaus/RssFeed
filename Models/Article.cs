using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Data;

namespace RssFeed.Models
{
    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        [Display(Name = "Article Name")]
        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string ArticleName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Publish Date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? PubDate  {get; set;}


        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [StringLength(30)]
        public string Category { get; set; }

        public string Author { get; set; }

        public string Link { get; set; }

        [Display(Name = "Feed Url")]
        public string FeedUrl { get; set; }

        public string Description { get; set; }

    }
}
