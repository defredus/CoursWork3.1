using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }

}
