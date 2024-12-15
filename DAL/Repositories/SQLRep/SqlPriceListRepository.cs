using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories.SQLRep
{
    public class SqlPriceListRepository : IPriceListRepository
    {
        private readonly string _connectionString;

        public SqlPriceListRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Add a new PriceList to the SQL database
        public void Add(PriceList priceList)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "INSERT INTO PriceLists (ServiceId, Price, ValidFrom, ValidUntil) VALUES (@ServiceId, @Price, @ValidFrom, @ValidUntil)", connection);

                    command.Parameters.AddWithValue("@ServiceId", priceList.ServiceId);
                    command.Parameters.AddWithValue("@Price", priceList.Price);
                    command.Parameters.AddWithValue("@ValidFrom", priceList.ValidFrom);
                    command.Parameters.AddWithValue("@ValidUntil", priceList.ValidUntil);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the price list: " + ex.Message);
            }
        }

        // Delete a PriceList by its ID from the SQL database
        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("DELETE FROM PriceLists WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the price list: " + ex.Message);
            }
        }

        // Retrieve all PriceLists from the SQL database
        public IEnumerable<PriceList> GetAll()
        {
            var priceLists = new List<PriceList>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM PriceLists", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var priceList = new PriceList
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceId")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                ValidFrom = reader.GetDateTime(reader.GetOrdinal("ValidFrom")),
                                ValidUntil = reader.GetDateTime(reader.GetOrdinal("ValidUntil"))
                            };
                            priceLists.Add(priceList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the price lists: " + ex.Message);
            }

            return priceLists;
        }

        // Retrieve a PriceList by its ID from the SQL database
        public PriceList GetById(int id)
        {
            PriceList priceList = null;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM PriceLists WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            priceList = new PriceList
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ServiceId = reader.GetInt32(reader.GetOrdinal("ServiceId")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                ValidFrom = reader.GetDateTime(reader.GetOrdinal("ValidFrom")),
                                ValidUntil = reader.GetDateTime(reader.GetOrdinal("ValidUntil"))
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the price list: " + ex.Message);
            }

            return priceList;
        }

        // Update an existing PriceList in the SQL database
        public void Update(PriceList priceList)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "UPDATE PriceLists SET ServiceId = @ServiceId, Price = @Price, ValidFrom = @ValidFrom, ValidUntil = @ValidUntil WHERE Id = @Id", connection);

                    command.Parameters.AddWithValue("@ServiceId", priceList.ServiceId);
                    command.Parameters.AddWithValue("@Price", priceList.Price);
                    command.Parameters.AddWithValue("@ValidFrom", priceList.ValidFrom);
                    command.Parameters.AddWithValue("@ValidUntil", priceList.ValidUntil);
                    command.Parameters.AddWithValue("@Id", priceList.Id);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the price list: " + ex.Message);
            }
        }
    }
}
