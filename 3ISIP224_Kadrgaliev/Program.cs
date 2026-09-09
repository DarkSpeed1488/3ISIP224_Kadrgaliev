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
            Console.Write("Введите количество операций, которые будут записаны: ");
            int countFinTran = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            if (countFinTran < 2)
            {
                Console.WriteLine("Вы ввели слишком мало операций!");
            }
            else if (countFinTran > 40)
            {
                Console.WriteLine("Вы ввели слишком много операций!");
            }
            else
            {
                Console.WriteLine("Введите свои траты, которые будут записаны: ");
                Console.Write("Пример: (Влажные салфетки \"Лента\"; 235)");
            }
        }
    }
}
