using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IAdminService
    {
        void GetAllUsers();
        void ToggleUserStatus(string str);
        void RollBackTransaction();
    }
}
