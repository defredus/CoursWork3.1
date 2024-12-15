using BLL.IServices;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientRepository _clientRepository;

        public AuthService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public (string? id, string? role) Authenticate(string phone, string password)
        {
            // Пример вызова репозитория для аутентификации
            var(id, role)= _clientRepository.AuthenticateUser(phone, password);

            if (id == null)
            {
                //Исключительная ситуация 
                return (null, null);
            }
            return (id, role);
        }
    }
}
