using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class PaymentType
    {
        [BsonId]
        public ObjectId MongoId { get; set; }

        public int Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } // Описание типа платежа (например, "Интернет-платеж")
    }

}
