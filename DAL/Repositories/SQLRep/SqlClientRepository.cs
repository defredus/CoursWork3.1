using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Microsoft.Data.SqlClient;
using MongoDB.Driver.Core.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net;
using System.Numerics;

namespace DAL.Repositories.SQLRep
{
    public class SQLClientRepository : IClientRepository
    {
        private readonly string _connectionString;

        public SQLClientRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void ChangePassword(string id, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Запрос на изменение пароля пользователя
                string query = "UPDATE Clients SET password = @newPassword WHERE id = @clientId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Добавляем параметры для запроса
                    command.Parameters.AddWithValue("@newPassword", password);
                    command.Parameters.AddWithValue("@clientId", id);

                    try
                    {
                        // Выполняем запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("Пользователь с таким ID не найден.");
                        }
                        else
                        {
                            Console.WriteLine("Пароль успешно изменен.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Обработка ошибок
                        Console.WriteLine("Ошибка при изменении пароля: " + ex.Message);
                    }
                }
            }
        }
        public List<string?> ShowMyTariffPlan(string id)
        {
            const string query = @"SELECT Services.name
                                    FROM Services
                                    INNER JOIN ServiceFacts ON Services.id = ServiceFacts.service_id
                                    WHERE ServiceFacts.client_id = @ClientId;";
            List<string?> services = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClientId", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Заполняем данные клиента, если они еще не заполнены
                                services.Add(reader["name"].ToString());
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


        public void ChangeTariffPlan(string id, string choose)
        {
            const string queryGetServiceId = @"
        SELECT id FROM Services
        WHERE name = @ServiceName;
    ";

            const string queryDeleteServiceFact = @"
        DELETE FROM ServiceFacts
        WHERE client_id = @ClientId AND service_id = @ServiceId;
    ";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Шаг 1: Найти service_id по названию тарифа
                    int serviceId = -1;
                    using (SqlCommand command = new SqlCommand(queryGetServiceId, connection))
                    {
                        command.Parameters.AddWithValue("@ServiceName", choose);

                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            serviceId = Convert.ToInt32(result);
                        }
                        else
                        {
                            Console.WriteLine("Тариф с таким названием не найден.");
                            return; // Если тариф не найден, выходим из метода
                        }
                    }

                    // Шаг 2: Удалить запись из ServiceFacts по найденному service_id
                    using (SqlCommand command = new SqlCommand(queryDeleteServiceFact, connection))
                    {
                        command.Parameters.AddWithValue("@ClientId", id);        // client_id
                        command.Parameters.AddWithValue("@ServiceId", serviceId); // найденный service_id

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Тарифный план успешно удален.");
                        }
                        else
                        {
                            Console.WriteLine("Тарифный план для данного клиента не найден.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка: " + ex.Message);
            }
        }



        public (string? name, string? address, string? phone, string? email, string? balance) GetDataOfClient(string id)
        {
            const string query = @"
                                    SELECT * FROM Clients
                                    WHERE Clients.id = @ClientId";
            string? name = null;
            string? address = null;
            string? phone = null;
            string? email = null;
            string? balance = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClientId", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Заполняем данные клиента, если они еще не заполнены
                                if (name == null)
                                {
                                    name = reader["name"]?.ToString();
                                    address = reader["address"]?.ToString();
                                    phone = reader["phone"]?.ToString();
                                    email = reader["email"]?.ToString();
                                    balance = reader["balance"]?.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return (name, address, phone, email, balance);
        }


        public (string? id, string? role) AuthenticateUser(string phone, string password)
        {
            const string query = "SELECT Id, Role FROM Clients WHERE Phone = @Phone AND Password = @Password";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           return (Convert.ToString(reader.GetInt32(0)), reader.GetString(1));
                        }
                        else
                        {
                            return (null, null); 
                        }
                    }
                }
            }
        }


        // CRUD операции
        public void Add(Client entity)
        {
            const string query = @"
            INSERT INTO Clients (Name, Address, Phone, Email, Role, Password)
            VALUES (@Name, @Address, @Phone, @Email, @Role, @Password)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    command.Parameters.AddWithValue("@Address", entity.Address);
                    command.Parameters.AddWithValue("@Phone", entity.Phone);
                    command.Parameters.AddWithValue("@Email", entity.Email);
                    command.Parameters.AddWithValue("@Role", entity.Role);
                    command.Parameters.AddWithValue("@Password", entity.Password);

                    command.ExecuteNonQuery();
                }
            }
        }

        public Client GetById(int id)
        {
            const string query = "SELECT * FROM Clients WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Client
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString(),
                                Password = reader["Password"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public IEnumerable<Client> GetAll()
        {
            const string query = "SELECT * FROM Clients";
            var clients = new List<Client>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new Client
                            {
                                Id = (int)reader["Id"],
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString(),
                                Password = reader["Password"].ToString()
                            });
                        }
                    }
                }
            }
            return clients;
        }

        public void Update(Client entity)
        {
            const string query = @"
            UPDATE Clients
            SET Name = @Name, Address = @Address, Phone = @Phone, Email = @Email, Role = @Role, Password = @Password
            WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", entity.Id);
                    command.Parameters.AddWithValue("@Name", entity.Name);
                    command.Parameters.AddWithValue("@Address", entity.Address);
                    command.Parameters.AddWithValue("@Phone", entity.Phone);
                    command.Parameters.AddWithValue("@Email", entity.Email);
                    command.Parameters.AddWithValue("@Role", entity.Role);
                    command.Parameters.AddWithValue("@Password", entity.Password);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            const string query = "DELETE FROM Clients WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}