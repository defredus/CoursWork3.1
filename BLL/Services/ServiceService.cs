using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.IServices;
using DAL.Interfaces;
using DAL.Models;

namespace BLL.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        public ServiceService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        public void GetAll()
        {
            IEnumerable<Service> service = _serviceRepository.GetAll();
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            foreach (Service serviceItem in service) {
                Console.WriteLine("{0,-30} | {1,-50} | {2,10}",
                                serviceItem.Name, serviceItem.Description, serviceItem.Price);
            }
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");

        }
        public void AddNewServiceToClient(string id)
        {
            Console.Write("Введите название сервиса на добавление --> ");
            string service = Console.ReadLine();
            _serviceRepository.AddNewServiceToClient(id, service);
        }
    }
}
