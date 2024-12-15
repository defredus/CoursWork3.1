using DAL.Interfaces;
using Microsoft.Data.SqlClient;

namespace DAL.Repositories.SQLRep
{
    public class SqlAdminRepository : IAdminRepository
    {
        private readonly string _connectionString;

        public SqlAdminRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void RollBackTransaction(string id)
        {
            // Проверка, что id не пустое
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty", nameof(id));
            }

            // SQL-запросы
            string deletePaymentQuery = "DELETE FROM Payments WHERE id = @id";
            string updateClientBalanceQuery = @"
                        UPDATE Clients
                        SET balance = balance - @balance
                        WHERE id = @id";

            // SQL-запрос для получения client_id и amount
            string selectPaymentDetailsQuery = @"
                        SELECT client_id, amount
                        FROM Payments
                        WHERE id = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Получить client_id и amount
                        int clientId;
                        decimal amount;

                        using (SqlCommand selectCommand = new SqlCommand(selectPaymentDetailsQuery, connection, transaction))
                        {
                            // Убедитесь, что параметр передается правильно
                            selectCommand.Parameters.AddWithValue("@id", id);

                            using (SqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    clientId = reader.GetInt32(0); // ClientId
                                    amount = reader.GetDecimal(1); // Amount
                                }
                                else
                                {
                                    throw new Exception("Payment not found.");
                                }
                            }
                        }

                        // Удалить запись из Payments
                        using (SqlCommand deleteCommand = new SqlCommand(deletePaymentQuery, connection, transaction))
                        {
                            // Параметр для удаления
                            deleteCommand.Parameters.AddWithValue("@id", id);
                            deleteCommand.ExecuteNonQuery();
                        }

                        // Обновить баланс клиента
                        using (SqlCommand updateCommand = new SqlCommand(updateClientBalanceQuery, connection, transaction))
                        {
                            updateCommand.Parameters.AddWithValue("@id", clientId);
                            updateCommand.Parameters.AddWithValue("@balance", amount);
                            updateCommand.ExecuteNonQuery();
                        }

                        // Подтвердить транзакцию
                        transaction.Commit();
                    }
                    catch
                    {
                        // Откатить транзакцию в случае ошибки
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        public void ToggleUserStatus(string id, string str)
        {
            // Проверка входного значения str
            if (str != "true" && str != "false")
            {
                Console.WriteLine("Invalid status value. Use 'true' or 'false'.");
                return;
            }

            // Преобразование строки str в логическое значение
            bool isActive = str == "true";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // SQL-запрос для обновления статуса пользователя
                    string query = "UPDATE Clients SET isActive = @isActive WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Добавление параметров
                        command.Parameters.AddWithValue("@isActive", isActive);
                        command.Parameters.AddWithValue("@id", id);

                        // Выполнение команды
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Пользователь с id =  {id} был {(isActive ? "активирован" : "диактивирован")}.");
                        }
                        else
                        {
                            Console.WriteLine($"Пользователь с id =  {id} не найден.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        public void GetAllUsers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Запрос для получения всех пользователей
                    string query = @"
                    SELECT [id], [name], [address], [phone], [email], [role], [password], [balance], [isActive]
                    FROM [CoursWorkKt].[dbo].[Clients]
                    ORDER BY 
                        CASE 
                            WHEN [role] = 'admin' THEN 1
                            WHEN [role] = 'manager' THEN 2
                            WHEN [role] = 'client' THEN 3
                            ELSE 4
                        END, [name]";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine($"{"ID",-5}{"Name",-35}{"Address",-40}{"Phone",-30}{"Email",-30}{"Role",-30}{"Balance",-30}{"IsActive",-10}");
                            Console.WriteLine(new string('-', 210));
                            Console.WriteLine("\n");
                            while (reader.Read())
                            {
                                Console.WriteLine($"{reader["id"],-5}{reader["name"],-35}{reader["address"],-40}{reader["phone"],-30}{reader["email"],-35}{reader["role"],-30}{reader["balance"],-30}{(reader["isActive"].ToString() == "True" ? "Yes" : "No"),-10}");
                            }
                            Console.WriteLine(new string('-', 210));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
