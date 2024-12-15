using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IAuthService
    {
        (string id, string role) Authenticate(string phone, string password);
    }

}
