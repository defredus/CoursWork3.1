using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [BsonIgnoreExtraElements] 
    public class ServiceFact
    {

        [BsonId]
        public ObjectId MongoServiceFactId { get; set; }
        public int Id { get; set; }
        [BsonElement("client_id")]
        public ObjectId MongoClientId { get; set; }
        public int ClientId { get; set; }
        [BsonElement("service_id")]
        public ObjectId MongoServiceId { get; set; }
        public int ServiceId { get; set; }
        [BsonElement("start_date")]
        public DateTime StartDate { get; set; }
        [BsonElement("end_date")]
        public DateTime EndDate { get; set; }
        [BsonElement("quantity")]
        public decimal Quantity { get; set; }
        [BsonElement("status")]  // Указывает на поле "status" в MongoDB
        public string Status { get; set; }
    }

}
