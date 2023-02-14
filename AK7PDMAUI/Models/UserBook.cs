using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK7PDMAUI.Models
{
    public class UserBook
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("book_id")]
        public string BookId { get; set; }

        [BsonElement("user_id")]
        public string UserId { get; set; }

        [BsonElement("borrowing_date")]
        public DateTime BorrowingDate { get; set; }

        [BsonElement("expiration_date")]
        public DateTime ExpirationDate { get; set; }

        [BsonElement("return_date")]
        public DateTime ReturnDate { get; set; }

        [BsonElement("active")]
        public bool IsActive { get; set; }
    }
}
