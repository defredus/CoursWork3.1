using DAL.Interfaces;
using DAL.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

public class MongoDbClientRepository : IClientRepository
{
    private readonly IMongoCollection<Client> _clients;
    private readonly IMongoCollection<ServiceFact> _serviceFacts;
    private readonly IMongoCollection<Service> _services;

    // Конструктор для инициализации подключения к MongoDB
    public MongoDbClientRepository(string connectionString)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("test");
        _clients = database.GetCollection<Client>("Clients");
        _serviceFacts = database.GetCollection<ServiceFact>("ServiceFacts");
        _services = database.GetCollection<Service>("Services");
    }

    // Получить всех клиентов
    public IEnumerable<Client> GetAll()
    {
        try
        {
            // Получение всех клиентов из коллекции "Clients"
            var clients = _clients.Find(client => true).ToList();

            // Вывод всех найденных клиентов в консоль
            foreach (var client in clients)
            {
                Console.WriteLine($"Id: {client.Id}, Name: {client.Name}, Phone: {client.Phone}, Email: {client.Email}, Role: {client.Role}");
            }

            return clients; // Возвращаем список клиентов
        }
        catch (Exception ex)
        {
            // Логируем ошибку или обрабатываем ее по мере необходимости
            Console.WriteLine($"Ошибка при получении всех клиентов: {ex.Message}");
            return new List<Client>(); // Возвращаем пустой список в случае ошибки
        }
    }

    // Получить клиента по ID
    public Client GetById(int id)
    {
        return _clients.Find(client => client.Id == id).FirstOrDefault();
    }

    // Добавить нового клиента
    public void Add(Client client)
    {
        try
        {
            _clients.InsertOne(client);
            Console.WriteLine("Клиент добавлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении клиента: {ex.Message}");
        }
    }

    // Обновить данные клиента
    public void Update(Client client)
    {
        try
        {
            var result = _clients.ReplaceOne(c => c.MongoClientId == client.MongoClientId, client);
            if (result.MatchedCount > 0)
            {
                Console.WriteLine("Клиент обновлен.");
            }
            else
            {
                Console.WriteLine("Клиент не найден.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении клиента: {ex.Message}");
        }
    }

    // Удалить клиента по ID
    public void Delete(int id)
    {
        try
        {
            var result = _clients.DeleteOne(client => client.Id == id);
            if (result.DeletedCount > 0)
            {
                Console.WriteLine("Клиент удален.");
            }
            else
            {
                Console.WriteLine("Клиент не найден.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении клиента: {ex.Message}");
        }
    }

    // Получить данные клиента по ID
    public (string name, string address, string phone, string email, string balance) GetDataOfClient(string id)
    {
        var client = _clients.Find(c => c.MongoClientId.ToString() == id).FirstOrDefault();
        if (client == null)
        {
            throw new Exception("Клиент не найден.");
        }

        return (client.Name, client.Address, client.Phone, client.Email, client.Balance.ToString());
    }

    // Смена тарифного плана (реализуйте по необходимости)
    public void ChangeTariffPlan(string id)
    {
        throw new NotImplementedException();
    }

    // Показать текущий тарифный план
    public List<string?> ShowMyTariffPlan(string id)
    {
        try
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                Console.WriteLine($"Невозможно преобразовать строку в ObjectId: {id}");
                return new List<string?>(); 
            }

            var serviceFacts = _serviceFacts.Find(sf => sf.MongoServiceFactId == objectId).ToList();

            if (serviceFacts == null || serviceFacts.Count == 0)
            {
                Console.WriteLine("У вас нет подключенных сервисов");
                return new List<string?>();
            }

            var serviceIds = serviceFacts.Select(sf => sf.ServiceId).ToList();

            var services = _services.Find(s => serviceIds.Contains(s.Id)).ToList();

            var serviceNames = services.Select(s => s.Name).ToList();

            return serviceNames;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении тарифного плана: {ex.Message}");
            return new List<string?>();
        }
    }
    public void ChangeTariffPlan(string id, string choose)
    {
        try
        {
            ObjectId objectId = ObjectId.Parse(id);
            // 1. Найти service_id по названию тарифа
            var service = _services.Find(s => s.Name == choose).FirstOrDefault();

            if (service == null)
            {
                Console.WriteLine("Тариф с таким названием не найден.");
                return; // Если тариф не найден, выходим из метода
            }

            // 2. Найти все записи в ServiceFacts для данного клиента и удалить их
            var result = _serviceFacts.DeleteMany(sf => sf.MongoServiceFactId == objectId && sf.ServiceId == service.Id);

            if (result.DeletedCount > 0)
            {
                Console.WriteLine("Тарифный план успешно изменен.");
            }
            else
            {
                Console.WriteLine("Тарифный план для данного клиента не найден.");
            }
        }
        catch (Exception ex)
        {
            // Логируем ошибку
            Console.WriteLine($"Ошибка при изменении тарифного плана: {ex.Message}");
        }
    }


    public void ChangePassword(string id, string password)
    {
        ObjectId objectId = ObjectId.Parse(id);
        var client = _clients.Find(c => c.MongoClientId == objectId).FirstOrDefault();
        if (client == null)
        {
            throw new Exception("Клиент не найден.");
        }

        client.Password = password;
        Update(client);
    }

    public (string? id, string? role) AuthenticateUser(string phone, string password)
    {
        var client = _clients.Find(c => c.Phone == phone).FirstOrDefault();
        if (client == null)
        {
            return (null, null);
        }

        if (client.Password == password)
        {
            return (client.MongoClientId.ToString(), client.Role);
        }

        return (null, null);
    }
}
