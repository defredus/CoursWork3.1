using DAL.Interfaces;
using DAL.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DAL.Repositories.SQLRep
{
    public class SqlServiceFactRepository : IServiceFactRepository
    {
        private readonly string _connectionString;

        public SqlServiceFactRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Add a new ServiceFact record
        public void Add(ServiceFact serviceFact)
        {
            const string query = "INSERT INTO ServiceFacts (client_id, service_id, start_date, end_date, quantity) " +
                                 "VALUES (@clientId, @serviceId, @startDate, @endDate, @quantity)";
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@clientId", serviceFact.ClientId);
                        command.Parameters.AddWithValue("@serviceId", serviceFact.ServiceId);
                        command.Parameters.AddWithValue("@startDate", serviceFact.StartDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@endDate", serviceFact.EndDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@quantity", serviceFact.Quantity);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the service fact: " + ex.Message);
            }
        }

        // Delete a ServiceFact record by ID
        public void Delete(int id)
        {
            const string query = "DELETE FROM ServiceFacts WHERE id = @id";
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the service fact: " + ex.Message);
            }
        }

        // Get all ServiceFact records
        public IEnumerable<ServiceFact> GetAll()
        {
            const string query = "SELECT id, client_id, service_id, start_date, end_date, quantity FROM ServiceFacts";
            List<ServiceFact> serviceFacts = new List<ServiceFact>();

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
                                serviceFacts.Add(new ServiceFact
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    ClientId = reader.GetInt32(reader.GetOrdinal("client_id")),
                                    ServiceId = reader.GetInt32(reader.GetOrdinal("service_id")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal("end_date")),
                                    Quantity = reader.GetDecimal(reader.GetOrdinal("quantity"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching service facts: " + ex.Message);
            }

            return serviceFacts;
        }

        // Get a ServiceFact by its ID
        public ServiceFact GetById(int id)
        {
            const string query = "SELECT id, client_id, service_id, start_date, end_date, quantity FROM ServiceFacts WHERE id = @id";
            ServiceFact serviceFact = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                serviceFact = new ServiceFact
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    ClientId = reader.GetInt32(reader.GetOrdinal("client_id")),
                                    ServiceId = reader.GetInt32(reader.GetOrdinal("service_id")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal("end_date")),
                                    Quantity = reader.GetDecimal(reader.GetOrdinal("quantity"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the service fact: " + ex.Message);
            }

            return serviceFact;
        }

        // Update a ServiceFact record
        public void Update(ServiceFact serviceFact)
        {
            const string query = "UPDATE ServiceFacts SET client_id = @clientId, service_id = @serviceId, " +
                                 "start_date = @startDate, end_date = @endDate, quantity = @quantity WHERE id = @id";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", serviceFact.Id);
                        command.Parameters.AddWithValue("@clientId", serviceFact.ClientId);
                        command.Parameters.AddWithValue("@serviceId", serviceFact.ServiceId);
                        command.Parameters.AddWithValue("@startDate", serviceFact.StartDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@endDate", serviceFact.EndDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@quantity", serviceFact.Quantity);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the service fact: " + ex.Message);
            }
        }
    }
}
