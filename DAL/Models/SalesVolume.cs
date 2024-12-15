using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class SalesVolume
    {
        [BsonId]
        public ObjectId MongoSalesVolumeId { get; set; }  // Для MongoDB используем ObjectId
        public int Id { get; set; }
        [BsonElement("service_id")]
        public ObjectId MongoServiceId { get; set; }
        public int ServiceId { get; set; }
        [BsonElement("quantity_sold")]
        public int QuantitySold { get; set; }
        [BsonElement("month_year")]
        public string MonthYear { get; set; }
    }

}
