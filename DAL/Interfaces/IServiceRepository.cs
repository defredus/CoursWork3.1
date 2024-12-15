using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IServiceRepository: IRepository<Service>
    {
        IEnumerable<Service> GetAll();
        Service GetById(int id);
        void Add(Service service);
        void Update(Service service);
        void Delete(int id);
        void AddNewServiceToClient(string id, string service);
    }

}
