using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    internal interface IReportRepository: IRepository<Report>
    {
        IEnumerable<Report> GetAll();
        Report GetById(int id);
        void Add(Report priceList);
        void Update(Report priceList);
        void Delete(int id);
    }
}
