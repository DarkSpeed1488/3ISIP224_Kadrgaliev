using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP224_Kadrgaliev
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            IAccountService accountService = new Core();
            Account currentAccount = null;
            string choice;
            do
            {
                Console.Clear();
                Console.WriteLine("Файл аккаунтов: " + Core.AccountPath);
                if (currentAccount != null)
                {
                    Console.WriteLine("\nВы вошли как: " + currentAccount.UserName);
                }
                Console.WriteLine("\n1 — Каталог товаров");
                if (currentAccount == null)
                {
                    Console.WriteLine("2 — Регистрация");
                    Console.WriteLine("3 — Войти в аккаунт");
                }
                else
                {
                    Console.WriteLine("2 — Выйти из аккаунта");
                }
                Console.WriteLine("0 — Закрыть программу");
                Console.Write("Выберите действие: ");
                choice = Console.ReadLine();
                try
                {
                    if (choice == "1")
                    {
                        var products = Core.Context.Product.Where(product => product.IsActive).OrderBy(product => product.Id).ToList();
                        foreach (var product in products)
                        {
                            Console.WriteLine("{0}. {1} — {2:F2} руб.", product.Id, product.ProductName, product.Price);
                        }
                    }
                    else if (choice == "2")
                    {
                        if (currentAccount == null)
                        {
                            Console.Write("Ник: ");
                            string userName = Console.ReadLine();
                            Console.Write("Пароль: ");
                            string password = Console.ReadLine();
                            Console.Write("Повторите пароль: ");
                            string confirmation = Console.ReadLine();
                            Console.WriteLine(accountService.Register(userName, password, confirmation));
                        }
                        else
                        {
                            currentAccount = null;
                            Console.WriteLine("Вы вышли из аккаунта.");
                        }
                    }
                    else if (choice == "3" && currentAccount == null)
                    {
                        Console.Write("Ник: ");
                        string userName = Console.ReadLine();
                        Console.Write("Пароль: ");
                        string password = Console.ReadLine();
                        currentAccount = accountService.Login(userName, password);
                        if (currentAccount == null) Console.WriteLine("Неверный ник или пароль.");
                        else Console.WriteLine("Вход выполнен!");
                    }
                    else if (choice != "0" && choice != null)
                    {
                        Console.WriteLine("Такого пункта нет.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Ошибка: " + exception.GetBaseException().Message);
                }
                if (choice != "0" && choice != null)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения.");
                    Console.ReadKey(true);
                }
            } while (choice != "0" && choice != null);
        }
    }
}