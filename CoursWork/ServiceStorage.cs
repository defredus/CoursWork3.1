using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BLL.IServices;

namespace CoursWorkUI
{
    public class ServiceStorage
    {
        public IAuthService authorization { get; set; }
        public IClientService clientService { get; set; }
        public IServiceService serviceService { get; set; }
        public IPaymentService paymentService { get; set; }
        public IAdminService adminService { get; set; }
        public ServiceStorage(IAuthService authorization, IClientService clientService,
                                IServiceService serviceService, IPaymentService paymentService,
                                IAdminService adminService)
        {
            this.authorization = authorization;
            this.clientService = clientService;
            this.serviceService = serviceService;
            this.paymentService = paymentService;
            this.adminService = adminService;
        }
    }
}
