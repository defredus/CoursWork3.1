using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursWorkUI.Interfaces;

namespace CoursWorkUI.Users
{
    public class AdminMenu:IShowMenu
    {
        private string _id;
        private ServiceStorage _serviceStorage;
        public AdminMenu(string id, ServiceStorage serviceStorage) {
            _id = id;
            _serviceStorage = serviceStorage;
        }
        public void ShowMenu()
        {
            bool procces = true;
            while (procces)
            {   //попкорном пахло? 12.12.2024. ИГИ. 
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("1. Просмотр всех пользователей");
                Console.WriteLine("2. Отключить пользователя");
                Console.WriteLine("3. Активировать пользователя");
                Console.WriteLine("4. Просмотр транзакций");
                Console.WriteLine("5. Возврат платежа");
                Console.WriteLine("0. Выход");
                Console.WriteLine("=================================");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        _serviceStorage.adminService.GetAllUsers();
                        Utils.WaitUser();
                        break;
                    case "2":
                        Console.Clear();
                        _serviceStorage.adminService.GetAllUsers();
                        _serviceStorage.adminService.ToggleUserStatus("false");
                        Utils.WaitUser();
                        break;
                    case "3":
                        Console.Clear();
                        _serviceStorage.adminService.GetAllUsers();
                        _serviceStorage.adminService.ToggleUserStatus("true");
                        Utils.WaitUser();
                        break;
                    case "4":
                        Console.Clear();
                        _serviceStorage.paymentService.GetAll();
                        Utils.WaitUser();
                        break;
                    case "5":
                        Console.Clear();
                        _serviceStorage.paymentService.GetAll();
                        _serviceStorage.adminService.RollBackTransaction();
                        Utils.WaitUser();
                        break;
                    case "0":
                        procces = false;
                        break;
                }

            }
        }
    }
}
