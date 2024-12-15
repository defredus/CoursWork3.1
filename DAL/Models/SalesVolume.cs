using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class SalesVolume
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int QuantitySold { get; set; }
        public DateTime MonthYear { get; set; }
    }

}
