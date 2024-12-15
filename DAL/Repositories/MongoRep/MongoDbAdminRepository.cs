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
    public class MongoDbAdminRepository : IAdminRepository
    {
        private readonly IMongoCollection<Client> _clients;
        private readonly IMongoCollection<Payment> _payments;

        public MongoDbAdminRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test");
            _clients = database.GetCollection<Client>("Clients");
            _payments = database.GetCollection<Payment>("Payments");

        }
        public void GetAllUsers()
        {
            try
            {
                // Получение всех клиентов из коллекции "Clients"
                var users = _clients.Find(client => true).ToList();

                if (users.Count == 0)
                {
                    Console.WriteLine("Нет пользователей в базе данных.");
                }
                else
                {
                    // Заголовок
                    Console.WriteLine("{0,-25} {1,-30} {2,-25} {3,-20} {4,-10} {5,-15}",
                        "ID", "Имя", "Почта", "Баланс", "Активен", "Роль");
                    Console.WriteLine(new string('-', 120)); // Отделяем заголовок от данных
                    Console.WriteLine("\n");
                    // Перебираем всех пользователей и выводим их данные
                    foreach (var user in users)
                    {
                        Console.WriteLine("{0,-25} {1,-30} {2,-25} {3,-20:F2} {4,-10} {5,-15}",
                            user.MongoClientId,
                            user.Name,
                            user.Email,
                            user.Balance,
                            user.IsActive ? "Да" : "Нет",
                            user.Role);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении пользователей: {ex.Message}");
            }
        }

        public void RollBackTransaction(string id)
        {
            ObjectId objectId = ObjectId.Parse(id);
            // Проверка, что id не пустое
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty", nameof(id));
            }

            // Коллекции MongoDB

            try
            {
                // 1. Найти платеж и извлечь данные клиента и сумму
                var payment = _payments.Find(p => p.MongoId == objectId).FirstOrDefault();
                if (payment == null)
                {
                    throw new Exception("Payment not found.");
                }

                // 2. Получить client_id и amount
                var clientId = payment.MongoClientId;
                var amount = payment.Amount;

                // 3. Удалить запись из Payments
                var deleteResult = _payments.DeleteOne(p => p.MongoId == objectId);
                if (deleteResult.DeletedCount == 0)
                {
                    throw new Exception("Failed to delete the payment.");
                }

                // 4. Обновить баланс клиента
                ObjectId objectClient = clientId;
                var updateDefinition = Builders<Client>.Update.Inc(c => c.Balance, -amount);
                var updateResult = _clients.UpdateOne(c => c.MongoClientId == objectClient, updateDefinition);
                if (updateResult.ModifiedCount == 0)
                {
                    throw new Exception("Failed to update the client's balance.");
                }
            }
            catch
            {
                throw; // Пробрасываем исключение для обработки на более высоком уровне
            }
        }


        public void ToggleUserStatus(string id, string str)
        {
            // Проверка входного значения str
            if (str != "true" && str != "false")
            {
                Console.WriteLine("Invalid status value. Use 'true' or 'false'.");
                return;
            }

            // Преобразование строки str в логическое значение
            bool isActive = str == "true";

            try
            {
                // Преобразование строки id в ObjectId
                var objectId = new MongoDB.Bson.ObjectId(id);

                // Фильтр для поиска пользователя по id
                var filter = Builders<Client>.Filter.Eq(c => c.MongoClientId, objectId);

                // Обновление поля isActive
                var update = Builders<Client>.Update.Set(c => c.IsActive, isActive);

                // Применение обновления
                var result = _clients.UpdateOne(filter, update);

                if (result.ModifiedCount > 0)
                {
                    Console.WriteLine($"Пользователь с id = {id} был {(isActive ? "активирован" : "деактивирован")}.");
                }
                else
                {
                    Console.WriteLine($"Пользователь с id = {id} не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

    }
}
