using BLL.IServices;
using BLL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoursWorkUI
{

    public static class ConfigurationExtensionsUI
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration, string dbType)
        {
            string connectionString;

            switch (dbType)
            {
                case "MONGO":
                    connectionString = configuration.GetConnectionString("MONGO");
                    services.ConfigureBLL(connectionString, dbType);
                    break;

                case "SQL":
                    connectionString = configuration.GetConnectionString("SQL");
                    services.ConfigureBLL(connectionString, dbType);
                    break;

                default:
                    throw new ArgumentException($"Нет такой базы данных: {dbType}");
            }

            // Регистрация ServiceStorage с необходимыми сервисами
            services.AddSingleton(provider =>
            {
                return new ServiceStorage(
                    provider.GetRequiredService<IAuthService>(),
                    provider.GetRequiredService<IClientService>(),
                    provider.GetRequiredService<IServiceService>(),
                    provider.GetRequiredService<IPaymentService>(),
                    provider.GetRequiredService<IAdminService>(),
                    provider.GetRequiredService<IManagerService>()
                    );
            });
        }
    }

}