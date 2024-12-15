using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class PriceList
    {
        [BsonId]
        public ObjectId MongoPriceListId { get; set; }  // Для MongoDB используем ObjectId
        public int Id { get; set; }
        [BsonElement("service_id")]
        public int ServiceId { get; set; }
        [BsonElement("price")]
        public decimal Price { get; set; }
        [BsonElement("valid_form")]
        public DateTime ValidFrom { get; set; }
        [BsonElement("valid_until")]
        public DateTime ValidUntil { get; set; }
    }

}
