using BLL.Services;
using BLL.IServices;
using BLL.Profiles;
using DAL;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace BLL
{
    public static class ConfigurationExtensionsBLL
    {
        public static void ConfigureBLL(this IServiceCollection services, string connection, string ConnectionType)
        {
            services.ConfigureDAL(connection, ConnectionType);

            services.AddAutoMapper(
                typeof(ClientProfile),
                typeof(PaymentProfile),
                typeof(ServicesProfile)
                );
            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IServiceService, ServiceService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IManagerService, ManagerService>();
        }
    }
}