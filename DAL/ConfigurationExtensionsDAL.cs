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
using DAL.Repositories.MongoRep;

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
                    services.AddScoped<IServiceRepository, MongoDbServiceRepository>(repo => new MongoDbServiceRepository(connection));
                    services.AddScoped<IPaymentRepository, MongoDbPaymentRepository>(repo => new MongoDbPaymentRepository(connection));
                    services.AddScoped<IAdminRepository, MongoDbAdminRepository>(repo => new MongoDbAdminRepository(connection));
                    services.AddScoped<IServiceFactRepository, MongoServiceFactRepository>(repo => new MongoServiceFactRepository(connection));
                    services.AddScoped<IPriceListRepository, MongoPriceListRepository>(repo => new MongoPriceListRepository(connection));
                    services.AddScoped<ISalesVolumeRepository, MongoSalesVolumeRepository>(repo => new MongoSalesVolumeRepository(connection));
                    services.AddScoped<IManagerRepository, MongoDbManagerRepository>(repo => new MongoDbManagerRepository(connection));
                    break;

                case "SQL":
                    // Регистрация SQL репозитория
                    services.AddScoped<IClientRepository, SQLClientRepository>(repo => new SQLClientRepository(connection));
                    services.AddScoped<IServiceRepository, SqlServiceRepository>(repo => new SqlServiceRepository(connection));
                    services.AddScoped<IPaymentRepository, SqlPaymentRepository>(repo => new SqlPaymentRepository(connection));
                    services.AddScoped<IAdminRepository, SqlAdminRepository>(repo => new SqlAdminRepository(connection));
                    services.AddScoped<IServiceFactRepository, SqlServiceFactRepository>(repo => new SqlServiceFactRepository(connection));
                    services.AddScoped<IPriceListRepository, SqlPriceListRepository>(repo => new SqlPriceListRepository(connection));
                    services.AddScoped<ISalesVolumeRepository, SqlSalesVolumeRepository>(repo => new SqlSalesVolumeRepository(connection));
                    services.AddScoped<IManagerRepository, SqlManagerRepository>(repo => new SqlManagerRepository(connection));
                    break;

                default:
                    throw new ArgumentException("Нет такой базы данных");
            }
        }

    }
}
