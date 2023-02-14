using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK7PDMAUI.Models
{
    public class Book
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("author")]
        public Author Author { get; set; }

        [BsonElement("img_url")]
        public string ImgUrl { get; set; }

        [BsonElement("total_count")]
        public int TotalCount { get; set; }

        [BsonElement("borrowed_count")]
        public int BorrowedCount { get; set; }

        [BsonElement("pages")]
        public int Pages { get; set; }
    }
}
