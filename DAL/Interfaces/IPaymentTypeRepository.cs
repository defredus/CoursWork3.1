using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IPaymentTypeRepository: IRepository<PaymentType>
    {
        IEnumerable<PaymentType> GetAll();
        PaymentType GetById(int id);
        void Add(PaymentType paymentType);
        void Update(PaymentType paymentType);
        void Delete(int id);
    }


}
