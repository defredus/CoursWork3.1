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
    public class MongoDbPaymentRepository : IPaymentRepository
    {
        private readonly IMongoCollection<Payment> _payment;
        private readonly IMongoCollection<Client> _client;

        public MongoDbPaymentRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test");
            _payment = database.GetCollection<Payment>("Payments");
            _client = database.GetCollection<Client>("Clients");
        }

        // Добавление нового платежа
        public void Add(Payment payment)
        {
            try
            {
                // Вставляем новый платеж в коллекцию Payments
                _payment.InsertOne(payment);
                Console.WriteLine($"Платеж на сумму {payment.Amount} успешно добавлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении платежа: {ex.Message}");
            }
        }

        // Удаление платежа по ID
        public void Delete(int id)
        {
            try
            {
                // Преобразуем ID в ObjectId
                var objectId = new ObjectId(id.ToString());

                // Удаляем платеж из коллекции
                var result = _payment.DeleteOne(payment => payment.MongoId == objectId);

                if (result.DeletedCount > 0)
                {
                    Console.WriteLine($"Платеж с ID {id} был успешно удален.");
                }
                else
                {
                    Console.WriteLine($"Платеж с ID {id} не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении платежа: {ex.Message}");
            }
        }

        // Получение платежа по ID
        public Payment GetById(int id)
        {
            try
            {
                // Преобразуем ID в ObjectId
                var objectId = new ObjectId(id.ToString());

                // Ищем платеж по MongoId
                var payment = _payment.Find(p => p.MongoId == objectId).FirstOrDefault();

                if (payment != null)
                {
                    return new Payment
                    {
                        MongoId = payment.MongoId,
                        MongoClientId = payment.MongoClientId,
                        MongoPaymentTypeId = payment.MongoPaymentTypeId,
                        Amount = payment.Amount,
                        PaymentDate = payment.PaymentDate
                    };
                }
                else
                {
                    Console.WriteLine($"Платеж с ID {id} не найден.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении платежа: {ex.Message}");
                return null;
            }
        }

        // Обновление платежа
        public void Update(Payment payment)
        {
            try
            {
                // Формируем обновление платежа
                var updateDefinition = Builders<Payment>.Update
                    .Set(p => p.MongoClientId, payment.MongoClientId)
                    .Set(p => p.MongoPaymentTypeId, payment.MongoPaymentTypeId)
                    .Set(p => p.Amount, payment.Amount)
                    .Set(p => p.PaymentDate, payment.PaymentDate);

                // Преобразуем MongoId в ObjectId
                var result = _payment.UpdateOne(p => p.MongoId == payment.MongoId, updateDefinition);

                if (result.ModifiedCount > 0)
                {
                    Console.WriteLine($"Платеж с ID {payment.MongoId} был успешно обновлен.");
                }
                else
                {
                    Console.WriteLine($"Платеж с ID {payment.MongoId} не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении платежа: {ex.Message}");
            }
        }

        // Получение всех платежей
        public IEnumerable<Payment> GetAll()
        {
            try
            {
                // Получение всех транзакций из коллекции "Payments"
                var payments = _payment.Find(payment => true).ToList();

                if (payments.Count == 0)
                {
                    // Возвращаем пустую коллекцию, если транзакций нет
                    return Enumerable.Empty<Payment>();
                }

                // Возвращаем список транзакций
                return payments.Select(payment => new Payment
                {
                    MongoId = payment.MongoId,
                    MongoClientId = payment.MongoClientId,
                    MongoPaymentTypeId = payment.MongoPaymentTypeId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate
                });
            }
            catch (Exception ex)
            {
                // Логируем ошибку и возвращаем пустую коллекцию
                Console.WriteLine($"Ошибка при получении транзакций: {ex.Message}");
                return Enumerable.Empty<Payment>();
            }
        }

        // Пополнение баланса клиента
        public void ReplenishBalance(string id, decimal sum)
        {
            // Преобразуем строковый ID в ObjectId
            ObjectId objectId = ObjectId.Parse(id);

            try
            {
                // 1. Проверяем, существует ли клиент с таким MongoClientId
                var client = _client.Find(c => c.MongoClientId == objectId).FirstOrDefault();  // Находим клиента
                if (client == null)
                {
                    Console.WriteLine("Клиент с таким MongoClientId не найден.");
                    return;
                }

                // 2. Вставляем новый платеж в коллекцию Payments
                var payment = new Payment
                {
                    MongoClientId = objectId,  // Здесь указываем MongoClientId для платежа
                    PaymentTypeId = 1, // Тип платежа (например, 1 - карта)
                    Amount = sum,
                    PaymentDate = DateTime.UtcNow
                };
                _payment.InsertOne(payment);  // Вставка в Payments

                // 3. Обновляем баланс клиента
                var updateClient = Builders<Client>.Update.Inc(c => c.Balance, sum);  // Увеличиваем баланс на сумму
                var result = _client.UpdateOne(c => c.MongoClientId == objectId, updateClient);  // Обновление баланса клиента

                if (result.ModifiedCount > 0)
                {
                    Console.WriteLine($"Баланс клиента успешно пополнен на {sum} рублей.");
                }
                else
                {
                    Console.WriteLine("Не удалось обновить баланс клиента.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
    }
}
