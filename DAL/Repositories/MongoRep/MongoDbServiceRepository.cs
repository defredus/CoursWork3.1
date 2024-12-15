using DAL.Interfaces;
using DAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.MongoRep
{
    public class MongoDbServiceRepository : IServiceRepository
    {
        private readonly IMongoCollection<Service> _services;
        private readonly IMongoCollection<Client> _clients;
        private readonly IMongoCollection<SalesVolume> _salesVolume;
        private readonly IMongoCollection<ServiceFact> _serviceFacts;

        public MongoDbServiceRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test");
            _services = database.GetCollection<Service>("Services");
            _clients = database.GetCollection<Client>("Clients");
            _serviceFacts = database.GetCollection<ServiceFact>("ServiceFacts");
            _salesVolume = database.GetCollection<SalesVolume>("SalesVolumes");
        }

        public void AddNewServiceToClient(string id, string service)
        {
            ObjectId objectId = ObjectId.Parse(id);

            try
            {
                // 1. Получаем сервис по названию
                var serviceRecord = _services.Find(s => s.Name == service).FirstOrDefault();
                if (serviceRecord == null)
                {
                    Console.WriteLine("Сервис с указанным названием не найден.");
                    return;
                }

                // Получаем цену сервиса и его Mongo ID
                string serviceId = serviceRecord.MongoServiceId.ToString();
                decimal servicePrice = serviceRecord.Price;

                // 2. Получаем баланс клиента
                var client = _clients.Find(c => c.MongoClientId == objectId).FirstOrDefault();
                if (client == null)
                {
                    Console.WriteLine("Клиент с указанным ID не найден.");
                    return;
                }

                decimal clientBalance = client.Balance;

                // Проверяем, достаточно ли средств на балансе клиента
                if (clientBalance < servicePrice)
                {
                    Console.WriteLine("Недостаточно средств на балансе клиента.");
                    return;
                }

                // 3. Проверяем, существует ли уже запись в ServiceFacts для этого клиента и сервиса
                var serviceFact = _serviceFacts.Find(sf => sf.MongoServiceFactId == objectId && sf.ServiceId.ToString() == serviceId).FirstOrDefault();
                if (serviceFact != null)
                {
                    Console.WriteLine("У вас уже подключен этот сервис.");
                    return;
                }

                // 4. Создаем новый ServiceFact и обновляем баланс клиента
                var startDate = DateTime.Now;
                var endDate = startDate.AddDays(30);  // Предполагаем, что сервис длится 30 дней

                var newServiceFact = new ServiceFact
                {
                    MongoServiceFactId = objectId,  // Преобразуем string id в ObjectId
                    MongoServiceId = new ObjectId(serviceId),
                    StartDate = startDate,
                    EndDate = endDate,
                    Quantity = servicePrice
                };

                // 4.1. Вставляем новый документ в ServiceFacts
                _serviceFacts.InsertOne(newServiceFact);

                // 4.2. Обновляем баланс клиента
                var updateClient = Builders<Client>.Update.Set(c => c.Balance, clientBalance - servicePrice);
                _clients.UpdateOne(c => c.MongoClientId == objectId, updateClient);

                // 4.3. Обновляем количество проданных услуг в SalesVolumes
                var updateSalesVolume = Builders<SalesVolume>.Update.Inc(sv => sv.QuantitySold, servicePrice);
                _salesVolume.UpdateOne(sv => sv.ServiceId.ToString() == serviceId, updateSalesVolume);

                Console.WriteLine("Сервис успешно подключен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        public IEnumerable<Service> GetAll()
        {
            try
            {
                // Получаем все записи из коллекции Services
                var services = _services.Find(service => true).ToList();

                return services; // Возвращаем все найденные услуги
            }
            catch (Exception ex)
            {
                // Логируем ошибку, если она произошла
                Console.WriteLine($"Ошибка при получении всех сервисов: {ex.Message}");
                return new List<Service>(); // Возвращаем пустой список в случае ошибки
            }
        }
        public void Add(Service service)
        {
            try
            {
                // Проверяем, существует ли уже сервис с таким названием
                var existingService = _services.Find(s => s.Name == service.Name).FirstOrDefault();
                if (existingService != null)
                {
                    Console.WriteLine("Сервис с таким названием уже существует.");
                    return;
                }

                // Вставляем новый сервис в коллекцию
                _services.InsertOne(service);
                Console.WriteLine($"Сервис '{service.Name}' успешно добавлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении сервиса: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                // Преобразуем ID в ObjectId
                var objectId = new ObjectId(id.ToString());

                // Удаляем сервис по ID
                var result = _services.DeleteOne(s => s.MongoServiceId == objectId);

                if (result.DeletedCount > 0)
                {
                    Console.WriteLine("Сервис успешно удален.");
                }
                else
                {
                    Console.WriteLine("Сервис с таким ID не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении сервиса: {ex.Message}");
            }
        }

        public Service GetById(int id)
        {
            try
            {
                // Преобразуем ID в ObjectId
                var objectId = new ObjectId(id.ToString());

                // Ищем сервис по ID
                var service = _services.Find(s => s.MongoServiceId == objectId).FirstOrDefault();

                if (service == null)
                {
                    Console.WriteLine("Сервис с таким ID не найден.");
                    return null;
                }

                return service;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении сервиса по ID: {ex.Message}");
                return null;
            }
        }

        public void Update(Service service)
        {
            try
            {
                // Преобразуем ID в ObjectId
                var objectId = new ObjectId(service.MongoServiceId.ToString());

                // Ищем существующий сервис для обновления
                var existingService = _services.Find(s => s.MongoServiceId == objectId).FirstOrDefault();

                if (existingService == null)
                {
                    Console.WriteLine("Сервис с таким ID не найден.");
                    return;
                }

                // Обновляем данные сервиса
                var update = Builders<Service>.Update
                    .Set(s => s.Name, service.Name)
                    .Set(s => s.Price, service.Price)
                    .Set(s => s.Description, service.Description);

                // Выполняем обновление
                var result = _services.UpdateOne(s => s.MongoServiceId == objectId, update);

                if (result.ModifiedCount > 0)
                {
                    Console.WriteLine("Сервис успешно обновлен.");
                }
                else
                {
                    Console.WriteLine("Не удалось обновить сервис.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении сервиса: {ex.Message}");
            }
        }

    }
}
