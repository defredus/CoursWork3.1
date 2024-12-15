using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IServiceFactRepository: IRepository<ServiceFact>
    {
        IEnumerable<ServiceFact> GetAll();
        ServiceFact GetById(int id);
        void Add(ServiceFact serviceFact);
        void Update(ServiceFact serviceFact);
        void Delete(int id);
    }

}
