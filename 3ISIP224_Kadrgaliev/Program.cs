using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP224_Kadrgaliev
{
    enum Category
    {
        Еда,
        Электроника,
        Одежда
    }

    class Product
    {
        private static int nextID = 1;
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Category ProductCategory { get; set; }
        public bool InStock => Quantity > 0;

        public Product(string name, decimal price, int quantity, Category category)
        {
            ID = nextID++;
            Name = name;
            Price = price;
            Quantity = quantity;
            ProductCategory = category;
        }

    }


    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>();

            products.Add(new Product("Хлеб", 87, 33, Category.Еда));
            products.Add(new Product("Молоко", 117, 41, Category.Еда));
            products.Add(new Product("Телефон", 35000, 5, Category.Электроника));
            products.Add(new Product("Наушники", 5000, 13, Category.Электроника));
            products.Add(new Product("Футболка", 1500, 12, Category.Одежда));
            SearchProduct(products);
        }
        static void ShowInfo(Product product)
        {
            Console.WriteLine($"Код: {product.ID}");
            Console.WriteLine($"Название: {product.Name}");
            Console.WriteLine($"Цена: {product.Price} руб.");
            Console.WriteLine($"Количество: {product.Quantity}");
            Console.WriteLine($"На складе: {(product.InStock ? "Да" : "Нет")}");
            Console.WriteLine($"Категория: {product.ProductCategory}");
            Console.WriteLine();
        }

        static void ShowAllProducts(List<Product> products)
        {
            if (products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
                return;
            }
            foreach (Product product in products)
            {
                ShowInfo(product);
            }
        }

        static Category ChooseCategory()
        {
            while (true)
            {
                Console.WriteLine("Категории:");
                Console.WriteLine("1. Еда");
                Console.WriteLine("2. Электроника");
                Console.WriteLine("3. Одежда");
                Console.Write("Выберите категорию: ");
                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    if (number >= 1 && number <= 3)
                    {
                        return (Category)(number - 1);
                    }
                }
                Console.WriteLine("Неверная категория.");
            }
        }

        static void AddProduct(List<Product> products)
        {
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название не может быть пустым.");
                Console.Write("Введите название товара: ");
                name = Console.ReadLine();
            }
            decimal price;
            while (true)
            {
                Console.Write("Введите цену: ");
                if (decimal.TryParse(Console.ReadLine(), out price) && price >= 0)
                {
                    break;
                }
                Console.WriteLine("Введите корректную цену.");
            }
            int quantity;
            while (true)
            {
                Console.Write("Введите количество: ");
                if (int.TryParse(Console.ReadLine(), out quantity) && quantity >= 0)
                {
                    break;
                }
                Console.WriteLine("Количество не может быть отрицательным.");
            }
            Category category = ChooseCategory();
            Product product = new Product(name, price, quantity, category);
            products.Add(product);
            Console.WriteLine($"Товар добавлен. Его код: {product.ID}");
        }
        static void SearchProduct(List<Product> products)
        {
            Console.Write("Введите код товара: ");
            if (!int.TryParse(Console.ReadLine(), out int ID))
            {
                Console.WriteLine("Некорректный код.");
                return;
            }
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название не может быть пустым.");
                return;
            }
            Category category = ChooseCategory();
            bool found = false;
            foreach (Product product in products)
            {
                if (product.ID == ID && product.Name.ToLower() == name.ToLower() && product.ProductCategory == category)
                {
                    ShowInfo(product);
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("Товар не найден.");
            }
        }
    }
}