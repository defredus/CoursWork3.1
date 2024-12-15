using DAL.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.SQLRep
{
    public class SqlManagerRepository : IManagerRepository
    {
        private readonly string _connectionString;

        public SqlManagerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void TopService()
        {
            // SQL-запрос для выборки данных, отсортированных по количеству (quantity)
            string query = @"
        SELECT [id], [client_id], [service_id], [start_date], [end_date], [quantity]
        FROM [CoursWorkKt].[dbo].[ServiceFacts]
        ORDER BY [quantity] DESC";  // Сортировка по quantity от большего к меньшему

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Заголовок таблицы
                        Console.WriteLine("-----------------------------------------------------------------------------------------");
                        Console.WriteLine("|   ID   | ClientID | ServiceID | StartDate   | EndDate     | Quantity |");
                        Console.WriteLine("-----------------------------------------------------------------------------------------");

                        // Чтение данных и вывод строк
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int clientId = reader.GetInt32(1);
                            int serviceId = reader.GetInt32(2);
                            DateTime startDate = reader.GetDateTime(3);
                            DateTime endDate = reader.GetDateTime(4);
                            decimal quantity = reader.GetDecimal(5);

                            // Вывод строки с данными
                            Console.WriteLine($"| {id,6} | {clientId,8} | {serviceId,9} | {startDate:yyyy-MM-dd} | {endDate:yyyy-MM-dd} | {quantity,8} |");
                        }

                        Console.WriteLine("-----------------------------------------------------------------------------------------");
                    }
                }
            }
        }

        public void MonthlySalesVolume()
        {
            string query = @"
        SELECT [id], [service_id], [quantity_sold], [month_year]
        FROM [CoursWorkKt].[dbo].[SalesVolumes]
        WHERE LEFT([month_year], 7) = FORMAT(GETDATE(), 'yyyy-MM')";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Заголовок таблицы
                        Console.WriteLine("------------------------------------------------------------");
                        Console.WriteLine("|   ID   | ServiceID | QuantitySold | MonthYear |");
                        Console.WriteLine("------------------------------------------------------------");

                        // Чтение данных и вывод строк
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int serviceId = reader.GetInt32(1);
                            decimal quantitySold = reader.GetDecimal(2);
                            string monthYear = reader.GetString(3);

                            // Вывод строки с данными
                            Console.WriteLine($"| {id,6} | {serviceId,9} | {quantitySold,12} | {monthYear} |");
                        }

                        Console.WriteLine("------------------------------------------------------------");
                    }
                }
            }
        }


        public void ShowTransactionsPerMonth()
        {
                string query = @"
            SELECT [id], [client_id], [payment_type_id], [amount], [payment_date]
            FROM [CoursWorkKt].[dbo].[Payments]
            WHERE MONTH([payment_date]) = MONTH(GETDATE()) 
            AND YEAR([payment_date]) = YEAR(GETDATE())";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Заголовок таблицы
                        Console.WriteLine("------------------------------------------------------------");
                        Console.WriteLine("|   ID   | ClientID | TypeID |   Amount   |    Date    |");
                        Console.WriteLine("------------------------------------------------------------");

                        // Чтение данных и вывод строк
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int clientId = reader.GetInt32(1);
                            int paymentTypeId = reader.GetInt32(2);
                            decimal amount = reader.GetDecimal(3);
                            DateTime paymentDate = reader.GetDateTime(4);

                            // Вывод строки с данными
                            Console.WriteLine($"| {id,6} | {clientId,8} | {paymentTypeId,6} | {amount,10:F2} | {paymentDate:yyyy-MM-dd} |");
                        }

                        Console.WriteLine("------------------------------------------------------------");
                    }
                }
            }
        }

    }
}
