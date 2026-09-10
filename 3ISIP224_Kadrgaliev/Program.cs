using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP224_Kadrgaliev {
    internal class Program {
        static void Main(string[] args) {
            Console.Write("Введите количество операций, которые будут записаны: ");
            int countFinTran = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            if (countFinTran < 2) {
                Console.WriteLine("Вы ввели слишком мало операций!");
            }
            else if (countFinTran > 40) {
                Console.WriteLine("Вы ввели слишком много операций!");
            }
            else {
                List<(string name, decimal amount)> expenses = new List<(string name, decimal amount)>();
                Console.WriteLine("Введите свои траты, которые будут записаны");
                for (int i = 0; i < countFinTran; i++) {
                    Console.WriteLine();
                    Console.WriteLine($"Операция #{i + 1}");
                    Console.Write("Название услуги или товара: ");
                    string name = Console.ReadLine();
                    Console.Write("Количество потраченных денег: ");
                    decimal amount = Convert.ToDecimal(Console.ReadLine());
                    expenses.Add((name, amount));
                }
                bool programIsRunning = true;
                while (programIsRunning) {
                    Console.WriteLine();
                    Console.WriteLine("------ Меню ------");
                    Console.WriteLine("1. Вывод данных ");
                    Console.WriteLine("2. Статистика трат ");
                    Console.WriteLine("3. Сортировка по цене ");
                    Console.WriteLine("4. Конвертация валюты ");
                    Console.WriteLine("5. Поиск по названию ");
                    Console.WriteLine("0. Выход ");
                    Console.WriteLine();
                    Console.Write("Ваш выбор: ");
                    int choise = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    switch (choise) {
                        case 1:
                            Console.Clear();
                            showExpenses(expenses);
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("Статистика по вашим тратам:");
                            showStatistic(expenses);
                            break;
                        case 3:
                            Console.Clear();
                            Console.WriteLine("Вы выбрали сортировку по цене");
                            bubbleSort(expenses);
                            Console.WriteLine();
                            showExpenses(expenses);
                            break;
                        case 4:
                            Console.Clear();
                            convertCurrency(expenses);
                            break;
                        case 5:
                            Console.Clear();
                            Console.WriteLine("Поиск по имени");
                            Console.WriteLine();
                            SearchByName(expenses);
                            break;
                        case 0:
                            Console.Clear();
                            Console.WriteLine("Выход из программы");
                            programIsRunning = false;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Неверный выбор. Попробуйте снова.");
                            break;
                    }
                }
            }
        }
        static void showExpenses(List<(string name, decimal amount)> expenses) {
            Console.WriteLine("Список трат:");
            for (int i = 0; i < expenses.Count; i++) {
                Console.WriteLine($"{i + 1}. {expenses[i].name} — " + $"{expenses[i].amount:F2} руб.");
            }
        }   
        static void showStatistic(List<(string name, decimal amount)> expenses) {
            decimal sum = 0;
            decimal min = expenses[0].amount;
            decimal max = expenses[0].amount;
            foreach (var expense in expenses) {
                sum += expense.amount;
                if (expense.amount < min) {
                    min = expense.amount;
                }
                if (expense.amount > max) {
                    max = expense.amount;
                }
            }
            decimal average = sum / expenses.Count;
            Console.WriteLine($"Сумма: {sum:F2} руб.");
            Console.WriteLine($"Среднее: {average:F2} руб.");
            Console.WriteLine($"Минимум: {min:F2} руб.");
            Console.WriteLine($"Максимум: {max:F2} руб.");
        }
        static void bubbleSort(List<(string name, decimal amount)> expenses) {
            for (int i = 0; i < expenses.Count - 1; i++) {
                for (int j = 0; j < expenses.Count - i - 1; j++) {
                    if (expenses[j].amount > expenses[j + 1].amount) {
                        var temporary = expenses[j];
                        expenses[j] = expenses[j + 1];
                        expenses[j + 1] = temporary;
                    }
                }
            }
        }
        static void convertCurrency(List<(string name, decimal amount)> expenses) {
            Console.Write("Введите курс валюты — стоимость одной единицы в рублях: ");
            decimal exchangeRate = Convert.ToDecimal(Console.ReadLine());
            if (exchangeRate <= 0) {
                Console.WriteLine("Курс должен быть больше нуля.");
                return;
            }
            Console.Write("Введите название валюты, например USD: ");
            string currencyName = Console.ReadLine();
            Console.WriteLine();
            foreach (var expense in expenses) {
                decimal convertedAmount = expense.amount / exchangeRate;
                Console.WriteLine($"{expense.name} — " + $"{convertedAmount:F2} {currencyName}");
            }
        }
        static void SearchByName(List<(string name, decimal amount)> expenses) {
            Console.Write("Введите название для поиска: ");
            string searchText = Console.ReadLine().ToLower();
            bool found = false;
            foreach (var expense in expenses) {
                if (expense.name.ToLower().Contains(searchText)) {
                    Console.WriteLine($"{expense.name} — {expense.amount:F2} руб.");
                    found = true;
                }
            }
            if (!found) {
                Console.WriteLine("Траты с таким названием не найдены.");
            }
        }
    }
}