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
    public class SqlServiceRepository : IServiceRepository
    {
        private readonly string _connectionString;

        public SqlServiceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Add(Service service)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Service> GetAll()
        {
            const string query = "SELECT id, name, description, price FROM Services;";
            List<Service> services = new List<Service>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                services.Add(new Service
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    Description = reader.GetString(reader.GetOrdinal("description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("price"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return services;
        }

        public void AddNewServiceToClient(string id, string service)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Шаг 1: Получить id и price сервиса
                int serviceId;
                decimal servicePrice;

                using (SqlCommand getServiceCommand = new SqlCommand(
                    "SELECT id, price FROM Services WHERE name = @serviceName", connection))
                {
                    getServiceCommand.Parameters.AddWithValue("@serviceName", service);

                    using (SqlDataReader reader = getServiceCommand.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            throw new Exception("Сервис с указанным именем не найден.");
                        }

                        serviceId = reader.GetInt32(0);
                        servicePrice = reader.GetDecimal(1);
                    }
                }

                // Шаг 2: Проверить баланс клиента
                decimal clientBalance;

                using (SqlCommand getClientBalanceCommand = new SqlCommand(
                    "SELECT balance FROM Clients WHERE id = @clientId", connection))
                {
                    getClientBalanceCommand.Parameters.AddWithValue("@clientId", id);

                    object result = getClientBalanceCommand.ExecuteScalar();

                    if (result == null)
                    {
                        Console.WriteLine("Клиент с указанным ID не найден.");
                    }

                    clientBalance = Convert.ToDecimal(result);
                }

                if (clientBalance < servicePrice)
                {
                    Console.WriteLine("Недостаточно средств на балансе клиента.");
                    return;
                }

                // Шаг 3: Проверить, существует ли запись в ServiceFacts
                using (SqlCommand checkServiceFactCommand = new SqlCommand(
                    "SELECT COUNT(*) FROM ServiceFacts WHERE client_id = @clientId AND service_id = @serviceId", connection))
                {
                    checkServiceFactCommand.Parameters.AddWithValue("@clientId", id);
                    checkServiceFactCommand.Parameters.AddWithValue("@serviceId", serviceId);

                    int existingRecords = (int)checkServiceFactCommand.ExecuteScalar();

                    if (existingRecords > 0)
                    {
                        Console.WriteLine("У вас уже подключенна данная услуга");
                        return;
                    }
                }

                // Шаг 4: Добавить запись в ServiceFacts и обновить баланс клиента
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Добавление записи в ServiceFacts
                        using (SqlCommand insertServiceFactCommand = new SqlCommand(
                            "INSERT INTO ServiceFacts (client_id, service_id, start_date, end_date, quantity) " +
                            "VALUES (@clientId, @serviceId, @startDate, @endDate, @quantity)", connection, transaction))
                        {
                            DateTime startDate = DateTime.Now;
                            DateTime endDate = startDate.AddDays(30);

                            insertServiceFactCommand.Parameters.AddWithValue("@clientId", id);
                            insertServiceFactCommand.Parameters.AddWithValue("@serviceId", serviceId);
                            insertServiceFactCommand.Parameters.AddWithValue("@startDate", startDate.ToString("yyyy-MM-dd"));
                            insertServiceFactCommand.Parameters.AddWithValue("@endDate", endDate.ToString("yyyy-MM-dd"));
                            insertServiceFactCommand.Parameters.AddWithValue("@quantity", servicePrice);

                            insertServiceFactCommand.ExecuteNonQuery();
                        }

                        // Обновление баланса клиента
                        using (SqlCommand updateClientBalanceCommand = new SqlCommand(
                            "UPDATE Clients SET balance = balance - @servicePrice WHERE id = @clientId", connection, transaction))
                        {
                            updateClientBalanceCommand.Parameters.AddWithValue("@servicePrice", servicePrice);
                            updateClientBalanceCommand.Parameters.AddWithValue("@clientId", id);

                            updateClientBalanceCommand.ExecuteNonQuery();
                        }

                        // Шаг 5: Обновить количество проданных услуг в SalesVolumes
                        using (SqlCommand updateSalesVolumeCommand = new SqlCommand(
                            "UPDATE SalesVolumes SET quantity_sold = quantity_sold + @quantitySold WHERE service_id = @serviceId", connection, transaction))
                        {
                            updateSalesVolumeCommand.Parameters.AddWithValue("@quantitySold", servicePrice);
                            updateSalesVolumeCommand.Parameters.AddWithValue("@serviceId", serviceId);

                            updateSalesVolumeCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
 
        public Service GetById(int id)
            {
            throw new NotImplementedException();
            }

        public void Update(Service service)
            {
            throw new NotImplementedException();
            }

    }
}
