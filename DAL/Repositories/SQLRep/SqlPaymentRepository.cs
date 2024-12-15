using DAL.Interfaces;
using DAL.Models;
using Microsoft.Data.SqlClient;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.SQLRep
{
    public class SqlPaymentRepository : IPaymentRepository
    {
        private readonly string _connectionString;

        public SqlPaymentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ReplenishBalance(string id, decimal sum)
        {
            string query = @"
                                BEGIN TRANSACTION;

                                INSERT INTO Payments (client_id, payment_type_id, amount, payment_date)
                                VALUES (@id, 1, @sum, GETDATE());

                                UPDATE Clients
                                SET balance = balance + @sum
                                WHERE id = @id;

                                COMMIT TRANSACTION;
                            ";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@sum", sum);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Ваш баланс успешно поплнен на" + sum + "рублей");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
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
            var payments = new List<Payment>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"SELECT [id], [client_id], [payment_type_id], [amount], [payment_date] 
                                 FROM [CoursWorkKt].[dbo].[Payments]";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var payment = new Payment
                                {
                                    Id = reader.GetInt32(0),
                                    ClientId = reader.GetInt32(1),
                                    PaymentTypeId = reader.GetInt32(2),
                                    Amount = reader.GetDecimal(3),
                                    PaymentDate = reader.GetDateTime(4)
                                };
                                payments.Add(payment);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return payments;
        }
        public Payment GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}
