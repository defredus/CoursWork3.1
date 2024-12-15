using BLL.Services;
using DAL;
using DAL.Interfaces;
using DAL.Repositories;
using MongoDB.Driver;

namespace CoursWorkUI.UI
{
    public static class AuthMenu
    {
       public static (string phone, string password) Show()
        {
            Console.Clear();
            Console.Write("Введите номер телефона --> ");
            string phone = Console.ReadLine();
            Console.Write("Введите пароль --> ");
            string password = Console.ReadLine();

            return (phone, password);
        }
    }
}
