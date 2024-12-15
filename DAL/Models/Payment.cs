using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Payment
    {
        [BsonId]
        public ObjectId MongoId { get; set; }  // Для MongoDB используем ObjectId
        public int Id { get; set; }
        [BsonElement("client_id")]
        public ObjectId MongoClientId { get; set; }
        public int ClientId { get; set; }
        [BsonElement("payment_type_id")]
        public ObjectId MongoPaymentTypeId { get; set; }
        public int PaymentTypeId { get; set; }
        [BsonElement("amount")]
        public decimal Amount { get; set; }
        [BsonElement("payment_date")]
        public DateTime PaymentDate { get; set; }
    }

}
