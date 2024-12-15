using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursWorkUI.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CoursWorkUI.Users
{

    public class ClientMenu : IShowMenu
    {
        private string _id;
        private ServiceStorage _serviceStorage;
        public ClientMenu(string id, ServiceStorage serviceStorage)
        {
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
                Console.WriteLine("1. Данные счета");
                Console.WriteLine("2. Отключить сервис");
                Console.WriteLine("3. Добавить сервис");
                Console.WriteLine("4. Мои сервисы");
                Console.WriteLine("5. Прейскурант");
                Console.WriteLine("6. Пополнить баланс");
                Console.WriteLine("7. Изменить пароль");
                Console.WriteLine("0. Выход");
                Console.WriteLine("=================================");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        _serviceStorage.clientService.GetDataOfClient(_id);
                        Utils.WaitUser();
                        break;
                    case "2":
                        Console.Clear();
                        _serviceStorage.clientService.ShowMyTariffPlan(_id);
                        _serviceStorage.clientService.ChangeTariffPlan(_id);
                        Utils.WaitUser();
                        break;
                    case "3":
                        Console.Clear();
                        _serviceStorage.serviceService.GetAll();
                        _serviceStorage.serviceService.AddNewServiceToClient(Convert.ToInt32(_id));
                        Utils.WaitUser();
                        break;
                    case "4":
                        Console.Clear();
                        _serviceStorage.clientService.ShowMyTariffPlan(_id);
                        Utils.WaitUser();
                        break;
                    case "5":
                        Console.Clear();
                        _serviceStorage.serviceService.GetAll();
                        Utils.WaitUser();
                        break;
                    case "6":
                        Console.Clear();
                        _serviceStorage.paymentService.ReplenishBalance(_id);
                        Utils.WaitUser();
                        break;
                    case "7":
                        Console.Clear();
                        _serviceStorage.clientService.ChangePassword(_id);
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
