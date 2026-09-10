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
                List<(string Name, decimal Amount)> expenses = new List<(string Name, decimal Amount)>();
                Console.WriteLine("Введите свои траты, которые будут записаны: ");
                Console.Write("Пример: (Влажные салфетки \"Лента\"; 235)");
                Console.WriteLine();
                for (int i = 0; i < countFinTran; i++)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Операция #{i + 1}");
                    Console.Write("Название услуги или товара: ");
                    string name = Console.ReadLine();
                    Console.Write("Количество потраченных денег: ");
                    decimal amount = Convert.ToDecimal(Console.ReadLine());
                    expenses.Add((name, amount));
                }
                bool programIsRunning = true;
                while (programIsRunning)
                {
                    Console.WriteLine();
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
                    Console.WriteLine();
                    switch (choise)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("Вы выбрали вывод данных");
                            Console.WriteLine();
                            ShowExpenses(expenses);
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("Вы выбрали статистику");
                            ShowStatistic(expenses);
                            break;
                        case 3:
                            Console.WriteLine("Вы выбрали сортировку по цене");
                            BubbleSort(expenses);
                            break;
                        case 4:
                            Console.WriteLine("Вы выбрали конвертацию валюты");
                            break;
                        case 5:
                            Console.WriteLine("Вы выбрали ввод своих трат");
                            break;
                        case 0:
                            Console.WriteLine("Выход из программы");
                            programIsRunning = false;
                            break;
                        default:
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }
                }
            }
        }

        static void ShowExpenses(List<(string Name, decimal Amount)> expenses)
        {
            Console.WriteLine("Список трат:");
            for (int i = 0; i < expenses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {expenses[i].Name} — " + $"{expenses[i].Amount:F2} руб.");
            }
        }   
        static void ShowStatistic(List<(string Name, decimal Amount)> expenses)
        {
            decimal sum = 0;
            decimal min = expenses[0].Amount;
            decimal max = expenses[0].Amount;
            foreach (var expense in expenses)
            {
                sum += expense.Amount;

                if (expense.Amount < min)
                {
                    min = expense.Amount;
                }

                if (expense.Amount > max)
                {
                    max = expense.Amount;
                }
            }
            decimal average = sum / expenses.Count;
            Console.WriteLine($"Сумма: {sum:F2} руб.");
            Console.WriteLine($"Среднее: {average:F2} руб.");
            Console.WriteLine($"Минимум: {min:F2} руб.");
            Console.WriteLine($"Максимум: {max:F2} руб.");
        }

        static void BubbleSort(List<(string Name, decimal Amount)> expenses)
        {
            for (int i = 0; i < expenses.Count - 1; i++)
            {
                for (int j = 0; j < expenses.Count - i - 1; j++)
                {
                    if (expenses[j].Amount > expenses[j + 1].Amount)
                    {
                        var temporary = expenses[j];
                        expenses[j] = expenses[j + 1];
                        expenses[j + 1] = temporary;
                    }
                }
            }
        }
    }
}
