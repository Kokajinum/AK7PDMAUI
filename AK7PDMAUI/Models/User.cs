using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK7PDMAUI.Models
{
    public class User
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("login")]
        public string Login { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("first_name")]
        public string FirstName { get; set; }

        [BsonElement("last_name")]
        public string LastName { get; set; }

        [BsonElement("address")]
        public Address Address { get; set; }

        [BsonElement("birth_number")]
        public int BirthNumber { get; set; }

        [BsonElement("account_state")]
        public string AccountState { get; set; } = "pending";

        [BsonElement("is_admin")]
        public bool IsAdmin { get; set; }
    }
}
