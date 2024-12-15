using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IServiceService
    {
        void GetAll();
        void AddNewServiceToClient(int id);
    }
}
