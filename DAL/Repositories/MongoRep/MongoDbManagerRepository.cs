using DAL.Interfaces;
using DAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace DAL.Repositories.MongoRep
{
    public class MongoDbManagerRepository : IManagerRepository
    {
        private readonly IMongoCollection<Client> _clients;
        private readonly IMongoCollection<Payment> _payments;
        private readonly IMongoCollection<SalesVolume> _salesVolume;
        private readonly IMongoCollection<ServiceFact> _serviceFact;

        public MongoDbManagerRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test");
            _clients = database.GetCollection<Client>("Clients");
            _payments = database.GetCollection<Payment>("Payments");
            _salesVolume = database.GetCollection<SalesVolume>("SalesVolumes");
            _serviceFact = database.GetCollection<ServiceFact>("ServiceFacts");
        }

        public void MonthlySalesVolume()
        {
            try
            {
                // Определяем текущий месяц и год в формате "yyyy-MM"
                string currentMonthYear = DateTime.Now.ToString("yyyy-MM");

                // Фильтр для поиска записей, соответствующих текущему месяцу
                var filter = Builders<SalesVolume>.Filter.Eq(sv => sv.MonthYear, currentMonthYear);

                // Поиск всех записей с этим фильтром
                var salesVolumes = _salesVolume.Find(filter).ToList();

                // Заголовок таблицы
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("| {0,-30} | {1,-30} | {2,-30} | {3,-30} |", "ID", "ServiceID", "QuantitySold", "MonthYear");
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------");
               
                // Чтение данных и вывод строк
                foreach (var volume in salesVolumes)
                {
                    string id = volume.MongoSalesVolumeId.ToString().PadRight(30); // ID занимает 30 символов
                    string serviceId = volume.MongoServiceId.ToString().PadRight(30); // ServiceID занимает 30 символов
                    string quantitySold = volume.QuantitySold.ToString("F2").PadRight(30); // QuantitySold с двумя знаками после запятой
                    string monthYear = volume.MonthYear.PadRight(30); // MonthYear в формате yyyy-MM

                    // Вывод строки с данными
                    Console.WriteLine("| {0,-30} | {1,-30} | {2,-30} | {3,-30} |", id, serviceId, quantitySold, monthYear);
                }

                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public void ShowTransactionsPerMonth()
        {
            try
            {
                // Определяем текущую дату
                var currentDate = DateTime.Now;
                var currentYear = currentDate.Year;
                var currentMonth = currentDate.Month;

                // Агрегатор для фильтрации по месяцу и году
                var aggregate = _payments.Aggregate()
                    .Match(p => p.PaymentDate.Year == currentYear && p.PaymentDate.Month == currentMonth)
                    .Project(p => new
                    {
                        p.MongoId,
                        p.MongoClientId,
                        p.MongoPaymentTypeId,
                        p.Amount,
                        p.PaymentDate
                    })
                    .ToList();

                // Заголовок таблицы с учетом ширины столбцов
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("| {0,-30} | {1,-30} | {2,-30} | {3,-30} | {4,-30} |", "ID", "ClientID", "TypeID", "Amount", "Date");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------");

                // Чтение данных и вывод строк
                foreach (var payment in aggregate)
                {
                    string id = payment.MongoId.ToString().PadRight(30); // ID занимает 30 символов
                    string clientId = payment.MongoClientId.ToString().PadRight(30); // ClientID занимает 30 символов
                    string paymentTypeId = payment.MongoPaymentTypeId.ToString().PadRight(30); // TypeID занимает 30 символов
                    string amount = payment.Amount.ToString("F2").PadRight(30); // Amount занимает 30 символов с двумя знаками после запятой
                    string paymentDate = payment.PaymentDate.ToString("yyyy-MM-dd").PadRight(30); // Date занимает 30 символов

                    // Вывод строки с данными
                    Console.WriteLine("| {0,-30} | {1,-30} | {2,-30} | {3,-30} | {4,-30} |", id, clientId, paymentTypeId, amount, paymentDate);
                }

                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------------------------------");


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public void TopService()
        {
            try
            {
                // Получение данных из коллекции ServiceFacts и сортировка по quantity (от большего к меньшему)
                var filter = Builders<ServiceFact>.Filter.Empty; // Без фильтра, все записи
                var sort = Builders<ServiceFact>.Sort.Descending(sf => sf.Quantity); // Сортировка по количеству

                // Получение топ-10 записей с сортировкой
                var topServices = _serviceFact.Find(filter)
                                              .Sort(sort)   // Применение сортировки
                                              .Limit(10)    // Ограничение на топ-10 записей
                                              .ToList();    // Преобразование в список

                // Полоса сверху
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

                // Заголовок таблицы (каждый столбец по 30 символов)
                Console.WriteLine("| {0,-30} | {1,-30} | {2,-30} | {3,-30} | {4,-30} | {5,-30} |",
                    "ID", "ClientID", "ServiceID", "StartDate", "EndDate", "Quantity");

                // Полоса между заголовком и данными
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");

                // Чтение данных и вывод строк
                foreach (var service in topServices)
                {
                    ObjectId id = service.MongoServiceFactId;
                    ObjectId clientId = service.MongoClientId;
                    ObjectId serviceId = service.MongoServiceId;
                    DateTime startDate = service.StartDate;
                    DateTime endDate = service.EndDate;
                    decimal quantity = service.Quantity;

                    // Вывод строки с данными (каждый столбец по 30 символов)
                    Console.WriteLine("| {0,-30} | {1,-30} | {2,-30} | {3,-30} | {4,-30} | {5,-30} |",
                        id.ToString(), clientId.ToString(), serviceId.ToString(), startDate.ToString("yyyy-MM-dd"),
                        endDate.ToString("yyyy-MM-dd"), quantity.ToString());
                }

                // Полоса снизу
                Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }



    }
}
