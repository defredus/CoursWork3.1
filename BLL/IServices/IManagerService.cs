using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IServices
{
    public interface IManagerService
    {
        void ShowTransactionsPerMonth();
        void MonthlySalesVolume();
        void TopService();
    }
}
