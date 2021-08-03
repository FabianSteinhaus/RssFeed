using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RssFeed.Models
{
    public class Feed
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        [Display(Name = "Feed Name")]
        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string FeedName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Last Update Date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? UpdateDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        [StringLength(30)]
        public string Category { get; set; }

        public string Url { get; set; }

        [Display(Name = "Number of Articles")]
        public int NumberOfArticles { get; set; }

        public List<Feed> Feeds { get; set; }

        public SelectList Categorys { get; set; }
    }
}
