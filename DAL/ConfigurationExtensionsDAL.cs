using Microsoft.EntityFrameworkCore;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DAL.Repositories.SQLRep;

namespace DAL
{
    public static class ConfigurationExtensionsDAL
    {
        public static void ConfigureDAL(this IServiceCollection services, string connection, string DBType)
        {
            switch (DBType)
            {
                case "MONGO":
                    // Регистрация MongoDB репозитория
                    services.AddScoped<IClientRepository, MongoDbClientRepository>(repo => new MongoDbClientRepository(connection));
                    break;

                case "SQL":
                    // Регистрация SQL репозитория
                    services.AddScoped<IClientRepository, SQLClientRepository>(repo => new SQLClientRepository(connection));
                    services.AddScoped<IServiceRepository, SqlServiceRepository>(repo => new SqlServiceRepository(connection));
                    services.AddScoped<IPaymentRepository, SqlPaymentRepository>(repo => new SqlPaymentRepository(connection));
                    services.AddScoped<IAdminRepository, SqlAdminRepository>(repo => new SqlAdminRepository(connection));
                    break;

                default:
                    throw new ArgumentException("Нет такой базы данных");
            }
        }

    }
}
