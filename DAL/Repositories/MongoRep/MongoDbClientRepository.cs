using DAL.Interfaces;
using DAL.Models;
using MongoDB.Driver;

public class MongoDbClientRepository : IClientRepository
{
    private readonly IMongoCollection<Client> _clients;

    public MongoDbClientRepository(string connectionString)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("CoursWorkKt");
        _clients = database.GetCollection<Client>("Сlients");
    }

    public IEnumerable<Client> GetAll() => _clients.Find(client => true).ToList();

    public Client GetById(int id) => _clients.Find(client => client.Id == id).FirstOrDefault();

    public void Add(Client client) => _clients.InsertOne(client);

    public void Update(Client client) => _clients.ReplaceOne(c => c.Id == client.Id, client);

    public void Delete(int id) => _clients.DeleteOne(client => client.Id == id);

    public (string id, string phone, string role) AuthenticateUser(string phone, string password)
    {
        var client = _clients.Find(c => c.Phone == phone).FirstOrDefault();
        if (client == null) ;

        return ("1", "1", "1");
    }

    (string id, string role) IClientRepository.AuthenticateUser(string phone, string password)
    {
        throw new NotImplementedException();
    }

    public (string name, string address, string phone, string email, string balance) GetDataOfClient(string id)
    {
        throw new NotImplementedException();
    }

    public void ChangeTariffPlan(string id)
    {
        throw new NotImplementedException();
    }

    public List<string?> ShowMyTariffPlan(string id)
    {
        throw new NotImplementedException();
    }

    public void ChangeTariffPlan(string id, string choose)
    {
        throw new NotImplementedException();
    }

    public void ChangePassword(string id, string password)
    {
        throw new NotImplementedException();
    }
}
