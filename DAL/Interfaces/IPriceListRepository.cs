using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IPriceListRepository: IRepository<PriceList>
    {
        IEnumerable<PriceList> GetAll();
        PriceList GetById(int id);
        void Add(PriceList priceList);
        void Update(PriceList priceList);
        void Delete(int id);
    }

}
