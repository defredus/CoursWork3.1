using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAdminRepository
    {
        void GetAllUsers();
        void ToggleUserStatus(string id, string str);
        void RollBackTransaction(string id);
    }
}
