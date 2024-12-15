using DAL.Interfaces;
using DAL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.MongoRep
{
    public class MongoServiceFactRepository : IServiceFactRepository
    {
        private readonly IMongoCollection<ServiceFact> _serviceFact;

        public MongoServiceFactRepository(string connectionString)
        {
            var service = new MongoClient(connectionString);
            var database = service.GetDatabase("test");
            _serviceFact = database.GetCollection<ServiceFact>("ServiceFacts");
        }
        public void Add(ServiceFact serviceFact)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ServiceFact> GetAll()
        {
            throw new NotImplementedException();
        }

        public ServiceFact GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ServiceFact serviceFact)
        {
            throw new NotImplementedException();
        }
    }
}
