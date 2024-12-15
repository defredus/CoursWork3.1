using BLL.IServices;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ManagerService :IManagerService
    {
        private IManagerRepository _managerRepository;

        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }
        public void ShowTransactionsPerMonth()
        {
            _managerRepository.ShowTransactionsPerMonth();
        }
        public void MonthlySalesVolume()
        {
            _managerRepository.MonthlySalesVolume();
        }
        public void TopService()
        {
            _managerRepository.TopService();
        }
    }
}
