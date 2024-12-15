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
        public void Add(Payment payment)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

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


        public Payment GetById(int id)
        {
            throw new NotImplementedException();
        }

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



        public void Update(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
