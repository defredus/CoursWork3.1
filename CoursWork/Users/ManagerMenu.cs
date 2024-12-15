using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursWorkUI.Interfaces;
using SharpCompress.Common;

namespace CoursWorkUI.Users
{
    public class ManagerMenu : IShowMenu
    {
        private readonly string _id;
        private readonly ServiceStorage _serviceStorage;
        public ManagerMenu(string id, ServiceStorage serviceStorage) {
            _id = id;
            _serviceStorage = serviceStorage;
        }
        public void ShowMenu()
        {
            bool procces = true;
            while (procces)
            {  
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("1. Просмотр транзакций за этот месяц");
                Console.WriteLine("2. Объемы реализации за месяц");
                Console.WriteLine("3. Статистика по сервисам");
                Console.WriteLine("4. Просмотр прейскуранта");
                Console.WriteLine("0. Выход");
                Console.WriteLine("=================================");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        _serviceStorage.managerService.ShowTransactionsPerMonth();
                        Utils.WaitUser();
                        break;
                    case "2":
                        Console.Clear();
                        _serviceStorage.managerService.MonthlySalesVolume();
                        Utils.WaitUser();
                        break;
                    case "3":
                        Console.Clear();
                        _serviceStorage.managerService.TopService();
                        Utils.WaitUser();
                        break;
                    case "4":
                        Console.Clear();
                        _serviceStorage.serviceService.GetAll();
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
