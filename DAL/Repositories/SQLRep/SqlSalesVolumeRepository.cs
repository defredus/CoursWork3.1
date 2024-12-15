using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories.SQLRep
{
    public class SqlSalesVolumeRepository : ISalesVolumeRepository
    {
        private readonly string _connectionString;

        public SqlSalesVolumeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Add a new SalesVolume to the SQL database
        public void Add(SalesVolume salesVolume)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "INSERT INTO SalesVolumes (ServiceId, QuantitySold, MonthYear) VALUES (@ServiceId, @QuantitySold, @MonthYear)", connection);

                    command.Parameters.AddWithValue("@ServiceId", salesVolume.ServiceId);
                    command.Parameters.AddWithValue("@QuantitySold", salesVolume.QuantitySold);
                    command.Parameters.AddWithValue("@MonthYear", salesVolume.MonthYear);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the sales volume: " + ex.Message);
            }
        }

        // Delete a SalesVolume by its ID from the SQL database
        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("DELETE FROM SalesVolumes WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the sales volume: " + ex.Message);
            }
        }

        // Retrieve all SalesVolumes from the SQL database
        public IEnumerable<SalesVolume> GetAll()
        {
            var salesVolumes = new List<SalesVolume>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM SalesVolumes", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var salesVolume = new SalesVolume
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceId")),
                                QuantitySold = reader.GetInt32(reader.GetOrdinal("QuantitySold")),
                                MonthYear = reader.GetString(reader.GetOrdinal("MonthYear"))
                            };
                            salesVolumes.Add(salesVolume);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the sales volumes: " + ex.Message);
            }

            return salesVolumes;
        }

        // Retrieve a SalesVolume by its ID from the SQL database
        public SalesVolume GetById(int id)
        {
            SalesVolume salesVolume = null;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM SalesVolumes WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            salesVolume = new SalesVolume
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceId")),
                                QuantitySold = reader.GetInt32(reader.GetOrdinal("QuantitySold")),
                                MonthYear = reader.GetString(reader.GetOrdinal("MonthYear"))
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the sales volume: " + ex.Message);
            }

            return salesVolume;
        }

        // Update an existing SalesVolume in the SQL database
        public void Update(SalesVolume salesVolume)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "UPDATE SalesVolumes SET ServiceId = @ServiceId, QuantitySold = @QuantitySold, MonthYear = @MonthYear WHERE Id = @Id", connection);

                    command.Parameters.AddWithValue("@ServiceId", salesVolume.ServiceId);
                    command.Parameters.AddWithValue("@QuantitySold", salesVolume.QuantitySold);
                    command.Parameters.AddWithValue("@MonthYear", salesVolume.MonthYear);
                    command.Parameters.AddWithValue("@Id", salesVolume.Id);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the sales volume: " + ex.Message);
            }
        }
    }
}
