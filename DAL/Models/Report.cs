using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Report
    {
        [BsonId]
        public ObjectId MongoReportId { get; set; }  // Для MongoDB используем ObjectId
        public int Id { get; set; }
        [BsonElement("report_type")]
        public string ReportType { get; set; }
        [BsonElement("report_date")]
        public DateTime ReportDate { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
    }

}
