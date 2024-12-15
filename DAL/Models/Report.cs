using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string ReportType { get; set; }
        public DateTime ReportDate { get; set; }
        public string Content { get; set; }
    }

}
