using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP224_Kadrgaliev
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                var products = Core.Context.Product
                    .Where(product => product.IsActive)
                    .OrderBy(product => product.Id)
                    .ToList();

                Console.WriteLine("Каталог товаров\n");

                foreach (var product in products)
                {
                    Console.WriteLine(
                        "{0}. {1} — {2:F2} руб.",
                        product.Id,
                        product.ProductName,
                        product.Price);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    "Ошибка получения товаров: " +
                    exception.GetBaseException().Message);
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода.");
            Console.ReadKey();
        }
    }
}
