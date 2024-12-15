using CoursWorkUI.Interfaces;
using CoursWorkUI.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursWorkUI
{
    public static class Utils
    {
        public static ServiceStorage ConfigurationDI(IConfiguration configuration, string dbType)
        {
            // Конфигурация DI-контейнера
            var servCollection = new ServiceCollection();
            servCollection.Configure(configuration, dbType);

            var services = servCollection.BuildServiceProvider();
            var serviceStorage = services.GetRequiredService<ServiceStorage>();

            Console.WriteLine($"Работаем с базой данных: {dbType}");

            return serviceStorage;
        }
        public static IShowMenu? SwitchMenu(string role, string id, ServiceStorage serviceStorage)
        {
            switch (role)
            {
                case "client":
                    var client = new ClientMenu(id, serviceStorage);
                    return client;
                case "admin":
                    var admin = new AdminMenu(id, serviceStorage);
                    return admin;
                case "manager":
                    var manager = new ManagerMenu(id, serviceStorage);
                    return manager;
            }
            return null;
        }
        public static void WaitUser()
        {
            Console.WriteLine("Для продолжение нажмите Enter");
            Console.ReadLine();
        }
    }
}
