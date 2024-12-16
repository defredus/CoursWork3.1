using DAL.Interfaces;
using DAL.Models;
using BLL.IServices;
using DAL.Repositories;
using DAL.Repositories.SQLRep;
using System.Numerics;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository repository)
    {
        _clientRepository = repository;
    }

    public void GetDataOfClient(string id)
    {
        //Console.Clear();
        (string? name, string? address, string? phone, string? email, string? balance) = _clientRepository.GetDataOfClient(id);
        Console.WriteLine("Имя пользователя:" + name);
        Console.WriteLine("Адресс: "+ address);
        Console.WriteLine("Номер телефона: " + phone);
        Console.WriteLine("Почта: " + email );
        Console.WriteLine("Баланс счета: " +  balance);
    }
    public void ChangeTariffPlan(string id)
    {
        Console.Write("Введите название тарифа, который хотите удалить --> ");
        string choose = Console.ReadLine(); 
        _clientRepository.ChangeTariffPlan(id, choose);
    }
    public void ShowMyTariffPlan(string id)
    {
        List<string>elements = _clientRepository.ShowMyTariffPlan(id);
        int _ = 1;
        foreach(string elem in elements)
        {
            Console.WriteLine(_ + " "+ elem);
            _++;
        }
    }
    public void ChangePassword(string id)
    {
        Console.Write("Введите новый пароль --> ");
        string password = Console.ReadLine();
        _clientRepository.ChangePassword(id, password);
    }
}
