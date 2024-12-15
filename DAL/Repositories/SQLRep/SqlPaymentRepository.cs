using DAL.Interfaces;
using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

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
            string query = @"
                                INSERT INTO Payments (client_id, payment_type_id, amount, payment_date)
                                VALUES (@clientId, @paymentTypeId, @amount, @paymentDate)
                            ";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@clientId", payment.ClientId);
                    command.Parameters.AddWithValue("@paymentTypeId", payment.PaymentTypeId);
                    command.Parameters.AddWithValue("@amount", payment.Amount);
                    command.Parameters.AddWithValue("@paymentDate", payment.PaymentDate);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Платеж успешно добавлен.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при добавлении платежа: " + ex.Message);
                    }
                }
            }
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM Payments WHERE id = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Платеж удален.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при удалении платежа: " + ex.Message);
                    }
                }
            }
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
            string query = "SELECT [id], [client_id], [payment_type_id], [amount], [payment_date] FROM Payments WHERE id = @id";
            Payment payment = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                payment = new Payment
                                {
                                    Id = reader.GetInt32(0),
                                    ClientId = reader.GetInt32(1),
                                    PaymentTypeId = reader.GetInt32(2),
                                    Amount = reader.GetDecimal(3),
                                    PaymentDate = reader.GetDateTime(4)
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при получении платежа: " + ex.Message);
                    }
                }
            }

            return payment;
        }

        public void Update(Payment payment)
        {
            string query = @"
                                UPDATE Payments
                                SET client_id = @clientId, payment_type_id = @paymentTypeId, amount = @amount, payment_date = @paymentDate
                                WHERE id = @id
                            ";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", payment.Id);
                    command.Parameters.AddWithValue("@clientId", payment.ClientId);
                    command.Parameters.AddWithValue("@paymentTypeId", payment.PaymentTypeId);
                    command.Parameters.AddWithValue("@amount", payment.Amount);
                    command.Parameters.AddWithValue("@paymentDate", payment.PaymentDate);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Платеж обновлен.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при обновлении платежа: " + ex.Message);
                    }
                }
            }
        }
    }
}
