using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories.SQLRep
{
    public class SqlPaymentTypeRepository : IPaymentTypeRepository
    {
        private readonly string _connectionString;

        public SqlPaymentTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Add a new PaymentType to SQL Server
        public void Add(PaymentType paymentType)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO PaymentTypes (Name, Description) VALUES (@Name, @Description)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", paymentType.Name);
                        command.Parameters.AddWithValue("@Description", paymentType.Description);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the payment type: " + ex.Message);
            }
        }

        // Delete a PaymentType by its ID from SQL Server
        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM PaymentTypes WHERE Id = @Id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting the payment type: " + ex.Message);
            }
        }

        // Retrieve all PaymentTypes from SQL Server
        public IEnumerable<PaymentType> GetAll()
        {
            try
            {
                var paymentTypes = new List<PaymentType>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM PaymentTypes";
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                paymentTypes.Add(new PaymentType
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description"))
                                });
                            }
                        }
                    }
                }
                return paymentTypes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the payment types: " + ex.Message);
                return new List<PaymentType>();
            }
        }

        // Retrieve a PaymentType by its ID from SQL Server
        public PaymentType GetById(int id)
        {
            try
            {
                PaymentType paymentType = null;
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM PaymentTypes WHERE Id = @Id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                paymentType = new PaymentType
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description"))
                                };
                            }
                        }
                    }
                }
                return paymentType;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while fetching the payment type: " + ex.Message);
                return null;
            }
        }

        // Update an existing PaymentType in SQL Server
        public void Update(PaymentType paymentType)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "UPDATE PaymentTypes SET Name = @Name, Description = @Description WHERE Id = @Id";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", paymentType.Name);
                        command.Parameters.AddWithValue("@Description", paymentType.Description);
                        command.Parameters.AddWithValue("@Id", paymentType.Id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the payment type: " + ex.Message);
            }
        }
    }
}
