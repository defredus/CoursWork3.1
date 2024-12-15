using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ISalesVolumeRepository : IRepository<SalesVolume>
    {
        IEnumerable<SalesVolume> GetAll();
        SalesVolume GetById(int id);
        void Add(SalesVolume salesVolume);
        void Update(SalesVolume salesVolume);
        void Delete(int id);
    }

}
