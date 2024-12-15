using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IClientRepository: IRepository<Client>
    {
        IEnumerable<Client> GetAll();
        Client GetById(int id);
        void Add(Client client);
        void Update(Client client);
        void Delete(int id);
        (string? id, string? role) AuthenticateUser(string phone, string password);

        (
        string name, string address, string phone, 
        string email, string balance
         ) 
        GetDataOfClient(string id);
        void ChangeTariffPlan(string id, string choose);
        List<string?> ShowMyTariffPlan(string id);
        void ChangePassword(string id, string password);
    }
}
