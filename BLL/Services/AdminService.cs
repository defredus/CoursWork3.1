using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.IServices;
using DAL.Interfaces;

namespace BLL.Services
{
    public class AdminService:IAdminService
    {
        private IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public void RollBackTransaction()
        {
            Console.Write("Введите id трансакции --> ");
            string id = Console.ReadLine();
            _adminRepository.RollBackTransaction(id);
        }
        public void GetAllUsers()
        {
            _adminRepository.GetAllUsers();
        }
        public void ToggleUserStatus(string str)
        {
            Console.Write("Введите id пользователя --> ");
            string? id = Console.ReadLine();
            _adminRepository.ToggleUserStatus(id, str);
        }
    }
}
