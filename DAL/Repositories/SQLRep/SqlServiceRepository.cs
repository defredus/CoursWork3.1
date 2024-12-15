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
            const string query = "INSERT INTO Services (name, description, price) VALUES (@name, @description, @price);";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", service.Name);
                    command.Parameters.AddWithValue("@description", service.Description);
                    command.Parameters.AddWithValue("@price", service.Price);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Service added successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }

        public void Delete(int id)
        {
            const string query = "DELETE FROM Services WHERE id = @id;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Service deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
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

        public Service GetById(int id)
        {
            const string query = "SELECT id, name, description, price FROM Services WHERE id = @id;";
            Service service = null;

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
                                service = new Service
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    Description = reader.GetString(reader.GetOrdinal("description")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("price"))
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return service;
        }

        public void Update(Service service)
        {
            const string query = @"
                UPDATE Services 
                SET name = @name, description = @description, price = @price 
                WHERE id = @id;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", service.Id);
                    command.Parameters.AddWithValue("@name", service.Name);
                    command.Parameters.AddWithValue("@description", service.Description);
                    command.Parameters.AddWithValue("@price", service.Price);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Service updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }

        public void AddNewServiceToClient(string id, string service)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Step 1: Get service id and price
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
                            throw new Exception("Service with the specified name not found.");
                        }

                        serviceId = reader.GetInt32(0);
                        servicePrice = reader.GetDecimal(1);
                    }
                }

                // Step 2: Check client balance
                decimal clientBalance;

                using (SqlCommand getClientBalanceCommand = new SqlCommand(
                    "SELECT balance FROM Clients WHERE id = @clientId", connection))
                {
                    getClientBalanceCommand.Parameters.AddWithValue("@clientId", id);

                    object result = getClientBalanceCommand.ExecuteScalar();

                    if (result == null)
                    {
                        Console.WriteLine("Client with the specified ID not found.");
                        return;
                    }

                    clientBalance = Convert.ToDecimal(result);
                }

                if (clientBalance < servicePrice)
                {
                    Console.WriteLine("Insufficient funds on the client's balance.");
                    return;
                }

                // Step 3: Check if record exists in ServiceFacts
                using (SqlCommand checkServiceFactCommand = new SqlCommand(
                    "SELECT COUNT(*) FROM ServiceFacts WHERE client_id = @clientId AND service_id = @serviceId", connection))
                {
                    checkServiceFactCommand.Parameters.AddWithValue("@clientId", id);
                    checkServiceFactCommand.Parameters.AddWithValue("@serviceId", serviceId);

                    int existingRecords = (int)checkServiceFactCommand.ExecuteScalar();

                    if (existingRecords > 0)
                    {
                        Console.WriteLine("This service is already connected to the client.");
                        return;
                    }
                }

                // Step 4: Add record to ServiceFacts and update client balance
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert ServiceFact record
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

                        // Update client balance
                        using (SqlCommand updateClientBalanceCommand = new SqlCommand(
                            "UPDATE Clients SET balance = balance - @servicePrice WHERE id = @clientId", connection, transaction))
                        {
                            updateClientBalanceCommand.Parameters.AddWithValue("@servicePrice", servicePrice);
                            updateClientBalanceCommand.Parameters.AddWithValue("@clientId", id);

                            updateClientBalanceCommand.ExecuteNonQuery();
                        }

                        // Step 5: Update sales volume in SalesVolumes
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
    }
}
