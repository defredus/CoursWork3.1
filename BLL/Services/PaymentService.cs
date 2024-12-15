using BLL.IServices;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository repository)
        {
            _paymentRepository = repository;
        }

        public void ReplenishBalance(string id)
        {
            Console.Write("Введите сумму, на которую хотите поплнить баланс --> ");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            _paymentRepository.ReplenishBalance(id, sum);
        }
        public void GetAll()
        {
            IEnumerable<Payment> payments = _paymentRepository.GetAll();

            // Подписи столбцов
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("|      ID                  |    ClientID              |    TypeID               |    Amount   |      Date    |");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");

            // Данные в строках
            foreach (var payment in payments)
            {
                Console.WriteLine($"| {payment.MongoId,6} | {payment.MongoClientId,8} | {payment.MongoPaymentTypeId,6} | {payment.Amount,10} | {payment.PaymentDate:yyyy-MM-dd} |");
            }

            Console.WriteLine("-------------------------------------------------------");

        }
    }
}
