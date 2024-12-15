using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Client
    {
        [BsonId]
        public ObjectId MongoClientId { get; set; }  // Для MongoDB используем ObjectId
        public int Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("address")]
        public string Address { get; set; }
        [BsonElement("phone")]
        public string Phone { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("role")]
        public string Role { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("isActive")]
        public bool IsActive { get; set; }
        [BsonElement("balance")]
        public decimal Balance { get; set; }
        [BsonElement("services")]
        public List<Service> Services { get; set; }  // Коллекция тарифных планов
    }
}
public enum ClientRole
{
    Admin,
    Client
}
