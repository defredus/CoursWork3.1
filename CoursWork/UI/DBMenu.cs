using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CoursWorkUI.Users;

namespace CoursWorkUI.UI
{
    public static class DBMenu
    {
        static DBMenu()
        {
        }
        public static void Show()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            bool process = true;

            while (process)
            {
                Console.Clear();
                Console.WriteLine("=================================");
                Console.WriteLine("1. Работать в MSSQL");
                Console.WriteLine("2. Работать в MongoDB");
                Console.WriteLine("0. Выход из программы");
                Console.WriteLine("=================================");

                var choice = Console.ReadLine();

                string dbType = choice switch
                {
                    "1" => "SQL",
                    "2" => "MONGO",
                    "0" => null,
                    _ => null
                };

                if (dbType == null)
                {
                    if (choice == "0")
                    {
                        Console.WriteLine("Выход из программы...");
                        process = false;
                    }
                    else
                    {
                        Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    }
                    continue;
                }
                //boss kfc?
                var serviceStorage = Utils.ConfigurationDI(configuration, dbType);

                var(phone, password) = AuthMenu.Show();
                var (id, role) = serviceStorage.authorization.Authenticate(phone, password);
                if (id != null)
                {
                    Console.Clear();
                    Console.WriteLine("Аутентификация прошла успешно :)");
                    Thread.Sleep(2000);
                    var entity = Utils.SwitchMenu(role, id, serviceStorage);
                    entity.ShowMenu();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Пользователь не найден :(");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
