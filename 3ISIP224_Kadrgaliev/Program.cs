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
                //Console.WriteLine("Введите свои траты, которые будут записаны: ");
                //Console.Write("Пример: (Влажные салфетки \"Лента\"; 235)");
                Console.WriteLine("------ Меню ------");
                Console.WriteLine("1. Вывод данных ");
                Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма) ");
                Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка) ");
                Console.WriteLine("4. Конвертация валюты (пользователь вводит курс или выбирает из списка) ");
                Console.WriteLine("5. Введите свои траты, которые будут записаны ");
                Console.WriteLine("0. Выход ");
                Console.WriteLine();
                Console.Write("Ваш выбор: ");
                int choise = Convert.ToInt32(Console.ReadLine());
                switch (choise)
                {
                    case 1:
                        Console.WriteLine("Вы выбрали вывод данных");
                        break;
                    case 2:
                        Console.WriteLine("Вы выбрали статистику");
                        break;
                    case 3:
                        Console.WriteLine("Вы выбрали сортировку по цене");
                        break;
                    case 4:
                        Console.WriteLine("Вы выбрали конвертацию валюты");
                        break;
                    case 5:
                        Console.WriteLine("Вы выбрали ввод своих трат");
                        break;
                    case 0:
                        Console.WriteLine("Выход из программы");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}
