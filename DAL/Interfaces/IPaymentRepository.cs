using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IPaymentRepository: IRepository<Payment>
    {
        IEnumerable<Payment> GetAll();
        Payment GetById(int id);
        void Add(Payment payment);
        void Update(Payment payment);
        void Delete(int id);

        void ReplenishBalance(string id, decimal sum);
    }

}
